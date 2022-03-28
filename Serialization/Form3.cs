using System;
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
    public partial class Form3 : Form
    {
        object obj;
        Type type;
        string property;
        Type propertyType;

        System.Reflection.PropertyInfo prop;
        public Form3(object obj,Type type, String property)
        {
            InitializeComponent();
            this.obj = obj;
            this.type = type;
            this.property = property;

            prop = type.GetProperty(property);
            
            propertyType = prop.PropertyType;
            label1.Text= "(" + propertyType.Name + ")";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                object og = Convert.ChangeType(textBox1.Text, propertyType);
                
                prop.SetValue(obj, og);

                

                this.Close();
                this.Dispose();
            }
            catch
            {
                MessageBox.Show("enter currect type");
            }
        }

    }
}
