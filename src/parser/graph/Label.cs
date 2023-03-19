namespace parser {
  namespace graph {
    /**
    *  Stores a label and its corresponding description
    */
    class Label : LinkingNode {
      public String name = "";
      public String description = "";

      public Label(int id) : base(id) {}

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}