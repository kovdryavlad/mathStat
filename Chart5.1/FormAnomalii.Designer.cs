using System;
namespace Chart1._1
{
    partial class FormAnomalii
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxMetod = new System.Windows.Forms.ComboBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButRemove = new System.Windows.Forms.Button();
            this.ButOK = new System.Windows.Forms.Button();
            this.ButCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownA = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownB = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxMetod
            // 
            this.comboBoxMetod.FormattingEnabled = true;
            this.comboBoxMetod.Items.AddRange(new object[] {
            "-Не вибрано-",
            "Перший спосіб",
            "Другий спосіб",
            "Третій спосіб"});
            this.comboBoxMetod.Location = new System.Drawing.Point(196, 13);
            this.comboBoxMetod.Name = "comboBoxMetod";
            this.comboBoxMetod.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMetod.TabIndex = 0;
            this.comboBoxMetod.SelectedIndexChanged += new System.EventHandler(this.comboBoxMetod_SelectedIndexChanged);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(12, 78);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(307, 329);
            this.listBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Автоматиний метод вилучення";
            // 
            // ButRemove
            // 
            this.ButRemove.Location = new System.Drawing.Point(46, 421);
            this.ButRemove.Name = "ButRemove";
            this.ButRemove.Size = new System.Drawing.Size(75, 23);
            this.ButRemove.TabIndex = 4;
            this.ButRemove.Text = "Вилучити";
            this.ButRemove.UseVisualStyleBackColor = true;
            this.ButRemove.Click += new System.EventHandler(this.ButRemove_Click);
            // 
            // ButOK
            // 
            this.ButOK.Location = new System.Drawing.Point(127, 421);
            this.ButOK.Name = "ButOK";
            this.ButOK.Size = new System.Drawing.Size(75, 23);
            this.ButOK.TabIndex = 5;
            this.ButOK.Text = "OK";
            this.ButOK.UseVisualStyleBackColor = true;
            this.ButOK.Click += new System.EventHandler(this.ButOK_Click);
            // 
            // ButCancel
            // 
            this.ButCancel.Location = new System.Drawing.Point(208, 421);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(75, 23);
            this.ButCancel.TabIndex = 6;
            this.ButCancel.Text = "Скасувати";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "a = ";
            // 
            // numericUpDownA
            // 
            this.numericUpDownA.Location = new System.Drawing.Point(46, 47);
            this.numericUpDownA.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numericUpDownA.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.numericUpDownA.Name = "numericUpDownA";
            this.numericUpDownA.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownA.TabIndex = 1;
            this.numericUpDownA.ValueChanged += new System.EventHandler(this.numericUpDownA_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(188, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "b = ";
            // 
            // numericUpDownB
            // 
            this.numericUpDownB.Location = new System.Drawing.Point(222, 47);
            this.numericUpDownB.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numericUpDownB.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.numericUpDownB.Name = "numericUpDownB";
            this.numericUpDownB.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownB.TabIndex = 2;
            this.numericUpDownB.ValueChanged += new System.EventHandler(this.numericUpDownB_ValueChanged);
            // 
            // FormAnomalii
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 456);
            this.Controls.Add(this.numericUpDownB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ButCancel);
            this.Controls.Add(this.ButOK);
            this.Controls.Add(this.ButRemove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.comboBoxMetod);
            this.Name = "FormAnomalii";
            this.Text = "Вилучення Аномалій";
            this.Load += new System.EventHandler(this.FormAnomalii_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMetod;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButRemove;
        private System.Windows.Forms.Button ButOK;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownB;
    }
}

