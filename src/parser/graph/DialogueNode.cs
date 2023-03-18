namespace parser {
  namespace graph {
    /**
    *  Node corresponding with a single dialogue item.
    */
    class DialogueNode : LinkingNode {
      public String speaker = "";
      public String dialogue = "";

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}