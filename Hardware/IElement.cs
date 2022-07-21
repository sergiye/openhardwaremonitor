namespace OpenHardwareMonitor.Hardware {
  
  public interface IElement {
    // accept visitor on this element
    void Accept(IVisitor visitor);

    // call accept(visitor) on all child elements (called only from visitors)
    void Traverse(IVisitor visitor);
  }

}
