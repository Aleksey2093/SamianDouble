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
    class NodeValueMathUp2
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
                foreach (var nod in list)
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
            foreach (var ot in nod.connects_in)
            {
                //if (!nodes.getEstProperyTrueFix(ot.props))
                kolvo *= getMatСмежКолСтрок(ot, false);
                //else
            }//);
            return kolvo;
        }

        private int getMatCмежКолСтолбцов(Node_struct nod, bool ifi)
        {
            Node nodes = new Node();
            int kolvo = 1;
            //Parallel.ForEach(nod.connects_in, (ot, state) =>
            foreach (var ot in nod.connects_in)
            {
                //if (!nodes.getEstProperyTrueFix(ot.props))
                kolvo += getMatCмежКолСтолбцов(ot, false);
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



        public List<Node_struct> getMathAll(List<Node_struct> list)
        {
            foreach(Node_struct nod in list)
            {
                if (nod.connects_in.Count == 0 && nod.connects_out.Count > 0)
                {

                }
            }

            return list;
        }

        public bool getИзвестныеДети(Node_struct nod)
        {
            Node nodeclass = new Node();
            bool ifi = false;
            /*foreach(var child in nod.connects_out)
            {
                if ()
            }*/
            return ifi;
        }
    }
}
