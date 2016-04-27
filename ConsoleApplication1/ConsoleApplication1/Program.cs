using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static List<Node.Nodes_struct> listnodes_global = new List<Node.Nodes_struct>();
        static void Main(string[] args)
        {
            Console.Title = "Samian double";
            Console.WriteLine("Введите команду: ");
            while(true)
            {
                String command = Console.ReadLine();
                if (command == "-addnode")
                {
                    addNode();
                }
                else if (command == "-delnode")
                {
                    deleteNode();
                }
                else if (command == "-loadfile")
                {
                    loadFromFie();
                }
                else if (command == "-savefile")
                {
                    saveToFile();
                }
                else
                {
                    Console.WriteLine("Вы ввели несвязный бред повторите ввод");
                    continue;
                }
                Console.WriteLine("Введите новую команду: ");
            }
        }
        private static void addNode()
        {
            Node node_class = new Node();
            Node.Nodes_struct node = node_class.get_new_node(listnodes_global);
            ret1: Console.WriteLine("-----------------\nВведите название узла");
            node.name = Console.ReadLine();
            if (node_class.prov_new_name(listnodes_global,node.name))
            {
            ret2:
                Console.WriteLine("Узел с таким именем уже есть в базе. Хотите изменить имя (true/false)?");
                bool ifi = true;
                if (bool.TryParse(Console.ReadLine(), out ifi) && ifi)
                {
                    goto ret1;
                }
                else if (ifi == false)
                {
                    //идем дальше;
                }
                else
                {
                    goto ret2;
                }
            }
            Console.WriteLine("-----------------\nНачинается ввод свойст узла (по завершении ввода введите '-exit'):");
            int p_count = 0;
            while(true)
            {
                Node.Propertys_struct prop = new Node.Propertys_struct();
                Console.WriteLine("Введите имя свойства и значение (Например, 'Хорошо 0.5')");
                String tmp = Console.ReadLine();
                if (tmp == "-exit")
                {
                    if (p_count >= 2)
                    {
                        Console.WriteLine("Ввод свойств окончен");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Вы ввели недостаточно свойств чтобы закончить ввод. Нужно ввести минимум два свойства");
                        continue;
                    }
                }
                String tmpname = ""; int j = -1;
                for (int i=0;i<tmp.Length;i++)
                    if (tmp[i] != ' ')
                        tmpname += tmp[i];
                    else { j = i + 1; break; }
                if (j < 2)
                    continue;
                tmp = tmp.Remove(0, j);
                double value = -1;
                if (!double.TryParse(tmp, out value))
                    continue;
                prop.name = tmpname;
                Node.Propertys_struct_value val = new Node.Propertys_struct_value();
                val.value = value;
                prop.values.Add(val);
                node.props.Add(prop);
            }
            if (listnodes_global.Count != 0)
            {
                Console.WriteLine("-----------------\nДобавьте исходящие связи к другим узла которые сейчас есть в базе. " +
                    "Ниже представлен список доступных узлов введите номер узла к которому вы хотите направить связь от создаемого," +
                    " если связей несколько то введите несколько цифр через пробел");
                for (int i = 0; i < listnodes_global.Count; i++)
                {
                    Console.WriteLine(listnodes_global[i].id + " " + listnodes_global[i].name);
                }
                Console.WriteLine("Выберите один узел или несколько из списка выше");
                List<int> v = new List<int>();
                String tmp = Console.ReadLine();
                try
                {
                    v.Add(int.Parse(tmp));
                }
                catch (Exception)
                {
                    tmp += " ";
                    int i = 0, j = 0;
                    while (i < tmp.Length)
                    {
                        if (tmp[i] != ' ')
                            i++;
                        else
                        {
                            v.Add(int.Parse(tmp.Substring(j, i - j)));
                            j = i + 1;
                        }
                    }
                }
            ret3:
                for (int i = 0; i < v.Count; i++)
                    for (int j = 0; j < v.Count; j++)
                        if (v[i] == v[j])
                        {
                            v.RemoveAt(i);
                            goto ret3;
                        }
                for (int i = 0; i < listnodes_global.Count; i++)
                    for (int j = 0; j < v.Count; j++)
                    {
                        if (listnodes_global[i].id == v[i])
                        { //найдена пара

                        }
                    }
            }
        }
        private static void deleteNode()
        {
            throw new NotImplementedException();
        }
        private static void loadFromFie()
        {
            throw new NotImplementedException();
        }
        private static void saveToFile()
        {
            throw new NotImplementedException();
        }
    }
    class Node
    {
        public struct Nodes_struct
        {
            public int id;
            public string name;
            public List<Propertys_struct> props;
            public List<int> connects;
        };
        public struct Propertys_struct
        {
            public String name;
            public List<Propertys_struct_value> values;
        };
        public struct Propertys_struct_value
        {
            public double value;
            public List<int> id_источника;
        };

        public int get_new_id(List<Nodes_struct> list)
        {
            int new_id = -1;

            for (int i = 0; i < list.Count;i++ )
            {
                if (new_id < list[i].id)
                    new_id = list[i].id;
            }
            return new_id+1;
        }
        public Nodes_struct get_new_node(List<Nodes_struct> list)
        {
            Nodes_struct nod = new Nodes_struct();
            nod.id = get_new_id(list);
            nod.name = "New Node " + nod.id;
            nod.props = new List<Propertys_struct>();
            return nod;
        }
        public bool prov_new_name(List<Nodes_struct> list, String name)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].name == name)
                    return true;
            return false;
        }
    }
}