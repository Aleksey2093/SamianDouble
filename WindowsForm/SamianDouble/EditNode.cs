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
using System.Threading;

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

            this.Text = "EditNode " + thisnod.Name;

            UpdateDataGrivTable(true);
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
            foreach(var nood in tmpinode.connects_in)
            {
                   idnodes.Add(nood);
            }
            if (idnodes.Count == 0)
                return mat;
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
        public static bool thhhhreadактивити = false;
        private void UpdateDataGrivTable(bool параметр)
        {
            Thread thread = new Thread(delegate()
                {
                    DateTime time1 = DateTime.Now;
                    Node nodeclass = new Node();
                    int попытки = 0; //bool вероятностьстолбец = false;
                retaw:
                    if (thhhhreadактивити == true)
                    {
                        попытки++;
                        if (попытки > 10)
                            return;
                        Thread.Sleep(1000);
                        goto retaw;
                    }
                    else
                        thhhhreadактивити = true;
                    DataTable table = new DataTable();
                    Invoke(new MethodInvoker(() => { labelPrintInformation.Text = "Выполняется расчет вероятности"; labelPrintInformation.BackColor = Color.Red; }));
                    tmplistnodes = new NodeValueMathUp().getMathNodesAll(tmplistnodes);
                    int len_columns = thisnod.props[0].values.Count + 1, rows;
                    try
                    {
                        rows = thisnod.props.Count + thisnod.connects_in.Count;
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
                        table.Rows.Add();
                        table.Rows[i][0] = thisnod.connects_in[i].Name;
                        gridcell[i, 0].setvalue(i, 0, Color.Red, true);
                    }
                    int j = 0;
                    for (i = i + 0, j = 0; i < rows; i++, j++)
                    {
                        table.Rows.Add();
                        table.Rows[i][0] = thisnod.props[j].name;
                        gridcell[i, 0].setvalue(i, 0, Color.LightGreen, true);
                    }
                    //заполнены строки и столбцы. Перехожу к заполнению самой матрицы;
                    rows = rows - thisnod.props.Count;
                    MatrixСмежная[][] mat = getMatrixСмежность(thisnod, rows, len_columns - 1, tmplistnodes);
                    for (i = 0; i < rows; i++)
                    {
                        for (j = 1; j < len_columns; j++)
                        {
                            table.Rows[i][j] = mat[i][j - 1].property.name;
                            gridcell[i, j].setvalue(i, j, Color.LightBlue, true);
                        }
                    }
                    table.Columns.Add("Вероятности");
                    for (i = 0; i < thisnod.props.Count; i++)
                    {
                        gridcell[i + rows, len_columns].setvalue(i, j, Color.MistyRose, true);
                        table.Rows[i + rows]["Вероятности"] = Math.Round(thisnod.props[i].value_editor, 4);
                    }
                    for (i = 0; i < thisnod.props.Count; i++)
                    {
                        for (j = 0; j < thisnod.props[i].values.Count; j++)
                        {
                            table.Rows[i + rows][j + 1] = thisnod.props[i].values[j];
                        }
                    }
                    Invoke(new MethodInvoker(() =>
                        {
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = table;
                            dataGridView1.ColumnHeadersVisible = false;
                            for (i = 0; i < table.Rows.Count; i++)
                                for (j = 0; j < table.Columns.Count; j++)
                                {
                                    dataGridView1.Rows[i].Cells[j].Style.BackColor = gridcell[i, j].color;
                                    dataGridView1.Rows[i].Cells[j].ReadOnly = gridcell[i, j].ReadOnly;
                                }
                            labelPrintInformation.Text = "Расчет вероятности завершен"; labelPrintInformation.BackColor = Color.White;
                        }));
                    thhhhreadактивити = false;
                    Console.WriteLine("Время выполнения расчета: " + (DateTime.Now - time1));
                });
            thread.Name = "Выполняетс расчет";
            thread.Start();
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
                if (list[i].ID != thisnod.ID)
                {
                    if (getProvConnectNodeToNode(list[i]))
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
            bool[] ifi = new bool[] { true, true, true };
            Parallel.Invoke(
                () =>
                {
                    for (int i = 0; i < thisnod.connects_in.Count; i++)
                        if (thisnod.connects_in[i].ID == nodes_struct.ID)
                        {
                            ifi[0] = false;
                            break;
                        }
                },
                    () =>
                    {
                        for (int i = 0; i < thisnod.connects_out.Count; i++)
                            if (thisnod.connects_out[i].ID == nodes_struct.ID)
                            {
                                ifi[1] = false;
                                break;
                            }
                    },
                    () =>
                    {
                        ifi[2] = new Node().getProvBoolЗацикленность(tmplistnodes, thisnod, nodes_struct, true, true);
                    });
            if (ifi[0] && ifi[1] && !ifi[2])
                return true;
            else
                return false;
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
                lbl.DoDragDrop("2" + id + " " + name, DragDropEffects.Copy | DragDropEffects.Move);
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
                lbl.DoDragDrop("1" + id + " " + name, DragDropEffects.Copy | DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void listBox1ConnectIn_DragDrop(object sender, DragEventArgs e)
        {
            listBoxПеретаскивание(e, 1);
        }
        
        private void listBox2ConnectOut_DragDrop(object sender, DragEventArgs e)
        {
            listBoxПеретаскивание(e, 2);
        }

        private void listBox3OtherNode_DragDrop(object sender, DragEventArgs e)
        {
            listBoxПеретаскивание(e, 0);
        }

        private Othernode nodКоторыйПеретаскивается(String name, int wh, Node nodeclass)
        {
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
                return null;
            Node_struct nod = nodeclass.getNodeПоИд(tmplistnodes, id);
            Othernode ot = new Othernode();
            ot.Node = nod;
            return ot;
        }

        private void updateListBoxПослеПерестаскивания()
        {
            listBox1ConnectIn.DataSource = null;
            listBox1ConnectIn.DisplayMember = "Name";
            listBox1ConnectIn.ValueMember = "ID";
            listBox1ConnectIn.DataSource = thisnod.connects_in;

            listBox2ConnectOut.DataSource = null;
            listBox2ConnectOut.DisplayMember = "Name";
            listBox2ConnectOut.ValueMember = "ID";
            listBox2ConnectOut.DataSource = thisnod.connects_out;

            listBox3OtherNode.DataSource = null;
            listBox3OtherNode.DisplayMember = "Name";
            listBox3OtherNode.ValueMember = "ID";
            listBox3OtherNode.DataSource = othernods;
        }

        private void listBoxПеретаскивание(DragEventArgs e, int idlistbox)
        {
            string name = e.Data.GetData(DataFormats.Text).ToString();
            int what = int.Parse(name.Substring(0, 1));
            if (what == idlistbox)
                return;
            Node nodeclass = new Node();
            UpdateNode upnode = new UpdateNode();
            Othernode othernod = nodКоторыйПеретаскивается(name, idlistbox, nodeclass);
            if (othernod == null)
                return;
            if (nodeclass.getProvBoolЗацикленность(tmplistnodes, thisnod, othernod.Node, true, true))
                return;
            if (idlistbox == 1)
            {
                tmplistnodes = upnode.updateNodeConnectIn(tmplistnodes, thisnod, othernod.ID);
                if (what == 2)
                    tmplistnodes = upnode.deleteNodeConnectOut(tmplistnodes, thisnod, othernod.ID);
            }
            else if (idlistbox == 2)
            {
                tmplistnodes = upnode.updateNodeConnectOut(tmplistnodes, thisnod, othernod.ID);
                if (what == 1)
                    tmplistnodes = upnode.deleteNodeConnectIn(tmplistnodes, thisnod, othernod.ID);
            }
            else
            {
                othernods.Add(othernod);
                if (what == 1)
                    tmplistnodes = upnode.deleteNodeConnectIn(tmplistnodes, thisnod, othernod.ID);
                else
                    tmplistnodes = upnode.deleteNodeConnectOut(tmplistnodes, thisnod, othernod.ID);
            }
            if (what == 0)
            {
                bool удален = false;
                Parallel.For(0, othernods.Count, (i, state) =>
                    {
                        try
                        {
                            if (удален == false && othernods[i].ID == othernod.ID)
                            {
                                удален = true;
                                othernods.RemoveAt(i);
                                state.Break();
                            }
                        }
                        catch(System.ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine("Обращение к элементу массива, который был уже удален. " + ex.ToString());
                        }
                    });
            }
            //если нужно будет обновить thisnod;
            updateListBoxПослеПерестаскивания();
            UpdateDataGrivTable(true);
        }

        private void listBox2ConnectOut_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listBox1ConnectIn_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
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

        bool cellchangedсейча = false;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cellchangedсейча)
                return;
            Thread thread = new Thread(delegate()
                {
                    if (cellchangedсейча)
                        return;
                    cellchangedсейча = true;
                    double valueНовоеЗначение = -1;
                    try
                    {
                        double сумма = 0; double v = 0;
                        for (int i = thisnod.connects_in.Count; i < dataGridView1.Rows.Count; i++)
                        {
                            v = 0;
                            if (!double.TryParse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString(), out v))
                                if (!double.TryParse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString().Replace(".", ","), out v))
                                    if (!double.TryParse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString().Replace(",", "."), out v))
                                    {
                                        dataGridView1.Rows[i].Cells[e.ColumnIndex].Value = "0";
                                        cellchangedсейча = false;
                                        return;
                                    }
                            if (v < 0)
                            {
                                v = Math.Abs(v);
                                Invoke(new MethodInvoker(() => { dataGridView1.Rows[i].Cells[e.ColumnIndex].Value = v; }));
                            }
                            if (thisnod.props.Count == 2)
                            {
                                Invoke(new MethodInvoker(() =>
                                    {
                                        if (!double.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out v))
                                            if (!double.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace(".", ","), out v))
                                                if (!double.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace(",", "."), out v))
                                                {  }
                                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = v;
                                        if (dataGridView1.Rows.Count - 1 == e.RowIndex)
                                            dataGridView1.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value = Math.Round(1 - v, 4);
                                        else
                                            dataGridView1.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value = Math.Round(1 - v, 4);
                                    }));
                                cellchangedсейча = false;
                                return;
                            }
                            сумма += v;
                        }
                        if (сумма > 1)
                            valueНовоеЗначение = 0;
                    }
                    catch (Exception)
                    {
                        valueНовоеЗначение = 0;
                    }
                    finally
                    {
                        if (valueНовоеЗначение != -1)
                        {
                            Invoke(new MethodInvoker(() => { dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valueНовоеЗначение; }));
                        }
                    }
                    cellchangedсейча = false;
                });
            thread.Name = "Значения в столбце после ввода пользователем";
            thread.Start();
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
