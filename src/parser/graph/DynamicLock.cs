namespace parser {
  namespace graph {
    class DynamicLock : ASTNode {
      public List<String> requirements = new List<String>();
      public String passLabel = "";
      public String failLabel = "";
    }
  }
}