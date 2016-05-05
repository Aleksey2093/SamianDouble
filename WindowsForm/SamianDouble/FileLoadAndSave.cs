using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SamianDouble
{
    class FileLoadAndSave
    {
        public List<Nodes_struct> loadFife(List<Nodes_struct> listnodesold)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Multiselect = false;
                op.Filter = "UGL файлы (*.samiandouble)|*.samiandouble|Все файлы (*.*)|*.*";
                DialogResult res = op.ShowDialog();
                if (res != DialogResult.OK)
                    return listnodesold;
                String path = op.FileName;
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                if (listnodesold.Count > 0)
                {
                    res = MessageBox.Show("У вас уже открыт файл. Сохранить его прежде чем открыть новый?", "Вопрос", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        if (!saveToFile(listnodesold))
                        {
                        retgoto:
                            res = MessageBox.Show("Ошибка при сохранении. Повторить попытку или открыть новый файл? "+
                                "Отказавшись от сохранения текущего проекта, вы рискуете потерять изменения, которые еще не сохранены", "Вопрос", 
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (res == DialogResult.Yes && saveToFile(listnodesold) == false)
                            goto retgoto;
                        }
                    }
                    else
                    {
                    retelse:
                        res = MessageBox.Show("Отказавшись от сохранения текущего проекта, вы рискуете потерять изменения, которые еще не сохранены. Сохранить открытый проект?", "Вопрос",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (res == DialogResult.Yes && saveToFile(listnodesold) == false)
                            goto retelse;
                    }
                }
                List<Nodes_struct> listnodes = new List<Nodes_struct>();
                foreach (XmlNode line in doc.DocumentElement)
                {
                    Nodes_struct nod = new Nodes_struct();
                    nod.id = int.Parse(line.ChildNodes[0].InnerText);
                    nod.name = line.ChildNodes[1].InnerText;
                    nod.props = new List<Propertys_struct>();
                    foreach (XmlNode pxml in line.ChildNodes[2].ChildNodes) //проверти
                    {
                        Propertys_struct prrrr = new Propertys_struct();
                        prrrr.name = pxml.ChildNodes[0].InnerText;
                        prrrr.values = new List<double>();
                        foreach (XmlNode valuexml in pxml.ChildNodes[1].ChildNodes) //значения из списка
                        {
                            double val;
                            if (!double.TryParse(valuexml.InnerText, out val))
                                if (!double.TryParse(valuexml.InnerText.Replace(".", ","), out val))
                                    if (!double.TryParse(valuexml.InnerText.Replace(",", "."), out val))
                                    {
                                        MessageBox.Show("Указанный файл хранит данные неподходящие для приложения. Выберите правильный файл.", "Ошибка",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                        return listnodesold;
                                    }
                            prrrr.values.Add(val);
                        }
                        nod.props.Add(prrrr);
                    }
                    if (line.ChildNodes.Count > 2)
                    {
                        List<Connect_list> connecslist = new List<Connect_list>();
                        Connect_list oneconnect = new Connect_list();

                        foreach (XmlNode conxml in line.ChildNodes[3])
                        {
                            oneconnect = new Connect_list();
                            oneconnect.ID = int.Parse(conxml.ChildNodes[0].InnerText);
                            oneconnect.Name = conxml.ChildNodes[1].InnerText;
                            connecslist.Add(oneconnect);
                        }

                        if (line.ChildNodes[3].Name == "in")
                        {
                            nod.connects_in = connecslist;
                        }
                        else if (line.ChildNodes[3].Name == "out")
                        {
                            nod.connect_out = connecslist;
                        }
                        else
                        {
                            MessageBox.Show("Указанный файл хранит данные неподходящие для приложения. Выберите правильный файл.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return listnodesold;
                        }
                    }
                    if (line.ChildNodes.Count > 3)
                    {
                        List<Connect_list> connecslist = new List<Connect_list>();
                        Connect_list oneconnect = new Connect_list();

                        foreach (XmlNode conxml in line.ChildNodes[4])
                        {
                            oneconnect = new Connect_list();
                            oneconnect.ID = int.Parse(conxml.ChildNodes[0].InnerText);
                            oneconnect.Name = conxml.ChildNodes[1].InnerText;
                            connecslist.Add(oneconnect);
                        }

                        if (line.ChildNodes[4].Name == "in")
                        {
                            nod.connects_in = connecslist;
                        }
                        else if (line.ChildNodes[4].Name == "out")
                        {
                            nod.connect_out = connecslist;
                        }
                        else
                        {
                            MessageBox.Show("Указанный файл хранит данные неподходящие для приложения. Выберите правильный файл.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return listnodesold;
                        }
                    }
                    listnodes.Add(nod);
                }
                if (проверкаЗагруженногоФайла(listnodes))
                    return listnodes;
                else
                    return listnodesold;
            }
            catch (XmlException ex)
            {
                MessageBox.Show("Указанный файл хранит данные неподходящие для приложения. Выберите правильный файл. \n" + ex.ToString(), "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return listnodesold;
            }
        }

        public bool проверкаЗагруженногоФайла(List<Nodes_struct> list)
        {
            bool corr = true;
            List<string> errrors = new List<string>();
            Parallel.ForEach(list, (nod, statenod) =>
            {
                if (nod.name == null || nod.name == "")
                {
                    corr = false;
                    errrors.Add("Имя узла с ид " + nod.id + "задано не правильно;");
                    statenod.Break(); //имя не сошлось
                }
                List<int> сколькодолжныбытьзначений = new List<int>();
                Parallel.ForEach(nod.props, (ppprop, stateppprop) =>
                    {
                        сколькодолжныбытьзначений.Add(ppprop.values.Count);
                    });
                Parallel.ForEach(сколькодолжныбытьзначений, (зн1, sttta) =>
                    {
                        Parallel.ForEach(сколькодолжныбытьзначений, (зн2, staaw) =>
                            {
                                if (зн1 != зн2)
                                {
                                    errrors.Add("значение в узле id " + nod.id + " name " + nod.name + " заданы неправильно." +
                                        "Похоже, что количество значений у свойств разное.");
                                    corr = false;
                                    sttta.Break();
                                }
                            });
                        if (corr == false)
                            sttta.Break();
                    });
                if (corr == false)
                    statenod.Break(); //непорядок со значениями
                List<int> колвознаподм = new List<int>();
                Parallel.ForEach(nod.connects_in, (conn, stateподключени) =>
                    {
                        bool нашли = false;
                        Parallel.ForEach(list, (ot, stateot) =>
                            {
                                if (ot.id == conn.ID)
                                {
                                    if (ot.name != conn.Name)
                                    {
                                        conn.Name = ot.name;
                                    }
                                    нашли = true;
                                    колвознаподм.Add(ot.props.Count);
                                    stateot.Break();
                                }
                            });
                        if (!нашли)
                        {
                            corr = false;
                            stateподключени.Break();
                        }
                    });
                if (corr == false)
                    statenod.Break();
                Parallel.For(1, колвознаподм.Count, (i, stateпод) =>
                {
                    колвознаподм[0] = колвознаподм[0] * колвознаподм[i];
                });
                try
                {
                    if (колвознаподм[0] == сколькодолжныбытьзначений[0])
                    {
                        corr = false;
                        statenod.Break();
                    }
                }
                catch
                {
                    if (колвознаподм.Count != 0 && сколькодолжныбытьзначений[0] != 1)
                    {
                        corr = false;
                        statenod.Break();
                    }
                }
            });
            return corr;
        }

        public bool saveToFile(List<Nodes_struct> listnodes)
        {
            if (listnodes.Count == 0)
            {
                MessageBox.Show("Сохранять нечего", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "UGL файлы (*.samiandouble)|*.samiandouble|Все файлы (*.*)|*.*";
            s.DefaultExt = ".samiandouble";
            if (s.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return false;
            String path = s.FileName;
            List<String> lineslist = new List<string>();
            lineslist.Add("<variables>");
            foreach (Nodes_struct nod in listnodes)
            {
                lineslist.Add("\t<node>");


                lineslist.Add("\t<id>" + nod.id + "</id>");
                lineslist.Add("\t<name>" + nod.name + "</name>");

                lineslist.Add("\t<propertis>");
                foreach (Propertys_struct pr in nod.props)
                {
                    lineslist.Add("\t\t<prop>");
                    lineslist.Add("\t\t\t<name>" + pr.name + "</name>");
                    lineslist.Add("\t\t\t<values>");
                    foreach (double val in pr.values)
                    {
                        lineslist.Add("\t\t\t\t<value>" + val + "</value>");
                    }
                    lineslist.Add("\t\t\t</values>");
                    lineslist.Add("\t\t</prop>");
                }
                lineslist.Add("\t</propertis>");



                lineslist.Add("\t<in>");
                foreach (Connect_list connect in nod.connects_in)
                {
                    lineslist.Add("\t\t<connect>");
                    lineslist.Add("\t\t\t<idcon>" + connect.ID + "</idcon>");
                    lineslist.Add("\t\t\t<namecon>" + connect.Name + "</namecon>");
                    lineslist.Add("\t\t</connect>");
                }
                lineslist.Add("\t</in>");


                lineslist.Add("\t<out>");
                foreach (Connect_list connect in nod.connect_out)
                {
                    lineslist.Add("\t\t<connect>");
                    lineslist.Add("\t\t\t<idcon>" + connect.ID + "</idcon>");
                    lineslist.Add("\t\t\t<namecon>" + connect.Name + "</namecon>");
                    lineslist.Add("\t\t</connect>");
                }
                lineslist.Add("\t</out>");


                lineslist.Add("\t</node>");
            }
            lineslist.Add("</variables>");
            System.IO.File.WriteAllLines(path, lineslist.ToArray());
            return true;
        }
    }
}
