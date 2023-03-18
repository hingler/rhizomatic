namespace graph {
  /**
   *  Branch node, from which one of a number of labels can be accessed
   */
  class BranchNode : ASTNode {
    public List<ASTNode?> branches = new List<ASTNode?>();
  }
}