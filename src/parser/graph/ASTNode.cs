namespace parser {
  namespace graph {
    /**
    *  Base for all AST nodes.
    */
    class ASTNode {
      public virtual void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}