namespace parser {
  namespace graph {
    class RootNode : ASTNode {
      // contains all labels in a given file
      public List<ASTNode> nodes = new List<ASTNode>();

      public RootNode(int id) : base(id) {}

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}