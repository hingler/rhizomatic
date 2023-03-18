namespace parser {
  namespace graph {
    class LinkingNode : ASTNode {
      public ASTNode? next = null;

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}