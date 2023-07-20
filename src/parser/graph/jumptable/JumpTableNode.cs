namespace parser {
  namespace graph {
    namespace jumptable {
      class JumpTableNode : IJumpNode {
        private List<JumpTable> labels;
        private DialogueStateManager stateManager;
        public JumpTableNode(int id, DialogueStateManager stateManager) : base(id) {
          labels = new List<JumpTable>();
          this.stateManager = stateManager;
        }

        public void AddLabel(IReadOnlyCollection<string> locks, string label) {
          JumpTable newLabel = new JumpTable();
          newLabel.staticLocks.AddRange(locks);
          newLabel.passLabel = label;
          labels.Add(newLabel);
          Console.WriteLine(labels.Count());
        }

        public override string label {
          get {
            string currentLabel = "FAIL_LABEL";
            Console.WriteLine(labels.Count());
            for (int i = 0; i < labels.Count(); i++) {
              JumpTable currentJump = labels[i];
              Console.WriteLine(currentJump.passLabel);
              for (int j = 0; j < currentJump.staticLocks.Count(); j++) {
                string currentStaticLock = currentJump.staticLocks[j];
                if (!stateManager.IsUnlocked(currentStaticLock)) {
                  return currentLabel;
                }
              }

              currentLabel = currentJump.passLabel;
            }

            return currentLabel;
          }
        }
      }
    }
  }
}