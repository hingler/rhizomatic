namespace graph {
  class DynamicLock : ASTNode {
    public List<String> requirements = new List<String>();
    public ASTNode? passLabel = null;
    public ASTNode? failLabel = null;
  }
}