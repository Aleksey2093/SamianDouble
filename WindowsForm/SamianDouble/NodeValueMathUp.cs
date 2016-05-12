using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamianDouble
{
    /// <summary>
    /// класс считает узлы вверх с условием, что есть известные события
    /// </summary>
    class NodeValueMathUp
    {
        private List<Node_struct> startMathDownСначало(List<Node_struct> list)
        {
            повторитьиззаошибки:
            NodeValueMathDown mathdown = new NodeValueMathDown();
            EditNode editnode = new EditNode();
            try
            {
                bool нуженповтор = false;
                //Parallel.ForEach(list, (nod, state) =>
                foreach(Node_struct nod in list)
                {
                    if (nod.connects_out.Count == 0 && nod.connects_in.Count > 0)
                    {
                        MatrixСмежная[][] см = editnode.getMatrixСмежность(nod, nod.connects_in.Count, nod.props[0].values.Count, list);
                        if (mathdown.getValues_editors(см, nod, list)[0] == -1)
                            нуженповтор = true;
                        if (нуженповтор)
                            break;
                            //state.Break();
                    }
                }//);
                if (нуженповтор)
                    goto повторитьиззаошибки;
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Console.WriteLine("Ошибка в методе startMathDownСначало. " + ex.ToString());
                goto повторитьиззаошибки;
            }
            catch (System.AggregateException ex)
            {
                Console.WriteLine("Ошибка в методе startMathDownСначало. " + ex.ToString());
                goto повторитьиззаошибки;
            }
            return list;
        }

        private int getMatСмежКолСтрок(Node_struct nod)
        {
            Node nodes = new Node();
            int kolvo = nod.props.Count;
            Parallel.ForEach(nod.connects_in, (ot, state) =>
            {
                if (!nodes.getEstProperyTrueFix(ot.props))
                    kolvo *= getMatСмежКолСтрок(ot);
            });
            return kolvo;
        }

        private int getMatCмежКолСтолбцов(Node_struct nod)
        {
            Node nodes = new Node();
            int kolvo = 1;
            //Parallel.ForEach(nod.connects_in, (ot, state) =>
            foreach(Node_struct ot in nod.connects_in)    
            {
                    if (!nodes.getEstProperyTrueFix(ot.props))
                        kolvo += getMatCмежКолСтолбцов(ot);
                }//);
            return kolvo;
        }
        private /*MatrixСмежная[][]*/int getMatЗаполнитель(ref MatrixСмежная[][] matrix, int rows, int column, int h, Node_struct nod)
        {
            Node nodes = new Node();
            for (int i=0,hw=0,hv=0,j=0;i<rows;i++)
            {
            retigoto:
                if (hw < h)
                {
                    matrix[i][column - 1].nod = nod;
                    matrix[i][column - 1].property = nod.props[j];
                    matrix[i][column - 1].value = nod.props[j].values[hv];
                    hw++;
                }
                else
                {
                    j++;
                    if (j >= nod.props.Count)
                    {
                        j = 0; hv++;
                        if (hv >= nod.props[j].values.Count)
                        {
                            hv = 0;
                        }
                    }
                    hw = 0;
                    goto retigoto;
                }
            }
            if (h == 1)
                h=nod.props.Count;
            else
                h*=nod.props.Count;
            /*Parallel.ForEach(nod.connects_in, (nodfr, state) =>
                {
                    if (!nodes.getEstProperyTrueFix(nodfr.props))
                        matrix = getMatЗаполнитель(matrix, rows, column - 1, h, nodfr);
                });*/
            foreach(var nodfr in nod.connects_in)
            {
                if (!nodes.getEstProperyTrueFix(nodfr.props))
                {
                    h = getMatЗаполнитель(ref matrix, rows, column - 1, h, nodfr);
                    for (int i = matrix[0].Length-1; i >=0 ; i--)
                        if (matrix[0][i].nod == null)
                        {
                            column--;
                            break;
                        }
                }
            }
            //return matrix;
            return h;
        }

        private Node_struct MathДетейПоИзвестномуРодителю(Node_struct nod, List<Node_struct> list)
        {
            metkaошибказаполнителя:
            int rows = -1, column = -1;
            Parallel.Invoke(
                () => { rows = getMatСмежКолСтрок(nod); }, () => { column = getMatCмежКолСтолбцов(nod); });
            if (rows == -1 || column == -1)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Ошибка в вычислениях вверх метод - по известному родителю");
                Console.WriteLine("-------------------------------------------");
                return nod;
            }
            else
            {
                Console.WriteLine("Math up info: rows - " + rows + ", column -" + column);
            }
            Node classnode = new Node();
            MatrixСмежная[][] matrix = new MatrixСмежная[rows][];
            for (int i=0;i<rows;i++) 
            {
                matrix[i] = new MatrixСмежная[column]; 
                for (int j = 0; j < column; j++)
                {
                    matrix[i][j] = new MatrixСмежная();
                    matrix[i][j].nod = new Node_struct();
                    matrix[i][j].property = new Propertys_struct();
                    matrix[i][j].value = new double();
                }
            }
            bool ошибказаполнителя = false;
                /*matrix = */getMatЗаполнитель(ref matrix, rows, column, 1, nod);
                Console.WriteLine("Matrix: ");
                for (int i = 0; i < rows;i++ )
                {
                    for (int j=0;j<column;j++)
                    {
                        if (matrix[i][j] == null || matrix[i][j].nod.Name == null)
                            goto metkaошибказаполнителя;
                        Console.Write("["+i+"]["+j+"]: "+matrix[i][j].nod.Name + matrix[i][j].property.name + "\t");
                    }
                    Console.WriteLine();
                }
            if (ошибказаполнителя)
                goto metkaошибказаполнителя;
            double[] massres = new double[rows];
            for (int i = 0; i < rows;i++)
            {
                double proiz = matrix[i][column - 1].value;
                for (int j = 0; j < column - 1; j++)
                    proiz *= matrix[i][j].value;
                massres[i] = proiz;
            }
            for (int j = 0; j < column - 1; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (matrix[i][column - 1].property.proc100)
                    {
                        if (matrix[i][j].property.value_editor == -1)
                            matrix[i][j].property.value_editor = 0;
                        matrix[i][j].property.value_editor += massres[i] / matrix[i][column - 1].property.value_editor_down;
                    }
                }
            }
            return nod;
        }

        private List<Node_struct> startMathUp(List<Node_struct> list)
        {
            //Parallel.For(0, list.Count, (i, state) =>
            for (int i = 0; i < list.Count;i++ )
            {
                foreach (var pppp in list[i].props)
                {
                    if (pppp.proc100 == true && list[i].connects_in.Count > 0)
                    {
                        list[i] = MathДетейПоИзвестномуРодителю(list[i], list);
                        break;
                    }
                }
            }//);
            return list;
        }

        public List<Node_struct> getMathNodesAll(List<Node_struct> list)
        {
            list = startMathDownСначало(list);

            foreach(var nod in list)
            {
                foreach(var waw in nod.props)
                {
                    waw.value_editor = -1;
                }
            }

            list = startMathUp(list);

            foreach(var nod in list)
            {
                foreach(var waw in nod.props)
                {
                    if (waw.value_editor == -1)
                    waw.value_editor = waw.value_editor_down;
                }
            }

            return list;
        }

        public double[] getMathThisNode(List<Node_struct> list, Node_struct node)
        {
            double[] values = new double[node.props.Count];

            list = getMathNodesAll(list);
            for (int i = 0; i < node.props.Count;i++ )
            {
                values[i] = node.props[i].value_editor;
            }

                return values;
        }
    }
}
