using System;

namespace OpenHardwareMonitor.Hardware {

  public class SensorVisitor : IVisitor {
    private readonly SensorEventHandler handler;

    public SensorVisitor(SensorEventHandler handler) {
      if (handler == null)
        throw new ArgumentNullException("handler");
      this.handler = handler;
    }

    public void VisitComputer(IComputer computer) {
      if (computer == null)
        throw new ArgumentNullException("computer");
      computer.Traverse(this);
    }

    public void VisitHardware(IHardware hardware) {
      if (hardware == null)
        throw new ArgumentNullException("hardware");
      hardware.Traverse(this);
    }

    public void VisitSensor(ISensor sensor) {
      handler(sensor);
    }

    public void VisitParameter(IParameter parameter) { }
  }
}
