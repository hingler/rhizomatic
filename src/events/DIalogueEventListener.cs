namespace events {
  using handle;

  /**
   *  Dialogue will be processed through events
   *  we'll provide event listener with sufficient information,
   *  and wait for the client to respond via handles we provide
   */
  public interface DialogueEventListener {
    /**
     *  Called for dialogue
     */
    void onDialogue(DialogueHandle handle);

    /**
     *  Called for branches
     */
    void onBranch(BranchHandle handle);

    /**
     *  Called when dynamic lock encountered
     */
    void onDynamicLock(DynamicLockHandle handle);

    /**
     *  Called when current sequence completes
     */
    void onDialogueEnd();
  }
}