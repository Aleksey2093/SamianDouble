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
    public class Nodes_struct
    {
        public int id;
        public string name;
        public List<Propertys_struct> props;
        public List<Connect_list> connects_in;
        public List<Connect_list> connect_out;
    }
    public class Connect_list
    {
        public int conid;
        public string connodename;
        public int ID
        {
            get { return conid; }
            set { conid = value; }
        }
        public string Name
        {
            get { return connodename; }
            set { connodename = value; }
        }
    }
    public class Propertys_struct
    {
        public String name;
        public List<double> values;
        public double value_editor;
        public bool proc100 = false;
    }
    class Node
    {
        public int get_new_id(List<Nodes_struct> list)
        {
            int new_id = -1;

            for (int i = 0; i < list.Count; i++)
            {
                if (new_id < list[i].id)
                new_id = list[i].id;
            }
            return new_id + 1;
        }
        public List<Nodes_struct> getNewNode(List<Nodes_struct> list)
        {
            Nodes_struct nod = new Nodes_struct();
            nod.id = get_new_id(list);
            nod.name = "New Node " + nod.id;
            nod.props = new List<Propertys_struct>();

            Propertys_struct p_s = new Propertys_struct();
            p_s.name = "Props1";
            p_s.value_editor = 0.5;
            p_s.values = new List<double>();
            p_s.values.Add(0.5);
            nod.props.Add(p_s);

            p_s = new Propertys_struct();
            p_s.name = "Props2";
            p_s.value_editor = 0.5;
            p_s.values = new List<double>();
            p_s.values.Add(0.5);
            nod.props.Add(p_s);

            nod.connect_out = new List<Connect_list>();
            nod.connects_in = new List<Connect_list>();

            list.Add(nod);
            return list;
        }
        public bool provNewNameBool(List<Nodes_struct> list, String name)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].name == name)
                    return true;
            return false;
        }
        public List<Nodes_struct> nodeNameEdit(TreeNode nod, List<Nodes_struct> list)
        {
            //узнаем, что мы изменили свойство или название самого узла
            if (nod.Level == 1)
            {
                for (int i = 0; i < list.Count; i++)
                    if ((list[i].id.ToString() + list[i].name) == nod.Name)
                    {
                        list[i].name = nod.Text;
                        return list;
                    }
            }
            else if (nod.Level == 2)
            {
                for (int i = 0; i < list.Count; i++)
                    for (int j = 0; j < list[i].props.Count; j++)
                        if ((list[i].id.ToString() + list[i].props[j].name) == nod.Name)
                        {
                            list[i].props[j].name = nod.Text;
                            return list;
                        }
            }
            return list;
        }
        public int getSelectNode(TreeNode nod, List<Nodes_struct> list)
        {
            for (int i = 0; i < list.Count; i++)
                if ((list[i].id.ToString() + list[i].name) == nod.Name)
                    return i;
            return -1;
        }
    }
}
