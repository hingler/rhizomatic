namespace parser {
  namespace graph {
    /**
    *  Branch node, from which one of a number of labels can be accessed
    */
    class BranchNode : ASTNode {

      public BranchNode(int id) : base(id) {}
      public List<ASTNode> branches = new List<ASTNode>();

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}