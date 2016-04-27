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
    class Node
    {

        public struct Nodes_struct
        {
            public Label label;
            public int id;
            public TextBox name;
            public List<Propertys_struct> props;
            public ListBox listprops;
            public List<int> connects;
        };
        public struct Propertys_struct
        {
            public String name;
            public double value;
        };

        public int get_new_id(List<Nodes_struct> list)
        {
            int new_id = -1;

            for (int i = 0; i < list.Count;i++ )
            {
                if (new_id < list[i].id)
                    new_id = list[i].id;
            }
            return new_id;
        }
        public List<Nodes_struct> get_new_node(List<Nodes_struct> list)
        {
            int new_id = get_new_id(list);
            Nodes_struct nod = new Nodes_struct();
            nod.id = new_id;
            nod.label = new Label(); nod.label.Text = nod.id.ToString();
            nod.name = new TextBox(); nod.name.Text = "New Node";
            nod.props = new List<Propertys_struct>();
            nod.listprops = new ListBox();
            nod.listprops.DataSource = nod.props;

            list.Add(nod);
            return list;
        }
    }
}
