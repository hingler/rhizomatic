namespace parser {
  namespace reader {
    class DialogueFileReader {
      private List<String> content;
      public int offset = 0;

      public static DialogueFileReader fromFile(String path) {
        // TODO: handle failure gracefully
        String content = System.IO.File.ReadAllText(path);
        return new DialogueFileReader(content);
      }

      /**
       *  Creates new dialogue file reader
       *  @param content - file content
       *  @param fd - descriptor which identifies this file
       */
      public DialogueFileReader(String content) {
        this.content = content.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList<String>();
      }

      public String peekLine() {
        return (hasContent() ? content[offset] : "");
      }

      public String nextLine() {
        return (hasContent() ? content[offset++] : "");
      }

      public bool hasContent() {
        return (content.Count > offset);
      }
    }
  }
}