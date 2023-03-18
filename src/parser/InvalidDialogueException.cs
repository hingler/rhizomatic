[Serializable]
public class InvalidDialogueException : Exception {
  public InvalidDialogueException() : base() {}
  public InvalidDialogueException(String message) : base(message) {}
}