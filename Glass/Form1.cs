using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


/*
To-do:
    * конфиг считывается в Form, и в Game передаётся массив Brain'ов
*/

namespace Glass
{
    public partial class Form1 : Form
    {
        List<Game> allGames = new List<Game>();
        public static Random RandGen = new Random();

        public Form1()
        {
            string configFilePath = Directory.GetCurrentDirectory() + @"\config.txt";
            if (!File.Exists(configFilePath)) {
                MessageBox.Show(@"Не найден конфигурационный файл config.txt");
                Environment.Exit(0);
            }

            InitializeComponent();
            //Game game = new Game(this.panel1, 1);
            //this.allGames.Add(game);
            //game.StartGame();

            Label infoLabel = new Label();
            infoLabel.AutoSize = true;
            infoLabel.Location = new Point(20, 20);
            infoLabel.Text = "Для запуска новой игры нажмите на таб +";
            tabControl1.TabPages[0].Controls.Add(infoLabel);

            this.Show();
        }

        private void ChangeTab(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == newTabPage) {
                int numberOfNewTab = tabControl1.TabPages.Count - 1;

                // создаём новый таб
                TabPage createdTabPage = new TabPage();
                createdTabPage.Name = "tabPage" + numberOfNewTab;
                createdTabPage.Text = "Game #" + numberOfNewTab;
                createdTabPage.UseVisualStyleBackColor = true;

                // создаём новую панель
                Panel newPanel = new Panel();
                newPanel.Name = "panel" + numberOfNewTab;
                newPanel.Location = new Point(9, 7);
                newPanel.Size = new Size(227, 203);
                newPanel.Paint += new PaintEventHandler(this.panel_Paint);
                typeof(Panel).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, newPanel, new object[] { true });

                // создаём новую надпись
                Label newLabel = new Label();
                newLabel.AutoSize = true;
                newLabel.Location = new Point(9, 217);
                newLabel.Name = "label" + numberOfNewTab;
                //newLabel.Size = new Size(67, 13);
                newLabel.Text = "Идёт игра...";

                // создаём новые кнопки прокрутки ходов
                Button newPrevButton = new Button();
                newPrevButton.Location = new Point(9, 235);
                newPrevButton.Name = "prevButton" + numberOfNewTab;
                newPrevButton.Size = new Size(27, 23);
                newPrevButton.Text = "<";
                newPrevButton.UseVisualStyleBackColor = true;
                newPrevButton.Click += new EventHandler(this.prevStep_Click);

                Button newNextButton = new Button();
                newNextButton.Location = new Point(44, 235);
                newNextButton.Name = "NextButton" + numberOfNewTab;
                newNextButton.Size = new Size(27, 23);
                newNextButton.Text = ">";
                newNextButton.UseVisualStyleBackColor = true;
                newNextButton.Click += new EventHandler(this.nextStep_Click);

                // добавляем таб и панель
                //newPanel.Controls.Add(newLabel);
                createdTabPage.Controls.Add(newPanel);
                createdTabPage.Controls.Add(newLabel);
                createdTabPage.Controls.Add(newPrevButton);
                createdTabPage.Controls.Add(newNextButton);
                tabControl1.TabPages.Insert(numberOfNewTab, createdTabPage);
                tabControl1.SelectedTab = createdTabPage;
                newLabel.Show();
                // создаём новую игру
                Game newGame = new Game(newPanel, newLabel, numberOfNewTab);
                this.allGames.Add(newGame);
                newGame.StartGame();
            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            DrawField(e.Graphics);
        }

        public void DrawField(Graphics g)
        {
            for (int row = 0; row < 10; row++) {
                for (int col = 0; col < 10; col++) {
                    this.DrawCell(this.allGames[tabControl1.SelectedIndex - 1].field.Cells[row][col], row, col, g);
                }
            }
        }

        private void DrawCell(char cell, int row, int col, Graphics g)
        {
            int x, y; // левый верхний угол клетки
            int cellWidth = 20, cellHeight = 20;
            Brush color;

            x = col * cellWidth;
            y = row * cellHeight;

            if (cell == '-') {
                g.FillRectangle(Brushes.LightGray, x, y, cellWidth, cellHeight);
                g.DrawRectangle(Pens.Black, x, y, cellWidth, cellHeight);
                return;
            }

            if (cell == 'X') color = Brushes.Blue;
            else color = Brushes.Purple;

            g.FillRectangle(Brushes.White, x, y, cellWidth, cellHeight);
            g.DrawRectangle(Pens.Black, x, y, cellWidth, cellHeight);
            g.DrawString(Convert.ToString(cell).ToLower(), new Font("Tahoma", 15, FontStyle.Regular), color, x + 2, y - 2);
        }

        private void prevStep_Click(object sender, EventArgs e)
        {
            this.allGames[tabControl1.SelectedIndex - 1].prevStep();
        }

        private void nextStep_Click(object sender, EventArgs e)
        {
            this.allGames[tabControl1.SelectedIndex - 1].nextStep();
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Glass",
                MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
            }

            int numberOfGame;
            foreach (Game game in this.allGames) {
                numberOfGame = Directory.GetDirectories(game.LogPath).Length + 1;
                if (Directory.Exists(game.GlassPath)) // юзер мог уже удалить каталог
                    Directory.Move(game.GlassPath, game.LogPath + "game" + numberOfGame);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.J)
                this.allGames[tabControl1.SelectedIndex - 1].prevStep();
            else if (e.KeyCode == Keys.K)
                this.allGames[tabControl1.SelectedIndex - 1].nextStep();
        }
    }
}