using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serialization
{
    public partial class Form2 : Form
    {
        Type type;
        List<object> fields = new List<object>();

        object createdVariable;

        public Form2()
        {
            InitializeComponent();
            var properteis = typeof(AllPersons).GetFields();
            foreach(var p in properteis)
            {
                checkedListBox1.Items.Add(p.Name);
            }
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
                checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
            }
            object obj = checkedListBox1.SelectedItem;
            string name = obj as string;
            //string name2 = obj.GetType().Name;
            //string name3 = obj.GetType().FullName;
            //string name4 = obj.GetType().ToString();
            var oneType = typeof(ListPersons).GetField(name);
            //var property = typeof(AllPersons).GetProperty(name);
            //Type type = property.GetType();
            //var properteis = type.GetFields();
            type = oneType.FieldType;
            createdVariable = Activator.CreateInstance(type);
            //var typeOfList = type.GetType().GetGenericArguments().GetType();
            //var typeOfList = GetTypeList(type.get);
            var fields = type.GetProperties();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            var tempValue = Activator.CreateInstance(type);
            foreach (var p in fields)
            {
                listBox1.Items.Add(p.Name);
                listBox2.Items.Add("Enter...");
                listBox3.Items.Add(p.GetValue(tempValue).GetType().Name);
            }
        }
        static Type GetTypeList<T>(IEnumerable<T> enumerable) where T : System.Collections.IEnumerable
        {
            return typeof(T);
        }
        static List<T> CreateTypeList<T>(IEnumerable<T> enumerable) //where T : System.Collections.IEnumerable
        {
            return new List<T>();
        }
       
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if(listBox1.SelectedIndex >=0)
            textBox1.Text = listBox1.Items[listBox1.SelectedIndex].ToString();*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //if((listBox3.Items[listBox1.SelectedIndex])textBox1.Text)
                string typeOfField = listBox3.Items[listBox1.SelectedIndex].ToString();
                // Type typeOfField = listBox3.Items[listBox1.SelectedIndex].GetType();
                //   var field = Activator.CreateInstance(typeOfField);
                var thisType = type.GetProperty(listBox1.Items[listBox1.SelectedIndex].ToString()).PropertyType;
                object og = Convert.ChangeType(textBox1.Text, thisType);
                var prop = type.GetProperty(listBox1.Items[listBox1.SelectedIndex].ToString());
                prop.SetValue(createdVariable, og);
                /*
                if (typeOfField== "Boolean")
                {
                    bool bl = Convert.ToBoolean(textBox1.Text);
                    //object og = Convert.ChangeType(textBox1.Text, typeof(object));
                    var prop = type.GetProperty(listBox1.Items[listBox1.SelectedIndex].ToString());
                    prop.SetValue(createdVariable, bl);

                }*/
                // field = textBox1.Text;
                listBox2.Items[listBox1.SelectedIndex] = textBox1.Text;
                if (IsAllFields())
                {
                    button2.Enabled = true;
                    listBox1.Enabled = false;
                }

                else
                {
                    button2.Enabled = false;
                    listBox1.Enabled = true;
                }


                /* {
                     MessageBox.Show("can't convert. Check nessesary type");
                 }*/
            }
            catch
            {
                MessageBox.Show("enter nessesary type");
            }


        }
        
        private bool IsAllFields()
        {
            foreach (var item in listBox2.Items)
            {
                if ((string)item == "Enter..." || (string)item == "" || (string)item == " ")
                    return false;
            }
            return true;
        }

        private void AddObject(Type type, object existField, object obj, string typename)
        {
            var field = typeof(AllPersons).GetField(type.Name+"s");
            var value = field?.GetValue(Form1.allPersons);
            ((IList)value).Add(obj);
            //value.GetType().GetProperty(typename).SetValue(existField, value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // var temp  = Activator.CreateInstance(type, listBox2.Items);
            
            var field = typeof(AllPersons).GetField(type.Name+"s");
            var value = field?.GetValue(Form1.allPersons);
            //((List<object>)value).Add(temp);
            Type type2 = type;
            AddObject(type2, value, createdVariable, type2.Name);
            //var list = CreateTypeList(Activator.CreateInstance(List.);

            //var value2 = ((IEnumerable<Person>)value).ToList();
            //field?.SetValue(Form1.allPersons, value2);
            this.Close();
            this.Dispose();
        }

    }
}
