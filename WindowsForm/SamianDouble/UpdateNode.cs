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
        public List<Node_struct> updateNodeConnectIn(List<Node_struct> list, Node_struct nod, int id_other)
        {
            Node_struct other_nod = new Node_struct();
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].ID == id_other)
                {
                    other_nod = list[i];
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });


            nod.connects_in.Add(other_nod);
            other_nod.connects_out.Add(nod);


            for (int j = 0; j < nod.props.Count; j++)
            {
                int pos = 0;
                Parallel.For(0, nod.props[j].values.Count * other_nod.props.Count-nod.props[j].values.Count, (i, state) =>
                {
                    nod.props[j].values.Add(nod.props[j].values[pos]);
                    pos++;
                });
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
        public List<Node_struct> updateNodeConnectOut(List<Node_struct> list, Node_struct nod, int id_other)
        {
            Node_struct other_nod = new Node_struct();
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].ID == id_other)
                {
                    other_nod = list[i];
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно добавить связи: исходяющую связь в другой нод и в наш входяющую

            other_nod.connects_in.Add(nod);
            nod.connects_out.Add(other_nod);

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
        public List<Node_struct> deleteNodeConnectIn(List<Node_struct> list, Node_struct nod, int id_other)
        {
            int jb = 0;
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].ID == id_other)
                {
                    for (int j = 0; j < list[i].connects_out.Count; j++)
                    {
                        if (list[i].connects_out[j].ID == nod.ID)
                        {
                            list[i].connects_out.RemoveAt(j);
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
                if (nod.ID == list[i].ID)
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
        public List<Node_struct> deleteNodeConnectOut(List<Node_struct> list, Node_struct nod, int id_other)
        {
            int jb = 0;
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].ID == id_other)
                {
                    for (int j = 0; j < list[i].connects_in.Count; j++)
                    {
                        if (list[i].connects_in[j].ID == nod.ID)
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
                if (nod.ID == list[i].ID)
                {
                    for (int j = 0; j < list[i].connects_out.Count; j++)
                    {
                        if (list[i].connects_out[j].ID == id_other)
                        {
                            list[i].connects_out.RemoveAt(j);
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
}
