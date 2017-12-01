using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HandRecognition.Core;

namespace HandRecognition.WindowsApp
{
    public partial class Form1 : Form
    {
        private Bitmap mainPicture;

        internal HandRecognizer HandRecognizer { get; set; }

        public Image WebcamCapture
        {
            set
            {
                mainPicture = new Bitmap(value);
                pictureBox1.Image = value;
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new CameraForm().Show(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists("neuralnetwork.gz"))
            {
                MessageBox.Show("File \"neuralnetwork.gz\" not found.", "Error", MessageBoxButtons.OK);
                Application.Exit();
            }

            LoadingForm loadingForm = new LoadingForm(@"neuralnetwork.gz");
            if (loadingForm.ShowDialog(this) == DialogResult.Cancel)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckPathExists = true,
                Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp"
            };

            // Show open file dialog box 
            var result = dialog.ShowDialog();

            // Process open file dialog box results 
            if (result == DialogResult.OK)
            {
                // Open document 
                string filename = dialog.FileName;
                WebcamCapture = new Bitmap(filename);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mainPicture != null)
            {
                var bitmapArray = Helpers.ImageProcessingWindows.GetBitmapArray(mainPicture);
                double value = HandRecognizer.RecognizeImage(bitmapArray, mainPicture.Width, mainPicture.Height);
                pictureBox3.Image = Helpers.ImageProcessingWindows.GetGrayscaleBlue(mainPicture);
                pictureBox2.Image = Helpers.ImageProcessingWindows.GetBlueChannel(mainPicture);
                label1.Text = string.Format("Computed value: {0:P2}", value);
            }
            
        }
    }
}
