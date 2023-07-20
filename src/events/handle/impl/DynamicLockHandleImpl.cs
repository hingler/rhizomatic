using parser.graph;
using label;

namespace events {
  namespace handle {
    namespace _impl {
      class DynamicLockHandleImpl : DynamicLockHandle {
        private DynamicLock dynamicLock;

        private LabelMap map;

        private DialogueStateManager manager;

        private bool advanced_ = false;

        public String speaker_;

        public String speaker => speaker_;

        public DynamicLockHandleImpl(DialogueStateManager manager, LabelMap map, DynamicLock dynamicLock, String speaker) {
          this.dynamicLock = dynamicLock;
          this.map = map;
          this.manager = manager;
          this.speaker_ = speaker;
        }

        public void advance(params String[] dedupes) {
          if (advanced_) {
            throw new InvalidOperationException("attempted to advance multiple times on same handle");
          }
          
          foreach (String dedupe in dynamicLock.requirements) {
            if (!dedupes.Contains(dedupe)) {
              advanced_ = true;
              manager.AdvanceDialogueState(map.GetLabel(dynamicLock.failLabel));
              return;
            }
          }

          advanced_ = true;
          manager.AdvanceDialogueState(map.GetLabel(dynamicLock.passLabel));
        }
      }
    }
  }
}