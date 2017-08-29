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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.StreamReader readerBase = null;
            System.IO.StreamReader readerNew = null;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                readerBase = new System.IO.StreamReader(openFileDialog1.FileName);
            }

            /*
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                readerNew = new System.IO.StreamReader(openFileDialog2.FileName);
            }
            */

            string BaseLine = readerBase.ReadLine();
            if (BaseLine != null)
            {
                string[] headerBase = BaseLine.Split(';');

                if (headerBase != null && headerBase.Length > 0)
                {
                    dataGridView1.Rows.Add(headerBase);
                }
            }

            
            /*
            string NewLine = readerNew.ReadLine();
            if (BaseLine != null)
            {

            }

            while (true)
            {
                string line = readerBase.ReadLine();
                if (line == null)
                {
                    break;
                }

                MessageBox.Show(line, "My Application",MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

                Console.WriteLine(line); // Use line.
            }*/
        }
        
    }
}
