using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace money
{
    public class DataManager
    {
        public bool isInitialized { get; private set; }
        public string dataPath { get; private set; }

        public DataManager()
        {
            isInitialized = false;
        }

        public void initialize(FolderBrowserDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                dataPath = dialog.SelectedPath;
                isInitialized = true;
            }
        }

        public void saveData(DataTable data, string fileName)
        {
            string csvData = string.Empty;

            foreach (DataColumn col in data.Columns)
            {
                csvData += col.ColumnName + ',';
            }

            csvData += "\r\n";

            foreach (DataRow row in data.Rows)
            {
                foreach (object cell in row.ItemArray)
                {
                    csvData += cell.ToString().Replace(",", "{;}") + ",";
                }

                csvData += "\r\n";
            }

            File.WriteAllText(dataPath + "\\" + fileName, csvData);
        }

        public DataTable loadCSV(string fileName)
        {
            DataTable dt = new DataTable(Path.GetFileName(fileName));
            try
            {
                OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + Path.GetDirectoryName(fileName) + "\";Extended Properties='text;HDR=yes;FMT=Delimited(,)';");
                OleDbCommand cmd = new OleDbCommand(string.Format("select * from [{0}]", new FileInfo(fileName).Name), conn);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
                return dt;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                return null;
            }
        }

        /*
         * Processes raw data into predefined columns:
         * date, description, amount, reference
         */
        public DataTable processData(DataTable data)
        {
            DataTable cleanData = new DataTable();

            int[] indexes = { -1, -1, -1, -1 };
            foreach (DataColumn col in data.Columns)
            {
                switch (col.ColumnName)
                {
                    case "Date":
                        indexes[0] = col.Ordinal;
                        DataColumn dateCol = new DataColumn("date");
                        dateCol.DataType = Type.GetType("System.DateTime");
                        cleanData.Columns.Add(dateCol);
                        break;
                    case "Description":
                        indexes[1] = col.Ordinal;
                        DataColumn descCol = new DataColumn("description");
                        descCol.DataType = Type.GetType("System.String");
                        cleanData.Columns.Add(descCol);
                        break;
                    case "Amount":
                        indexes[2] = col.Ordinal;
                        DataColumn amntCol = new DataColumn("amount");
                        amntCol.DataType = Type.GetType("System.Double");
                        cleanData.Columns.Add(amntCol);
                        break;
                    case "Reference":
                        indexes[3] = col.Ordinal;
                        DataColumn refCol = new DataColumn("reference");
                        refCol.DataType = Type.GetType("System.Int64");
                        cleanData.Columns.Add(refCol);
                        break;
                    default:
                        break;
                }
            }

            foreach (DataRow row in data.Rows)
            {
                DataRow newRow = cleanData.NewRow();
                if (indexes[0] != -1)
                {
                    newRow["date"] = DateTime.Parse(row.ItemArray[indexes[0]].ToString());
                }

                if (indexes[1] != -1)
                {
                    newRow["description"] = row.ItemArray[indexes[1]].ToString();
                }

                if (indexes[2] != -1)
                {
                    newRow["amount"] = Double.Parse(row.ItemArray[indexes[2]].ToString());
                }

                if (indexes[3] != -1)
                {
                    newRow["reference"] = Int64.Parse(row.ItemArray[indexes[3]].ToString().Split('\'')[1]);
                }

                cleanData.Rows.Add(newRow);
            }

            return cleanData;
        }
    }
}
