using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamianDouble
{
    /// <summary>
    /// класс считает узлы сверху вниз с условием, что отсутствуют известные события
    /// </summary>
    class NodeValueMathDown
    {
        /// <summary>
        /// получение вероятностей
        /// </summary>
        /// <param name="смежность"></param>
        /// <param name="nod"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public double[] getValues_editors(EditNode.propсмежность[][] смежность, Node_struct nod, List<Node_struct> list)
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
                for (int i = 1; i < n; i++)
                {
                    matrix[0, j] = matrix[0, j] * matrix[i, j];
                }
            }
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = 0;
                for (int j = 0; j < nod.props[i].values.Count; j++)
                {
                    values[i] += nod.props[i].values[j] * matrix[0, j];
                }
                values[i] = Math.Round(values[i], 4);
                nod.props[i].value_editor = values[i];
            }
            return values;
        }
        public double getNodPropsValueEditor(List<Node_struct> list, int id, string nameprop)
        {
            Node_struct nod = new Node_struct();

            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].ID == id)
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
                    bool enab = false;
                    foreach(var propi in nod.props)
                    {
                        if (propi.proc100)
                        {
                            enab = true;
                            break;
                        }
                    }
                    if (enab)
                    {
                        if (nod.props[i].proc100)
                            value = 1;
                        else
                            value = 0;
                    }
                    else if (nod.props[i].values.Count > 1)
                    {
                        EditNode.propсмежность[][] см = new EditNode().getMatrixСмежность(nod, nod.connects_in.Count, nod.props[0].values.Count, list);
                        getValues_editors(см, nod, list);
                        value = nod.props[i].value_editor;
                        state.Break();
                    }
                    else
                    {
                        value = nod.props[i].values[0];
                        state.Break();
                    }
                }
            });
            return value;
        }
    }
}
