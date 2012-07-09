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
        void TransformInput()
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

                writer.WriteLine(line);
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
                string vertexPath =  @"E:\Lab\Triangles data\vertex.txt";

                StreamReader reader = new StreamReader(inputFile);
                StreamWriter xadjWriter = new StreamWriter(xadjPath);
                StreamWriter adjncyWriter = new StreamWriter(adjncyPath);
                StreamWriter vertexWriter = new StreamWriter(vertexPath);
                
                StreamReader infoReader = new StreamReader(infoPath);
                int nodesNumber = int.Parse(infoReader.ReadLine());
                int edgesNumber = int.Parse(infoReader.ReadLine());

                string line;
                
                string lastId = "0";
                xadjWriter.WriteLine(0);
                vertexWriter.WriteLine("0");

                int rightbound = 0;
                
                for (int i = 0; i < edgesNumber; i++)
                {
                    line = reader.ReadLine(); // Format: SourceId \t DestId
                    string[] parts = line.Split(new string[] { TAB }, StringSplitOptions.None);
                    string id = parts[0];
                    string destId = parts[1];

                    if (lastId != id) // Change id occurs
                    {
                        xadjWriter.WriteLine(rightbound);
                        vertexWriter.WriteLine(id);
                        lastId = id;
                    }
                   
                    adjncyWriter.WriteLine(destId);
                    rightbound++;
                }

                xadjWriter.WriteLine(rightbound);
               
                reader.Close();
                xadjWriter.Close();
                adjncyWriter.Close();
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

        Random ra = new Random();

        private void btnCheckInfo_Click(object sender, EventArgs e)
        {
            //TransformInput();

            //string xadjPath = @"E:\Lab\Triangles data\xadj.txt";
            //StreamReader reader = new StreamReader(xadjPath);

            //string s = reader.ReadToEnd();
            //string[] parts = s.Split(new string[] { " " }, StringSplitOptions.None);

            //reader.Close();
            //reader.Dispose();

            //bool b = 6 - 4 > 1;
            //---------

            string adjncyPath = @"E:\Lab\Triangles data\numeric2screen";
            StreamReader reader = new StreamReader(adjncyPath);

            int count = 0;
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 1000; i++)
            {
                builder.Append(reader.ReadLine() + "\r\n");
            }

            txtContent.Text = builder.ToString();

            //reader.Close();
        }


        void GenerateForMapReduce()
        {
            //List<string> interests = LoadList(@"E:\Lab\Triangles data\Interests.txt");
            List<string> emailHosts = LoadList(@"E:\Lab\Triangles data\EmailHost.txt");

            string inputPath = @"E:\Lab\Triangles data\soc-LiveJournal1_new.txt";
            StreamReader inputReader = new StreamReader(inputPath);

            string namesPath = @"E:\Lab\Triangles data\numeric2screen";
            StreamReader namesReader = new StreamReader(namesPath);

            string outputPath = @"E:\Lab\Triangles data\MROutput.txt";
            StreamWriter writer = new StreamWriter(outputPath);
            string lastId = "0";
            List<string> friends = new List<string>();
            // Get names from screen to name database
            string  line = namesReader.ReadLine();
            string someid, name; Split(line, " ", out someid, out name);

            for (int i = 0; i < 69532892; i++) // Đã biết trước số cạnh
            {
                line = inputReader.ReadLine();
                string id, friendId; Split(line, "\t", out id, out friendId);

                if (lastId != id) // Change to another person
                {
                    //* Push old things to output
                    writer.WriteLine("ID: " + lastId);
                    writer.WriteLine("Name: " + name);
                    writer.WriteLine("Email: " + name + "@" + emailHosts[ra.Next(emailHosts.Count)]);
                    writer.WriteLine("Birthday: " + GenerateBirthday().ToShortDateString());
                    writer.WriteLine();

                    // Push friendList, all friends in one line, seperated by blank space
                    writer.WriteLine(friends.Count + " friends");
                    for(int j = 0; j < friends.Count; j++)
                        writer.Write(friends[j] + " ");

                    // Put 2 blank lines to seperated between every person
                    writer.WriteLine();
                    writer.WriteLine();
                    //----------------------------------------------------

                    //* Get name for the new man
                    line = namesReader.ReadLine();
                    Split(line, " ", out someid, out name);

                    // Reset friends list
                    friends = new List<string>();

                    lastId = id;
                }

                friends.Add(friendId);
            }

            inputReader.Close();
            namesReader.Close();
            writer.Close();
        }

        void Split(string source, string seperator, out string part1, out string part2)
        {
            string[] parts = source.Split(new string[] { seperator }, StringSplitOptions.None);
            part1 = parts[0];
            part2 = parts[2];
        }


        DateTime GenerateBirthday()
        {
            int currentYear = DateTime.Now.Year;
            int year = currentYear - ra.Next(100); // NO more than 100 years old
            int month = ra.Next(12) + 1;
            int day = ra.Next(DateTime.DaysInMonth(year, month)) + 1;
            DateTime birthDay = new DateTime(year, month, day);

            return birthDay;
        }


        List<string> LoadList(string path)
        {
            StreamReader reader = new StreamReader(path);

            List<string> list = new List<string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                list.Add(line);
            }

            reader.Close();

            return list;
        }
    }
}
