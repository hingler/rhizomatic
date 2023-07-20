namespace parser {
  namespace graph {
    namespace jumptable {
      /**
       * Represents a single jump label in a `.jumptable` file
       */
      struct JumpTable {
        public List<string> staticLocks;
        public string passLabel;

        public JumpTable() {
          staticLocks = new List<string>();
          passLabel = "";
        }
      }
    }
  }
}