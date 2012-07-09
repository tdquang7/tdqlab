using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestReadTwitterData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundGenerateParMetisInput);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnGenerate.Enabled = true;
            btnCancel.Enabled = false;
            MessageBox.Show("Completed");
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }


        /// <summary>
        /// Transform input, add missing edges
        /// </summary>
        void TransformInput(object sender, DoWorkEventArgs e)
        {
            const string SPACE = " ";
            const string TAB = "\t";
            string inputPath = @"E:\Lab\Triangles data\soc-LiveJournal1.txt";            
            string newInputPath = @"E:\Lab\Triangles data\soc-LiveJournal1_new.txt";
            StreamReader reader = new StreamReader(inputPath);
            StreamWriter writer = new StreamWriter(newInputPath);

            // Header
            string line;

            // First two lines kept for debug info
            line = reader.ReadLine(); 
            line = reader.ReadLine(); 

            // Third line show how many nodes and edges
            line = reader.ReadLine(); // Formar: "# Nodes: x Edges: y". Need x and y

            // Extract x and y                
            string[] parts = line.Split(new string[] { SPACE }, StringSplitOptions.None);
            int nodesNumber = int.Parse(parts[2]);
            int edgesNumber = int.Parse(parts[4]);

            line = reader.ReadLine(); // Forth line just the table header

            string lastId = "";
            int actualEdges = edgesNumber;

            for (int i = 0; i < edgesNumber; i++)
            {
                line = reader.ReadLine(); // Format: SourceId \t DestId
                writer.WriteLine(line);

                parts = line.Split(new string[] { TAB }, StringSplitOptions.None);
                string id = parts[0];
                string destId = parts[1];
                

                if (lastId != id)
                {
                    if (lastId == "")
                    {
                        lastId = "0";
                    }

                    int left = int.Parse(lastId);
                    int right = int.Parse(id);

                    if ((right - left) > 1) // There are vertices skipped
                    {
                        for (int j = left + 1; j < right; j++)
                        {
                            writer.WriteLine(j + TAB + j);
                        }

                        actualEdges += right - left - 1;
                    }

                    lastId = id;
                }
            }            

            reader.Close();
            writer.Close();

            string infoPath = @"E:\Lab\Triangles data\info.txt";
            StreamWriter infoWriter = new StreamWriter(infoPath);

            infoWriter.WriteLine(nodesNumber);
            infoWriter.WriteLine(actualEdges);

            infoWriter.Close();
        }


        void backgroundGenerateParMetisInput(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending)
                e.Cancel = true;
            else
            {
                const string SPACE = " ";
                const string TAB = "\t";

                string inputFile = @"E:\Lab\Triangles data\soc-LiveJournal1_New.txt";
                string xadjPath = @"E:\Lab\Triangles data\xadj.txt";
                string adjncyPath = @"E:\Lab\Triangles data\ajdncy.txt";
                string infoPath = @"E:\Lab\Triangles data\info.txt";

                StreamReader reader = new StreamReader(inputFile);
                StreamWriter xadjWriter = new StreamWriter(xadjPath);
                StreamWriter adjncyWriter = new StreamWriter(adjncyPath);

                
                StreamReader infoReader = new StreamReader(infoPath);
                int nodesNumber = int.Parse(infoReader.ReadLine());
                int edgesNumber = int.Parse(infoReader.ReadLine());

                string line;
                
                string lastId = "0";
                xadjWriter.WriteLine(0);

                int rightbound = 0;
                
                for (int i = 0; i < edgesNumber; i++)
                {
                    line = reader.ReadLine(); // Format: SourceId \t DestId
                    string[] parts = line.Split(new string[] { TAB }, StringSplitOptions.None);
                    string id = parts[0];
                    string destId = parts[1];

                    if (id != lastId) // Change id occurs
                    {
                        xadjWriter.WriteLine(rightbound);
                    }
                   
                    adjncyWriter.WriteLine(destId);
                    rightbound++;
                }

                xadjWriter.WriteLine(rightbound);
               
                reader.Close();
                xadjWriter.Close();
                adjncyWriter.Close();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;
            btnCancel.Enabled = true;

            backgroundWorker1.RunWorkerAsync();            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
                backgroundWorker1.CancelAsync();
        }

        private void btnCheckInfo_Click(object sender, EventArgs e)
        {
            //string xadjPath = @"E:\Lab\Triangles data\xadj.txt";
            //StreamReader reader = new StreamReader(xadjPath);

            //string s = reader.ReadToEnd();
            //string[] parts = s.Split(new string[] { " " }, StringSplitOptions.None);

            //reader.Close();
            //reader.Dispose();

            //bool b = 6 - 4 > 1;

            string adjncyPath = @"E:\Lab\Triangles data\ajdncy.txt";
            StreamReader reader = new StreamReader(adjncyPath);

            int count = 0;
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 1000; i++)
            {
                builder.Append(reader.ReadLine() + "\r\n");
            }

            txtContent.Text = builder.ToString();

            reader.Close();
        }

    }
}
