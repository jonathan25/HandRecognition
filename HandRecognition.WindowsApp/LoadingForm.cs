using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandRecognition.WindowsApp
{
    public partial class LoadingForm : Form
    {
        private Core.HandRecognizer hr;
        private double progress = 0;
        private Core.Helpers.ProgressState progressState = Core.Helpers.ProgressState.Decompressing;
        private string path;
        private Task progressChecker;

        public LoadingForm(string path)
        {
            this.path = path;
            InitializeComponent();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            Task.Run(() => Loader());
        }

        private async Task CheckProgress()
        {
            while (progressState != Core.Helpers.ProgressState.Done)
            {
                UpdateProgressBar((int)Math.Round(progress * 100));

                if (progressState == Core.Helpers.ProgressState.Decompressing)
                {
                    UpdateLabel("Decompressing file...");
                }
                else if (progressState == Core.Helpers.ProgressState.InputLayer)
                {
                    UpdateLabel("Loading input layer...");
                }
                else if (progressState == Core.Helpers.ProgressState.HiddenLayers)
                {
                    UpdateLabel("Loading hidden layer...");
                }
                else if (progressState == Core.Helpers.ProgressState.OutputLayer)
                {
                    UpdateLabel("Loading output layer...");
                }
                else if (progressState == Core.Helpers.ProgressState.Synapses)
                {
                    UpdateLabel("Loading synapses...");
                }
                else if ((progressState == Core.Helpers.ProgressState.Done))
                {
                    UpdateLabel("Done.");
                    return;
                }

                await Task.Delay(250);
            }

        }

        public void UpdateLabel(string text)
        {
            if (label2.InvokeRequired)
            {
                label2.Invoke(new Action<string>(UpdateLabel), text);
                return;
            }
            label2.Text = text;
        }

        private void UpdateProgressBar(int progress)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action<int>(UpdateProgressBar), progress);
                return;
            }

            progressBar1.Value = progress;
        }

        private async Task Loader()
        {
            try
            {
                progressChecker = Task.Run(() => CheckProgress());
                hr = new Core.HandRecognizer(path, ref progress, ref progressState);
                ((Form1)Owner).HandRecognizer = hr;
                await progressChecker;
                Finish();
            } catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(path);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        private void Finish()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Finish));
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
