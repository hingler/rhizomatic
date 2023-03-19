namespace parser {
  namespace graph {
    class UnlockNode : LinkingNode {
      public String unlockName = "";

      public UnlockNode(int id) : base(id) {}

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}