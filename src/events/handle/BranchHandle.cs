namespace events {
  namespace handle {
    public interface BranchHandle: Handle {

      // front facing descriptions for branches
      IReadOnlyList<String> branchDescriptions { get; }

      // true if branch is a dynamic lock
      IReadOnlyList<bool> isDynamicLock { get; }

      // true if branch has been visited at some point prior
      IReadOnlyList<bool> visited { get; }

      /**
       *  Choose branch
       */
      void advance(int branch);
    }
  }
}