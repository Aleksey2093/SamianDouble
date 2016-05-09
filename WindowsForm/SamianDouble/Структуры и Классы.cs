using System;
using System.Collections.Generic;
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
    /*public class Connect_list
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
    }*/
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
    }
}
