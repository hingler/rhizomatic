namespace parser {
  namespace graph {
    class JumpNodeImpl : IJumpNode {

      private string _label;

      public override string label {
        get { return _label; }
      }

      public JumpNodeImpl(int id, string newLabel) : base(id) {
        _label = newLabel;
      }

      override public void accept(visitor.DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}