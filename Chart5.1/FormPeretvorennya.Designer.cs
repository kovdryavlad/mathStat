namespace Chart1._1
{
    partial class FormPeretvorennya
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.UpDownZsuv = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LogOsnUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DovilnaRadioButton = new System.Windows.Forms.RadioButton();
            this.ExpRadioButton = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.StepinUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownZsuv)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogOsnUpDown)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StepinUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(359, 206);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.UpDownZsuv);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(351, 180);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Зсув";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // UpDownZsuv
            // 
            this.UpDownZsuv.DecimalPlaces = 2;
            this.UpDownZsuv.Location = new System.Drawing.Point(207, 31);
            this.UpDownZsuv.Maximum = new decimal(new int[] {
            20000000,
            0,
            0,
            0});
            this.UpDownZsuv.Minimum = new decimal(new int[] {
            2000000,
            0,
            0,
            -2147483648});
            this.UpDownZsuv.Name = "UpDownZsuv";
            this.UpDownZsuv.Size = new System.Drawing.Size(120, 20);
            this.UpDownZsuv.TabIndex = 1;
            this.UpDownZsuv.ValueChanged += new System.EventHandler(this.UpDownZsuv_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Зсунути на:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.LogOsnUpDown);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.DovilnaRadioButton);
            this.tabPage2.Controls.Add(this.ExpRadioButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(351, 180);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Логарифмування";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LogOsnUpDown
            // 
            this.LogOsnUpDown.DecimalPlaces = 2;
            this.LogOsnUpDown.Location = new System.Drawing.Point(216, 97);
            this.LogOsnUpDown.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.LogOsnUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.LogOsnUpDown.Name = "LogOsnUpDown";
            this.LogOsnUpDown.Size = new System.Drawing.Size(110, 20);
            this.LogOsnUpDown.TabIndex = 3;
            this.LogOsnUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.LogOsnUpDown.ValueChanged += new System.EventHandler(this.LogOsnUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Вкажіть довільну основу";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Основа:";
            // 
            // DovilnaRadioButton
            // 
            this.DovilnaRadioButton.AutoSize = true;
            this.DovilnaRadioButton.Checked = true;
            this.DovilnaRadioButton.Location = new System.Drawing.Point(45, 46);
            this.DovilnaRadioButton.Name = "DovilnaRadioButton";
            this.DovilnaRadioButton.Size = new System.Drawing.Size(72, 17);
            this.DovilnaRadioButton.TabIndex = 0;
            this.DovilnaRadioButton.TabStop = true;
            this.DovilnaRadioButton.Text = "Довільна";
            this.DovilnaRadioButton.UseVisualStyleBackColor = true;
            // 
            // ExpRadioButton
            // 
            this.ExpRadioButton.AutoSize = true;
            this.ExpRadioButton.Location = new System.Drawing.Point(218, 46);
            this.ExpRadioButton.Name = "ExpRadioButton";
            this.ExpRadioButton.Size = new System.Drawing.Size(43, 17);
            this.ExpRadioButton.TabIndex = 0;
            this.ExpRadioButton.Text = "Exp";
            this.ExpRadioButton.UseVisualStyleBackColor = true;
            this.ExpRadioButton.CheckedChanged += new System.EventHandler(this.ExpRadioButton_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.StepinUpDown);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(351, 180);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Піднесення до степеня";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // StepinUpDown
            // 
            this.StepinUpDown.DecimalPlaces = 2;
            this.StepinUpDown.Location = new System.Drawing.Point(204, 33);
            this.StepinUpDown.Maximum = new decimal(new int[] {
            20000000,
            0,
            0,
            0});
            this.StepinUpDown.Minimum = new decimal(new int[] {
            20000000,
            0,
            0,
            -2147483648});
            this.StepinUpDown.Name = "StepinUpDown";
            this.StepinUpDown.Size = new System.Drawing.Size(120, 20);
            this.StepinUpDown.TabIndex = 1;
            this.StepinUpDown.ValueChanged += new System.EventHandler(this.StepinUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Вкажіть степінь:";
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(107, 234);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(204, 234);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Скасувати";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FormPeretvorennya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 270);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormPeretvorennya";
            this.Text = "Перетворення";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownZsuv)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogOsnUpDown)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StepinUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.NumericUpDown UpDownZsuv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton DovilnaRadioButton;
        private System.Windows.Forms.RadioButton ExpRadioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown LogOsnUpDown;
        private System.Windows.Forms.NumericUpDown StepinUpDown;
        private System.Windows.Forms.Label label4;
    }
}