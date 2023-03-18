namespace parser {
  namespace graph {
    class LockNode : LinkingNode {
      public String lockName = "";

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}