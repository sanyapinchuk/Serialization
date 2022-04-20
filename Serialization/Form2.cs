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
            var properteis = typeof(AllPersons).GetProperties();
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
            var oneType = typeof(ListPersons).GetProperty(name);
            type = oneType.PropertyType;
            createdVariable = Activator.CreateInstance(type);
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string typeOfField = listBox3.Items[listBox1.SelectedIndex].ToString();
                var thisType = type.GetProperty(listBox1.Items[listBox1.SelectedIndex].ToString()).PropertyType;
                object og = Convert.ChangeType(textBox1.Text, thisType);
                var prop = type.GetProperty(listBox1.Items[listBox1.SelectedIndex].ToString());
                prop.SetValue(createdVariable, og);

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
            var field = typeof(AllPersons).GetProperty(type.Name+"s");
            var value = field?.GetValue(Form1.allPersons);
            ((IList)value).Add(obj);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            var field = typeof(AllPersons).GetProperty(type.Name+"s");
            var value = field?.GetValue(Form1.allPersons);
            Type type2 = type;
            AddObject(type2, value, createdVariable, type2.Name);

            this.Close();
            this.Dispose();
        }

    }
}
