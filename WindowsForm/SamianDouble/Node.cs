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
                        pr.values.AddRange(list[i].props[0].values);
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
    }
    class UpdateNode
    {
        /// <summary>
        /// возвращает переделанный нод после создания входящей связи
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="nod">модицифируемый узел, он же возвращается</param>
        /// <param name="id_other">ид узла связь из которого идет в текущий узел</param>
        /// <returns>модицифированный узел</returns>
        public List<Nodes_struct> UpdateNodeConnectIn(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            Nodes_struct other_nod = new Nodes_struct();
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].id == id_other)
                {
                    other_nod = list[i];
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно добавить связи: исходяющую связь в другой нод и в наш входяющую
            Connect_list con = new Connect_list();
            con.ID = other_nod.id;
            con.Name = other_nod.name;
            nod.connects_in.Add(con);

            con = new Connect_list();
            con.ID = nod.id;
            con.Name = nod.name;
            other_nod.connect_out.Add(con);
            //связи созданы, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j = 0; j < nod.props.Count; j++)
            {
                double[] vals = new double[nod.props[j].values.Count * other_nod.props.Count];
                Parallel.For(0, vals.Length, (i, state) =>
                {
                    vals[i] = 0.5;
                });
                nod.props[j].values.Clear();
                nod.props[j].values.AddRange(vals);
            }
            return list;
        }
        /// <summary>
        /// возвращает переделанный список после создания связи исходящей связи
        /// </summary>
        /// <param name="list">список узлов, он же возвращается</param>
        /// <param name="nod">модифицируемый узел</param>
        /// <param name="id_other">ид узла в который направлена создаваемая связь</param>
        /// <returns>модифицированный список</returns>
        public List<Nodes_struct> UpdateNodeConnectOut(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            Nodes_struct other_nod = new Nodes_struct();
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].id == id_other)
                {
                    other_nod = list[i];
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно добавить связи: исходяющую связь в другой нод и в наш входяющую
            Connect_list con = new Connect_list();
            con.ID = nod.id;
            con.Name = nod.name;
            other_nod.connects_in.Add(con);

            con = new Connect_list();
            con.ID = other_nod.id;
            con.Name = other_nod.name;
            nod.connect_out.Add(con);
            //связи созданы, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j = 0; j < other_nod.props.Count; j++)
            {
                //List<double> vals = nod.props[j].values; //изначальное количество и значения до увеличения значений данного свойства
                double[] vals = new double[nod.props[j].values.Count * other_nod.props.Count];
                Parallel.For(0, vals.Length, (i, state) =>
                {
                    vals[i] = 0.5;
                });
                other_nod.props[j].values.Clear();
                other_nod.props[j].values.AddRange(vals);
            }
            return list;
        }

        public List<Nodes_struct> DeleteNodeConnectIn(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            int jb = 0;
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].id == id_other)
                {
                    for (int j = 0; j < list[i].connect_out.Count; j++)
                    {
                        if (list[i].connect_out[j].ID == nod.id)
                        {
                            list[i].connect_out.RemoveAt(j);
                            jb = i;
                            break;
                        }
                    }
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно удалить связи: исходяющую связь в другой нод и в наш входяющую
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (nod.id == list[i].id)
                {
                    for (int j = 0; j < list[i].connects_in.Count; j++)
                    {
                        if (list[i].connects_in[j].ID == id_other)
                        {
                            nod.connects_in.RemoveAt(j);
                            break;
                        }
                    }
                    state.Break();
                }
            });
            //связи удалены, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j = 0; j < nod.props.Count; j++)
            {
                List<double> vals = list[jb].props[j].values; //изначальное количество и значения до уменьшения значений данного свойства
                int len = list[jb].props.Count;
                int count = nod.props[j].values.Count;
                nod.props[j].values.RemoveRange(count / len, count - count / len); // на этом шаге уменьшаем в два раза количество значений
            }
            return list;
        }

        public List<Nodes_struct> DeleteNodeConnectOut(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            int jb = 0;
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].id == id_other)
                {
                    for (int j = 0; j < list[i].connects_in.Count; j++)
                    {
                        if (list[i].connects_in[j].ID == nod.id)
                        {
                            list[i].connects_in.RemoveAt(j);
                            jb = i;
                            break;
                        }
                    }
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно удалить связи: исходяющую связь в другой нод и в наш входяющую
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (nod.id == list[i].id)
                {
                    for (int j = 0; j < list[i].connect_out.Count; j++)
                    {
                        if (list[i].connect_out[j].ID == id_other)
                        {
                            list[i].connect_out.RemoveAt(j);
                            break;
                        }
                    }
                    state.Break();
                }
            });
            //связи удалены, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j = 0; j < list[jb].props.Count; j++)
            {
                List<double> vals = nod.props[j].values; //изначальное количество и значения до уменьшения значений данного свойства
                int len = nod.props.Count;
                int count = list[jb].props[j].values.Count;
                list[jb].props[j].values.RemoveRange(count / len, count - count / len); // на этом шаге уменьшаем в два раза количество значений
            }
            return list;
        }
    }

    class NodeValueMath
    {
        public struct propсмежность
        {
            public int id;
            public string prop_name;
        }
        /// <summary>
        /// получение вероятностей
        /// </summary>
        /// <param name="смежность"></param>
        /// <param name="nod"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public double[] values_editors(EditNode.propсмежность[][] смежность, Nodes_struct nod, List<Nodes_struct> list)
        {
            double[] values = new double[nod.props.Count];
            int n = смежность.Length;
            int m = смежность[0].Length;
            double[,] matrix = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    int idnod = смежность[i][j].id;
                    string nameprop = смежность[i][j].prop_name;
                    double v1 = getNodPropsValueEditor(list, idnod, nameprop);
                    matrix[i, j] = v1;
                }
            for (int j = 0; j < m; j++)
            {
                for (int i=1;i < n;i++)
                {
                    matrix[0, j] = matrix[0, j] * matrix[i, j];
                }
            }
            for (int i = 0; i < values.Length;i++ )
            {
                values[i] = 0;
                for (int j=0;j<nod.props[i].values.Count;j++)
                {
                    values[i] += nod.props[i].values[j] * matrix[0, j];
                }
                values[i] = Math.Round(values[i], 4);
                nod.props[i].value_editor = values[i];
                
            }
            return values;
        }
        public double getNodPropsValueEditor(List<Nodes_struct> list, int id,string nameprop)
        {
            Nodes_struct nod = new Nodes_struct();

            Parallel.For(0, list.Count, (i, state) =>
                {
                    if (list[i].id == id)
                    {
                        nod = list[i];
                        state.Break();
                    }
                });
            double value = -1;
            Parallel.For(0, nod.props.Count, (i, state) =>
                {
                    if (nod.props[i].name == nameprop)
                    {
                        if (nod.props[i].values.Count > 1)
                        {
                            EditNode.propсмежность[][] см = new EditNode().getMatrixСмежность(nod, nod.connects_in.Count, nod.props[0].values.Count, list);
                            values_editors(см, nod, list);
                            value = nod.props[i].value_editor;
                        }
                        else
                        {
                            value = nod.props[i].values[0];
                        }
                    }
                });
            return value;
        }
    }
}
