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

  public void UpdateCurrentNode() {
    if (currentNode != null) {
      Type t = currentNode.GetType();

      if (currentNode is BranchNode) {
        BranchHandleImpl branchHandler = new BranchHandleImpl(this, (currentNode as BranchNode)!, map);
        listener?.onBranch(branchHandler);
      } else if (currentNode is DialogueNode) {
        DialogueHandleImpl dialogueHandler = new DialogueHandleImpl(this, (currentNode as DialogueNode)!);
        listener?.onDialogue(dialogueHandler);
      } else if (currentNode is DynamicLock) {
        DynamicLockHandleImpl dynamicLockHandle = new DynamicLockHandleImpl(this, map, (currentNode as DynamicLock)!);
        listener?.onDynamicLock(dynamicLockHandle);
      } else if (currentNode is JumpNode) {
        currentNode = map.GetLabel((currentNode as JumpNode)!.label);
      } else if (currentNode is LinkingNode) {
        // temp default
        currentNode = (currentNode as LinkingNode)!.next;
        UpdateCurrentNode();
      }
    }
  }

  // this just tracks state and delegates events
  // the event impls themselves will track state, and inform us which model we want to visit next
  public void AdvanceDialogueState(ASTNode? node) {
    // use null to specify the end of a sequence
    if (node == null) {
      currentNode = null;
      listener?.onDialogueEnd();
    } else {
      currentNode = node;
      UpdateCurrentNode();
    }
  }
}