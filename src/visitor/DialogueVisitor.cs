using parser.graph;

namespace visitor {
  /**
   *
   */
  interface DialogueVisitor {
    public void visit(ASTNode node);

    public void visit(BranchNode node);

    public void visit(DialogueNode node);

    public void visit(DynamicLock node);

    public void visit(IJumpNode node);

    public void visit(Label label);

    public void visit(LinkingNode node);

    public void visit(LockNode node);

    public void visit(RootNode node);

    public void visit(StaticLock node);

    public void visit(UnlockNode node);
  }
}