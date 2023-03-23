namespace parser {
  namespace graph {
    /**
    *  Base for all AST nodes.
    */
    class ASTNode {

      public int id { get => id_; }
      private int id_;

      public ASTNode(int id) {
        this.id_ = id;
      }
      public virtual void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}