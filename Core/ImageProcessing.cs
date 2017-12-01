using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace HandRecognition.Core
{
    // .NET Drawing (GDI+) libraries handle raw images pixels in BGRA order (Blue, Green, Red and Alpha)
    public class ImageProcessing
    {
        #region -- Image I/O handling --
        public static byte[] ReadImage(string path)
        {
            return GetBitmapArray(new Bitmap(path));            
        }

        #endregion
        
        #region -- Core image handling --
        // Gets the raw image as a single dimension array in BGRA (32 bits per pixel or 8 bits per channel)
        public static byte[] GetBitmapArray(Bitmap bitmap)
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
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


        #region -- Core functionality --
        // Gets the blue channel from a BGRA array and converts it to a grayscale array,
        // where each double value goes from 0 (black) to 1 (white).
        public static double[] GetGrayscaleBlue(byte[] bitmapArray)
        {
            double[] grayscaleBlue = new double[bitmapArray.Length / 4];

            int j = 0;
            for (int i = 0; i < bitmapArray.Length; i = i + 4)
            {
                grayscaleBlue[j] = bitmapArray[i] / 255.0;
                j++;
            }

            return grayscaleBlue;
        }

        public static double[] GetGrayscaleBlueValues(Bitmap bitmap)
        {
            byte[] bitmapArray = GetBitmapArray(bitmap);

            double[] grayscaleBlue = new double[bitmapArray.Length / 4];

            int j = 0;
            for (int i = 0; i < bitmapArray.Length; i = i + 4)
            {
                grayscaleBlue[j] = bitmapArray[i] / 255.0;
                j++;
            }

            return grayscaleBlue;
        }

        // Resizes the image using the bicubic algorithm
        public static Bitmap ResizeImage(Bitmap bitmap, int width = 67, int height = 50)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.Bicubic;
                g.DrawImage(bitmap, 0, 0, width, height);
            }
            return result;
        }

        #endregion
    }
}
