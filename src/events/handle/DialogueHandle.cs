namespace events {
  namespace handle {
    public interface DialogueHandle {
      String speaker { get; }
      String dialogue { get; }

      /**
       *  Advance to next dialogue item
       */
      void advance();
    }
  }
}