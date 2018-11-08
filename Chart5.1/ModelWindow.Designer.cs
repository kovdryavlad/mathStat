namespace Chart1._1
{
    partial class ModelWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.comboBoxTypeDistr = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UpDowmNumbder = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Param1TextBox = new System.Windows.Forms.TextBox();
            this.Param1Name = new System.Windows.Forms.Label();
            this.Param2TextBox = new System.Windows.Forms.TextBox();
            this.Param2Name = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UpDowmNumbder)).BeginInit();
            this.SuspendLayout();
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(51, 11);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(189, 20);
            this.PathTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Шлях: ";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(246, 10);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "Встановити";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // comboBoxTypeDistr
            // 
            this.comboBoxTypeDistr.FormattingEnabled = true;
            this.comboBoxTypeDistr.Items.AddRange(new object[] {
            "Експоненціальний",
            "Нормальний",
            "Рівномірний",
            "Арксинуса"});
            this.comboBoxTypeDistr.Location = new System.Drawing.Point(171, 49);
            this.comboBoxTypeDistr.Name = "comboBoxTypeDistr";
            this.comboBoxTypeDistr.Size = new System.Drawing.Size(150, 21);
            this.comboBoxTypeDistr.TabIndex = 3;
            this.comboBoxTypeDistr.SelectedIndexChanged += new System.EventHandler(this.comboBoxTypeDistr_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Тип розподілу:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Кількість елементів: ";
            // 
            // UpDowmNumbder
            // 
            this.UpDowmNumbder.Location = new System.Drawing.Point(171, 85);
            this.UpDowmNumbder.Maximum = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.UpDowmNumbder.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.UpDowmNumbder.Name = "UpDowmNumbder";
            this.UpDowmNumbder.Size = new System.Drawing.Size(150, 20);
            this.UpDowmNumbder.TabIndex = 5;
            this.UpDowmNumbder.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(11, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 24);
            this.label4.TabIndex = 1;
            this.label4.Text = "Параметри";
            // 
            // Param1TextBox
            // 
            this.Param1TextBox.Enabled = false;
            this.Param1TextBox.Location = new System.Drawing.Point(132, 155);
            this.Param1TextBox.Name = "Param1TextBox";
            this.Param1TextBox.Size = new System.Drawing.Size(189, 20);
            this.Param1TextBox.TabIndex = 0;
            // 
            // Param1Name
            // 
            this.Param1Name.AutoSize = true;
            this.Param1Name.Enabled = false;
            this.Param1Name.Location = new System.Drawing.Point(12, 158);
            this.Param1Name.Name = "Param1Name";
            this.Param1Name.Size = new System.Drawing.Size(102, 13);
            this.Param1Name.TabIndex = 1;
            this.Param1Name.Text = "Назва параметру: ";
            // 
            // Param2TextBox
            // 
            this.Param2TextBox.Enabled = false;
            this.Param2TextBox.Location = new System.Drawing.Point(132, 190);
            this.Param2TextBox.Name = "Param2TextBox";
            this.Param2TextBox.Size = new System.Drawing.Size(189, 20);
            this.Param2TextBox.TabIndex = 0;
            // 
            // Param2Name
            // 
            this.Param2Name.AutoSize = true;
            this.Param2Name.Enabled = false;
            this.Param2Name.Location = new System.Drawing.Point(12, 193);
            this.Param2Name.Name = "Param2Name";
            this.Param2Name.Size = new System.Drawing.Size(102, 13);
            this.Param2Name.TabIndex = 1;
            this.Param2Name.Text = "Назва параметру: ";
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(177, 236);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Скасувати";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(80, 236);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ModelWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 276);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.UpDowmNumbder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxTypeDistr);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Param2Name);
            this.Controls.Add(this.Param1Name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Param2TextBox);
            this.Controls.Add(this.Param1TextBox);
            this.Controls.Add(this.PathTextBox);
            this.Name = "ModelWindow";
            this.Text = "Вікно моделювання";
            ((System.ComponentModel.ISupportInitialize)(this.UpDowmNumbder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.ComboBox comboBoxTypeDistr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown UpDowmNumbder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Param1TextBox;
        private System.Windows.Forms.Label Param1Name;
        private System.Windows.Forms.TextBox Param2TextBox;
        private System.Windows.Forms.Label Param2Name;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
    }
}