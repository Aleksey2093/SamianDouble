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
        public Node_struct thisnod { get; set; }

        public int thisnod_i { get; set; }

        public List<Othernode> othernods { get; set; }

        public List<Node_struct> tmplistnodes { get; set; }

        public EditNode()
        {
            InitializeComponent();
        }

        private void EditNode_Load(object sender, EventArgs e)
        {
            textBox1.Text = thisnod.Name;
            listBox1ConnectIn.DisplayMember = "Name";
            listBox1ConnectIn.ValueMember = "ID";
            listBox1ConnectIn.DataSource = thisnod.connects_in;

            listBox2ConnectOut.DisplayMember = "Name";
            listBox2ConnectOut.ValueMember = "ID";
            listBox2ConnectOut.DataSource = thisnod.connects_out;

            if (!getOtherNodes())
                this.Close();

            listBox3OtherNode.DisplayMember = "Name";
            listBox3OtherNode.ValueMember = "ID";
            listBox3OtherNode.DataSource = othernods;

            UpdateDataGrivTable(false);
            this.Text = "EditNode " + thisnod.Name;
        }

        /// <summary>
        /// возвращает смежную матрицу узлов
        /// </summary>
        /// <param name="colrow">строки - количество связей входящих</param>
        /// <param name="colcol">столбцы - длинна одной линии значений (они все одинаковые должны быть, если не было ошибок)</param>
        /// <returns></returns>
        public MatrixСмежная[][] getMatrixСмежность(Node_struct tmpinode, int colrow, int colcol, List<Node_struct> listik)
        {
            MatrixСмежная[][] mat = new MatrixСмежная[colrow][];
            List<Node_struct> idnodes = new List<Node_struct>();
            for (int i = 0; i < colrow; i++)
            {
                mat[i] = new MatrixСмежная[colcol];
            }
            /*Parallel.For(0, listik.Count, (i, state) =>
                {
                    for (int j = 0; j < tmpinode.connects_in.Count; j++)
                    {
                        if (tmpinode.connects_in[j].ID == listik[i].ID)
                        {
                            idnodes.Add(i);
                        }
                    }
                });*/
               foreach(var nood in tmpinode.connects_in)
               {
                   idnodes.Add(nood);
               }
            //сформировали список индексов в листе tmp тех узлов с которыми у нас есть связь, чтобы не приходилось их потом вылавливать
            for (int i = colrow - 1, index = 0, h = 1; i >= 0; i--, index++)
            {
                //цикл движется от последней строки до первой
                //в строке он заполняет ячейки матрицы ид нода и названием (уникально в пределах нода) нужного свойства
                int hh = 0; var othnod = idnodes[i];
                for (int j = 0, ij = 0; j < colcol; j++)
                {
                ifigoto:
                    if (hh < h)
                    {
                        mat[i][j].nod = othnod;
                        mat[i][j].property = othnod.props[ij];
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
                if (h == 1)
                    h = othnod.props.Count;
                else
                {
                    h = h * othnod.props.Count;
                }
            }
            //получили верхнюю часть матрицы смежности поидее в нужном нам виде.
            return mat;
        }

        public struct GridCellColor
        {
            private int r;
            private int c;
            private Color colorback;
            public bool setvalue(int i, int j, Color color)
            {
                r = i;
                c = j;
                colorback = color;
                return true;
            }
            public int rows
            {
                get {
                    return r;
                }
            }
            public int cell
            {
                get {return c;}
            }
            public Color color
            {
                get { return colorback; }
            }
        };

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
            GridCellColor[,] gridcell = new GridCellColor[rows, len_columns + 1];
            int i = 0;
            List<Node_struct> list = tmplistnodes;

            for (i = 0; i < len_columns; i++)
            {
                table.Columns.Add("");
            }
            for (i = 0; i < thisnod.connects_in.Count; i++)
            {
                int id = thisnod.connects_in[i].ID;
                for (int k = 0; k < list.Count; k++)
                {
                    if (id == list[i].ID)
                    {
                        id = i;
                        break;
                    }
                }
                table.Rows.Add();
                table.Rows[i][0] = list[id].Name;
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                gridcell[i, 0].setvalue(i, 0, Color.Red);
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Red;
            }
            int j = 0;
            for (i = i + 0, j = 0; i < rows; i++, j++)
            {
                table.Rows.Add();
                table.Rows[i][0] = thisnod.props[j].name;
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                gridcell[i, 0].setvalue(i, 0, Color.LightGreen);
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.LightGreen;
            }
            //заполнены строки и столбцы. Перехожу к заполнению самой матрицы;
            rows = rows - thisnod.props.Count;
            //if ((smej && thisnod.connects_in.Count > 0) || dДействие == 1)
            {
                MatrixСмежная[][] mat = getMatrixСмежность(thisnod, rows, len_columns - 1, tmplistnodes);
                for (i = 0; i < rows; i++)
                {
                    for (j = 1; j < len_columns; j++)
                    {
                        table.Rows[i][j] = mat[i][j - 1].property.name;
                        dataGridView1.Rows[i].Cells[j].ReadOnly = true;
                        gridcell[i, j].setvalue(i, j, Color.LightBlue);
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightBlue;
                    }
                }
                if (thisnod.connects_in.Count > 0 || new Node().getProvBoolПроверкаИзвестия(list))//((mat.Length >= 0 && параметр == true) || dДействие == 1)
                {
                    list = new NodeValueMathUp().getMathNodesAll(list);
                    table.Columns.Add("Вероятности");
                    for (i = 0; i < thisnod.props.Count; i++)
                    {
                        gridcell[i + rows, len_columns].setvalue(i, j, Color.MistyRose);
                        table.Rows[i + rows]["Вероятности"] = Math.Round(thisnod.props[i].value_editor, 4);
                    }
                    for (i = 0; i < table.Rows.Count; i++)
                        for (j = 0; j < table.Columns.Count; j++)
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = gridcell[i, j].color;
                        }
                }
            }
            for (i = 0; i < thisnod.props.Count; i++)
            {
                for (j = 0; j < thisnod.props[i].values.Count; j++)
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
            Node nodeclass = new Node();
            othernods = new List<Othernode>();
            List<Node_struct> list = tmplistnodes;
            Parallel.For(0, list.Count, (i, state) =>
            {
                bool ifi = true;
                if (list[i].ID == thisnod.ID)
                    ifi = false;
                else if (ifi)
                {
                    ifi = getProvConnectNodeToNode(list[i]);
                    if (ifi)
                    {
                        Othernode ot = new Othernode();
                        ot.Node = list[i];
                        othernods.Add(ot);
                    }
                }
            });
            return true;
        }

        private bool getProvConnectNodeToNode(Node_struct nodes_struct)
        {
            bool ifi = true;
            Parallel.Invoke(
                () =>
                {
                    for (int i = 0; i < thisnod.connects_in.Count; i++)
                        if (thisnod.connects_in[i].ID == nodes_struct.ID)
                        {
                            //return false;
                            ifi = false;
                            break;
                        }
                },
                    () =>
                    {
                        for (int i = 0; i < thisnod.connects_out.Count; i++)
                            if (thisnod.connects_out[i].ID == nodes_struct.ID)
                            {
                                //return false;
                                ifi = false;
                                break;
                            }
                    });
            return true;
        }

        private void EditNode_FormClosing(object sender, FormClosingEventArgs e)
        {
            thisnod.Name = textBox1.Text;
            Сохр_таблицу_в_узел();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void listBox2ConnectOut_DragDrop(object sender, DragEventArgs e)
        {
            Node nodeclass = new Node();
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
                    name = name.Remove(0, i + 1);
                    break;
                }
            }
            if (id == -1)
                return;
            if (nodeclass.getProvBoolЗацикленность(tmplistnodes, thisnod, nodeclass.getNodeПоИд(tmplistnodes,id), true, false))
            {
                return;
            }
            UpdateNode ap = new UpdateNode();
            for (int i = 0; i < thisnod.connects_in.Count; i++)
                if (id == thisnod.connects_in[i].ID)
                {
                    if (what == 1) //out
                    {
                        tmplistnodes = ap.deleteNodeConnectOut(tmplistnodes, thisnod, id);
                    }
                    else if (what == 2) //in
                    {
                        tmplistnodes = ap.deleteNodeConnectIn(tmplistnodes, thisnod, id);
                    }

                    Parallel.For(0, tmplistnodes.Count, (j, state) =>
                    {
                        if (tmplistnodes[j].ID == thisnod.ID)
                        {
                            thisnod = tmplistnodes[j];
                            state.Break();
                        }
                    });
                    if (what == 1) //out
                    {
                        listBox2ConnectOut.DataSource = null;
                        listBox2ConnectOut.DisplayMember = "Name";
                        listBox2ConnectOut.ValueMember = "ID";
                        listBox2ConnectOut.DataSource = thisnod.connects_out;
                    }
                    else if (what == 2)
                    {
                        listBox1ConnectIn.DataSource = null;
                        listBox1ConnectIn.DisplayMember = "Name";
                        listBox1ConnectIn.ValueMember = "ID";
                        listBox1ConnectIn.DataSource = thisnod.connects_in;
                    }
                    UpdateDataGrivTable(false);
                    //return;
                }
            for (int i = 0; i < thisnod.connects_out.Count; i++)
                if (id == thisnod.connects_out[i].ID)
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

            tmplistnodes = ap.updateNodeConnectOut(tmplistnodes, thisnod, id);

            Parallel.For(0, tmplistnodes.Count, (i, state) =>
                {
                    if (tmplistnodes[i].ID == thisnod.ID)
                    {
                        thisnod = tmplistnodes[i];
                        state.Break();
                    }
                });

            listBox2ConnectOut.DataSource = null;
            listBox2ConnectOut.DisplayMember = "Name";
            listBox2ConnectOut.ValueMember = "ID";
            listBox2ConnectOut.DataSource = thisnod.connects_out;

            UpdateDataGrivTable(false);
        }

        private void listBox2ConnectOut_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listBox1ConnectIn_DragDrop(object sender, DragEventArgs e)
        {
            Node nodeclass = new Node();
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
                    name = name.Remove(0, i + 1);
                    break;
                }
            }
            if (id == -1)
                return;
            if (nodeclass.getProvBoolЗацикленность(tmplistnodes, thisnod, nodeclass.getNodeПоИд(tmplistnodes, id), true, false))
                return;
            UpdateNode ap = new UpdateNode();
            for (int i = 0; i < thisnod.connects_out.Count; i++)
                if (id == thisnod.connects_out[i].ID)
                {
                    if (what == 1) //out
                    {
                        tmplistnodes = ap.deleteNodeConnectOut(tmplistnodes, thisnod, id);
                    }
                    else if (what == 2) //in
                    {
                        tmplistnodes = ap.deleteNodeConnectIn(tmplistnodes, thisnod, id);
                    }

                    Parallel.For(0, tmplistnodes.Count, (j, state) =>
                    {
                        if (tmplistnodes[j].ID == thisnod.ID)
                        {
                            thisnod = tmplistnodes[j];
                            state.Break();
                        }
                    });
                    if (what == 1) //out
                    {
                        listBox2ConnectOut.DataSource = null;
                        listBox2ConnectOut.DisplayMember = "Name";
                        listBox2ConnectOut.ValueMember = "ID";
                        listBox2ConnectOut.DataSource = thisnod.connects_out;
                    }
                    else if (what == 2)
                    {
                        listBox1ConnectIn.DataSource = null;
                        listBox1ConnectIn.DisplayMember = "Name";
                        listBox1ConnectIn.ValueMember = "ID";
                        listBox1ConnectIn.DataSource = thisnod.connects_in;
                    }
                    UpdateDataGrivTable(false);
                    //return;
                }
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

            tmplistnodes = ap.updateNodeConnectIn(tmplistnodes, thisnod, id);

            Parallel.For(0, tmplistnodes.Count, (i, state) =>
            {
                if (tmplistnodes[i].ID == thisnod.ID)
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
                    name = name.Remove(0, i + 1);
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
                tmplistnodes = ap.deleteNodeConnectOut(tmplistnodes, thisnod, id);
            }
            else if (what == 2) //in
            {
                tmplistnodes = ap.deleteNodeConnectIn(tmplistnodes, thisnod, id);
            }

            Parallel.For(0, tmplistnodes.Count, (i, state) =>
            {
                if (tmplistnodes[i].ID == thisnod.ID)
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
                listBox2ConnectOut.DataSource = thisnod.connects_out;
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
                    if (tmplistnodes[i].ID == thisnod.ID)
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
            if (Сохр_таблицу_в_узел())
                UpdateDataGrivTable(true);//new NodeValueMathUp().getMathNodesAll(tmplistnodes);
        }

        private bool Сохр_таблицу_в_узел()
        {
            int n = dataGridView1.Rows.Count;
            int m = 1 + thisnod.props[0].values.Count;
            for (int i = thisnod.connects_in.Count; i < n; i++)
                for (int j = 1; j < m; j++)
                {
                    double value;
                    if (!double.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString(), out value))
                        if (!double.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString().Replace(".", ","), out value))
                            if (!double.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString().Replace(",", "."), out value))
                            {
                                MessageBox.Show("Ошибка при проведении расчетов во время сохранения таблицы. Ошибка при конвертировании значения таблицы в double", "",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                return false;
                            }
                    thisnod.props[i - thisnod.connects_in.Count].values[j - 1] = value;
                }
            try
            {
                for (int i = thisnod.connects_in.Count; i < n; i++)
                {
                    double value;
                    if (!double.TryParse(dataGridView1.Rows[i].Cells["Вероятности"].Value.ToString(), out value))
                        if (!double.TryParse(dataGridView1.Rows[i].Cells["Вероятности"].Value.ToString().Replace(".", ","), out value))
                            if (!double.TryParse(dataGridView1.Rows[i].Cells["Вероятности"].Value.ToString().Replace(",", "."), out value))
                            {
                                MessageBox.Show("Ошибка при проведении расчетов во время сохранения таблицы. Ошибка при конвертировании значения таблицы в double", "",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                return false;
                            }
                    thisnod.props[i - thisnod.connects_in.Count].value_editor = value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Нету столбца вероятностей. Ошибка - " + ex.ToString());
            }
            return true;
        }

        private void button1СохранитьТаблицу_Click(object sender, EventArgs e)
        {
            Сохр_таблицу_в_узел();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                double сумма = 0;
                for (int i = thisnod.connects_in.Count; i < dataGridView1.Rows.Count; i++)
                {
                    double v = 0;
                    if (!double.TryParse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString(), out v))
                        if (!double.TryParse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString().Replace(".", ","), out v))
                            if (!double.TryParse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString().Replace(",", "."), out v))
                            {
                                dataGridView1.Rows[i].Cells[e.ColumnIndex].Value = "0";
                                return;
                            }
                    if (v < 0)
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Math.Abs(v).ToString();
                    else
                        v = Math.Abs(v);
                    сумма += v;
                }
                if (сумма > 1)
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
            }
            catch (Exception)
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
            }
        }
    }
    public class Othernode
    {
        private int id;
        private string name;
        private Node_struct node;

        /// <summary>
        /// присвоит значение всем полям в структуре или получит нод
        /// </summary>
        public Node_struct Node
        {
            get { return node; }
            set { node = value;
            id = node.ID;
            name = node.Name;
            }
        }

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
