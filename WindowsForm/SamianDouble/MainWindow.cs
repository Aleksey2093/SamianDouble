using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamianDouble
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public List<Nodes_struct> listnodes { get;set; }

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
            if (e.Label.Length < 2)
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
                Nodes_struct[] array = new Nodes_struct[listnodes.Count];
                listnodes.CopyTo(array);
                array = ((Nodes_struct[])array.Clone());
                ed.copyDataNew(i, ((Nodes_struct[])array.Clone()).ToList<Nodes_struct>());
                ed.ShowDialog();
                treeListReplace();
            }
        }

    }
}
