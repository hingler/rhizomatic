namespace events {
  namespace handle {
    public interface DialogueHandle: Handle {
      String dialogue { get; }

      /**
       *  Advance to next dialogue item
       */
      void advance();
    }
  }
}