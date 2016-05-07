﻿using System;
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
    public class Node_struct
    {
        private int id;
        private string name;
        public List<Propertys_struct> props;
        public List<Node_struct> connects_in;
        public List<Node_struct> connects_out;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    /*public class Connect_list
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
    }*/
    public class Propertys_struct
    {
        public String name;
        public List<double> values;
        public double value_editor;
        public bool proc100 = false;
    }
    class Node
    {
        public int get_new_id(List<Node_struct> list)
        {
            int new_id = -1;

            for (int i = 0; i < list.Count; i++)
            {
                if (new_id < list[i].ID)
                new_id = list[i].ID;
            }
            return new_id + 1;
        }
        public List<Node_struct> getNewNode(List<Node_struct> list)
        {
            Node_struct nod = new Node_struct();
            nod.ID = get_new_id(list);
            nod.Name = "New Node " + nod.ID;
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

            nod.connects_out = new List<Node_struct>();
            nod.connects_in = new List<Node_struct>();

            list.Add(nod);
            return list;
        }
        public bool provNewNameBool(List<Node_struct> list, String name)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].Name == name)
                    return true;
            return false;
        }
        public List<Node_struct> nodeNameEdit(TreeNode nod, List<Node_struct> list)
        {
            //узнаем, что мы изменили свойство или название самого узла
            if (nod.Level == 1)
            {
                for (int i = 0; i < list.Count; i++)
                    if ((list[i].ID.ToString() + list[i].Name) == nod.Name)
                    {
                        list[i].Name = nod.Text;
                        return list;
                    }
            }
            else if (nod.Level == 2)
            {
                for (int i = 0; i < list.Count; i++)
                    for (int j = 0; j < list[i].props.Count; j++)
                        if ((list[i].ID.ToString() + list[i].props[j].name) == nod.Name)
                        {
                            list[i].props[j].name = nod.Text;
                            return list;
                        }
            }
            return list;
        }
        public List<Node_struct> nodeAddNewProperty(TreeNode nod, List<Node_struct> list)
        {
            if (nod.Level == 1)
            {
                Parallel.ForEach(list,(nownod,state)=>
                {
                    if ((nownod.ID.ToString() + nownod.Name) == nod.Name)
                    {
                        Propertys_struct pr = new Propertys_struct();
                        pr.name = "Props" + (1 + nownod.props.Count);
                        pr.values = new List<double>();
                        Parallel.ForEach(nownod.props[0].values, (val) =>
                            {
                                pr.values.Add(0);
                            });
                        nownod.props.Add(pr);
                        state.Break();
                    }
                });
            }
            return list;
        }
        public int getSelectNode(TreeNode nod, List<Node_struct> list)
        {
            int idвлист = -1;
            Parallel.For(0,list.Count, (i, state) =>
            {
                if ((list[i].ID.ToString() + list[i].Name) == nod.Name)
                {
                    idвлист = i;
                    state.Break();
                }
            });
            if (idвлист == -1)
                MessageBox.Show("Ошибка поиска ид");
            return idвлист;
        }
        /// <summary>
        /// возвращает список в котором удалено свойство текущего узла
        /// </summary>
        /// <param name="nod">узел с дерева treeview</param>
        /// <param name="listnodes">список узлов</param>
        /// <returns></returns>
        public List<Node_struct> nodeDeletePropertyThisNod(TreeNode nod, List<Node_struct> listnodes)
        {
            Parallel.ForEach(listnodes, (nodlist, state) =>
                {
                    if (nodlist.ID.ToString() + nodlist.Name == nod.Parent.Name)
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

        public List<Node_struct> nodeFixPropertyThisNod(TreeNode nod, List<Node_struct> listnodes)
        {
            bool ifi1;
            bool ifi2;
            if (nod.BackColor != Color.Tomato)
            {
                ifi1 = true;
                ifi2 = false;
            }
            else
            {
                ifi1 = false;
                ifi2 = false;
            }
            Parallel.ForEach(listnodes, (nodlist, state) =>
            {
                if (nodlist.ID.ToString() + nodlist.Name == nod.Parent.Name)
                {
                    Parallel.For(0, nodlist.props.Count, (i, stateдва) =>
                    {
                        if (nodlist.props[i].name == nod.Text)
                        {
                            nodlist.props[i].proc100 = ifi1;
                            stateдва.Break();
                        }
                        else
                        {
                            nodlist.props[i].proc100 = ifi2;
                        }
                    });
                    state.Break();
                }
            });
            return listnodes;
        }

        /// <summary>
        /// получить нод по ид
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="id">ид искомого узла</param>
        /// <returns>Node_struct</returns>
        public Node_struct getNodeПоИд(List<Node_struct> list, int id)
        {
            Node_struct nod = null;

            Parallel.ForEach(list, (nodтекущий, state) =>
                {
                    if (nodтекущий.ID == id)
                    {
                        nod = nodтекущий;
                        state.Break();
                    }
                });
            return nod;
        }

        /// <summary>
        /// Возвращает вариант ответа:
        /// 0 - ничего нет
        /// 1 - есть узлы с пометкой известный среди детей, но не текущий
        /// 2 - есть узлы с пометкой известный среди детей, текущий тоже известен
        /// 3 - текущий известен остальные нет
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="nod">текущий узел</param>
        /// <returns>int</returns>
        public int getProvBoolПроверкаИзвестия(List<Node_struct> list, Node_struct nownod)
        {
            int res = 0;
            Parallel.ForEach(list, (nod, stateузлы) =>
                {
                    if (nod.ID != nownod.ID)
                    {
                        bool child = false;
                        Parallel.ForEach(nownod.connects_out, (связь, stateсвязь) =>
                            {
                                if (связь.ID == nod.ID)
                                {
                                    child = true;
                                    stateсвязь.Break();
                                }
                            });
                        if (child)
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

        /// <summary>
        /// Проверка на зацикленность перед установкой связи, возвращет true, если зацикливается, false если не зацикливается
        /// </summary>
        /// <param name="list">Список уздлв</param>
        /// <param name="nod">узел текущий</param>
        /// <param name="idconnect">ид узла с которым устанавливается связь</param>
        /// <param name="forback">в какую сторону связь true к детям, false - к родителям</param>
        /// <param name="ignorforback">игнорирования параметра направления проверки</param>
        /// <returns></returns>
        public bool getProvBoolЗацикленность(List<Node_struct> list, Node_struct nod, Node_struct friend, bool forback, bool ignorforback)
        {
            if (friend == null)
            {
                MessageBox.Show("В функцию проверки узлов на цикл, попал ид несуществующего нода. Возможно ваша база повреждена, попробуйте загрузить последнюю работую версию из файла", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return true;
            }
            bool ifi = false;
            if (forback || ignorforback)
            {
                Parallel.ForEach(friend.connects_out, (nodfriendfor, state) =>
                {
                    if (nodfriendfor.connects_out.Count == 0)
                        state.Break();
                    else
                    {
                        Parallel.ForEach(nodfriendfor.connects_out, (tmpnodfor, statefortwo) =>
                        {
                            if (tmpnodfor.ID == nod.ID)
                            {
                                ifi = true;
                                statefortwo.Break();
                            }
                            else
                            {
                                ifi = getProvBoolЗацикленность(list, nod, tmpnodfor, true, false);
                                if (ifi)
                                    statefortwo.Break();
                            }
                        });
                        if (ifi)
                            state.Break();
                    }
                });
            }
            if (forback == false || ignorforback)
            {
                Parallel.ForEach(friend.connects_in, (nodfriendfor, state) =>
                    {
                        if (nodfriendfor.connects_in.Count == 0)
                            state.Break();
                        else
                        {
                            Parallel.ForEach(nodfriendfor.connects_in, (tmpnodfor, statefortwo) =>
                                {
                                    if (tmpnodfor.ID == nodfriendfor.ID)
                                    {
                                        ifi = true;
                                        statefortwo.Break();
                                    }
                                    else
                                    {
                                        ifi = getProvBoolЗацикленность(list, nod, tmpnodfor, false, false);
                                        if (ifi)
                                            statefortwo.Break();
                                    }
                                });
                            if (ifi)
                                state.Break();
                        }
                    });
            }
            return ifi;
        }
    }
}
