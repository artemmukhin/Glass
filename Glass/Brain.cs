using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Glass
{

    interface IPlayer
    {
        char xORo { get; }
        string Path { get; }
        List<int> AllSteps { get; }
        int Step(int amountOfSteps);
    }

    class RandomBrain : IPlayer
    {
        public RandomBrain(char xORo)
        {
            this.sym = xORo;
        }
        private char sym;
        public char xORo { get { return this.sym; } }
        public string Path { get { return null; } }
        public List<int> AllSteps { get { return new List<int>(); } }

        public int Step(int amountOfSteps)
        {
            int step = Form1.RandGen.Next(0, 10);
            //MessageBox.Show(step.ToString());
            Thread.Sleep(20);
            return step;
        }
    }

    class Brain : IPlayer
    {
        public Brain(char xORo, string path, string exe)
        {
            this.sym = xORo;
            this.path = path;
            this.exe = exe;
            this.allSteps = new List<int>();
        }

        private string path;
        public string Path { get { return this.path; } }
        private List<int> allSteps;
        public List<int> AllSteps { get { return this.allSteps; } }
        private string exe;
        private char sym;
        public char xORo { get { return this.sym; } }

        public int Step(int amountOfSteps)
        {
            int step = 0;
            int timelimit = 5000;
            string text;
            string currentFile = this.path + this.sym + amountOfSteps + ".txt";

            string CLArguments = this.path + " " + this.sym + " " + timelimit;

            Process proc = Process.Start(this.exe, CLArguments);

            var timeout = DateTime.Now.Add(TimeSpan.FromMilliseconds(timelimit));
            while (!File.Exists(currentFile)) {
                if (DateTime.Now > timeout) {
                    MessageBox.Show("AI timeout");
                    step = -1;
                    return step;
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }

            proc.Close();
            Thread.Sleep(TimeSpan.FromMilliseconds(10));
            text = File.ReadAllText(currentFile);

            bool result = Int32.TryParse(text, out step);
            if (!result) {
                step = -1;
            }
            
            this.allSteps.Add(step);
            return step;
        }
    }
}
