﻿using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Interop;

internal static class D3dkmdt
{
    internal enum DXGK_ENGINE_TYPE
    {
        DXGK_ENGINE_TYPE_OTHER = 0,
        DXGK_ENGINE_TYPE_3D = 1,
        DXGK_ENGINE_TYPE_VIDEO_DECODE = 2,
        DXGK_ENGINE_TYPE_VIDEO_ENCODE = 3,
        DXGK_ENGINE_TYPE_VIDEO_PROCESSING = 4,
        DXGK_ENGINE_TYPE_SCENE_ASSEMBLY = 5,
        DXGK_ENGINE_TYPE_COPY = 6,
        DXGK_ENGINE_TYPE_OVERLAY = 7,
        DXGK_ENGINE_TYPE_CRYPTO,
        DXGK_ENGINE_TYPE_MAX
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DXGK_NODEMETADATA_FLAGS
    {
        public uint Value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct DXGK_NODEMETADATA
    {
        public DXGK_ENGINE_TYPE EngineType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FriendlyName;

        public DXGK_NODEMETADATA_FLAGS Flags;
        public byte GpuMmuSupported;
        public byte IoMmuSupported;
    }
}
