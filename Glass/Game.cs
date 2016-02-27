using System;
using System.Windows.Forms;
using System.IO;

namespace Glass
{
    class Game
    {
        public Game(Panel panel, Label label, int numberOfGame, string glassPath,
            string name1, string exe1, string name2, string exe2, int timelimit1, int timelimit2)
        {
            this.amountOfSteps = 0;
            this.numberOfStep = 0;
            this.panel = panel;
            this.label = label;
            this.field = new Field();
            this.numberOfGame = numberOfGame;
            this.glassPath = glassPath;
            this.timelimit1 = timelimit1;
            this.timelimit2 = timelimit2;

            string path1 = glassPath + Convert.ToString(numberOfGame) + @"\X\";
            string path2 = glassPath + Convert.ToString(numberOfGame) + @"\O\";
            this.logPath = glassPath + @"\log\";
            this.glassPath += Convert.ToString(numberOfGame);
            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(path2);
            if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
            this.player1 = new Brain('X', path1, name1, exe1, timelimit1);
            this.player2 = new Brain('O', path2, name2, exe2, timelimit2);
            this.players = new IPlayer[2] { player1, player2 };
            this.currentPlayer = this.players[0];
        }

        public Field field;
        private Panel panel;
        private Label label;
        private IPlayer player1, player2;
        private IPlayer currentPlayer;
        private IPlayer[] players;
        private int amountOfSteps; // increases every two steps
        private int numberOfStep; // increases every step
        public int NumberOfStep { get { return this.numberOfStep; } }
        private int numberOfLastStep; // amount of all steps in game
        public int NumberOfLastStep { get { return this.numberOfLastStep; } }
        private int numberOfGame; // unique number of game
        private enum status : Int16 { notFinished = 0, player1Win = 1, player2Win = 2, invalidStep = 3 };
        private string glassPath, logPath;
        public string GlassPath { get { return glassPath; } }
        public string LogPath { get { return logPath; } }
        private string winner;
        public string Winner { get { return this.winner; } }
        private int timelimit1, timelimit2; // timelimits for both players

        public void StartGame()
        {
            status currentStatus = status.notFinished;

            // players make moves in turn
            for (this.numberOfStep = 1; currentStatus == status.notFinished; this.numberOfStep++) {
                this.amountOfSteps += this.numberOfStep % 2;
                currentStatus = NewStep(this.currentPlayer);
                this.currentPlayer = players[this.numberOfStep % 2];
            }
            this.currentPlayer = players[(this.numberOfStep) % 2];
            this.numberOfStep--;
            this.numberOfLastStep = this.numberOfStep;

            if (currentStatus != status.notFinished) {
                if (currentStatus == status.player1Win) {
                    this.label.Text = "Player Х won!" + " (in " + this.amountOfSteps + " steps)";
                    this.winner = this.player1.Name;
                }
                else if (currentStatus == status.player2Win) {
                    this.label.Text = "Player О won!" + " (in " + this.amountOfSteps + " steps)";
                    this.winner = this.player2.Name;
                }
                else if (currentStatus == status.invalidStep) {
                    this.label.Text = String.Format("Player {0} made a bad move. Player {1} won!",
                        Char.ToUpper(currentPlayer.xORo), Char.ToUpper(this.players[this.numberOfStep % 2].xORo));
                    this.winner = this.players[this.numberOfStep % 2].Name;
                }
            }
            this.panel.Refresh();
        }

        /**
        * Player makes a move
        *
        * @param    player  current player (which will make a move)
        * @return   status.player1Win or status.player2Win or status.notFinished
        * @see      CheckStatus()
        */
        private status NewStep(IPlayer player)
        {
            int newStep = player.Step(this.amountOfSteps);
            bool IsValid = this.field.ChangeCell(newStep, player.xORo);
            if (!IsValid)
                return status.invalidStep;

            string newFile;
            if (player == this.player1)
                newFile = this.player2.Path + this.player1.xORo + this.amountOfSteps + ".txt";
            else
                newFile = this.player1.Path + this.player2.xORo + this.amountOfSteps + ".txt";

            StreamWriter file = new StreamWriter(newFile);
            file.WriteLine(Convert.ToString(newStep));
            file.Close();

            this.panel.Refresh();
            this.label.Refresh();

            // check status of the game every step
            return CheckStatus(newStep);
        }

        /**
        * Check status of the game
        *
        * @param  changedCol  coloumn on which a move was made
        * @return status.player1Win or status.player2Win or status.notFinished
        */
        private status CheckStatus(int changedCol)
        {
            int row, col;
            string currentRow;
            status firstWin, lastWin;
            string winStr1, winStr2;
            status result = status.notFinished;

            // at first check the current player, then the other player
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

            // horizontal
            for (row = 9; row >= 0; row--) {
                currentRow = String.Join("", this.field.Cells[row]);
                if (currentRow.Contains(winStr1)) {
                    int winStartCol = currentRow.IndexOf(winStr1);
                    for (int c = winStartCol; c < winStartCol + 5; c++) {
                        this.field.winCells[c - winStartCol, 0] = row;
                        this.field.winCells[c - winStartCol, 1] = c;
                    }
                    result = firstWin;
                }
                else if (currentRow.Contains(winStr2) && (result != firstWin)) {
                    int winStartCol = currentRow.IndexOf(winStr2);
                    for (int c = winStartCol; c < winStartCol + 5; c++) {
                        this.field.winCells[c - winStartCol, 0] = row;
                        this.field.winCells[c - winStartCol, 1] = c;
                    }
                    result = lastWin;
                }
            }

            // vertical
            string currentCol = "";
            for (row = 0; row < 10; row++) currentCol += this.field.Cells[row][changedCol];
            if (currentCol.Contains(winStr1)) {
                int winStartRow = currentCol.IndexOf(winStr1);
                for (int r = winStartRow; r < winStartRow + 5; r++) {
                    this.field.winCells[r - winStartRow, 0] = r;
                    this.field.winCells[r - winStartRow, 1] = changedCol;
                }
                result = firstWin;
            }
            if (currentCol.Contains(winStr2) && (result != firstWin)) {
                int winStartRow = currentCol.IndexOf(winStr2);
                for (int r = winStartRow; r < winStartRow + 5; r++) {
                    this.field.winCells[r - winStartRow, 0] = r;
                    this.field.winCells[r - winStartRow, 1] = changedCol;
                }
                result = lastWin;
            }

            // diagonal
            // right to left
            string currentDiag;
            int firstCol, firstRow;
            for (firstCol = 4; firstCol <= 9; firstCol++) {
                currentDiag = "";
                for (col = firstCol, row = 0; col >= 0; row++, col--)
                    currentDiag += this.field.Cells[row][col];

                if (currentDiag.Contains(winStr1)) {
                    int winStartRow = currentDiag.IndexOf(winStr1);
                    int winStartCol = firstCol - currentDiag.IndexOf(winStr1);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c--) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = firstWin;
                }
                if (currentDiag.Contains(winStr2) && (result != firstWin)) {
                    int winStartRow = currentDiag.IndexOf(winStr2);
                    int winStartCol = firstCol - currentDiag.IndexOf(winStr2);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c--) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = lastWin;
                }
            }

            for (firstRow = 1; firstRow <= 5; firstRow++) {
                currentDiag = "";
                for (col = 9, row = firstRow; row <= 9; row++, col--)
                    currentDiag += this.field.Cells[row][col];

                if (currentDiag.Contains(winStr1)) {
                    int winStartRow = firstRow + currentDiag.IndexOf(winStr1);
                    int winStartCol = 9 - currentDiag.IndexOf(winStr1);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c--) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = firstWin;
                }
                if (currentDiag.Contains(winStr2) && (result != firstWin)) {
                    int winStartRow = firstRow + currentDiag.IndexOf(winStr2);
                    int winStartCol = 9 - currentDiag.IndexOf(winStr2);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c--) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = lastWin;
                }
            }

            // left to right
            for (firstCol = 5; firstCol >= 0; firstCol--) {
                currentDiag = "";
                for (col = firstCol, row = 0; col <= 9; row++, col++)
                    currentDiag += this.field.Cells[row][col];

                if (currentDiag.Contains(winStr1)) {
                    int winStartRow = currentDiag.IndexOf(winStr1);
                    int winStartCol = firstCol + currentDiag.IndexOf(winStr1);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c++) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = firstWin;
                }
                if (currentDiag.Contains(winStr2) && (result != firstWin)) {
                    int winStartRow = currentDiag.IndexOf(winStr2);
                    int winStartCol = firstCol + currentDiag.IndexOf(winStr2);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c++) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = lastWin;
                }
            }

            for (firstRow = 1; firstRow <= 5; firstRow++) {
                currentDiag = "";
                for (col = 0, row = firstRow; row <= 9; row++, col++)
                    currentDiag += this.field.Cells[row][col];

                if (currentDiag.Contains(winStr1)) {
                    int winStartRow = firstRow + currentDiag.IndexOf(winStr1);
                    int winStartCol = currentDiag.IndexOf(winStr1);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c++) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = firstWin;
                }
                if (currentDiag.Contains(winStr2) && (result != firstWin)) {
                    int winStartRow = firstRow + currentDiag.IndexOf(winStr2);
                    int winStartCol = currentDiag.IndexOf(winStr2);
                    int r, c;
                    for (r = winStartRow, c = winStartCol; r < winStartRow + 5; r++, c++) {
                        this.field.winCells[r - winStartRow, 0] = r;
                        this.field.winCells[r - winStartRow, 1] = c;
                    }
                    result = lastWin;
                }
            }
            return result;
        }

        // switch between steps
        public void prevStep()
        {
            if (this.amountOfSteps == 0) {
                return;
            }

            int lastStep = this.currentPlayer.AllSteps[this.amountOfSteps - 1];
            this.field.DeleteCell(lastStep);
            this.currentPlayer = this.players[this.numberOfStep % 2];
            this.amountOfSteps -= this.numberOfStep % 2;
            this.numberOfStep--;
            this.panel.Refresh();
            this.label.Refresh();
        }

        // switch between steps
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
            catch (ArgumentOutOfRangeException) {
                this.currentPlayer = oldPlayer;
                this.amountOfSteps = oldAmountOfSteps;
                this.numberOfStep--;
                return;
            }
        }
    }
}
