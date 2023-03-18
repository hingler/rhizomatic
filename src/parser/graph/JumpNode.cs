namespace parser {
  namespace graph {
    class JumpNode : ASTNode {
      public String label = "";

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}