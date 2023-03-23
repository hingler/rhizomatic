using parser.graph;

namespace events {
  namespace handle {
    namespace _impl {
      class DialogueHandleImpl : DialogueHandle {
        private DialogueNode node;

        public string speaker => node.speaker;

        public string dialogue => node.dialogue;

        private DialogueStateManager manager;

        private bool advanced_ = false;

        public DialogueHandleImpl(DialogueStateManager manager, DialogueNode node) {
          this.manager = manager;
          this.node = node;
        }

        public void advance() {
          if (advanced_) {
            throw new InvalidOperationException("attempted to advance multiple times on same handle");
          }

          advanced_ = true;
          manager.AdvanceDialogueState(node.next);
        }
      }
    }
  }
}