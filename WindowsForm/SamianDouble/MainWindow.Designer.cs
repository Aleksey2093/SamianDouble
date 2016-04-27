namespace SamianDouble
{
    partial class MainWindow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_AddNode = new System.Windows.Forms.Button();
            this.button_DeleteNode = new System.Windows.Forms.Button();
            this.nodes_площадкадляразмещения = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.nodes_площадкадляразмещения, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1106, 469);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.button_AddNode);
            this.flowLayoutPanel1.Controls.Add(this.button_DeleteNode);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 435);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1100, 31);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // button_AddNode
            // 
            this.button_AddNode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_AddNode.Location = new System.Drawing.Point(3, 3);
            this.button_AddNode.Name = "button_AddNode";
            this.button_AddNode.Size = new System.Drawing.Size(101, 23);
            this.button_AddNode.TabIndex = 0;
            this.button_AddNode.Text = "Добавить узел";
            this.button_AddNode.UseVisualStyleBackColor = true;
            this.button_AddNode.Click += new System.EventHandler(this.button_AddNode_Click);
            // 
            // button_DeleteNode
            // 
            this.button_DeleteNode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_DeleteNode.Location = new System.Drawing.Point(110, 3);
            this.button_DeleteNode.Name = "button_DeleteNode";
            this.button_DeleteNode.Size = new System.Drawing.Size(91, 23);
            this.button_DeleteNode.TabIndex = 1;
            this.button_DeleteNode.Text = "Удалить узел";
            this.button_DeleteNode.UseVisualStyleBackColor = true;
            // 
            // nodes_площадкадляразмещения
            // 
            this.nodes_площадкадляразмещения.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.nodes_площадкадляразмещения.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodes_площадкадляразмещения.Location = new System.Drawing.Point(3, 3);
            this.nodes_площадкадляразмещения.Name = "nodes_площадкадляразмещения";
            this.nodes_площадкадляразмещения.Size = new System.Drawing.Size(1100, 426);
            this.nodes_площадкадляразмещения.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 469);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button_AddNode;
        private System.Windows.Forms.Button button_DeleteNode;
        private System.Windows.Forms.FlowLayoutPanel nodes_площадкадляразмещения;

    }
}

