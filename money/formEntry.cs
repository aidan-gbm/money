using System;
using System.Windows.Forms;

namespace money
{
    public partial class formEntry : Form
    {
        private DataManager dataManager;
        public bool isRunning;

        public formEntry(DataManager dm)
        {
            InitializeComponent();
            isRunning = true;
            dataManager = dm;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            dataManager.initialize(startFolderDialog);
            isRunning = false;
            this.Close();
        }
    }
}
