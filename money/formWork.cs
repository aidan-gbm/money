using System;
using System.Data;
using System.Windows.Forms;

namespace money
{
    public partial class formWork : Form
    {
        private DataManager dm;

        private DataTable dt;

        public formWork(DataManager dataManager)
        {
            InitializeComponent();
            dm = dataManager;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (dt != null)
            {
                dm.saveData(dt, "data.csv");
                labelStatus.Text = "Saved!";
            }

            else
            {
                labelStatus.Text = "Must Load Data First!";
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openStatementFile.Filter = "CSV|*.csv";
            if (openStatementFile.ShowDialog() == DialogResult.OK)
            {
                dt = dm.loadCSV(openStatementFile.FileName);
                if (dt != null)
                {
                    dataGridView1.DataSource = dm.processData(dt);
                    labelStatus.Text = "Loaded " + openStatementFile.FileName;
                }
                
                else
                {
                    labelStatus.Text = "Error Loading Statement";
                }
            }
        }
    }
}
