using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SamianDouble
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static List<Node_struct> listnodes { get;set; }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            listnodes = new List<Node_struct>();
        }

        bool add_node = false;
        private void button_AddNode_Click(object sender, EventArgs e)
        {
            if (add_node == false)
            {
                add_node = true;
                Node a = new Node();
                listnodes = a.getNewNode(listnodes);
                treeListReplace();
                add_node = false;
            }
        }

        private static bool перерисовкаДерева = false;

        /// <summary>
        /// перерисовывает дерево узлов
        /// </summary>
        /// <returns>возвращает успех работы</returns>
        private void treeListReplace()
        {
            Thread thread = new Thread(delegate()
                {
                    int err = 0;
                    while(перерисовкаДерева)
                    {
                        Thread.Sleep(3000);
                        err++;
                        if (err > 5)
                            return;
                    }
                    перерисовкаДерева = true;
                    new NodeValueMathUp().getMathNodesAll(listnodes);
                    TreeNode n1 = new TreeNode("Родители");
                    TreeNode n2 = new TreeNode("Промежуточные");
                    TreeNode n3 = new TreeNode("Дети");
                    TreeNode n4 = new TreeNode("Несвязные");
                    foreach (var node_st in listnodes)
                    {
                        TreeNode nod = new TreeNode();
                        nod.Name = node_st.ID.ToString() + node_st.Name;
                        nod.Text = node_st.Name;

                        bool estizvest = false;

                        foreach (var prope in node_st.props)
                        {
                            if (prope.proc100)
                            {
                                estizvest = true;
                                break;
                            }
                        }
                        foreach (var prope in node_st.props)
                        {
                            TreeNode nod_p = new TreeNode();
                            Parallel.Invoke(
                            () => { nod_p.Name = node_st.ID.ToString() + prope.name; },
                            () =>
                            {
                                nod_p.Text = prope.name;
                                if (prope.proc100)
                                {
                                    nod_p.BackColor = Color.Tomato;
                                    nod_p.Text += " " + 100 + "%";
                                }
                                else if (estizvest)
                                {
                                    nod_p.Text += " " + "0%";
                                }
                                else
                                {
                                    nod_p.Text += " " + (prope.value_editor * 100).ToString() + "%";
                                }
                            }
                            );
                            nod.Nodes.Add(nod_p);
                        }
                        if (node_st.connects_in.Count == 0)
                        {
                            if (node_st.connects_out.Count == 0)
                                n4.Nodes.Add(nod);
                            else
                                n1.Nodes.Add(nod);
                        }
                        else
                        {
                            if (node_st.connects_out.Count == 0)
                                n3.Nodes.Add(nod);
                            else
                                n2.Nodes.Add(nod);
                        }
                    }
                    Invoke(new MethodInvoker(() =>
                    {
                        treeView1.Nodes.Clear();
                        treeView1.Nodes.Add(n1);
                        treeView1.Nodes.Add(n2);
                        treeView1.Nodes.Add(n3);
                        treeView1.Nodes.Add(n4);
                        treeView1.ExpandAll();
                        перерисовкаДерева = false;
                    }));
                });
            thread.Name = "Перерисовка дерева";
            thread.Start();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }
            if (e.Label == e.Node.Text)
            {
                e.CancelEdit = true;
                return;
            }
            if (e.Label.Length > 2)
            {
                TreeNode nod = e.Node;
                nod.Text = e.Label;
                Node n = new Node();
                listnodes = n.nodeNameEdit(nod, listnodes);
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Level == 1)
            {
                e.Node.ExpandAll();
                EditNode ed = new EditNode();
                int i = new Node().getSelectNode(e.Node, listnodes);
                if (i < 0)
                    return;

                ed.thisnod_i = i;
                ed.thisnod = listnodes[i];
                ed.tmplistnodes = listnodes;

                ed.ShowDialog();

                treeListReplace();
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Node.Level == 1)
            {
                Parallel.For(0,treeView1.Nodes.Count,(j)=>
                {
                    Parallel.For(0, treeView1.Nodes[j].Nodes.Count, (i, state) =>
                        {
                            treeView1.Nodes[j].Nodes[i].BackColor = Color.White;
                        });
                });
                e.Node.BackColor = Color.Red;
                
                int index = new Node().getSelectNode(e.Node, listnodes);
                Parallel.For(0,treeView1.Nodes[0].Nodes.Count,(i,state)=>
                {
                    for (int j=0;j<listnodes[index].connects_in.Count;j++)
                    {
                        var tr = treeView1.Nodes[0].Nodes[i];
                        var ln = listnodes[index].connects_in[j];
                        if (tr.Name == ln.ID.ToString()+ln.Name)
                        {
                            tr.BackColor = Color.LightBlue;
                        }
                    }
                });
                Parallel.For(0, treeView1.Nodes[1].Nodes.Count, (i, state) =>
                {
                    for (int j = 0; j < listnodes[index].connects_in.Count; j++)
                    {
                        var tr = treeView1.Nodes[1].Nodes[i];
                        var ln = listnodes[index].connects_in[j];
                        if (tr.Name == ln.ID.ToString() + ln.Name)
                        {
                            tr.BackColor = Color.LightBlue;
                        }
                    }
                    for (int j = 0; j < listnodes[index].connects_out.Count; j++)
                    {
                        var tr = treeView1.Nodes[1].Nodes[i];
                        var ln = listnodes[index].connects_out[j];
                        if (tr.Name == ln.ID.ToString() + ln.Name)
                        {
                            tr.BackColor = Color.LightGreen;
                        }
                    }
                });
                Parallel.For(0, treeView1.Nodes[2].Nodes.Count, (i, state) =>
                {
                    for (int j = 0; j < listnodes[index].connects_out.Count; j++)
                    {
                        var tr = treeView1.Nodes[2].Nodes[i];
                        var ln = listnodes[index].connects_out[j];
                        if (tr.Name == ln.ID.ToString() + ln.Name)
                        {
                            tr.BackColor = Color.LightGreen;
                        }
                    }
                });
            } else if (e.Button == MouseButtons.Right && e.Node.Level == 1)
            {
                if (DialogResult.Yes != MessageBox.Show("Добавить узлу " + e.Node.Text + " новое свойство", "Добавление", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    return;
                }
                TreeNode nod = e.Node;
                Node n = new Node();
                listnodes = n.nodeAddNewProperty(nod, listnodes);
                treeListReplace();
            } else if (e.Button == MouseButtons.Right && e.Node.Level == 2 && e.Node.Parent.Nodes.Count > 2)
            {
                if (DialogResult.Yes != MessageBox.Show("Удалить узлу " + e.Node.Parent.Text + " свойство " + e.Node.Text, "Удаление",
    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    return;
                }
                TreeNode nod = e.Node;
                Node n = new Node();
                listnodes = n.nodeDeletePropertyThisNod(nod, listnodes);
                treeListReplace();
            } else if (e.Button == MouseButtons.Left && e.Node.Level == 2)
            {
                Node n = new Node();
                listnodes = n.nodeSetFixPropertyThisNod(e.Node, listnodes);
                treeListReplace();
                e.Node.Checked = false;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listnodes = new FileLoadAndSave().loadFife(listnodes);
            treeListReplace();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLoadAndSave f = new FileLoadAndSave();
            f.saveToFile(listnodes);
        }

        private void button_DeleteNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level != 1)
                return;
            int i = new Node().getSelectNode(treeView1.SelectedNode, listnodes);
            listnodes.RemoveAt(i);
            treeListReplace();
        }

        private void button1ClearFix_Click(object sender, EventArgs e)
        {
            Parallel.ForEach(listnodes, (nod) =>
                {
                    Parallel.ForEach(nod.props, (propppppp) =>
                        {
                            propppppp.proc100 = false;
                        });
                });
            treeListReplace();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLoadAndSave f = new FileLoadAndSave();
            if (f.newНовыйФайлПроекта(listnodes))
            {
                listnodes.Clear();
                listnodes = new List<Node_struct>();
                treeListReplace();
            }
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            /*List<Node_struct> proНарисован = new List<Node_struct>();
            int Size = 100;
            Random rand = new Random();
            foreach (var nod in listnodes)
            {
            ццц:
                nod.cordx = rand.Next(101, pictureBox1.Size.Width - 100);
                nod.cordy = rand.Next(101, pictureBox1.Size.Height - 100);
                foreach (var waw in proНарисован)
                {
                    if (Math.Sqrt(Math.Pow(nod.cordx - waw.cordx, 2) + Math.Pow(nod.cordy - waw.cordy, 2)) < Size + 10)
                    {
                        goto ццц;
                    }
                }
                proНарисован.Add(nod);
            }

            foreach(var toch in proНарисован)
            {
                int powtor = 0;
            wawfлол:
                foreach(var line1 in proНарисован)
                {
                    if (line1 != toch)
                    {
                        foreach (var line2 in line1.connects_out)
                        {
                            if (line2 != toch)
                            {
                                double A = line2.cordy - line1.cordy;
                                double B = line1.cordx - line2.cordx;
                                double C = (-1) * line1.cordx * (line2.cordy - line1.cordy) + line1.cordy * (line2.cordx - line1.cordx);
                                double T = A * A + B * B; T = Math.Sqrt(T);
                                double res = A * toch.cordx + B * toch.cordy + C;
                                res = res / T;
                                if (Math.Abs(res) <= Size + 10)
                                {
                                    toch.cordx = rand.Next(101, pictureBox1.Size.Width - 100);
                                    toch.cordy = rand.Next(101, pictureBox1.Size.Height - 100);
                                    foreach (var waw in proНарисован)
                                    {
                                        if (Math.Sqrt(Math.Pow(toch.cordx - waw.cordx, 2) + Math.Pow(toch.cordy - waw.cordy, 2)) < Size + 10)
                                        {
                                            powtor++;
                                            if (powtor > 50)
                                            {
                                                rand = new Random(DateTime.Now.Millisecond * toch.cordx);
                                            }
                                            goto wawfлол;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach(var nod in proНарисован)
            {
                foreach(var wwwprop in nod.connects_out)
                {
                    e.Graphics.DrawLine(Pens.Black, nod.cordx, nod.cordy, wwwprop.cordx, wwwprop.cordy);
                }
            }
            foreach(var nod in proНарисован)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.White), nod.cordx - Size / 2, nod.cordy - Size / 2, Size, Size);
                e.Graphics.DrawEllipse(new Pen(Color.Black), nod.cordx - Size / 2, nod.cordy - Size / 2, Size, Size);
                e.Graphics.DrawString(nod.ID.ToString(), DefaultFont, new SolidBrush(Color.Red), nod.cordx - 5, nod.cordy - 5);
            }
        */}
    }
}
