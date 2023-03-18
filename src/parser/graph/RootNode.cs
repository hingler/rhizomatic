namespace parser {
  namespace graph {
    class RootNode : ASTNode {
      // contains all labels in a given file
      public List<ASTNode> nodes = new List<ASTNode>();
    }
  }
}