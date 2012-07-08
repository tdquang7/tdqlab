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

        void backgroundGenerateParMetisInput(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending)
                e.Cancel = true;
            else
            {
                string inputFile = @"E:\Lab\Triangles data\soc-LiveJournal1.txt";
                string xadjPath = @"E:\Lab\Triangles data\xadj.txt";
                string adjncyPath = @"E:\Lab\Triangles data\ajdncy.txt";
                string infoPath = @"E:\Lab\Triangles data\info.txt";

                StreamReader reader = new StreamReader(inputFile);
                StreamWriter xadjWriter = new StreamWriter(xadjPath);
                StreamWriter adjncyWriter = new StreamWriter(adjncyPath);
                StreamWriter infoWriter = new StreamWriter(infoPath);

                // Header
                string line;

                // First two lines kept for debug info
                line = reader.ReadLine();
                line = reader.ReadLine();

                // Third line show how many nodes and edges
                line = reader.ReadLine(); // Formar: "# Nodes: x Edges: y". Need x and y

                string SPACE = " ";
                string TAB = "\t";

                // Extract x and y                
                string[] parts = line.Split(new string[] { SPACE }, StringSplitOptions.None);
                int nodesNumber = int.Parse(parts[2]);
                int edgesNumber = int.Parse(parts[4]);
                

                line = reader.ReadLine(); // Forth line just the table header
                                
                string lastId = "";
                int rightbound = 0;
                int count = 0; // Counting to ensure generate equals to nodesNumber

                for (int i = 0; i < edgesNumber; i++)
                {
                    line = reader.ReadLine(); // Format: SourceId \t DestId
                    parts = line.Split(new string[] { TAB }, StringSplitOptions.None);
                    string id = parts[0];
                    string destId = parts[1];                   

                    if (id != lastId) // Change id occurs
                    {
                        if (lastId == "") lastId = "0"; // Lazy, short style

                        int left = int.Parse(lastId);
                        int right = int.Parse(id);

                        if (right - left > 1) // There are vertices skipped
                        {
                            xadjWriter.Write(rightbound + " "); // Close previous                            
                            rightbound++;

                            for (int j = left + 1; j < right; j++)
                            {
                                xadjWriter.Write(rightbound + " ");                               

                                adjncyWriter.Write(j + " ");
                                rightbound++;
                            }                            
                        }

                        lastId = id;
                        xadjWriter.Write(rightbound + " "); 
                        count+= right - left;
                    }

                    adjncyWriter.Write(destId + " ");
                    rightbound++;
                }

                // The rest of nodes, not check yet
                infoWriter.WriteLine(nodesNumber);
                infoWriter.WriteLine(edgesNumber);
               
                reader.Close();
                xadjWriter.Close();
                adjncyWriter.Close();
                infoWriter.Close();

                reader.Dispose();
                xadjWriter.Dispose();
                adjncyWriter.Dispose();
                infoWriter.Dispose();
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

    }
}
