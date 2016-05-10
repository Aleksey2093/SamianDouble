using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class Propertys_struct
    {
        public String name;
        public List<double> values;
        public double value_editor;
        public bool proc100 = false;
    }
    public struct MatrixСмежная
    {
        private Node_struct nod1;
        private Propertys_struct property1;
        private double value1;

        public Node_struct nod
        {
            get { return nod1; }
            set { nod1 = value; }
        }
        public Propertys_struct property
        {
            get { return property1; }
            set { property1 = value; }
        }
        public double value
        {
            get { return value1; }
            set { value1 = value; }
        }
    };
    /// <summary>
    /// Цвета ячеек в datagridview
    /// </summary>
    public struct GridCellColor
    {
        private int r;
        private int c;
        private Color colorback;
        private bool read;
        /// <summary>
        /// установить значения
        /// </summary>
        /// <param name="i">строка</param>
        /// <param name="j">столбец</param>
        /// <param name="color">цвет</param>
        /// <param name="re">ReadOnly</param>
        /// <returns></returns>
        public bool setvalue(int i, int j, Color color, bool re)
        {
            r = i;
            c = j;
            colorback = color;
            read = re;
            return true;
        }

        /// <summary>
        /// вернуть значение строки
        /// </summary>
        public int rows
        {
            get { return r; }
        }

        /// <summary>
        /// вернуть значение столбца
        /// </summary>
        public int cell
        {
            get { return c; }
        }
        /// <summary>
        /// вернуть цвет
        /// </summary>
        public Color color
        {
            get { return colorback; }
        }
        public bool ReadOnly
        {
            get { return read; }
        }
    };
}
