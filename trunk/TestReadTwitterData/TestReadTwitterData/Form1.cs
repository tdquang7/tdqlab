﻿using System;
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
                string vertexPath = @"E:\Lab\Triangles data\vertexList.txt";

                StreamReader reader = new StreamReader(inputFile);
                StreamWriter xadjWriter = new StreamWriter(xadjPath);
                StreamWriter adjncyWriter = new StreamWriter(adjncyPath);
                StreamWriter infoWriter = new StreamWriter(infoPath);
                StreamWriter vertexWriter = new StreamWriter(vertexPath);

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
                int actualEdges = edgesNumber;

                for (int i = 0; i < edgesNumber; i++)
                {
                    line = reader.ReadLine(); // Format: SourceId \t DestId
                    parts = line.Split(new string[] { TAB }, StringSplitOptions.None);
                    string id = parts[0];
                    string destId = parts[1];

                    if (id != lastId) // Change id occurs
                    {
                        if (lastId == "")
                        {
                            lastId = "0"; 
                        }

                        int left = int.Parse(lastId);
                        int right = int.Parse(id);

                        if ((right - left) > 1) // There are vertices skipped
                        {
                            xadjWriter.WriteLine(rightbound); // Close previous   
                            rightbound++;

                            for (int j = left + 1; j < right; j++)
                            {
                                xadjWriter.WriteLine(rightbound);
                                adjncyWriter.WriteLine(j);
                                rightbound++;
                            }

                            // We have added some edges so need to update actual edges
                            actualEdges += right - left - 1; 
                        }
                        else
                        {
                            xadjWriter.WriteLine(rightbound);                      
                        }

                        lastId = id;                        
                    }

                    adjncyWriter.WriteLine(destId);
                    rightbound++;
                }

                xadjWriter.WriteLine(rightbound);

                infoWriter.WriteLine(nodesNumber);
                infoWriter.WriteLine(actualEdges);
               
                reader.Close();
                xadjWriter.Close();
                adjncyWriter.Close();
                infoWriter.Close();
                vertexWriter.Close();
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
            while (!reader.EndOfStream)
            {
                reader.ReadLine();
                count++;
            }
            reader.Close();
        }

    }
}
