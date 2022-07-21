namespace OpenHardwareMonitor.Hardware {

  public interface IVisitor {
    void VisitComputer(IComputer computer);
    void VisitHardware(IHardware hardware);
    void VisitSensor(ISensor sensor);
    void VisitParameter(IParameter parameter);
  }

}
