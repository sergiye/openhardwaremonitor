using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OpenHardwareMonitor.Utilities {

  public static class IconFactory {

    private struct BitmapInfoHeader {
      public readonly uint Size;
      public readonly int Width;
      public readonly int Height;
      public readonly ushort Planes;
      public readonly ushort BitCount;
      public readonly uint Compression;
      public readonly uint SizeImage;
      public readonly int XPelsPerMeter;
      public readonly int YPelsPerMeter;
      public readonly uint ClrUsed;
      public readonly uint ClrImportant;

      public BitmapInfoHeader(int width, int height, int bitCount) {
        Size = 40;
        Width = width;
        Height = height;
        Planes = 1;
        BitCount = (ushort)bitCount;
        Compression = 0;
        SizeImage = 0;
        XPelsPerMeter = 0;
        YPelsPerMeter = 0;
        ClrUsed = 0;
        ClrImportant = 0;
      }

      public void Write(BinaryWriter bw) {
        bw.Write(Size);
			  bw.Write(Width);
			  bw.Write(Height);
			  bw.Write(Planes);
			  bw.Write(BitCount);
			  bw.Write(Compression);
			  bw.Write(SizeImage);
			  bw.Write(XPelsPerMeter);
			  bw.Write(YPelsPerMeter);
			  bw.Write(ClrUsed);
			  bw.Write(ClrImportant);
      }
    }

    private struct IconImage {
      public BitmapInfoHeader Header;
      public readonly byte[] Colors;
      public readonly int MaskSize;

      public IconImage(int width, int height, byte[] colors) {
        Header = new BitmapInfoHeader(width, height << 1,
          (8 * colors.Length) / (width * height));
        Colors = colors;
        MaskSize = (width * height) >> 3;
      }

      public void Write(BinaryWriter bw) {
        Header.Write(bw);
        var stride = Header.Width << 2;
        for (var i = (Header.Height >> 1) - 1; i >= 0; i--)
          bw.Write(Colors, i * stride, stride);
        for (var i = 0; i < 2 * MaskSize; i++)
          bw.Write((byte)0);
      }
    }

    private struct IconDirEntry {
      public readonly byte Width;
      public readonly byte Height;
      public readonly byte ColorCount;
      public readonly byte Reserved;
      public readonly ushort Planes;
      public readonly ushort BitCount;
      public readonly uint BytesInRes;
      public uint ImageOffset;

      public IconDirEntry(IconImage image, int imageOffset) {
        Width = (byte)image.Header.Width;
        Height = (byte)(image.Header.Height >> 1);
        ColorCount = 0;
        Reserved = 0;
        Planes = image.Header.Planes;
        BitCount = image.Header.BitCount;
        BytesInRes = (uint)(image.Header.Size +
          image.Colors.Length + image.MaskSize + image.MaskSize);
        ImageOffset = (uint)imageOffset;
      }

      public void Write(BinaryWriter bw) {
        bw.Write(Width);
        bw.Write(Height);
        bw.Write(ColorCount);
        bw.Write(Reserved);
        bw.Write(Planes);
        bw.Write(BitCount);
        bw.Write(BytesInRes);
        bw.Write(ImageOffset);
      }

      public uint Size => 16;
    }

    private readonly struct IconDir {
      public readonly ushort Reserved;
      public readonly ushort Type;
      public readonly ushort Count;
      public readonly IconDirEntry[] Entries;

      public IconDir(IconDirEntry[] entries) {
        Reserved = 0;
        Type = 1;
        Count = (ushort)entries.Length;
        Entries = entries;
      }

      public void Write(BinaryWriter bw) {
        bw.Write(Reserved);
        bw.Write(Type);
        bw.Write(Count);
        for (var i = 0; i < Entries.Length; i++)
          Entries[i].Write(bw);
      }

      public uint Size => (uint)(6 + Entries.Length * (Entries.Length > 0 ? Entries[0].Size : 0));
    }

    private static readonly BinaryWriter binaryWriter =
      new BinaryWriter(new MemoryStream());

    public static Icon Create(byte[] colors, int width, int height, PixelFormat format) {
      if (format != PixelFormat.Format32bppArgb)
        throw new NotImplementedException();

      var image = new IconImage(width, height, colors);
      var dir = new IconDir(new[] { new IconDirEntry(image, 0) } );
      dir.Entries[0].ImageOffset = dir.Size;

      binaryWriter.BaseStream.Position = 0;
			dir.Write(binaryWriter);
      image.Write(binaryWriter);

			binaryWriter.BaseStream.Position = 0;
      var icon = new Icon(binaryWriter.BaseStream);
      return icon;
    }
  }
}
