namespace parser {
  namespace graph {
    class StaticLock : ASTNode {
      public List<String> locks = new List<String>();

      public String passLabel = "";
    }
  }
}