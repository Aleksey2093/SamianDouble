using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamianDouble
{
    public partial class EditNode : Form
    {
        /// <summary>
        /// узел с которым будем работать
        /// </summary>
        public static Nodes_struct thisnod { get; set; }
        /// <summary>
        /// ид этого узла
        /// </summary>
        public static int thisnod_i { get; set; }
        /// <summary>
        /// список других узлов в дереве за исключением тех, с которым у нас уже есть связь
        /// </summary>
        public static List<Othernode> othernods { get; set; }

        public static List<Nodes_struct> tmplistnodes { get; set; }

        public EditNode()
        {
            InitializeComponent();
        }

        private void EditNode_Load(object sender, EventArgs e)
        {
            textBox1.Text = thisnod.name;
            listBox1ConnectIn.DisplayMember = "Name";
            listBox1ConnectIn.ValueMember = "ID";
            listBox1ConnectIn.DataSource = thisnod.connects_in;

            listBox2ConnectOut.DisplayMember = "Name";
            listBox2ConnectOut.ValueMember = "ID";
            listBox2ConnectOut.DataSource = thisnod.connect_out;

            if (!getOtherNodes())
                this.Close();

            listBox3OtherNode.DisplayMember = "Name";
            listBox3OtherNode.ValueMember = "ID";
            listBox3OtherNode.DataSource = othernods;

            UpdateDataGrivTable();
        }

        private void UpdateDataGrivTable()
        {
            DataTable table = new DataTable();
            int len_columns = thisnod.props[0].values.Count + 1, rows;
            try
            {
                rows = thisnod.props.Count + thisnod.connects_in.Count;
            }
            catch
            {
                rows = thisnod.props.Count;
            }
            int i = 0;
            List<Nodes_struct> list = tmplistnodes;
            for (i = 0; i < len_columns; i++)
            {
                table.Columns.Add("");
            }
            for (i = 0; i < thisnod.connects_in.Count; i++)
            {
                int id = thisnod.connects_in[i].ID;
                for (int k = 0; k < list.Count; k++)
                {
                    if (id == list[i].id)
                    {
                        id = i;
                        break;
                    }
                }
                table.Rows.Add();
                table.Rows[i][0] = list[id].name;
            }
            int j = 0;
            for (i = i + 0, j = 0; i < rows; i++, j++)
            {
                table.Rows.Add();
                table.Rows[i][0] = thisnod.props[j].name;
            }
            //заполнены строки и столбцы. Перехожу к заполнению самой матрицы;
            rows = rows - thisnod.props.Count;
            for (i = 0; i < thisnod.props.Count;i++ )
            {
                for (j=0;j<thisnod.props[i].values.Count;j++)
                {
                    table.Rows[i + rows][j + 1] = thisnod.props[i].values[j];
                }
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = table;
            dataGridView1.ColumnHeadersVisible = false;
        }
        /// <summary>
        /// загружает список узлов с которыми нет связи.
        /// </summary>
        /// <returns>возвращает список в глобальную переменную и bool в качестве успеха</returns>
        private bool getOtherNodes()
        {
            othernods = new List<Othernode>();
            List<Nodes_struct> list = tmplistnodes;
            Othernode ot;
            for (int i = 0; i < list.Count; i++)
            {
                bool ifi = true;
                if (list[i] == thisnod)
                {
                    ifi = false;
                }

                ifi = getProvConnectNodeToNode(list[i]);

                if (ifi)
                {
                    ot = new Othernode();
                    ot.id = list[i].id;
                    ot.name = list[i].name;
                    othernods.Add(ot);
                }
            }
            return true;
        }

        private bool getProvConnectNodeToNode(Nodes_struct nodes_struct)
        {
            for (int i=0;i<thisnod.connects_in.Count;i++)
                if (thisnod.connects_in[i].conid == nodes_struct.id)
                    return false;
            for (int i = 0; i < thisnod.connect_out.Count; i++)
                if (thisnod.connect_out[i].conid == nodes_struct.id)
                    return false;
            return true;
        }

        private void EditNode_FormClosing(object sender, FormClosingEventArgs e)
        {
            thisnod = null;
            thisnod_i = -1;
            othernods = null;
        }

        private void listBox3OtherNode_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lbl = (ListBox)sender;
            Othernode ot = new Othernode();
            int id = (int)listBox3OtherNode.SelectedValue;
            string name = lbl.Text.ToString();
            ot.id = id;
            ot.name = name;
            lbl.DoDragDrop(id + " " + name, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void listBox2ConnectOut_DragDrop(object sender, DragEventArgs e)
        {
            string name = e.Data.GetData(DataFormats.Text).ToString();
            int id = -1;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    id = int.Parse(name.Substring(0, i));
                    name = name.Remove(0, i);
                    break;
                }
            }
            if (id == -1) 
                return;
            for (int i = 0; i < thisnod.connect_out.Count; i++)
                if (id == thisnod.connect_out[i].ID)
                    return;
            for (int i = 0; i < thisnod.connects_in.Count; i++)
                if (id == thisnod.connects_in[i].ID)
                    return;
            Connect_list cl = new Connect_list();
            cl.conid = id;
            cl.connodename = name;
            thisnod.connect_out.Add(cl);
            listBox2ConnectOut.DataSource = null;
            listBox2ConnectOut.DisplayMember = "Name";
            listBox2ConnectOut.ValueMember = "ID";
            listBox2ConnectOut.DataSource = thisnod.connect_out;
            for (int i=0;i<othernods.Count;i++)
                if (id == othernods[i].ID)
                {
                    othernods.RemoveAt(i);
                    break;
                }
            listBox3OtherNode.DataSource = null;
            listBox3OtherNode.DisplayMember = "Name";
            listBox3OtherNode.ValueMember = "ID";
            listBox3OtherNode.DataSource = othernods;

            UpdateNode ap = new UpdateNode();
            tmplistnodes = ap.UpdateNodeConnectOut(tmplistnodes, thisnod, id);

            UpdateDataGrivTable();
        }

        private void listBox2ConnectOut_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listBox1ConnectIn_DragDrop(object sender, DragEventArgs e)
        {
            string name = e.Data.GetData(DataFormats.Text).ToString();
            int id = -1;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    id = int.Parse(name.Substring(0, i));
                    name = name.Remove(0, i);
                    break;
                }
            }
            if (id == -1)
                return;
            for (int i = 0; i < thisnod.connect_out.Count; i++)
                if (id == thisnod.connect_out[i].ID)
                    return;
            for (int i = 0; i < thisnod.connects_in.Count; i++)
                if (id == thisnod.connects_in[i].ID)
                    return;
            Connect_list cl = new Connect_list();
            cl.conid = id;
            cl.connodename = name;
            thisnod.connect_out.Add(cl);

            listBox1ConnectIn.DataSource = null;
            listBox1ConnectIn.DisplayMember = "Name";
            listBox1ConnectIn.ValueMember = "ID";
            listBox1ConnectIn.DataSource = thisnod.connects_in;
            for (int i = 0; i < othernods.Count; i++)
                if (id == othernods[i].ID)
                {
                    othernods.RemoveAt(i);
                    break;
                }
            listBox3OtherNode.DataSource = null;
            listBox3OtherNode.DisplayMember = "Name";
            listBox3OtherNode.ValueMember = "ID";
            listBox3OtherNode.DataSource = othernods;

            UpdateNode ap = new UpdateNode();
            thisnod = ap.UpdateNodeConnectIn(tmplistnodes, thisnod, id);

            UpdateDataGrivTable();
        }

        private void listBox1ConnectIn_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
    public class Othernode
    {
        public int id;
        public string name;

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

    public class UpdateNode
    {
        /// <summary>
        /// возвращает переделанный нод после создания входящей связи
        /// </summary>
        /// <param name="list">список узлов</param>
        /// <param name="nod">модицифируемый узел, он же возвращается</param>
        /// <param name="id_other">ид узла связь из которого идет в текущий узел</param>
        /// <returns>модицифированный узел</returns>
        public Nodes_struct UpdateNodeConnectIn(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            Nodes_struct other_nod = new Nodes_struct();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id_other)
                {
                    other_nod = list[i];
                    break;
                }
            }

            for (int i = 0; i < nod.props.Count; i++)
            {
                for (int j=0;j<other_nod.props.Count*nod.props[i].values.Count;j++)
                {
                    nod.props[i].values.Add(0.5);
                }
            }

            return nod;
        }
        /// <summary>
        /// возвращает переделанный список после создания связи исходящей связи
        /// </summary>
        /// <param name="list">список узлов, он же возвращается</param>
        /// <param name="nod">модифицируемый узел</param>
        /// <param name="id_other">ид узла в который направлена создаваемая связь</param>
        /// <returns>модифицированный список</returns>
        public List<Nodes_struct> UpdateNodeConnectOut(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            Nodes_struct other_nod = new Nodes_struct();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id_other)
                {
                    other_nod = list[i];
                    break;
                }
            }

            for (int i = 0; i < nod.props.Count; i++)
            {
                for (int j = 0; j < other_nod.props.Count * nod.props[i].values.Count; j++)
                {
                    nod.props[i].values.Add(0.5);
                }
            }

            return list;
        }
    }
}
