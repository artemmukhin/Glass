﻿using System;
using System.Windows.Forms;
using System.IO;

namespace Glass
{
    class Game
    {
        public Game(Panel panel, Label label, int numberOfGame, string glassPath,
            string name1, string exe1, string name2, string exe2)
        {
            this.amountOfSteps = 0;
            this.numberOfStep = 0;
            this.panel = panel;
            this.label = label;
            this.field = new Field();
            this.numberOfGame = numberOfGame;
            this.glassPath = glassPath;

            //string configFilePath = @"C:\config.txt";

            //MessageBox.Show("Brains: " + exe1 + "\nLast player: " + exe2);

            string path1 = glassPath + Convert.ToString(numberOfGame) + @"\X\";
            string path2 = glassPath + Convert.ToString(numberOfGame) + @"\O\";
            this.logPath = glassPath + @"\log\";
            this.glassPath += Convert.ToString(numberOfGame);
            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(path2);
            if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
            this.player1 = new Brain('X', path1, name1, exe1);
            this.player2 = new Brain('O', path2, name2, exe2);
            this.players = new IPlayer[2] { player1, player2 };
            this.currentPlayer = this.players[0];
        }

        public Field field;
        private Panel panel;
        private Label label;
        private IPlayer player1, player2;
        private IPlayer currentPlayer;
        private IPlayer[] players;
        private int amountOfSteps; // увеличивается каждые 2 хода
        private int numberOfStep; // увеличивается каждый ход
        private int numberOfGame; // уникальный номер игры
        private enum status : Int16 { notFinished = 0, player1Win = 1, player2Win = 2, invalidStep = 3 };
        private string glassPath, logPath;
        public string GlassPath { get { return glassPath; } }
        public string LogPath { get { return logPath; } }
        private string winner;
        public string Winner { get { return this.winner; } }

        public void StartGame()
        {
            status currentStatus = status.notFinished;

            for (this.numberOfStep = 1; currentStatus == status.notFinished; this.numberOfStep++) {
                this.amountOfSteps += this.numberOfStep % 2;
                currentStatus = NewStep(this.currentPlayer);
                this.currentPlayer = players[this.numberOfStep % 2];
            }
            this.currentPlayer = players[(this.numberOfStep) % 2];
            this.numberOfStep--;

            if (currentStatus != status.notFinished) {
                if (currentStatus == status.player1Win) {
                    this.label.Text = "Игрок Х победил!";
                    this.winner = this.player1.Name;
                }
                else if (currentStatus == status.player2Win) {
                    this.label.Text = "Игрок О победил!";
                    this.winner = this.player2.Name;
                }
                else if (currentStatus == status.invalidStep) {
                    this.label.Text = String.Format("Игрок {0} сходил неправильно. Игрок {1} победил!",
                        Char.ToUpper(currentPlayer.xORo), Char.ToUpper(this.players[this.numberOfStep % 2].xORo));
                    this.winner = this.players[this.numberOfStep % 2].Name;
                }
            }
            //MessageBox.Show("Amount of steps: " + this.amountOfSteps + "\nLast player: " + this.currentPlayer.xORo);
        }

        private status NewStep(IPlayer player)
        {
            int newStep = player.Step(this.amountOfSteps);
            //Application.DoEvents();
            bool IsValid = this.field.ChangeCell(newStep, player.xORo);
            if (!IsValid) {
                //MessageBox.Show("Неправильный ход!");
                return status.invalidStep;
            }

            string newFile;
            if (player == this.player1) newFile = this.player2.Path + this.player1.xORo + this.amountOfSteps + ".txt";
            else newFile = this.player1.Path + this.player2.xORo + this.amountOfSteps + ".txt";

            StreamWriter file = new StreamWriter(newFile);
            file.WriteLine(Convert.ToString(newStep));
            file.Close();

            this.panel.Refresh();
            this.label.Refresh();

            return CheckStatus(newStep);
        }

        // проверка окончания игры (выигрыша)
        private status CheckStatus(int changedCol)
        {
            int row, col;
            string currentRow;
            status firstWin, lastWin;
            string winStr1, winStr2;

            // чтобы проверять сначала победу текущего игрока (по ТЗ)
            if (currentPlayer == this.player1) {
                firstWin = status.player1Win;
                lastWin = status.player2Win;
                winStr1 = new string(this.player1.xORo, 5);
                winStr2 = new string(this.player2.xORo, 5);
            }
            else {
                firstWin = status.player2Win;
                lastWin = status.player1Win;
                winStr1 = new string(this.player2.xORo, 5);
                winStr2 = new string(this.player1.xORo, 5);
            }

            // горизонтали
            for (row = 9; row >= 0; row--) {
                currentRow = String.Join("", this.field.Cells[row]);
                if (currentRow.Contains(winStr1)) return firstWin;
                else if (currentRow.Contains(winStr2)) return lastWin;
            }

            // вертикали
            string currentCol = "";
            for (row = 0; row < 10; row++) currentCol += this.field.Cells[row][changedCol];
            if (currentCol.Contains(winStr1)) return firstWin;
            if (currentCol.Contains(winStr2)) return lastWin;

            // диагонали
            // справа-налево
            string currentDiag;
            int firstCol, firstRow;
            for (firstCol = 4, currentDiag = ""; firstCol <= 9; firstCol++) {
                for (col = firstCol, row = 0; col >= 0; row++, col--)
                    currentDiag += this.field.Cells[row][col];
                if (currentDiag.Contains(winStr1)) return firstWin;
                if (currentDiag.Contains(winStr2)) return lastWin;
            }
            // ниже главной
            for (firstRow = 1, currentDiag = ""; firstRow <= 5; firstRow++) {
                for (col = 9, row = firstRow; row <= 9; row++, col--)
                    currentDiag += this.field.Cells[row][col];
                if (firstRow == 1) MessageBox.Show(currentDiag);
                if (currentDiag.Contains(winStr1)) return firstWin;
                if (currentDiag.Contains(winStr2)) return lastWin;
            }

            // слева-направо
            for (firstCol = 5, currentDiag = ""; firstCol >= 0; firstCol--) {
                for (col = firstCol, row = 0; col <= 9; row++, col++)
                    currentDiag += this.field.Cells[row][col];
                if (currentDiag.Contains(winStr1)) return firstWin;
                if (currentDiag.Contains(winStr2)) return lastWin;
            }
            // ниже главной
            for (firstRow = 1, currentDiag = ""; firstRow >= 5; firstRow++) {
                for (col = 0, row = firstRow; row <= 9; row++, col++)
                    currentDiag += this.field.Cells[row][col];
                if (currentDiag.Contains(winStr1)) return firstWin;
                if (currentDiag.Contains(winStr2)) return lastWin;
            }

            return status.notFinished;
        }

        public void prevStep()
        {
            if (this.amountOfSteps == 0) {
                MessageBox.Show("нет предыдущего");
                return;
            }
            int lastStep = this.currentPlayer.AllSteps[this.amountOfSteps - 1];
            this.field.DeleteCell(lastStep);
            this.panel.Refresh();
            this.label.Refresh();
            this.currentPlayer = this.players[this.numberOfStep % 2];
            this.amountOfSteps -= this.numberOfStep % 2;
            this.numberOfStep--;
        }

        public void nextStep()
        {
            IPlayer oldPlayer = this.currentPlayer;
            int oldAmountOfSteps = this.amountOfSteps;

            this.currentPlayer = this.players[this.numberOfStep % 2];
            this.amountOfSteps += (this.numberOfStep + 1) % 2;
            this.numberOfStep++;

            try {
                int nextStep = this.currentPlayer.AllSteps[this.amountOfSteps - 1];
                this.field.ChangeCell(nextStep, this.currentPlayer.xORo);
                this.panel.Refresh();
                this.label.Refresh();
            }
            catch {
                this.currentPlayer = oldPlayer;
                this.amountOfSteps = oldAmountOfSteps;
                this.numberOfStep--;
                MessageBox.Show("нет следующего");
                return;
            }
        }
    }
}
