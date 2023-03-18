namespace graph {
  /**
   *  Node corresponding with a single dialogue item.
   */
  class DialogueNode : ASTNode {
    public String speaker = "";
    public String dialogue = "";

    public ASTNode? next;
  }
}