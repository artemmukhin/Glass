namespace Glass
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.stat_2_count = new System.Windows.Forms.Label();
            this.stat_1_count = new System.Windows.Forms.Label();
            this.stat_2 = new System.Windows.Forms.Label();
            this.stat_1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.newTabPage = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.newTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(444, 422);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.ChangeTab);
            this.tabControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.stat_2_count);
            this.tabPage1.Controls.Add(this.stat_1_count);
            this.tabPage1.Controls.Add(this.stat_2);
            this.tabPage1.Controls.Add(this.stat_1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.infoLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(436, 396);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // stat_2_count
            // 
            this.stat_2_count.AutoSize = true;
            this.stat_2_count.Location = new System.Drawing.Point(160, 102);
            this.stat_2_count.Name = "stat_2_count";
            this.stat_2_count.Size = new System.Drawing.Size(13, 13);
            this.stat_2_count.TabIndex = 6;
            this.stat_2_count.Text = "0";
            // 
            // stat_1_count
            // 
            this.stat_1_count.AutoSize = true;
            this.stat_1_count.Location = new System.Drawing.Point(160, 89);
            this.stat_1_count.Name = "stat_1_count";
            this.stat_1_count.Size = new System.Drawing.Size(13, 13);
            this.stat_1_count.TabIndex = 5;
            this.stat_1_count.Text = "0";
            // 
            // stat_2
            // 
            this.stat_2.AutoSize = true;
            this.stat_2.Location = new System.Drawing.Point(27, 102);
            this.stat_2.Name = "stat_2";
            this.stat_2.Size = new System.Drawing.Size(47, 13);
            this.stat_2.TabIndex = 4;
            this.stat_2.Text = "Игрок 2";
            // 
            // stat_1
            // 
            this.stat_1.AutoSize = true;
            this.stat_1.Location = new System.Drawing.Point(27, 89);
            this.stat_1.Name = "stat_1";
            this.stat_1.Size = new System.Drawing.Size(47, 13);
            this.stat_1.TabIndex = 3;
            this.stat_1.Text = "Игрок 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(23, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Статистика";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(20, 20);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(225, 13);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Для запуска новой игры нажмите на таб +";
            // 
            // newTabPage
            // 
            this.newTabPage.Location = new System.Drawing.Point(4, 22);
            this.newTabPage.Name = "newTabPage";
            this.newTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.newTabPage.Size = new System.Drawing.Size(309, 269);
            this.newTabPage.TabIndex = 1;
            this.newTabPage.Text = "+";
            this.newTabPage.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 422);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Glass";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage newTabPage;
        private System.Windows.Forms.Label infoLabel;
        //private System.Windows.Forms.Button button2;
        //private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label stat_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label stat_2;
        private System.Windows.Forms.Label stat_2_count;
        private System.Windows.Forms.Label stat_1_count;
    }
}

