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
        private Nodes_struct thisnod;
        
        private int thisnod_i;

        private List<Othernode> othernods;

        private List<Nodes_struct> tmplistnodes;

        public void copyDataNew(Nodes_struct n1, int i, List<Nodes_struct> nmany)
        {
            thisnod = n1;
            thisnod_i = i;
            tmplistnodes = nmany;
        }

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
            for (int i = colrow - 1, index = 0, h = 1; i >= 0;i--,index++,h++ )
            {
                //цикл движется от последней строки до первой
                //в строке он заполняет ячейки матрицы ид нода и названием (уникально в пределах нода) нужного свойства
                int hh = 0;
                for (int j=0;j<colcol;j++)
                {
                    var othnod = tmplistnodes[idnodes[index]];
                    if (hh < h)
                    {
                        mat[i][j].id = othnod.id;
                        mat[i][j].prop_name = othnod.props[hh].name;
                    }
                    else
                    {
                        hh = 0;
                    }
                }
            }
            //получили верхнюю часть матрицы смежности поидее в нужном нам виде.
                return mat;
        }

        private void UpdateDataGrivTable()
        {
            DataTable table = new DataTable(); bool smej = false;
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
            }
            int j = 0;
            for (i = i + 0, j = 0; i < rows; i++, j++)
            {
                table.Rows.Add();
                table.Rows[i][0] = thisnod.props[j].name;
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

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = table;
            dataGridView1.ColumnHeadersVisible = false;
            List<Nodes_struct> dwad = MainWindow.listnodes;
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
            thisnod = new Nodes_struct();
            thisnod_i = new int();
            tmplistnodes = new List<Nodes_struct>();
            othernods = new List<Othernode>();
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

            UpdateDataGrivTable();
        }

        private void listBox1ConnectIn_DragEnter(object sender, DragEventArgs e)
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
        public List<Nodes_struct> UpdateNodeConnectIn(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            Nodes_struct other_nod = new Nodes_struct();
            Parallel.For(0, list.Count, (i, state) =>
                {
                    if (list[i].id == id_other)
                    {
                        other_nod = list[i];
                        state.Break(); //находим нужный нам нод и выходим из цикла
                    }
                });
            //теперь нужно добавить связи: исходяющую связь в другой нод и в наш входяющую
            Connect_list con = new Connect_list();
            con.ID = other_nod.id;
            con.Name = other_nod.name;
            nod.connects_in.Add(con);

            con = new Connect_list();
            con.ID = nod.id;
            con.Name = nod.name;
            other_nod.connect_out.Add(con);
            //связи созданы, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j=0;j<nod.props.Count;j++)
            {
                double[] vals = new double[nod.props[j].values.Count * other_nod.props.Count];
                Parallel.For(0, vals.Length, (i, state) =>
                    {
                        vals[i] = 0.5;
                    });
                nod.props[j].values.Clear();
                nod.props[j].values.AddRange(vals);
            }
            return list;
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
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].id == id_other)
                {
                    other_nod = list[i];
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно добавить связи: исходяющую связь в другой нод и в наш входяющую
            Connect_list con = new Connect_list();
            con.ID = other_nod.id;
            con.Name = other_nod.name;
            other_nod.connects_in.Add(con);

            con = new Connect_list();
            con.ID = nod.id;
            con.Name = nod.name;
            nod.connect_out.Add(con);
            //связи созданы, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j = 0; j < other_nod.props.Count; j++)
            {
                //List<double> vals = nod.props[j].values; //изначальное количество и значения до увеличения значений данного свойства
                double[] vals = new double[nod.props[j].values.Count * other_nod.props.Count];
                Parallel.For(0, vals.Length, (i, state) =>
                {
                    vals[i] = 0.5;
                });
                other_nod.props[j].values.Clear();
                other_nod.props[j].values.AddRange(vals);
            }
            return list;
        }

        public List<Nodes_struct> DeleteNodeConnectIn(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            int jb = 0;
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].id == id_other)
                {
                    for (int j = 0; j < list[i].connect_out.Count; j++)
                    {
                        if (list[i].connect_out[j].ID == nod.id)
                        {
                            list[i].connect_out.RemoveAt(j);
                            jb = i;
                            break;
                        }
                    }
                    state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно удалить связи: исходяющую связь в другой нод и в наш входяющую
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (nod.id == list[i].id)
                {
                    for (int j = 0; j < list[i].connects_in.Count; j++)
                    {
                        if (list[i].connects_in[j].ID == id_other)
                        {
                            list[i].connects_in.RemoveAt(j);
                            break;
                        }
                    }
                    state.Break();
                }
            });
            //связи удалены, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j = 0; j < nod.props.Count; j++)
            {
                List<double> vals = list[jb].props[j].values; //изначальное количество и значения до уменьшения значений данного свойства
                int len = list[jb].props[j].values.Count;
                for (int i = 0; i < nod.props.Count; i++)
                {
                    int count = nod.props[j].values.Count;
                    nod.props[j].values.RemoveRange(count / len, count - count / len); // на этом шаге уменьшаем в два раза количество значений
                }
            }
            return list;
        }

        public List<Nodes_struct> DeleteNodeConnectOut(List<Nodes_struct> list, Nodes_struct nod, int id_other)
        {
            int jb = 0;
            Parallel.For(0, list.Count, (i, state) =>
            {
                if (list[i].id == id_other)
                {
                    for (int j = 0; j < list[i].connects_in.Count;j++ )
                    {
                        if (list[i].connects_in[j].ID == nod.id)
                        {
                            list[i].connects_in.RemoveAt(j);
                            jb = i;
                            break;
                        }
                    }
                        state.Break(); //находим нужный нам нод и выходим из цикла
                }
            });
            //теперь нужно удалить связи: исходяющую связь в другой нод и в наш входяющую
            Parallel.For(0, list.Count, (i, state) =>
                {
                    if (nod.id == list[i].id)
                    {
                        for (int j=0;j<list[i].connect_out.Count;j++)
                        {
                            if (list[i].connect_out[j].ID == id_other)
                            {
                                list[i].connect_out.RemoveAt(j);
                                break;
                            }
                        }
                        state.Break();
                    }
                });
            //связи удалены, теперь нужно добавить дополнительные свойства
            //кол-во значений свойств увел. в количество раз - кол-во свойств другого нода.
            for (int j = 0; j < list[jb].props.Count; j++)
            {
                List<double> vals = nod.props[j].values; //изначальное количество и значения до уменьшения значений данного свойства
                int len = nod.props[j].values.Count;
                for (int i = 0; i < nod.props.Count; i++)
                {
                    int count = list[jb].props[j].values.Count;
                    list[jb].props[j].values.RemoveRange(count / len, count - count / len); // на этом шаге уменьшаем в два раза количество значений
                }
            }
            return list;
        }
    }
}
