using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamianDouble
{
    /// <summary>
    /// Класс обновляет узлы при удалении или создании связей
    /// </summary>
    class UpdateNode
    {
        /// <summary>
        /// возвращает переделанный нод после создания входящей связи
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="nod">модицифируемый узел, он же возвращается</param>
        /// <param name="id_other">ид узла связь из которого идет в текущий узел</param>
        /// <returns>модицифированный узел</returns>
        public List<Nodes_struct> updateNodeConnectIn(List<Nodes_struct> list, Nodes_struct nod, int id_other)
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
        public List<Nodes_struct> updateNodeConnectOut(List<Nodes_struct> list, Nodes_struct nod, int id_other)
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

        /// <summary>
        /// Возвращает список после удаления входящей связи в текущем узле
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="nod">текущий узел</param>
        /// <param name="id_other">ид узла с которым разорвана связь</param>
        /// <returns></returns>
        public List<Nodes_struct> deleteNodeConnectIn(List<Nodes_struct> list, Nodes_struct nod, int id_other)
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
        /// <summary>
        /// Возвращает список после удаления исходящей связи в текущем узле
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="nod">текущий узел</param>
        /// <param name="id_other">ид узла с которым разорвана связь</param>
        /// <returns></returns>
        public List<Nodes_struct> deleteNodeConnectOut(List<Nodes_struct> list, Nodes_struct nod, int id_other)
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

        public bool provConnЦикличность(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            List<int[]> conns = new List<int[]>();
            {
            int[] con1 = new int[2];
            if (nod.id > id_other)
            {
                con1[1] = nod.id;
                con1[0] = id_other;
            }
            else
            {
                con1[0] = nod.id;
                con1[1] = id_other;
            }
            conns.Add(con1);
            }
            Parallel.ForEach(list, (n1, state) =>
                {
                    Parallel.ForEach(nod.connects_in, (c1, statec1) =>
                        {
                            int[] con = new int[2];
                            if (n1.id > c1.ID)
                            {
                                con[1] = n1.id;
                                con[0] = c1.ID;
                            }
                            else
                            {
                                con[0] = n1.id;
                                con[1] = c1.ID;
                            }
                        });
                    Parallel.ForEach(nod.connect_out, (c2, statec2) =>
                    {
                        int[] con = new int[2];
                        if (n1.id > c2.ID)
                        {
                            con[1] = n1.id;
                            con[0] = c2.ID;
                        }
                        else
                        {
                            con[0] = n1.id;
                            con[1] = c2.ID;
                        }
                    });
                });
            for (int i = 0; i < conns.Count-1;i++ )
            {
                for (int j=i+1;j<conns.Count;j++)
                {
                    if (conns[i] == conns[j] && i != j)
                    {
                        conns.RemoveAt(j);
                        j--;
                    }
                }
            }
            /*пройтись по полученном циклу и если он зацикливается то это плохо.
            вычислить максимально возможное количество подключений для данного количества 
            узлов и сравнивать с этим числом если что когда буду проходить по связи одного узла вглубь*/
            return true;
        }
    }
}
