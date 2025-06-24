using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Utilities
{

    public static class SystemTools
    {

        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        private struct RECT
        {
            public int left, top, right, bottom;
        }

        private const int ABM_GETTASKBARPOS = 5;

        [DllImport("shell32.dll")]
        private static extern IntPtr SHAppBarMessage(int msg, ref APPBARDATA data);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public static Rectangle GetTaskbarPosition()
        {
            var data = new APPBARDATA();
            data.cbSize = Marshal.SizeOf(data);

            var retval = SHAppBarMessage(ABM_GETTASKBARPOS, ref data);
            if (retval == IntPtr.Zero)
            {
                throw new Win32Exception("Please re-install Windows");
            }

            return new Rectangle(data.rc.left, data.rc.top, data.rc.right - data.rc.left, data.rc.bottom - data.rc.top);
        }

        public static Color GetColourAt(Point location)
        {
            using (var screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
            using (var gdest = Graphics.FromImage(screenPixel))
            {
                using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    var hSrcDC = gsrc.GetHdc();
                    var hDC = gdest.GetHdc();
                    var retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }

                return screenPixel.GetPixel(0, 0);
            }
        }

        public static Color GetTaskbarColor()
        {
            var taskBarColour = GetColourAt(GetTaskbarPosition().Location);
            return taskBarColour;
        }
    }
}
