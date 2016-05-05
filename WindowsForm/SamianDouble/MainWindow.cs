using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        public static List<Nodes_struct> listnodes { get;set; }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            listnodes = new List<Nodes_struct>();
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

        /// <summary>
        /// перерисовывает дерево узлов
        /// </summary>
        /// <returns>возвращает успех работы</returns>
        private bool treeListReplace()
        {
            treeView1.Nodes.Clear();
            TreeNode n1 = new TreeNode("Родители");
            TreeNode n2 = new TreeNode("Промежуточные");
            TreeNode n3 = new TreeNode("Дети");
            TreeNode n4 = new TreeNode("Несвязные");
            for (int i = 0; i < listnodes.Count; i++)
            {
                Nodes_struct node_st = listnodes[i];
                TreeNode nod = new TreeNode();
                nod.Name = node_st.id.ToString() + node_st.name;
                nod.Text = node_st.name;
                for (int j = 0; j < node_st.props.Count; j++)
                {
                    TreeNode nod_p = new TreeNode();
                    nod_p.Name = node_st.id.ToString() + node_st.props[j].name;
                    nod_p.Text = node_st.props[j].name;
                    nod.Nodes.Add(nod_p);
                }
                if (node_st.connects_in.Count == 0)
                {
                    if (node_st.connect_out.Count == 0)
                        n4.Nodes.Add(nod);
                    else
                        n1.Nodes.Add(nod);
                }
                else
                {
                    if (node_st.connect_out.Count == 0)
                        n3.Nodes.Add(nod);
                    else
                        n2.Nodes.Add(nod);
                }
            }
            treeView1.Nodes.Add(n1);
            treeView1.Nodes.Add(n2);
            treeView1.Nodes.Add(n3);
            treeView1.Nodes.Add(n4);
            treeView1.ExpandAll();
            //в случае успеха в конце поадаем в удачный выход
            return true; 
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
                for (int j=0;j<treeView1.Nodes.Count;j++)
                {
                    Parallel.For(0, treeView1.Nodes[j].Nodes.Count, (i, state) =>
                        {
                            treeView1.Nodes[j].Nodes[i].BackColor = Color.White;
                        });
                }
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
                    for (int j = 0; j < listnodes[index].connect_out.Count; j++)
                    {
                        var tr = treeView1.Nodes[1].Nodes[i];
                        var ln = listnodes[index].connect_out[j];
                        if (tr.Name == ln.ID.ToString() + ln.Name)
                        {
                            tr.BackColor = Color.LightGreen;
                        }
                    }
                });
                Parallel.For(0, treeView1.Nodes[2].Nodes.Count, (i, state) =>
                {
                    for (int j = 0; j < listnodes[index].connect_out.Count; j++)
                    {
                        var tr = treeView1.Nodes[2].Nodes[i];
                        var ln = listnodes[index].connect_out[j];
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
    }
}
