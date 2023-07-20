namespace parser {
  namespace reader {
    class DialogueFileReader {
      private List<String> content;
      private String _descriptor;
      public String descriptor {
        get => _descriptor;
      }
      public int offset = 0;

      public static DialogueFileReader fromFile(String path) {
        // TODO: handle failure gracefully
        String content = System.IO.File.ReadAllText(path);
        return new DialogueFileReader(content, new FileInfo(path).Name.Split('.')[0]);
      }

      /**
       *  Creates new dialogue file reader
       *  @param content - file content
       *  @param fd - descriptor which identifies this file
       */
      public DialogueFileReader(String content, String fd) {
        this.content = content.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList<String>();
        this._descriptor = fd;
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