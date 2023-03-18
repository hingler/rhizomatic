using System.Text.RegularExpressions;

namespace parser {
  using reader;
  using graph;
  /**
   *  Visits dialogue items
   */
  class DialogueParser {
    // for this: thinking that we read line by line in visitor
    // and construct nodes as we go
    // (we're fortunate that nodes can snag an entire line and handle it in isolation)
    public List<Label> visitDialogue(String path) {
      DialogueFileReader reader = new DialogueFileReader(path);
      List<Label> res = new List<Label>();
      // convert to nodes per line, then connect???
      
      while (reader.hasContent()) {
        Label label = getNextLabel(reader.nextLine());
        ASTNode currentNode = label;
        while (reader.hasContent() && currentNode is LinkingNode) {
          LinkingNode currentLinkingNode = label as LinkingNode;
          String line = reader.nextLine();
          currentLinkingNode.next = parseNode(line);
        }

        // label is complete, or we've reached the end of the file
        // store the label
        res.Add(label);
      }

      return res;
    }

    private Label getNextLabel(String line) {
      String trimLine = line.TrimStart();
      if (!trimLine.StartsWith(LABEL_PREFIX)) {
        throw new InvalidDialogueException("did not find label");
      }

      List<String> labelData = trimLine.Substring(LABEL_PREFIX.Length).TrimStart().Split(':', StringSplitOptions.TrimEntries).ToList<String>();
      if (labelData.Count < 2) {
        throw new InvalidDialogueException("label does not contain description");
      }
      
      Label result = new Label();
      result.name = labelData[0];
      result.description = labelData[1];

      return result;
    }

    private ASTNode parseNode(String line) {
      String lineTrim = line.Trim();
      switch (lineTrim[0]) {
        case JUMP_INIT:
          return parseJumpNode(lineTrim);
        case UNLOCK_INIT:
          return parseUnlockNode(lineTrim);
        case LOCK_INIT:
          return parseLockNode(lineTrim);
      }

      // test the rest
      Match match;
      match = LABEL_REGEX.Match(lineTrim);
      if (match.Success) {
        return parseLabel(match);
      }

      match = BRANCH_REGEX.Match(lineTrim);
      if (match.Success) {
        return parseBranch(lineTrim);
      }

      match = DIALOGUE_REGEX.Match(lineTrim);
      if (match.Success) {
        return parseDialogueNode(match);
      }

      throw new InvalidDialogueException("Line does not match any known syntax: '" + line + "'");
    }

    private Label parseLabel(Match match) {
      Label result = new Label();
      result.name = match.Groups[0].Value;
      result.description = match.Groups[1].Value;
      return result;
    }

    private BranchNode parseBranch(String line) {
      BranchNode result = new BranchNode();
      List<String> branchContent = line.Split('|', StringSplitOptions.TrimEntries).ToList<String>();
      foreach (String label in branchContent) {
        // see if this label is a dynamic or static lock
        String labelContent = label.Trim();
        if (labelContent[0] == DYNAMIC_LOCK_INIT) {
          result.branches.Add(parseDynamicLock(labelContent));
        } else if (labelContent[0] == STATIC_LOCK_INIT) {
          result.branches.Add(parseStaticLock(labelContent));
        } else {
          JumpNode jump = new JumpNode();
          jump.label = labelContent;
          result.branches.Add(jump);
        }
      }
      return result;
    }

    private StaticLock parseStaticLock(String lockContent) {
      StaticLock result = new StaticLock();
      Match match = STATIC_LOCK_REGEX.Match(lockContent);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on dynamic lock '" + lockContent + "'");
      }
      List<String> locks = match.Groups[0].Value.Split(',', StringSplitOptions.TrimEntries).ToList();
      result.locks = locks;
      result.passLabel = match.Groups[1].Value.Trim();
      return result;
    }

    private DynamicLock parseDynamicLock(String lockContent) {
      DynamicLock result = new DynamicLock();
      Match match = DYNAMIC_LOCK_REGEX.Match(lockContent);
      // 0: dedupes
      // 1: pass
      // 2: fail
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on dynamic lock '" + lockContent + "'");
      }

      result.requirements = match.Groups[0].Value.Split(',', StringSplitOptions.TrimEntries).ToList();
      result.passLabel = match.Groups[1].Value.Trim();
      result.failLabel = match.Groups[2].Value.Trim();
      return result;
    }

    private DialogueNode parseDialogueNode(Match match) {
      DialogueNode result = new DialogueNode();
      String speaker = match.Groups[0].Value.Trim();
      String dialogue = match.Groups[1].Value.Trim();
      result.speaker = speaker;
      result.dialogue = dialogue;
      return result;
    }

    private JumpNode parseJumpNode(String line) {
      JumpNode res = new JumpNode();
      Match match = JUMP_REGEX.Match(line);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on jump node '" + line + "'");
      }

      res.label = match.Groups[0].Value.Trim();
      return res;
    }

    private UnlockNode parseUnlockNode(String line) {
      UnlockNode res = new UnlockNode();
      Match match = UNLOCK_REGEX.Match(line);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on unlock node '" + line + "'");
      }

      res.unlockName = match.Groups[0].Value.Trim();
      return res;
    }

    private LockNode parseLockNode(String line) {
      LockNode res = new LockNode();
      Match match = LOCK_REGEX.Match(line);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on lock node '" + line + "'");
      }

      res.lockName = match.Groups[0].Value.Trim();
      return res;
    }


    // DIALOGUE CONSTANTS
    // TODO: replace with https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-6.0

    // we could do a lot better but for now I'm just gonna regex test each one :)
    private const String LABEL_PREFIX = "#";
    private const char JUMP_INIT = '^';
    private const char DYNAMIC_LOCK_INIT = '[';
    private const char STATIC_LOCK_INIT = '(';
    private const char UNLOCK_INIT = '>';
    private const char LOCK_INIT = '<';

    private static Regex LABEL_REGEX = new Regex(@"^\s*#\s*([\w\s-]+)\s*:\s*([\w\s-]+)$");
    private static Regex DIALOGUE_REGEX = new Regex(@"^\s*([\w\s-]+)\s*:\s*([\w\s-]+)$");
    private static Regex BRANCH_REGEX = new Regex(@"^\s*([\w\s-]+)((\|[\w\s-]+)*)$");
    private static Regex STATIC_LOCK_REGEX = new Regex(@"\(\s*([\w\s-]+\s*(?:\,[\w\s-]+)*)\s*:\s*([\w\s-]+)\)");
    private static Regex DYNAMIC_LOCK_REGEX = new Regex(@"\[\s*([\w\s-]+(?:\,\s*[\w\s-]+)*)\s*\?\s*([\w\s-]+)\s*:([\w\s-]+)\s*\]");
    private static Regex JUMP_REGEX = new Regex(@"^\^([\w\s-]+)$");
    private static Regex UNLOCK_REGEX = new Regex(@"^>([\w\s-]+)<$");
    private static Regex LOCK_REGEX = new Regex(@"^<([\w\s-]+)>$");
  }
}