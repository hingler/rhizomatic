using System;
using parser;
using parser.graph;

using parser.reader;


namespace hellobusiness {
  class Test {

    static public void Main(String[] args) {
      DialogueParser parser = new DialogueParser();
      List<Label> labels = parser.visitDialogue(DialogueFileReader.fromFile("testfile.txt"));
      visitor.DebugVisitor visitor = new visitor.DebugVisitor();
      SampleEventListener listener = new SampleEventListener();
      DialogueStateManager manager = new DialogueStateManager(listener);
      foreach (Label label in labels) {
        visitor.visit(label);
        manager.AddLabel(label);


      }


      manager.StartSequence(labels[0].name);
    }

    /**
    
    Todo:
      - Create a simple event listener to simulate interaction, and write a little CLI thing to mimic conversation
      - Once that's confirmed, we can merge this into dingo
      - Write some code to retain some stateful bits - particularly: unlocks, and which nodes have been visited
      - parsing should be deterministic st we get the same files every time - we could store a representation of our AST or just reload from file
        - option: retool ID to be file specific, store a CRC for the associated file (as a super rough heuristic)
        - if the CRC changes we wipe the visited info
      - we could also just save the parsed info somewhere but thats difficult :<

    */

    // alt: just use string keys???
    // (dialogue file : id, per dialogue)
    // rewrite parser to accept this id field on ctor (second ctor arg - by default file name)
  }
}