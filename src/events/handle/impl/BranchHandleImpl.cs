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

        public IReadOnlyList<string> branchDescriptions => branchDescriptions_;

        public IReadOnlyList<bool> isDynamicLock => isDynamicLock_;

        public IReadOnlyList<bool> visited => visited_;

        private DialogueStateManager manager;

        private BranchNode branchNode;

        private LabelMap labels;

        private bool advanced_ = false;

        public BranchHandleImpl(DialogueStateManager manager, BranchNode node, LabelMap labels) {
          this.manager = manager;
          this.branchNode = node;
          this.labels = labels;

          foreach (ASTNode branch in node.branches) {
            handleBranch(branch);
          }
        }

        private void handleBranch(ASTNode node) {
          Label? destination = null;
          String labelName = "";
          bool dynamic = false;
          if (node is JumpNode) {
            labelName = (node as JumpNode)!.label;
          } else if (node is StaticLock) {
            // we'd need to test this lock, and add it only if it's a valid path to take
            // for now, assume true
            labelName = (node as StaticLock)!.passLabel;
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