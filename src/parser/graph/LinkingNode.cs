namespace parser {
  namespace graph {
    class LinkingNode : ASTNode {
      public ASTNode? next = null;

      public LinkingNode(int id) : base(id) {}

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}