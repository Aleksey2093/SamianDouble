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
        public List<Nodes_struct> nodeAddNewProperty(TreeNode nod, List<Nodes_struct> list)
        {
            if (nod.Level == 1)
            {
                for (int i = 0; i < list.Count; i++)
                    if ((list[i].id.ToString() + list[i].name) == nod.Name)
                    {
                        Propertys_struct pr = new Propertys_struct();
                        pr.name = "Props" + (1+list[i].props.Count);
                        pr.values = new List<double>();
                        Parallel.ForEach(list[i].props[0].values, (val) =>
                            {
                                pr.values.Add(0);
                            });
                        list[i].props.Add(pr);
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
        /// <summary>
        /// возвращает список в котором удалено свойство текущего узла
        /// </summary>
        /// <param name="nod">узел с дерева treeview</param>
        /// <param name="listnodes">список узлов</param>
        /// <returns></returns>
        public List<Nodes_struct> nodeDeletePropertyThisNod(TreeNode nod, List<Nodes_struct> listnodes)
        {
            Parallel.ForEach(listnodes, (nodlist, state) =>
                {
                    if (nodlist.id.ToString()+nodlist.name == nod.Parent.Name)
                    {
                        Parallel.For(0,nodlist.props.Count, (i, stateдва) =>
                            {
                                if (nodlist.props[i].name == nod.Text)
                                {
                                    nodlist.props.RemoveAt(i);
                                    stateдва.Break();
                                }
                            });
                        state.Break();
                    }
                });

            return listnodes;
        }
        /// <summary>
        /// Возвращает вариант ответа:
        /// 0 - ничего нет
        /// 1 - есть узлы с пометкой известный, но не текущий
        /// 2 - есть узлы с пометкой известный, текущий тоже известен
        /// 3 - текущий известен остальные нет
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="nod">текущий узел</param>
        /// <returns>int</returns>
        public int GetProvBoolПроверкаИзвестия(List<Nodes_struct> list, Nodes_struct nownod)
        {
            int res = 0;
            Parallel.ForEach(list, (nod, stateузлы) =>
                {
                    if (nod.id != nownod.id)
                    {
                        Parallel.ForEach(nod.props, (свойство, stateсвойство) =>
                            {
                                if (свойство.proc100 == true)
                                {
                                    res = 1; //изсвестные есть
                                    stateсвойство.Break();
                                }
                            });
                        if (res == 1)
                            stateузлы.Break();
                    }
                });
            Parallel.ForEach(nownod.props, (свойство, stateсвойство) =>
                {
                    if (свойство.proc100 == true)
                    {
                        if (res == 1)
                            res = 2; //остальные + текущий
                        else if (res == 0)
                            res = 3; //только текущий
                        stateсвойство.Break();
                    }
                });
            return res;
        }
    }
}
