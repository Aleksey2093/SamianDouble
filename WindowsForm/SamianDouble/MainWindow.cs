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

        List<Node.Nodes_struct> listnodes;

        private void MainWindow_Load(object sender, EventArgs e)
        {
            listnodes = new List<Node.Nodes_struct>();
        }

        private void button_AddNode_Click(object sender, EventArgs e)
        {
            Node a = new Node();
            listnodes = a.get_new_node(listnodes);
        }
    }
}
