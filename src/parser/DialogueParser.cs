using System.Text.RegularExpressions;

namespace parser {
  using reader;
  using graph;
  using graph.jumptable;
  /**
   *  Visits dialogue items
   */
  class DialogueParser {
    // for this: thinking that we read line by line in visitor
    // and construct nodes as we go
    // (we're fortunate that nodes can snag an entire line and handle it in isolation)

    private int idCounter = 0;
    private DialogueStateManager _stateManager;

    public DialogueParser(DialogueStateManager stateManager) {
      _stateManager = stateManager;
    }

    public List<Label> visitDialogue(String path) {
      // TODO: should probably separate, but keeping as is to handle ID
      if (path.EndsWith(".jumptable")) {
        // jumptable, handle separately
        return visitJumpTableFile(path);
      } else {
        return visitDialogueFile(path);
      }
    }

    public List<Label> visitJumpTableFile(String path) {
      DialogueFileReader reader = DialogueFileReader.fromFile(path);
      Label rootLabel = new Label(idCounter++);
      JumpTableNode jumpLabelNode = new JumpTableNode(idCounter++, _stateManager);
      while (reader.hasContent()) {
        string jumpString = reader.nextLine();
        Match match = JUMP_LABEL_REGEX.Match(jumpString);
        List<string> lockList = match.Groups[1].Value.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList<string>();
        string passLabel = match.Groups[2].Value.Trim();
        jumpLabelNode.AddLabel(lockList, passLabel);
      }

      rootLabel.next = jumpLabelNode;
      rootLabel.name = new FileInfo(path).Name.Split('.')[0];
      rootLabel.description = "jumptable";
      List<Label> labelList = new List<Label>();
      labelList.Add(rootLabel);
      return labelList;
    }

    public List<Label> visitDialogueFile(String path) {
      DialogueFileReader reader = DialogueFileReader.fromFile(path);
      List<Label> res = new List<Label>();
      // convert to nodes per line, then connect???
    
      while (reader.hasContent()) {
        Label label = getNextLabel(reader.nextLine());
        ASTNode currentNode = label;
        while (reader.hasContent()) {
          LinkingNode? currentLinkingNode = currentNode as LinkingNode;
          if (currentLinkingNode != null) {
            String line = reader.peekLine();
            ASTNode nextNode = parseNode(line);
            if (nextNode is Label) {
              break;
            } else {
              // assign as next node
              reader.nextLine();
              currentLinkingNode.next = nextNode;
              currentNode = currentLinkingNode.next;
            }

          } else {
            // label is complete, or we've reached the end of the file
            break;
          }
        }

        res.Add(label);
      }


      return res;
    }

    private Label getNextLabel(String line) {
      String trimLine = line.TrimStart();
      Match match = LABEL_REGEX.Match(trimLine);
      if (match.Success) {
        return parseLabel(match);
      }

      throw new InvalidDialogueException("not a label");
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
      Label result = new Label(idCounter++);
      result.name = match.Groups[1].Value.Trim();
      result.description = match.Groups[2].Value.Trim();
      return result;
    }

    private BranchNode parseBranch(String line) {
      BranchNode result = new BranchNode(idCounter++);
      List<String> branchContent = line.Split('|', StringSplitOptions.TrimEntries).ToList<String>();
      foreach (String label in branchContent) {
        // see if this label is a dynamic or static lock
        String labelContent = label.Trim();
        if (labelContent[0] == DYNAMIC_LOCK_INIT) {
          result.branches.Add(parseDynamicLock(labelContent));
        } else if (labelContent[0] == STATIC_LOCK_INIT) {
          result.branches.Add(parseStaticLock(labelContent));
        } else {
          JumpNodeImpl jump = new JumpNodeImpl(idCounter++, labelContent);
          result.branches.Add(jump);
        }
      }
      return result;
    }

    private StaticLock parseStaticLock(String lockContent) {
      StaticLock result = new StaticLock(idCounter++);
      Match match = STATIC_LOCK_REGEX.Match(lockContent);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on dynamic lock '" + lockContent + "'");
      }
      List<String> locks = match.Groups[1].Value.Split(',', StringSplitOptions.TrimEntries).ToList();
      result.locks = locks;
      result.passLabel = match.Groups[2].Value.Trim();
      return result;
    }

    private DynamicLock parseDynamicLock(String lockContent) {
      DynamicLock result = new DynamicLock(idCounter++);
      Match match = DYNAMIC_LOCK_REGEX.Match(lockContent);
      // 0: dedupes
      // 1: pass
      // 2: fail
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on dynamic lock '" + lockContent + "'");
      }

      result.requirements = match.Groups[1].Value.Split(',', StringSplitOptions.TrimEntries).ToList();
      result.passLabel = match.Groups[2].Value.Trim();
      result.failLabel = match.Groups[3].Value.Trim();
      return result;
    }

    private DialogueNode parseDialogueNode(Match match) {
      DialogueNode result = new DialogueNode(idCounter++);
      String speaker = match.Groups[1].Value.Trim();
      String dialogue = match.Groups[2].Value.Trim();
      result.speaker = speaker;
      result.dialogue = dialogue;
      return result;
    }

    private IJumpNode parseJumpNode(String line) {
      Match match = JUMP_REGEX.Match(line);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on jump node '" + line + "'");
      }

      IJumpNode res = new JumpNodeImpl(idCounter++, match.Groups[1].Value.Trim());
      return res;
    }

    private UnlockNode parseUnlockNode(String line) {
      UnlockNode res = new UnlockNode(idCounter++);
      Match match = UNLOCK_REGEX.Match(line);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on unlock node '" + line + "'");
      }

      res.unlockName = match.Groups[1].Value.Trim();
      return res;
    }

    private LockNode parseLockNode(String line) {
      LockNode res = new LockNode(idCounter++);
      Match match = LOCK_REGEX.Match(line);
      if (!match.Success) {
        throw new InvalidDialogueException("Invalid syntax on lock node '" + line + "'");
      }

      res.lockName = match.Groups[1].Value.Trim();
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
    private static Regex BRANCH_REGEX = new Regex(@"^\s*([^\|]+)((\|[^\|]+)+)$");
    private static Regex STATIC_LOCK_REGEX = new Regex(@"\(\s*([\w\s-]+\s*(?:\,[\w\s-]+)*)\s*:\s*([\w\s-]+)\)");
    private static Regex DYNAMIC_LOCK_REGEX = new Regex(@"\[\s*([\w\s-]+(?:\,\s*[\w\s-]+)*)\s*\?\s*([\w\s-]+)\s*:([\w\s-]+)\s*\]");
    private static Regex JUMP_REGEX = new Regex(@"^\^([\w\s-]+)$");
    private static Regex UNLOCK_REGEX = new Regex(@"^>([\w\s-]+)<$");
    private static Regex LOCK_REGEX = new Regex(@"^<([\w\s-]+)>$");

    private static Regex JUMP_LABEL_REGEX = new Regex(@"^\((.*)\)\s*->\s*([A-Za-z0-9\s]+)$");
    private static Regex FILE_NAME_REGEX = new Regex(@"([^\.\/\\]+)\.[^\.]+$");
  }
}