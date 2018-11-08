namespace Chart5._1
{
    partial class ModelTwoDimRegressionWindowParabol
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
            this.label1 = new System.Windows.Forms.Label();
            this.XMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.XMax = new System.Windows.Forms.Label();
            this.XMaxNumeric = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.NNumeric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.ANumeric = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.BNumeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.SigmaEpsilonNumeric = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.FileTextBOx = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cNumeric = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.XMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XMaxNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ANumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SigmaEpsilonNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(46, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Xmin";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // XMinNumeric
            // 
            this.XMinNumeric.DecimalPlaces = 2;
            this.XMinNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.XMinNumeric.InterceptArrowKeys = false;
            this.XMinNumeric.Location = new System.Drawing.Point(163, 32);
            this.XMinNumeric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.XMinNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.XMinNumeric.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.XMinNumeric.Name = "XMinNumeric";
            this.XMinNumeric.Size = new System.Drawing.Size(180, 26);
            this.XMinNumeric.TabIndex = 1;
            this.XMinNumeric.ValueChanged += new System.EventHandler(this.XMinNumeric_ValueChanged);
            // 
            // XMax
            // 
            this.XMax.AutoSize = true;
            this.XMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.XMax.Location = new System.Drawing.Point(46, 83);
            this.XMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.XMax.Name = "XMax";
            this.XMax.Size = new System.Drawing.Size(49, 20);
            this.XMax.TabIndex = 0;
            this.XMax.Text = "Xmax";
            this.XMax.Click += new System.EventHandler(this.label1_Click);
            // 
            // XMaxNumeric
            // 
            this.XMaxNumeric.DecimalPlaces = 2;
            this.XMaxNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.XMaxNumeric.InterceptArrowKeys = false;
            this.XMaxNumeric.Location = new System.Drawing.Point(163, 81);
            this.XMaxNumeric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.XMaxNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.XMaxNumeric.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.XMaxNumeric.Name = "XMaxNumeric";
            this.XMaxNumeric.Size = new System.Drawing.Size(180, 26);
            this.XMaxNumeric.TabIndex = 1;
            this.XMaxNumeric.ValueChanged += new System.EventHandler(this.XMinNumeric_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(46, 133);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "N";
            this.label3.Click += new System.EventHandler(this.label1_Click);
            // 
            // NNumeric
            // 
            this.NNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NNumeric.InterceptArrowKeys = false;
            this.NNumeric.Location = new System.Drawing.Point(163, 131);
            this.NNumeric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.NNumeric.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.NNumeric.Name = "NNumeric";
            this.NNumeric.Size = new System.Drawing.Size(180, 26);
            this.NNumeric.TabIndex = 1;
            this.NNumeric.ValueChanged += new System.EventHandler(this.XMinNumeric_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(46, 182);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "a";
            this.label4.Click += new System.EventHandler(this.label1_Click);
            // 
            // ANumeric
            // 
            this.ANumeric.DecimalPlaces = 2;
            this.ANumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ANumeric.InterceptArrowKeys = false;
            this.ANumeric.Location = new System.Drawing.Point(163, 180);
            this.ANumeric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ANumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.ANumeric.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.ANumeric.Name = "ANumeric";
            this.ANumeric.Size = new System.Drawing.Size(180, 26);
            this.ANumeric.TabIndex = 1;
            this.ANumeric.ValueChanged += new System.EventHandler(this.XMinNumeric_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(46, 228);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "b";
            this.label5.Click += new System.EventHandler(this.label1_Click);
            // 
            // BNumeric
            // 
            this.BNumeric.DecimalPlaces = 2;
            this.BNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BNumeric.InterceptArrowKeys = false;
            this.BNumeric.Location = new System.Drawing.Point(163, 226);
            this.BNumeric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.BNumeric.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.BNumeric.Name = "BNumeric";
            this.BNumeric.Size = new System.Drawing.Size(180, 26);
            this.BNumeric.TabIndex = 1;
            this.BNumeric.ValueChanged += new System.EventHandler(this.XMinNumeric_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(46, 320);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "SigmaEpsilon";
            this.label2.Click += new System.EventHandler(this.label1_Click);
            // 
            // SigmaEpsilonNumeric
            // 
            this.SigmaEpsilonNumeric.DecimalPlaces = 2;
            this.SigmaEpsilonNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SigmaEpsilonNumeric.InterceptArrowKeys = false;
            this.SigmaEpsilonNumeric.Location = new System.Drawing.Point(163, 318);
            this.SigmaEpsilonNumeric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SigmaEpsilonNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.SigmaEpsilonNumeric.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.SigmaEpsilonNumeric.Name = "SigmaEpsilonNumeric";
            this.SigmaEpsilonNumeric.Size = new System.Drawing.Size(180, 26);
            this.SigmaEpsilonNumeric.TabIndex = 1;
            this.SigmaEpsilonNumeric.ValueChanged += new System.EventHandler(this.XMinNumeric_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(46, 362);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Filename";
            this.label6.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(50, 402);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Обзор";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FileTextBOx
            // 
            this.FileTextBOx.Location = new System.Drawing.Point(163, 359);
            this.FileTextBOx.Name = "FileTextBOx";
            this.FileTextBOx.Size = new System.Drawing.Size(180, 26);
            this.FileTextBOx.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(88, 449);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 4;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(193, 450);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 35);
            this.button3.TabIndex = 5;
            this.button3.Text = "Скасувати";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(46, 275);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "c";
            this.label7.Click += new System.EventHandler(this.label1_Click);
            // 
            // cNumeric
            // 
            this.cNumeric.DecimalPlaces = 2;
            this.cNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cNumeric.InterceptArrowKeys = false;
            this.cNumeric.Location = new System.Drawing.Point(163, 273);
            this.cNumeric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.cNumeric.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.cNumeric.Name = "cNumeric";
            this.cNumeric.Size = new System.Drawing.Size(180, 26);
            this.cNumeric.TabIndex = 1;
            this.cNumeric.ValueChanged += new System.EventHandler(this.XMinNumeric_ValueChanged);
            // 
            // ModelTwoDimRegressionWindowParabol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 523);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.FileTextBOx);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SigmaEpsilonNumeric);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cNumeric);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BNumeric);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ANumeric);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NNumeric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.XMaxNumeric);
            this.Controls.Add(this.XMax);
            this.Controls.Add(this.XMinNumeric);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModelTwoDimRegressionWindowParabol";
            this.Text = "ModelTwoDimRegressionWindow";
            this.Load += new System.EventHandler(this.ModelTwoDimRegressionWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.XMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XMaxNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ANumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SigmaEpsilonNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown XMinNumeric;
        private System.Windows.Forms.Label XMax;
        private System.Windows.Forms.NumericUpDown XMaxNumeric;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NNumeric;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ANumeric;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown BNumeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown SigmaEpsilonNumeric;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox FileTextBOx;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown cNumeric;
    }
}