namespace events {
  namespace handle {
    /**
     *  Represents a dynamic lock, which requires user input
     */
    public interface DynamicLockHandle: Handle {

      /**
       *  Provide a list of dedupes to advance this handle.
       */
      void advance(params String[] dedupes);
    }
  }
}