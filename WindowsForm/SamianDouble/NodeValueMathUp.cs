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
        public double[] valueeditorup (EditNode.propсмежность[][] смежность, Node_struct nod, List<Node_struct> list)
        {
            double[] valueeditor = new double[nod.props.Count];
            EditNode editnod = new EditNode();
            NodeValueMathDown valdown = new NodeValueMathDown();
            foreach(var nownod in list)
            {
                if (nownod.connects_out.Count > 0)
                {
                    EditNode.propсмежность[][] tmpmat = editnod.getMatrixСмежность(nownod, nownod.connects_in.Count, nownod.props[0].values.Count, list);
                    valdown.values_editors(tmpmat, nownod, list);
                }
            }


                return valueeditor;
        }
    }
}
