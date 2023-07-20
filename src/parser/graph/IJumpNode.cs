using visitor;

namespace parser {
  namespace graph {
    abstract class IJumpNode : ASTNode {
      public abstract string label { get; }

      protected IJumpNode(int id) : base(id) {}

      public override void accept(DialogueVisitor v) {
        v.visit(this);
      }
    }
  }
}