using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SamianDouble
{
    public partial class EditNode : Form
    {
        public Nodes_struct thisnod { get; set; }

        public int thisnod_i { get; set; }

        public List<Othernode> othernods { get; set; }

        public List<Nodes_struct> tmplistnodes { get; set; }

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

            UpdateDataGrivTable(false);
        }

        public struct propсмежность
        {
            public int id;
            public string prop_name;
        }

        private propсмежность[][] getMatrixСмежность(int colrow, int colcol)
        {
            propсмежность[][] mat = new propсмежность[colrow][];
            List<int> idnodes = new List<int>();
            for (int i = 0; i < colrow; i++)
                mat[i] = new propсмежность[colcol];
            Parallel.For(0, tmplistnodes.Count, (i, state) =>
                {
                    for (int j=0;j<thisnod.connects_in.Count;j++)
                    {
                        if (thisnod.connects_in[j].ID == tmplistnodes[i].id)
                        {
                            idnodes.Add(i);
                        }
                    }
                });
            //сформировали список индексов в листе tmp тех узлов с которыми у нас есть связь, чтобы не приходилось их потом вылавливать
            for (int i = colrow - 1, index = 0, h = 1; i >= 0;i--,index++)
            {
                //цикл движется от последней строки до первой
                //в строке он заполняет ячейки матрицы ид нода и названием (уникально в пределах нода) нужного свойства
                int hh = 0; var othnod = tmplistnodes[idnodes[index]];
                for (int j=0, ij=0;j<colcol;j++)
                {
                ifigoto:
                    if (hh < h)
                    {
                        mat[i][j].id = othnod.id;
                        mat[i][j].prop_name = othnod.props[ij].name;
                        hh++;
                    }
                    else
                    {
                        ij++;
                        if (ij >= othnod.props.Count)
                            ij = 0;
                        hh = 0;
                        goto ifigoto;
                    }
                }
                if (h < 2)
                    h++;
                else
                {
                    h = h * 2;
                }
            }
            //получили верхнюю часть матрицы смежности поидее в нужном нам виде.
                return mat;
        }

        private void UpdateDataGrivTable(bool параметр)
        {
            DataTable table = new DataTable(); bool smej = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = table;
            dataGridView1.ColumnHeadersVisible = false;
            int len_columns = thisnod.props[0].values.Count + 1, rows;
            try
            {
                rows = thisnod.props.Count + thisnod.connects_in.Count;
                smej = true;
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
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Red;
            }
            int j = 0;
            for (i = i + 0, j = 0; i < rows; i++, j++)
            {
                table.Rows.Add();
                table.Rows[i][0] = thisnod.props[j].name;
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.LightGreen;
            }
            //заполнены строки и столбцы. Перехожу к заполнению самой матрицы;
            rows = rows - thisnod.props.Count;
            if (smej && thisnod.connects_in.Count > 0)
            {
                propсмежность[][] mat = getMatrixСмежность(rows, len_columns - 1);
                for (i = 0;i<rows;i++)
                {
                    for (j = 1; j < len_columns; j++)
                    {
                        table.Rows[i][j] = mat[i][j - 1].prop_name;
                        dataGridView1.Rows[i].Cells[j].ReadOnly = true;
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightBlue;
                    }
                }
                if (mat.Length >= 0 && параметр == true)
                {
                    table.Columns.Add("Вероятности");
                    double[] values_p = new NodeValueMath().values_editors(mat, thisnod, tmplistnodes);
                    for (i = 0; i < thisnod.props.Count; i++)
                    {
                        table.Rows[i + rows]["Вероятности"] = values_p[i];
                    }
                }
            }
            for (i = 0; i < thisnod.props.Count;i++ )
            {
                for (j=0;j<thisnod.props[i].values.Count;j++)
                {
                    table.Rows[i + rows][j + 1] = thisnod.props[i].values[j];
                }
            }
        }
        /// <summary>
        /// загружает список узлов с которыми нет связи.
        /// </summary>
        /// <returns>возвращает список в глобальную переменную и bool в качестве успеха</returns>
        private bool getOtherNodes()
        {
            othernods = new List<Othernode>();
            List<Nodes_struct> list = tmplistnodes;
            Parallel.For(0, list.Count, (i,state) =>
            {
                bool ifi = true;
                if (list[i].id == thisnod.id)
                {
                    ifi = false;
                }
                if (ifi)
                {
                    ifi = getProvConnectNodeToNode(list[i]);
                    if (ifi)
                    {
                        Othernode ot = new Othernode();
                        ot.id = list[i].id;
                        ot.name = list[i].name;
                        othernods.Add(ot);

                    }
                }
            });
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
            /*thisnod = new Nodes_struct();
            thisnod_i = new int();
            tmplistnodes = new List<Nodes_struct>();
            othernods = new List<Othernode>();*/
        }

        private void listBox3OtherNode_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ListBox lbl = (ListBox)sender;
                int id = (int)listBox3OtherNode.SelectedValue;
                string name = lbl.Text.ToString();
                lbl.DoDragDrop("0" + id + " " + name, DragDropEffects.Copy | DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void listBox2ConnectOut_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ListBox lbl = (ListBox)sender;
                int id = (int)listBox2ConnectOut.SelectedValue;
                string name = lbl.Text.ToString();
                lbl.DoDragDrop("1" + id + " " + name, DragDropEffects.Copy | DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void listBox1ConnectIn_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ListBox lbl = (ListBox)sender;
                int id = (int)listBox1ConnectIn.SelectedValue;
                string name = lbl.Text.ToString();
                lbl.DoDragDrop("2" + id + " " + name, DragDropEffects.Copy | DragDropEffects.Move);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void listBox2ConnectOut_DragDrop(object sender, DragEventArgs e)
        {
            string name = e.Data.GetData(DataFormats.Text).ToString();
            int what = int.Parse(name.Substring(0, 1));
            if (what == 1)
                return;
            name = name.Remove(0, 1);
            int id = -1;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    id = int.Parse(name.Substring(0, i));
                    name = name.Remove(0, i+1);
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

            Parallel.For(0, tmplistnodes.Count, (i, state) =>
                {
                    if (tmplistnodes[i].id == thisnod.id)
                    {
                        thisnod = tmplistnodes[i];
                        state.Break();
                    }
                });

            listBox2ConnectOut.DataSource = null;
            listBox2ConnectOut.DisplayMember = "Name";
            listBox2ConnectOut.ValueMember = "ID";
            listBox2ConnectOut.DataSource = thisnod.connect_out;

            UpdateDataGrivTable(false);
        }

        private void listBox2ConnectOut_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listBox1ConnectIn_DragDrop(object sender, DragEventArgs e)
        {
            string name = e.Data.GetData(DataFormats.Text).ToString();
            int what = int.Parse(name.Substring(0, 1));
            if (what == 2)
                return;
            name = name.Remove(0, 1);
            int id = -1;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    id = int.Parse(name.Substring(0, i));
                    name = name.Remove(0, i+1);
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
            tmplistnodes = ap.UpdateNodeConnectIn(tmplistnodes, thisnod, id);

            Parallel.For(0, tmplistnodes.Count, (i, state) =>
            {
                if (tmplistnodes[i].id == thisnod.id)
                {
                    thisnod = tmplistnodes[i];
                    state.Break();
                }
            });

            listBox1ConnectIn.DataSource = null;
            listBox1ConnectIn.DisplayMember = "Name";
            listBox1ConnectIn.ValueMember = "ID";
            listBox1ConnectIn.DataSource = thisnod.connects_in;

            UpdateDataGrivTable(false);
        }

        private void listBox1ConnectIn_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listBox3OtherNode_DragDrop(object sender, DragEventArgs e)
        {
            string name = e.Data.GetData(DataFormats.Text).ToString();
            int id = -1, what = int.Parse(name.Substring(0, 1));
            if (what == 0)
                return;
            name = name.Remove(0, 1);
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    id = int.Parse(name.Substring(0, i));
                    name = name.Remove(0, i+1);
                    break;
                }
            }

            if (what != 1 && what != 2)
                return;
            if (id == -1)
                return;
            for (int i = 0; i < othernods.Count; i++)
            {
                if (id == othernods[i].ID)
                    return;
            }

            Othernode ot = new Othernode();
            ot.ID = id;
            ot.Name = name;
            othernods.Add(ot);

            listBox3OtherNode.DataSource = null;
            listBox3OtherNode.DisplayMember = "Name";
            listBox3OtherNode.ValueMember = "ID";
            listBox3OtherNode.DataSource = othernods;

            UpdateNode ap = new UpdateNode();
            if (what == 1) //out
            {
                tmplistnodes = ap.DeleteNodeConnectOut(tmplistnodes, thisnod, id);
            }
            else if (what == 2) //in
            {
                tmplistnodes = ap.DeleteNodeConnectIn(tmplistnodes, thisnod, id);
            }

            Parallel.For(0, tmplistnodes.Count, (i, state) =>
            {
                if (tmplistnodes[i].id == thisnod.id)
                {
                    thisnod = tmplistnodes[i];
                    state.Break();
                }
            });
            if (what == 1) //out
            {
                listBox2ConnectOut.DataSource = null;
                listBox2ConnectOut.DisplayMember = "Name";
                listBox2ConnectOut.ValueMember = "ID";
                listBox2ConnectOut.DataSource = thisnod.connect_out;
            }
            else if (what == 2)
            {
                listBox1ConnectIn.DataSource = null;
                listBox1ConnectIn.DisplayMember = "Name";
                listBox1ConnectIn.ValueMember = "ID";
                listBox1ConnectIn.DataSource = thisnod.connects_in;
            }
            UpdateDataGrivTable(false);
        }

        private void listBox3OtherNode_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void button1Сохранить_Click(object sender, EventArgs e)
        {
            Parallel.For(0, tmplistnodes.Count, (i, state) =>
                {
                    if (tmplistnodes[i].id == thisnod.id)
                    {
                        tmplistnodes[i] = thisnod;
                        state.Break();
                    }
                });
        }

        private void button1Отменить_Click(object sender, EventArgs e)
        {
            
        }

        private void button1Math_Click(object sender, EventArgs e)
        {
            UpdateDataGrivTable(true);
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
}
