using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace money
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataManager dataManager = new DataManager();

            formEntry entry = new formEntry(dataManager);
            Application.Run(entry);

            formWork work = new formWork(dataManager);
            Application.Run(work);
        }
    }
}
