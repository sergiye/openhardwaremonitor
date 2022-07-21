using System.Collections.Generic;

namespace OpenHardwareMonitor.Collections {

  public interface IReadOnlyArray<T> : IEnumerable<T> {

    T this[int index] { get; }

    int Length { get; }
    
  }
}
