using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace Serialization
{
    public partial class Form1 : Form
    {
        public static AllPersons allPersons = new AllPersons();
        private XmlSerializer xmlSerializer = new XmlSerializer(typeof(AllPersons));

        private string path = "../../files/objects.xml";

        public Form1()
        {
            InitializeComponent();

            
        }
        public void Serialize(String path)
        {
            StreamWriter streamWriter = new StreamWriter(path);
            xmlSerializer.Serialize(streamWriter, allPersons);
            streamWriter.Close();
        }
        private void UpdateListObjects()
        {
            listBox1.Items.Clear();
            foreach (var item in allPersons.Managers)
            {
                listBox1.Items.Add(item.GetType().Name);
            }
            foreach (var item in allPersons.Workers)
            {
                listBox1.Items.Add(item.GetType().Name);
            }
            foreach (var item in allPersons.Individuals)
            {
                listBox1.Items.Add(item.GetType().Name);
            }
            foreach (var item in allPersons.Entitys)
            {
                listBox1.Items.Add(item.GetType().Name);
            }

        }

        private void Form3_CLOSE(object sender, FormClosingEventArgs e)
        {
            UpdateProperties();
        }

        public void UpdateProperties()
        {
            if (listBox1.SelectedIndex >= 0)
            {
                object obj = listBox1.SelectedItem;
                object selectedObject = GetObjectFromList(listBox1.SelectedIndex);
                string name = obj as string;
                var oneType = typeof(ListPersons).GetField(name + "s");
                var type = oneType.FieldType;
                var fields = type.GetProperties();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                int i = 0;
                foreach (var p in fields)
                {
                    listBox2.Items.Add(p.Name);
                    listBox3.Items.Add(p.GetValue(selectedObject));
                    i++;
                }
            }
        }

        public void DeSerialize(String path)
        {
            StreamReader streamReader = new StreamReader(path);
            allPersons = (AllPersons)xmlSerializer.Deserialize(streamReader);
            streamReader.Close();
            UpdateListObjects();    
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var newForm = new Form2();
            newForm.Show();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Serialize(path);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DeSerialize(path);
        }

        private object GetObjectFromList(int index)
        {
            int i = 0;
            foreach (var item in allPersons.Managers)
            {
                if(i == index)
                    return item;
                i++;
            }
            foreach (var item in allPersons.Workers)
            {
                if (i == index)
                    return item;
                i++;
            }
            foreach (var item in allPersons.Individuals)
            {
                if (i == index)
                    return item;
                i++;
            }
            foreach (var item in allPersons.Entitys)
            {
                if (i == index)
                    return item;
                i++;
            }
            return null;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProperties();
            listBox2.SelectedIndex = 0;
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateListObjects();
            if(listBox1.Items.Count == 0)
            {
                listBox2.Items.Clear();
                listBox3.Items.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int num = listBox1.SelectedIndex;
            listBox1.SelectedIndex = 0;
            int i = 0;
            foreach (var item in allPersons.Managers)
            {
                if (i == num)
                {
                    allPersons.Managers.Remove(item);
                    UpdateListObjects();
                    return;
                }
                ++i;
            }
            foreach (var item in allPersons.Workers)
            {
                if (i == num)
                {
                    allPersons.Workers.Remove(item);
                    UpdateListObjects();
                    return;
                }
                ++i;
            }
            foreach (var item in allPersons.Individuals)
            {
                if (i == num)
                {
                    allPersons.Individuals.Remove(item);
                    UpdateListObjects();
                    return;
                }
                ++i;
            }
            foreach (var item in allPersons.Entitys)
            {
                if (i == num)
                {
                    allPersons.Entitys.Remove(item);
                    UpdateListObjects();
                    return;
                }
                ++i;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex >=0)
            {
                int num = listBox1.SelectedIndex;
                int i = 0;
                foreach (var item in allPersons.Managers)
                {
                    if (i == num)
                    {
                        var newForm = new Form3(item, typeof(Manager), listBox2.Items[listBox2.SelectedIndex].ToString());
                        newForm.Show();
                        newForm.FormClosing += Form3_CLOSE;
                        return;
                    }
                    ++i;
                }
                foreach (var item in allPersons.Workers)
                {
                    if (i == num)
                    {
                        var newForm = new Form3(item, typeof(Worker), listBox2.Items[listBox2.SelectedIndex].ToString());
                        newForm.Show();
                        newForm.FormClosing += Form3_CLOSE;
                        return;
                    }
                    ++i;
                }
                foreach (var item in allPersons.Individuals)
                {
                    if (i == num)
                    {
                        var newForm = new Form3(item, typeof(Individual), listBox2.Items[listBox2.SelectedIndex].ToString());
                        newForm.Show();
                        newForm.FormClosing += Form3_CLOSE;
                        return;
                    }
                    ++i;
                }
                foreach (var item in allPersons.Entitys)
                {
                    if (i == num)
                    {
                        var newForm = new Form3(item, typeof(Entity), listBox2.Items[listBox2.SelectedIndex].ToString());
                        newForm.Show();
                        newForm.FormClosing += Form3_CLOSE;
                        return;
                    }
                    ++i;
                }
            }
            
        }
    }
}
