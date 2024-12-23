﻿using System;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.Motherboard.Lpc;

internal class EcioPortGigabyteController : IGigabyteController
{
    private const ushort ControllerVersionOffset = 0x00;
    private const ushort ControllerEnableRegister = 0x47;
    private const ushort ControllerFanControlArea = 0x900;

    private const ushort EcioRegisterPort = 0x3F4;
    private const ushort EcioValuePort = 0x3F0;

    private readonly IT879xEcioPort _port;

    private bool? _initialState;

    private EcioPortGigabyteController(IT879xEcioPort port)
    {
        _port = port;
    }

    public static EcioPortGigabyteController TryCreate()
    {
        IT879xEcioPort port = new(EcioRegisterPort, EcioValuePort);

        // Check compatibility by querying its version.
        if (!port.Read(ControllerFanControlArea + ControllerVersionOffset, out byte majorVersion) || majorVersion != 1)
        {
            return null;
        }

        return new EcioPortGigabyteController(port);
    }

    public bool Enable(bool enabled)
    {
        ushort offset = ControllerFanControlArea + ControllerEnableRegister;

        if (!_port.Read(offset, out byte bCurrent))
        {
            return false;
        }

        bool current = Convert.ToBoolean(bCurrent);

        _initialState ??= current;
        
        if (current != enabled)
        {
            if (!_port.Write(offset, Convert.ToByte(enabled)))
            {
                return false;
            }

            // Allow the system to catch up.
            Thread.Sleep(500);
        }

        return true;
    }

    public void Restore()
    {
        if (_initialState.HasValue)
        {
            Enable(_initialState.Value);
        }
    }
}
