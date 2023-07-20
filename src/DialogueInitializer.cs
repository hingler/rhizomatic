using events;
using parser.reader;
using parser;
using parser.graph;

/**
 *
 */
public class DialogueInitializer {
  // need to toss in a bunch of files
  // thinking: we just pass the text in and create a reader to go with it
  // (we'll probably be reading from resource files :D)

  // read at any point, add text files in the middle of reading
  private DialogueStateManager stateManager;
  private DialogueParser parser;

  public DialogueInitializer(DialogueEventListener listener) {
    this.stateManager = new DialogueStateManager();
    this.parser = new DialogueParser(stateManager);
    this.stateManager.RegisterDialogueEventListener(listener);
  }

  public void addDialogue(String dialogue) {
    DialogueFileReader reader = new DialogueFileReader(dialogue, "inplace");
    List<Label> labels = parser.visitDialogue(reader);
    foreach (Label label in labels) {
      stateManager.AddLabel(label);
    }
  }

  public bool startDialogue(String labelName) {
    // tba: mid-sequence handling, etc.
    return stateManager.StartSequence(labelName);
  }
}