namespace parser {
  namespace graph {
    class UnlockNode : LinkingNode {
      public String unlockName = "";

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}