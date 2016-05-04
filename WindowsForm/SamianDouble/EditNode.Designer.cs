namespace SamianDouble
{
    partial class EditNode
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox1ConnectIn = new System.Windows.Forms.ListBox();
            this.listBox2ConnectOut = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox3OtherNode = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1Сохранить = new System.Windows.Forms.Button();
            this.button1Отменить = new System.Windows.Forms.Button();
            this.button1Math = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(896, 529);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.listBox1ConnectIn, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.listBox2ConnectOut, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.listBox3OtherNode, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 263);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(890, 228);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // listBox1ConnectIn
            // 
            this.listBox1ConnectIn.AllowDrop = true;
            this.listBox1ConnectIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1ConnectIn.FormattingEnabled = true;
            this.listBox1ConnectIn.Location = new System.Drawing.Point(3, 23);
            this.listBox1ConnectIn.Name = "listBox1ConnectIn";
            this.listBox1ConnectIn.Size = new System.Drawing.Size(290, 202);
            this.listBox1ConnectIn.TabIndex = 0;
            this.listBox1ConnectIn.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1ConnectIn_DragDrop);
            this.listBox1ConnectIn.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1ConnectIn_DragEnter);
            this.listBox1ConnectIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1ConnectIn_MouseDown);
            // 
            // listBox2ConnectOut
            // 
            this.listBox2ConnectOut.AllowDrop = true;
            this.listBox2ConnectOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2ConnectOut.FormattingEnabled = true;
            this.listBox2ConnectOut.Location = new System.Drawing.Point(299, 23);
            this.listBox2ConnectOut.Name = "listBox2ConnectOut";
            this.listBox2ConnectOut.Size = new System.Drawing.Size(290, 202);
            this.listBox2ConnectOut.TabIndex = 1;
            this.listBox2ConnectOut.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox2ConnectOut_DragDrop);
            this.listBox2ConnectOut.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox2ConnectOut_DragEnter);
            this.listBox2ConnectOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox2ConnectOut_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Входящие связи";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(299, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(290, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Исходящие связи";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBox3OtherNode
            // 
            this.listBox3OtherNode.AllowDrop = true;
            this.listBox3OtherNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox3OtherNode.FormattingEnabled = true;
            this.listBox3OtherNode.Location = new System.Drawing.Point(595, 23);
            this.listBox3OtherNode.Name = "listBox3OtherNode";
            this.listBox3OtherNode.Size = new System.Drawing.Size(292, 202);
            this.listBox3OtherNode.TabIndex = 4;
            this.listBox3OtherNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox3OtherNode_DragDrop);
            this.listBox3OtherNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox3OtherNode_DragEnter);
            this.listBox3OtherNode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox3OtherNode_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(595, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Другие узлы";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(890, 228);
            this.dataGridView1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(890, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.button1Сохранить);
            this.flowLayoutPanel1.Controls.Add(this.button1Отменить);
            this.flowLayoutPanel1.Controls.Add(this.button1Math);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 497);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(890, 29);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // button1Сохранить
            // 
            this.button1Сохранить.Location = new System.Drawing.Point(812, 3);
            this.button1Сохранить.Name = "button1Сохранить";
            this.button1Сохранить.Size = new System.Drawing.Size(75, 23);
            this.button1Сохранить.TabIndex = 0;
            this.button1Сохранить.Text = "Сохранить";
            this.button1Сохранить.UseVisualStyleBackColor = true;
            this.button1Сохранить.Visible = false;
            this.button1Сохранить.Click += new System.EventHandler(this.button1Сохранить_Click);
            // 
            // button1Отменить
            // 
            this.button1Отменить.Location = new System.Drawing.Point(731, 3);
            this.button1Отменить.Name = "button1Отменить";
            this.button1Отменить.Size = new System.Drawing.Size(75, 23);
            this.button1Отменить.TabIndex = 1;
            this.button1Отменить.Text = "Отменить";
            this.button1Отменить.UseVisualStyleBackColor = true;
            this.button1Отменить.Visible = false;
            this.button1Отменить.Click += new System.EventHandler(this.button1Отменить_Click);
            // 
            // button1Math
            // 
            this.button1Math.Location = new System.Drawing.Point(650, 3);
            this.button1Math.Name = "button1Math";
            this.button1Math.Size = new System.Drawing.Size(75, 23);
            this.button1Math.TabIndex = 2;
            this.button1Math.Text = "Расчет";
            this.button1Math.UseVisualStyleBackColor = true;
            this.button1Math.Click += new System.EventHandler(this.button1Math_Click);
            // 
            // EditNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 529);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditNode";
            this.Text = "EditNode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditNode_FormClosing);
            this.Load += new System.EventHandler(this.EditNode_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListBox listBox1ConnectIn;
        private System.Windows.Forms.ListBox listBox2ConnectOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ListBox listBox3OtherNode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1Сохранить;
        private System.Windows.Forms.Button button1Отменить;
        private System.Windows.Forms.Button button1Math;
    }
}