
namespace HandRecognition.WindowsApp
{
    using System;
    using System.Windows.Forms;
    using System.Linq;

    using WebEye.Controls.WinForms.WebCameraControl;

    public partial class CameraForm : Form
    {
        public CameraForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The helper class for a combo box item.
        /// </summary>
        private class ComboBoxItem
        {
            public ComboBoxItem(WebCameraId id)
            {
                _id = id;
            }

            private readonly WebCameraId _id;
            public WebCameraId Id
            {
                get { return _id; }
            }

            public override string ToString()
            {
                // Generates the text shown in the combo box.
                return _id.Name;
            }
        }

        private void CameraForm_Load(object sender, EventArgs e)
        {
            var cameras = webCameraControl1.GetVideoCaptureDevices();
            foreach (WebCameraId camera in cameras)
            {
                comboBox1.Items.Add(new ComboBoxItem(camera));
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedItem = comboBox1.Items[0];
                /*webCameraControl1.StartCapture(cameras.ToArray()[0]);
                UpdateButtons();*/
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            ComboBoxItem i = (ComboBoxItem)comboBox1.SelectedItem;

            try
            {
                webCameraControl1.StartCapture(i.Id);
            }
            finally
            {
                UpdateButtons();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            webCameraControl1.StopCapture();
            UpdateButtons();
        }

        private void imageButton_Click(object sender, EventArgs e)
        {
            ((Form1)this.Owner).WebcamCapture = webCameraControl1.GetCurrentImage();
            webCameraControl1.StopCapture();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            startButton.Enabled = comboBox1.SelectedItem != null;
            stopButton.Enabled = webCameraControl1.IsCapturing;
            imageButton.Enabled = webCameraControl1.IsCapturing;
        }
    }
}
