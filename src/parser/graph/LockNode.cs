namespace parser {
  namespace graph {
    class LockNode : LinkingNode {
      public String lockName = "";

      public LockNode(int id) : base(id) {}

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}