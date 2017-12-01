using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using HandRecognition.Core.NetworkModels;
using HandRecognition.Core.Helpers;
using System.Drawing;
using System.Threading.Tasks;

namespace HandRecognition.Core
{
    public class HandRecognizer
    {
        #region -- Properties --
        private Network network;

        #endregion

        #region -- I/O Handling --
        // This class needs a previously trained neural network, since the file to read is large
        // it is compressed in GZip format.
        public HandRecognizer(string compressedFilePath, ref double progress, ref ProgressState progressState)
        {
            using (FileStream compressedFile = File.OpenRead(compressedFilePath))
            {
                using (GZipStream decompressionStream = new GZipStream(compressedFile, CompressionMode.Decompress))
                {
                    using (StreamReader streamReader = new StreamReader(decompressionStream))
                    {
                        string json = streamReader.ReadToEnd();
                        network = ImportHelper.ImportNetwork(json, ref progress, ref progressState);
                    }
                }

            }
        }

        #endregion

        #region -- Image recognition --
        public double RecognizeImage(Bitmap bitmap)
        {
            Bitmap resized = ImageProcessing.ResizeImage(bitmap);

            // get the blue channel only, so the skin of the hand is darker
            double[] values = ImageProcessing.GetGrayscaleBlueValues(resized);

            return network.Compute(values)[0];
        }

        public double RecognizeImage(byte[] bitmapArray, int width, int height)
        {
            return RecognizeImage(ImageProcessing.GetBitmap(bitmapArray, width, height));
        }
        #endregion
    }
}
