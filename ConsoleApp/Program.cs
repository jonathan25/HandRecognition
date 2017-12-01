using HandRecognition.Core;
using HandRecognition.Core.Helpers;
using HandRecognition.Core.NetworkModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\MOHI-S1-P1-50");
            List<DataSet> datasets = new List<DataSet>();

            var hiddenSizes = new int[] { 100, 100 };
            var network = new Network(67 * 50, hiddenSizes, 1);

            Console.WriteLine("datasets");
            string[] noAcceptable =
            {
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no.jpg",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no2.fw.png",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no3.fw.png",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no4.fw.png",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no5.fw.png",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no6.fw.png",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no7.fw.png",
            };
            foreach (var file in noAcceptable)
            {
                var image = ImageProcessing.ReadImage(file);
                var vts = ImageProcessing.GetGrayscaleBlue(image);
                datasets.Add(new DataSet(vts, new double[] { 0.0 }));
            }
            foreach (var file in files)
            {
                var image = ImageProcessing.ReadImage(file);
                var vts = ImageProcessing.GetGrayscaleBlue(image);
                datasets.Add(new DataSet(vts, new double[] { 1.0 }));
            }

            Console.WriteLine("Training.........");
            network.Train(datasets, 0.015);
            datasets = new List<DataSet>();

            Console.WriteLine("Done, testing...");

            string[] testfiles =
            {
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no.jpg",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no2.fw.png",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\no3.fw.png",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\MOHI-S1-P1-50\S1-P3-F-42-1.jpg",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\other.jpg",
                @"C:\Users\Jonathan\Documents\visual studio 2017\Projects\HandRecognition\Core\Datasets\S1-P52-M-14-1.jpg"
            };

            foreach (var test in testfiles)
            {
                var imageTest = ImageProcessing.ReadImage(test);
                var vtsTest = ImageProcessing.GetGrayscaleBlue(imageTest);

                var results = network.Compute(vtsTest);
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
                Console.WriteLine("--");
            }
            

            Console.WriteLine("End.");
            Console.ReadLine();
            HandRecognition.Core.Helpers.ExportHelper.ExportNetwork(network, "ho.txt");
        }
    }
}
