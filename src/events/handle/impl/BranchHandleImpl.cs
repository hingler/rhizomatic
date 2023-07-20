using parser.graph;
using label;

namespace events {
  namespace handle {
    namespace _impl {
      class BranchHandleImpl : BranchHandle {
        private List<Label> labels_ = new List<Label>();

        private List<String> branchDescriptions_ = new List<String>();

        private List<bool> isDynamicLock_ = new List<bool>();

        private List<bool> visited_ = new List<bool>();

        private String speaker_;

        public IReadOnlyList<string> branchDescriptions => branchDescriptions_;

        public IReadOnlyList<bool> isDynamicLock => isDynamicLock_;

        public IReadOnlyList<bool> visited => visited_;

        public String speaker => speaker_;

        private DialogueStateManager manager;

        private BranchNode branchNode;

        private LabelMap labels;

        private bool advanced_ = false;

        public BranchHandleImpl(DialogueStateManager manager, BranchNode node, LabelMap labels, String speaker) {
          this.manager = manager;
          this.branchNode = node;
          this.labels = labels;
          this.speaker_ = speaker;

          foreach (ASTNode branch in node.branches) {
            handleBranch(branch);
          }
        }

        private void handleBranch(ASTNode node) {
          Label? destination = null;
          String labelName = "";
          bool dynamic = false;
          if (node is IJumpNode) {
            labelName = (node as IJumpNode)!.label;
          } else if (node is StaticLock) {
            // we'd need to test this lock, and add it only if it's a valid path to take
            // for now, assume true
            // (TODO: static lock receives state manager, return prematurely if test fails)
            StaticLock staticLock = (node as StaticLock)!;
            labelName = staticLock.passLabel;
            for (int i = 0; i < staticLock.locks.Count(); i++) {
              string lockName = staticLock.locks[i];
              if (!manager.IsUnlocked(lockName)) {
                // locked, do not add
                return;
              }
            }
          } else if (node is DynamicLock) {
            labelName = (node as DynamicLock)!.passLabel;
            dynamic = true;
          }

          destination = labels.GetLabel(labelName);

          if (destination == null) {
            throw new InvalidDialogueException("Branch jumps to label `" + labelName + "` which does not exist");
          }

          branchDescriptions_.Add(destination.description);
          isDynamicLock_.Add(dynamic);
          visited_.Add(false);

          labels_.Add(destination);
        }

        public void advance(int branch) {
          if (advanced_) {
            throw new InvalidOperationException("attempted to advance multiple times on same handle");
          }
          // need to communicate back to component which branch we're taking
          // ideally: we'd have an idea within this handle which nodes we might visit next
          if (branch < 0 || branch >= labels_.Count) {
            throw new IndexOutOfRangeException("Cannot advance on invalid node");
          }

          advanced_ = true;
          manager.AdvanceDialogueState(labels_[branch]);
        }
      }
    }
  }
}