using events;
using events.handle;
using System;

public class SampleEventListener : DialogueEventListener {
  public void onBranch(BranchHandle handle)
  {
    for (int i = 0; i < handle.branchDescriptions.Count(); i++) {
      Console.WriteLine(i.ToString() + ": " + handle.branchDescriptions[i]);
    }

    Console.Write("> ");
    while (true) {
      String? s = Console.ReadLine();
      if (s != null) {
        int num = int.Parse(s);
        handle.advance(num);
        break;
      }
    }
  }

  public void onDialogue(DialogueHandle handle)
  {
    Console.WriteLine(handle.speaker + ": " + handle.dialogue);
    Console.ReadLine();
    handle.advance();
  }

  public void onDialogueEnd()
  {
    Console.WriteLine("ended:)");
    
  }

  public void onDynamicLock(DynamicLockHandle handle)
  {
    while (true) {

      String? s = Console.ReadLine();
      if (s != null) {
        String[] strings = s.Split(",");
        handle.advance(strings);
        break;
      }
    }
  }
}