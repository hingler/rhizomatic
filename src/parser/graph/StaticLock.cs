namespace parser {
  namespace graph {
    class StaticLock : ASTNode {
      public List<String> locks = new List<String>();

      public String passLabel = "";

      public StaticLock(int id) : base(id) {}

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}