using System;
using parser;
using parser.graph;


namespace hellobusiness {
  class Test {

    static public void Main(String[] args) {
      DialogueParser parser = new DialogueParser();
      List<Label> labels = parser.visitDialogue("testfile.txt");
      visitor.DebugVisitor visitor = new visitor.DebugVisitor();
      foreach (Label label in labels) {
        visitor.visit(label);
      }
    }

    /**
     *  Dialogues should be primarily event driven
     *  We'll tell listeners which types of dialogue they can expect, and we'll send those to them
     *  Visitors will rove through the code :)
     *  - our "real deal" visitor will need a label table
    */
  }
}