using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HandRecognition.WindowsApp.Helpers
{
    class ImageProcessingWindows
    {
        #region -- Core image handling --
        // Gets the raw image as a single dimension array in BGRA (32 bits per pixel or 8 bits per channel)
        public static byte[] GetBitmapArray(Bitmap bitmap)
        {
            bitmap = new Bitmap(bitmap);
            int height = bitmap.Height;
            int width = bitmap.Width;
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            byte[] bitmapArray = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, bitmapArray, 0, bitmapArray.Length);
            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return bitmapArray;
        }

        // Converts a BGRA array to an square bitmap
        public static Bitmap GetBitmap(byte[] bitmapArray, int resolutionSquare)
        {
            Bitmap bitmap = new Bitmap(resolutionSquare, resolutionSquare);
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, resolutionSquare, resolutionSquare),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            Marshal.Copy(bitmapArray, 0, data.Scan0, resolutionSquare * resolutionSquare * 4);
            bitmap.UnlockBits(data);

            return bitmap;
        }

        // Converts a BGRA array to an square bitmap
        public static Bitmap GetBitmap(byte[] bitmapArray, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            Marshal.Copy(bitmapArray, 0, data.Scan0, width * height * 4);
            bitmap.UnlockBits(data);

            return bitmap;
        }
        #endregion

        #region -- Visualization helpers --
        public static Bitmap GetGrayscaleBlue(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            byte[] bitmapArray = GetBitmapArray(bitmap);

            for (int i = 0; i < height * width * 4; i = i + 4)
            {
                bitmapArray[i] = bitmapArray[i];
                bitmapArray[i + 1] = bitmapArray[i];
                bitmapArray[i + 2] = bitmapArray[i];
            }

            return GetBitmap(bitmapArray, width, height);
        }

        public static Bitmap GetBlueChannel(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            byte[] bitmapArray = GetBitmapArray(bitmap);

            for (int i = 0; i < height * width * 4; i = i + 4)
            {
                bitmapArray[i] = bitmapArray[i];
                bitmapArray[i + 1] = 0;
                bitmapArray[i + 2] = 0;
            }

            return GetBitmap(bitmapArray, width, height);
        }

        #endregion
    }
}
