using System;
using System.Globalization;

namespace OpenHardwareMonitor.Hardware {

  internal struct ParameterDescription {
    private readonly string name;
    private readonly string description;
    private readonly float defaultValue;    

    public ParameterDescription(string name, string description, 
      float defaultValue) {
      this.name = name;
      this.description = description;
      this.defaultValue = defaultValue;
    }

    public string Name { get { return name; } }

    public string Description { get { return description; } }

    public float DefaultValue { get { return defaultValue; } }
  }

  internal class Parameter : IParameter {
    private readonly ISensor sensor;
    private ParameterDescription description;
    private float value;
    private bool isDefault;
    private readonly ISettings settings;

    public Parameter(ParameterDescription description, ISensor sensor, 
      ISettings settings) 
    {
      this.sensor = sensor;
      this.description = description;
      this.settings = settings;
      this.isDefault = !settings.Contains(Identifier.ToString());
      this.value = description.DefaultValue;
      if (!this.isDefault) {
        if (!float.TryParse(settings.GetValue(Identifier.ToString(), "0"),
          NumberStyles.Float,
          CultureInfo.InvariantCulture,
          out this.value))
          this.value = description.DefaultValue;
      }
    }

    public ISensor Sensor {
      get {
        return sensor;
      }
    }

    public Identifier Identifier {
      get {
        return new Identifier(sensor.Identifier, "parameter",
          Name.Replace(" ", "").ToLowerInvariant());
      }
    }

    public string Name { get { return description.Name; } }

    public string Description { get { return description.Description; } }

    public float Value {
      get {
        return value;
      }
      set {
        this.isDefault = false;
        this.value = value;
        this.settings.SetValue(Identifier.ToString(), value.ToString(
          CultureInfo.InvariantCulture));
      }
    }

    public float DefaultValue { 
      get { return description.DefaultValue; } 
    }

    public bool IsDefault {
      get { return isDefault; }
      set {
        this.isDefault = value;
        if (value) {
          this.value = description.DefaultValue;
          this.settings.Remove(Identifier.ToString());
        }
      }
    }

    public void Accept(IVisitor visitor) {
      if (visitor == null)
        throw new ArgumentNullException("visitor");
      visitor.VisitParameter(this);
    }

    public void Traverse(IVisitor visitor) { }
  }
}
