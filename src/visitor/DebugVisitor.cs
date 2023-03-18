using parser.graph;

namespace visitor {
  class DebugVisitor : DialogueVisitor
  {
    private int indentLevel = 0;
    public void visit(ASTNode node)
    {
      node.accept(this);  
    }

    public void visit(BranchNode node)
    {
      printLine("BranchNode");
      indentLevel++;
      foreach (ASTNode branch in node.branches) {
        branch.accept(this);
      }
      indentLevel--;
    }

    public void visit(DialogueNode node)
    {
      printLine(node.speaker + ": " + node.dialogue);
      node.next?.accept(this);
    }

    public void visit(DynamicLock node)
    {
      printLine("[DYNAMIC LOCK] (pass: " + node.passLabel + " // fail: " + node.failLabel + ")");
    }

    public void visit(JumpNode node)
    {
      printLine("JUMP: " + node.label);
    }

    public void visit(Label label)
    {
      printLine("Label: " + label.name + " - " + label.description);
      indentLevel++;
      label.next?.accept(this);
      indentLevel--;
    }

    public void visit(LinkingNode node)
    {
      node.next?.accept(this);
    }

    public void visit(LockNode node)
    {
      printLine("LOCK: " + node.lockName);
      node.next?.accept(this);
    }

    public void visit(RootNode node)
    {
      printLine("((ROOT))");
      indentLevel++;
      foreach (ASTNode childNode in node.nodes) {
        childNode.accept(this);
      }
      indentLevel--;
    }

    public void visit(StaticLock node)
    {
      printLine("(STATIC LOCK) -> " + node.passLabel);
    }

    public void visit(UnlockNode node)
    {
      printLine("UNLOCK: " + node.unlockName);
    }

    private void printLine(String line) {
      String content = line;
      for (int i = 0; i < indentLevel; i++) {
        content = "  " + content;
      }

      Console.WriteLine(content);
    }
  }
}