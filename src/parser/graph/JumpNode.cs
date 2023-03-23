namespace parser {
  namespace graph {
    class JumpNode : ASTNode {
      public String label = "";

      public JumpNode(int id) : base(id) {}

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}