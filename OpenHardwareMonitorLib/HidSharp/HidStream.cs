using System;
using System.Runtime.InteropServices;

#pragma warning disable 420

namespace HidSharp
{
    /// <summary>
    /// Communicates with a USB HID class device.
    /// </summary>
    [ComVisible(true), Guid("0C263D05-0D58-4c6c-AEA7-EB9E0C5338A2")]
    public abstract class HidStream : DeviceStream
    {
        /// <exclude/>
        protected HidStream(HidDevice device)
            : base(device)
        {
            ReadTimeout = 3000;
            WriteTimeout = 3000;
        }

        /// <exclude />
        public override void Flush()
        {

        }

        /// <summary>
        /// Sends a Get Feature setup request.
        /// </summary>
        /// <param name="buffer">The buffer to fill. Place the Report ID in the first byte.</param>
        public void GetFeature(byte[] buffer)
        {
            Throw.If.Null(buffer, "buffer");
            GetFeature(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Sends a Get Feature setup request.
        /// </summary>
        /// <param name="buffer">The buffer to fill. Place the Report ID in the byte at index <paramref name="offset"/>.</param>
        /// <param name="offset">The index in the buffer to begin filling with data.</param>
        /// <param name="count">The number of bytes in the feature request.</param>
        public abstract void GetFeature(byte[] buffer, int offset, int count);

        /// <summary>
        /// Reads HID Input Reports.
        /// </summary>
        /// <returns>The data read.</returns>
        public byte[] Read()
        {
            byte[] buffer = new byte[Device.GetMaxInputReportLength()];
            int bytes = Read(buffer); Array.Resize(ref buffer, bytes);
            return buffer;
        }

        /// <summary>
        /// Reads HID Input Reports.
        /// </summary>
        /// <param name="buffer">The buffer to place the reports into.</param>
        /// <returns>The number of bytes read.</returns>
        public int Read(byte[] buffer)
        {
            Throw.If.Null(buffer, "buffer");
            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Sends a Set Feature setup request.
        /// </summary>
        /// <param name="buffer">The buffer of data to send. Place the Report ID in the first byte.</param>
        public void SetFeature(byte[] buffer)
        {
            Throw.If.Null(buffer, "buffer");
            SetFeature(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Sends a Set Feature setup request.
        /// </summary>
        /// <param name="buffer">The buffer of data to send. Place the Report ID in the byte at index <paramref name="offset"/>.</param>
        /// <param name="offset">The index in the buffer to start the write from.</param>
        /// <param name="count">The number of bytes in the feature request.</param>
        public abstract void SetFeature(byte[] buffer, int offset, int count);

        /// <summary>
        /// Writes an HID Output Report to the device.
        /// </summary>
        /// <param name="buffer">The buffer containing the report. Place the Report ID in the first byte.</param>
        public void Write(byte[] buffer)
        {
            Throw.If.Null(buffer, "buffer");
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Gets the <see cref="HidDevice"/> associated with this stream.
        /// </summary>
        public new HidDevice Device
        {
            get { return (HidDevice)base.Device; }
        }
    }
}
