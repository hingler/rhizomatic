using label;
using parser.graph;


using events.handle._impl;
using events;

/**
 *  Moving engine for our dialogue machine
 */
class DialogueStateManager {
  private LabelMap map = new LabelMap();

  private DialogueEventListener? listener = null;

  private ASTNode? currentNode = null;

  private String lastSpeaker = "";
  private HashSet<string> lockSet = new HashSet<string>();

  public DialogueStateManager() {}

  public void AddLabel(Label label) {
    map.AddLabel(label);
  }

  public void RegisterDialogueEventListener(DialogueEventListener? listener) {
    this.listener = listener;
  }

  /**
   *  Starts a dialogue sequence at the specified label
   */
  public bool StartSequence(String labelName) {
    Label? init = map.GetLabel(labelName);
    if (init == null || this.listener == null) {
      // label does not exist, or listener is not registered
      return false;
    }

    AdvanceDialogueState(init);

    return true;
  }

  public bool IsUnlocked(string lockName) {
    return lockSet.Contains(lockName);
  }

  public void Unlock(string lockName) {
    lockSet.Add(lockName);
  }

  public void Lock(string lockName) {
    lockSet.Remove(lockName);
  }

  public void UpdateCurrentNode() {
    if (currentNode != null) {
      Type t = currentNode.GetType();
      if (currentNode is BranchNode) {
        BranchHandleImpl branchHandler = new BranchHandleImpl(this, (currentNode as BranchNode)!, map, lastSpeaker);
        listener?.onBranch(branchHandler);
      } else if (currentNode is DialogueNode) {
        lastSpeaker = ((DialogueNode)currentNode).speaker;
        DialogueHandleImpl dialogueHandler = new DialogueHandleImpl(this, (currentNode as DialogueNode)!);
        listener?.onDialogue(dialogueHandler);
      } else if (currentNode is DynamicLock) {
        DynamicLockHandleImpl dynamicLockHandle = new DynamicLockHandleImpl(this, map, (currentNode as DynamicLock)!, lastSpeaker);
        listener?.onDynamicLock(dynamicLockHandle);
      } else if (currentNode is IJumpNode) {
        currentNode = map.GetLabel((currentNode as IJumpNode)!.label);
        UpdateCurrentNode();
      } else if (currentNode is LinkingNode) {
        // temp default
        LinkingNode linkingNode = (currentNode as LinkingNode)!;
        _HandleLinkingNode(linkingNode);
        currentNode = linkingNode.next;
        UpdateCurrentNode();
      }
    } else {
      listener?.onDialogueEnd();
    }
  }

  private void _HandleLinkingNode(LinkingNode node) {
    if (node is UnlockNode) {
      Unlock((node as UnlockNode)!.unlockName);
    } else if (node is LockNode) {
      Lock((node as LockNode)!.lockName);
    }
  }

  // this just tracks state and delegates events
  // the event impls themselves will track state, and inform us which model we want to visit next
  public void AdvanceDialogueState(ASTNode? node) {
    // use null to specify the end of a sequence
    currentNode = node;
    UpdateCurrentNode();
  }

  // 7/19 - i feel like i know how to handle this better 
}