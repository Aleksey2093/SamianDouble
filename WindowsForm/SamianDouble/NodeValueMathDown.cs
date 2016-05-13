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

        /*struct MatrixСобиратель
        {
            MatrixСмежная[][] смежность;
            double matrix[,] matrix;
        }*/

        /// <summary>
        /// получение вероятностей
        /// </summary>
        /// <param name="смежность"></param>
        /// <param name="nod"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool getValues_editors(MatrixСмежная[][] смежность, Node_struct nod, List<Node_struct> list)
        {
            Node nodeclass = new Node();
            double[] values = new double[nod.props.Count];
            int n = смежность.Length;
            int m = смежность[0].Length;
            double[,] matrix = new double[n, m];
            for (int i = n-1; i >= 0; i--)
                for (int j = 0; j < m;j++)
                {
                    Node_struct idnod = смежность[i][j].nod;
                    Propertys_struct nameprop = смежность[i][j].property;
                    double v1 = getNodPropsValueEditor(list, idnod, nameprop);
                    matrix[i, j] = v1;
                }
            Console.WriteLine("down nod - " + nod.ID + " " + nod.Name);
            /*Parallel.For(0, m, (j, state) =>*/
            for (int j = 0; j < m;j++ )
            {
                for (int i = 1; i < n; i++)
                {
                    /*if (nodeclass.getEstProperyTrueFix(смежность[i][j].nod.props) == false)
                    {*/
                        double a1 = matrix[0, j];
                        double a2 = matrix[i, j];
                        matrix[0, j] = a1 * a2;
                    /*}
                    else if (смежность[i][j].property.proc100 == false)
                    {
                        matrix[0, j] = matrix[0, j] * 0;
                    }*/
                }
            }//);
            
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = 0;
                for (int j = 0; j < nod.props[i].values.Count; j++)
                {
                    try
                    {
                        values[i] += nod.props[i].values[j] * matrix[0, j];
                    }
                    catch(System.IndexOutOfRangeException ex)
                    {
                        Console.WriteLine("Ошибка индекса (getValues_editors). " + ex.ToString());
                        return false;
                    }
                }
                //values[i] = Math.Round(values[i], 4);
                nod.props[i].value_editor_down = nod.props[i].value_editor = values[i];
            }
            Console.WriteLine("down nod - " + nod.ID + " " + nod.Name);
            return true;
        }
        public double getNodPropsValueEditor(List<Node_struct> list, Node_struct nod, Propertys_struct proppppp)
        {
            double value = -1;
            bool enab = false;
            foreach (var propi in nod.props)
            {
                if (propi.proc100)
                {
                    enab = true;
                    break;
                }
            }
            if (enab /*&& nod.connects_in.Count == 0*/) //тут у нас просто ребенок поэтому ему можем спокойно присвоить 1 и 0
            {
                if (proppppp.proc100)
                    value = 1;
                else
                    value = 0;
            }
            /*else if (enab && nod.connects_in.Count > 0) //а тут раз у нас и родитель и беренок то это событие нужно отлавливать
            {
                //вот этот момент интересне это родитель и сын одновременно
            }*/
            else if (proppppp.values.Count > 1)
            {
            иззаошибки:
                MatrixСмежная[][] см = new EditNode().getMatrixСмежность(nod, nod.connects_in.Count, nod.props[0].values.Count, list);
                if (getValues_editors(см, nod, list)==false)
                    goto иззаошибки;
                value = proppppp.value_editor = proppppp.value_editor_down;//nod.props[i].value_editor;
            }
            else /*if (nod.connects_in.Count > 0)*/
            {
                value = proppppp.value_editor_down = proppppp.value_editor = proppppp.values[0];//nod.props[i].values[0];
            }
            return value;
        }
    }
}
