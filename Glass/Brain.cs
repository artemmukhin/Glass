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
        string Name { get; }
    }


    class Brain : IPlayer
    {
        public Brain(char xORo, string path, string name, string exe)
        {
            this.sym = xORo;
            this.path = path;
            this.exe = exe;
            this.allSteps = new List<int>();
            this.name = name;
        }

        private string path;
        public string Path { get { return this.path; } }
        private List<int> allSteps;
        public List<int> AllSteps { get { return this.allSteps; } }
        private string exe;
        private string name;
        public string Name { get { return this.name; } }
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
            while (!File.Exists(currentFile) && DateTime.Now < timeout)
                Application.DoEvents();

            proc.Close();
            if (!File.Exists(currentFile)) {
                MessageBox.Show("AI timeout");
                step = -1;
                this.allSteps.Add(step);
                try { proc.Kill(); } catch { }
                return step;
            }

            Thread.Sleep(20);
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
