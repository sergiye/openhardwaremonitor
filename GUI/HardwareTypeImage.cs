﻿using System.Drawing;
using System.Collections.Generic;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareMonitor.GUI {
  public class HardwareTypeImage {
    private static HardwareTypeImage instance = new HardwareTypeImage();

    private IDictionary<HardwareType, Image> images = 
      new Dictionary<HardwareType, Image>();

    private HardwareTypeImage() { }

    public static HardwareTypeImage Instance {
      get { return instance; }
    }

    public Image GetImage(HardwareType hardwareType) {
      Image image;
      if (images.TryGetValue(hardwareType, out image)) {
        return image;
      } else {
        switch (hardwareType) {
          case HardwareType.CPU:
            image = Utilities.EmbeddedResources.GetImage("cpu.png");
            break;
          case HardwareType.GpuNvidia:
            image = Utilities.EmbeddedResources.GetImage("nvidia.png");
            break;
          case HardwareType.GpuAti:
            image = Utilities.EmbeddedResources.GetImage("ati.png");
            break;
          case HardwareType.HDD:
            image = Utilities.EmbeddedResources.GetImage("hdd.png");
            break;
          case HardwareType.Heatmaster:
            image = Utilities.EmbeddedResources.GetImage("bigng.png");
            break;
          case HardwareType.Mainboard:
            image = Utilities.EmbeddedResources.GetImage("mainboard.png");
            break;
          case HardwareType.SuperIO:
            image = Utilities.EmbeddedResources.GetImage("chip.png");
            break;
          case HardwareType.TBalancer:
            image = Utilities.EmbeddedResources.GetImage("bigng.png");
            break;
          case HardwareType.RAM:
            image = Utilities.EmbeddedResources.GetImage("ram.png");
            break;
          default:
            image = new Bitmap(1, 1);
            break;
        }
        images.Add(hardwareType, image);
        return image;
      }
    }
  }
}
