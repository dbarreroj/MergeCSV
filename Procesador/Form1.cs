using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Procesador
{
    public partial class Form1 : Form
    {
        private Doc MainDoc = null;
        private Doc AuxDoc = null;

        public Form1()
        {
            InitializeComponent();
        }

        public class Doc
        {
            public string pathFile = null;

            public Dictionary<int, string> header = null;
            public List<Dictionary<string, string>> Data = null;
            public Dictionary<string, string> DataRow = null;

            public Doc (string path)
            {
                pathFile = path;
            }

            public void addHeader(string headerVal)
            {
                if (header == null)
                {
                    header = new Dictionary<int, string>();
                }

                header.Add(header.Count, headerVal);
            }
            
            public void addRow(string RowVal, string RowHeader, bool newRow)
            {
                if (DataRow == null)
                {
                    DataRow = new Dictionary<string, string>();
                }

                DataRow.Add(RowHeader, RowVal);

                if (newRow)
                {
                    if (Data == null)
                        Data = new List<Dictionary<string, string>>();

                    Data.Add(DataRow);
                    DataRow = new Dictionary<string, string>();
                }                
            }
            
            public void addLineInFile(string val)
            {
                if (pathFile != null)
                {
                    System.IO.StreamWriter writerFile = new System.IO.StreamWriter(pathFile, true);

                    writerFile.WriteLine(val);

                    writerFile.Close();
                }
            }

            public void readHeader()
            {
                if (pathFile != null)
                {
                    System.IO.StreamReader readerFile = new System.IO.StreamReader(pathFile);

                    string headerStr = readerFile.ReadLine();

                    string[] headerItems = headerStr.Split(';');

                    for(int i = 0; i < headerItems.Length; i++)
                    {
                        addHeader(headerItems[i]);
                    }

                    readerFile.Close();
                }
            }

            public List<Dictionary<string, string>> readFile(bool skipHeader)
            {
                if (pathFile != null)
                {
                    System.IO.StreamReader readerFile = new System.IO.StreamReader(pathFile);

                    if (skipHeader)
                        readerFile.ReadLine();

                    while (!readerFile.EndOfStream)
                    {
                        string DataStr = readerFile.ReadLine();

                        if (DataStr != null && DataStr != "")
                        {
                            string[] DataItems = DataStr.Split(';');

                            for (int i = 0; i < DataItems.Length; i++)
                            {
                                if(i == DataItems.Length-1)
                                { addRow(DataItems[i], header[i], true); }
                                else
                                { addRow(DataItems[i], header[i], false); }
                            }
                        }
                    }

                    readerFile.Close();
                }

                return Data;
            }
        }

        private void StartProcess(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            
            if (MainDoc != null && AuxDoc != null)
            {
                MainDoc.readHeader();
                AuxDoc.readHeader();

                if(MainDoc.header != null && AuxDoc.header != null && MainDoc.header.Count > 0 && AuxDoc.header.Count > 0)
                {
                    for (int i = 0; i < MainDoc.header.Count; i++)
                    {
                        string header = "";

                        MainDoc.header.TryGetValue(i, out header);

                        dataGridView1.Columns.Add(header, header);

                        DataGridViewComboBoxCell options = new DataGridViewComboBoxCell();

                        for (int j = 0; j < AuxDoc.header.Count; j++)
                        {
                            string AuxHeader = "";

                            AuxDoc.header.TryGetValue(j, out AuxHeader);

                            options.Items.Add(AuxHeader);
                        }
                        
                        dataGridView1.Rows[0].Cells[i] = options;
                    }

                }
            }
        }
                
        private void AddMainFile(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TxtMainFile.Text = openFileDialog1.FileName;
                MainDoc = new Doc(openFileDialog1.FileName);
            }
        }

        private void AddFileToMerge(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TxtFileToMerge.Text = openFileDialog1.FileName;
                AuxDoc = new Doc(openFileDialog1.FileName);
            }
        }

        private void CreateFile(object sender, EventArgs e)
        {
            List<string> Order = new List<string>();

            for (int i = 0; i < MainDoc.header.Count; i++)
            {
                try
                {
                    Order.Add(dataGridView1.Rows[0].Cells[i].Value.ToString());
                }
                catch
                {
                    Order.Add("");
                }
                
            }

            List<Dictionary<string, string>> DataAuxDoc = AuxDoc.readFile(true);
            
            for (int i = 0; i < DataAuxDoc.Count; i++)
            {
                string LineToAdd = "";

                for (int j = 0; j < MainDoc.header.Count; j++)
                {
                    string valToAdd = "";
                    DataAuxDoc[i].TryGetValue(Order[j], out valToAdd);

                    if (valToAdd == null || valToAdd == "")
                        valToAdd = "S/D";
                    
                    LineToAdd += valToAdd + ";";
                }

                MainDoc.addLineInFile(LineToAdd);
            }
        }
    }
}
