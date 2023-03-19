using parser.graph;

namespace label {
  /**
   *  Retrieves labels by their name
   */
  class LabelMap {
    private Dictionary<String, Label> _labels = new Dictionary<String, Label>();

    public void AddLabel(Label label) {
      _labels[label.name] = label;
    }

    public Label? GetLabel(String name) {
      return _labels[name];
    }
  }
}