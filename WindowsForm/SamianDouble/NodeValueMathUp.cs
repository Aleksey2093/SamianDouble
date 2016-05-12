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
                //bool нуженповтор = false;
                /*Parallel.ForEach(list, (nod, state) =>*/
                foreach(var nod in list)
                {
                    if (nod.connects_out.Count == 0 && nod.connects_in.Count > 0)
                    {
                        MatrixСмежная[][] см = editnode.getMatrixСмежность(nod, nod.connects_in.Count, nod.props[0].values.Count, list);
                        if (mathdown.getValues_editors(см, nod, list) == false) goto повторитьиззаошибки;
                           /* нуженповтор = true;
                        if (нуженповтор)
                            state.Break();*/
                    }
                }//);
                /*if (нуженповтор)
                    goto повторитьиззаошибки;*/
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

        private int getMatСмежКолСтрок(Node_struct nod, bool ifi)
        {
            Node nodes = new Node();
            int kolvo = nod.props.Count;
            //Parallel.ForEach(nod.connects_in, (ot, state) =>
            foreach(var ot in nod.connects_in)
            {
                if (!nodes.getEstProperyTrueFix(ot.props))
                    kolvo *= getMatСмежКолСтрок(ot,false);
                //else
            }//);
            return kolvo;
        }

        private int getMatCмежКолСтолбцов(Node_struct nod, bool ifi)
        {
            Node nodes = new Node();
            int kolvo = 1;
            //Parallel.ForEach(nod.connects_in, (ot, state) =>
            foreach(var ot in nod.connects_in)
                {
                    if (!nodes.getEstProperyTrueFix(ot.props))
                        kolvo += getMatCмежКолСтолбцов(ot,false);
                }//);
            return kolvo;
        }

        private struct MatЗаполнитель
        {
            public MatrixСмежная[][] matrix;
            public int rows;
            public int column;
            public int h;
        }

        private /*MatrixСмежная[][]*/ MatЗаполнитель getMatЗаполнитель(/*MatrixСмежная[][]*/ MatЗаполнитель mat, /*int rows, int column, int h, */Node_struct nod, bool izvest)
        {
            if (mat.column == 0)
            {
                Console.WriteLine("Заполнитель уперся в нулевой столбец");
                return mat;
            }
            Console.WriteLine("Заполнитель id nod - " + nod.ID);
            Node nodes = new Node();
            MatrixСмежная[][] matrix = mat.matrix;
            int rows = mat.rows, column = mat.column, h = mat.h;
            for (int i=0,hw=0,hv=0,j=0;i<rows;i++)
            {
            retigoto:
                if (hw < h)
                {
                    try
                    {
                        if (matrix[i][column - 1] == null)
                            matrix[i][column - 1] = new MatrixСмежная();
                        matrix[i][column - 1].nod = nod;
                    }
                    catch(System.NullReferenceException ex)
                    {
                        Console.WriteLine("Нулевой элемент матрицы создаем указатель. " + ex.Message);
                        matrix[i][column - 1] = new MatrixСмежная();
                        goto retigoto;
                    }
                    matrix[i][column - 1].property = nod.props[j];
                    //if (izvest == false)
                        matrix[i][column - 1].value = nod.props[j].values[hv];
                    /*else if (nod.props[j].proc100)
                        matrix[i][column - 1].value = 1;
                    else
                        matrix[i][column - 1].value = 0;*/
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
            mat.matrix = matrix;
            mat.rows = rows;
            mat.column = column-1;
            mat.h = h;
            /*Parallel.ForEach(nod.connects_in, (nodfr, state) =>
                {
                    if (!nodes.getEstProperyTrueFix(nodfr.props))
                        mat = getMatЗаполнитель(mat, nodfr);
                        //mat = getMatЗаполнитель(mat, rows, column - 1, h, nodfr);
                });*/
            List<Node_struct> incon = new List<Node_struct>();
            incon = nod.connects_in;
            /*for (int i = 0; i < incon.Count; i++)
                for (int j = 0; j < incon.Count;j++ )
                {
                    if (incon[i].ID>incon[j].ID)
                    {
                        Node_struct aw = incon[i];
                        incon[i] = incon[j];
                        incon[j] = incon[i];
                    }
                }*/
            bool waw = false;
                    foreach (var nodf in nod.connects_in)
                    {
                        if (!nodes.getEstProperyTrueFix(nodf.props))
                        {
                            mat = getMatЗаполнитель(mat, nodf, false);
                        }
                    }
            return mat;
        }

        private List<Node_struct> MathДетейПоИзвестномуРодителю(Node_struct nod, List<Node_struct> list)
        {
            metkaошибказаполнителя:
            int rows = -1, column = -1;
            //Parallel.Invoke(
               // () => { rows = getMatСмежКолСтрок(nod,false); }, () => { column = getMatCмежКолСтолбцов(nod,false); });
            rows = getMatСмежКолСтрок(nod, false); column = getMatCмежКолСтолбцов(nod, false);
            if (rows == -1 || column == -1)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Ошибка в вычислениях вверх метод - по известному родителю");
                Console.WriteLine("-------------------------------------------");
                return list;
            }
            else
            {
                Console.WriteLine("Math up info: rows - " + rows + ", column -" + column);
            }
            Node classnode = new Node();
            MatrixСмежная[][] matrix = new MatrixСмежная[rows][];
            Parallel.For(0, rows, (i, state) => { matrix[i] = new MatrixСмежная[column]; });
            MatЗаполнитель mat = new MatЗаполнитель();
            mat.matrix = matrix;
            mat.rows = rows;
            mat.column = column;
            mat.h = 1;
            bool ошибказаполнителя = false;
                matrix = getMatЗаполнитель(mat/*, rows, column, 1*/, nod,false).matrix;
                Console.WriteLine("--------------------------");
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        Console.Write(matrix[i][j].nod.ID.ToString() + "_" + matrix[i][j].property.name + "_" + matrix[i][j].value + "\t");
                    }
                    Console.WriteLine();
                }
                        Console.WriteLine("--------------------------");
                //Parallel.For(0, rows, (i, state) =>
                for (int i = 0; i < rows;i++ )
                {
                    for (int j = 0; j < column - 1; j++)
                        try
                        {
                            matrix[i][j].property.value_editor = 0;
                        }
                        catch (System.NullReferenceException ex)
                        {
                            Console.WriteLine("Ошибка заполнителя matirx[" + i + "][" + j + "]" + ex.ToString());
                            ошибказаполнителя = true;
                            break;
                        }
                    //if (ошибказаполнителя)
                    //state.Break();
                }//);
            if (ошибказаполнителя)
                goto metkaошибказаполнителя;
            double[] massres = new double[rows];
            double[,] mati = new double[rows, column - 1];
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
                        matrix[i][j].property.value_editor += massres[i] / matrix[i][column - 1].property.value_editor_down;
                    }
                }
            }
            return list;
        }

        private List<Node_struct> startMathUp(List<Node_struct> list)
        {
            //Parallel.For(0, list.Count, (i, state) =>
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].connects_out.Count == 0 && list[i].connects_in.Count > 0)
                {
                    startMathГлубже(list, list[i]);
                    foreach (var pppp in list[i].props)
                    {
                        if (pppp.proc100 == true)
                        {
                            list = MathДетейПоИзвестномуРодителю(list[i], list);
                            break;
                        }
                    }
                }
            }//);
            return list;
        }

        private List<Node_struct> startMathГлубже(List<Node_struct> list, Node_struct nod)
        {
            foreach(var n in nod.connects_in)
            {
                if (n.connects_in.Count > 0)
                {
                    foreach (var pppp in n.props)
                    {
                        if (pppp.proc100 == true)
                        {
                            list = MathДетейПоИзвестномуРодителю(n, list);
                            break;
                        }
                    }
                    startMathГлубже(list, n);
                }
            }
            return list;
        }

        public List<Node_struct> getMathNodesAll(List<Node_struct> list)
        {
            list = startMathDownСначало(list);
            foreach(var nod in list)
            {
                foreach(var w in nod.props)
                {
                    w.value_editor = -1;
                }
            }
            list = startMathUp(list);
            foreach (var nod in list)
            {
                foreach (var w in nod.props)
                {
                    if (w.value_editor == -1)
                        w.value_editor = w.value_editor_down;
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
