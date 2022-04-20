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
using System.Threading;

namespace Serialization
{
    public partial class Form1 : Form
    {
        public static AllPersons allPersons = new AllPersons();
        //private XmlSerializer xmlSerializer = new XmlSerializer(typeof(AllPersons));

        private string path = "../../files/objects.xml";

        public Form1()
        {
            InitializeComponent();


        }

        private void SerializeList(StreamWriter streamWriter, object prop, int countTab)
        {
            var enumer = (IList)prop;
            foreach (var value in enumer)
            {
                for (int i = 0; i < countTab; i++)
                    streamWriter.Write("\t");
                streamWriter.WriteLine("<" + value.GetType().Name + ">");

                if (value.GetType().GetInterface("IList", true) != null) 
                {   
                    SerializeList(streamWriter, value,countTab+1);
                }
                else
                {
                    var propeties = value.GetType().GetProperties();
                    foreach (var prop2 in propeties)
                    {
                        if(prop2.PropertyType.GetInterface("Ilist",true) != null)
                        {
                            for (int i = 0; i < countTab + 1; i++)
                                streamWriter.Write("\t");
                            streamWriter.WriteLine("<" + prop2.Name + ">");
                            SerializeList(streamWriter, prop2.GetValue(value), countTab + 2);
                            for (int i = 0; i < countTab + 1; i++)
                                streamWriter.Write("\t");
                            streamWriter.WriteLine("</" + prop2.Name + ">");

                        }
                        else
                        {
                            for (int i = 0; i < countTab+1; i++)
                                streamWriter.Write("\t");
                            streamWriter.WriteLine("<" + prop2.Name + ">" +
                                prop2.GetValue(value) + "</" + prop2.Name + ">");
                        }                            
                    }
                    if(propeties.Length == 0)
                    {
                        streamWriter.Flush();
                        streamWriter.BaseStream.Position= streamWriter.BaseStream.Length-2;
                        streamWriter.Write(value);
                        streamWriter.WriteLine("</" + value.GetType().Name + ">");
                        continue;
                    }
                }

                for (int i = 0; i < countTab; i++)
                    streamWriter.Write("\t");
                streamWriter.WriteLine("</" + value.GetType().Name + ">");
            }
        }

        public void Serialize(String path)
        {
           /*StreamWriter streamWriter = new StreamWriter(path);
            xmlSerializer.Serialize(streamWriter, allPersons);
            streamWriter.Close();*/
            StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            streamWriter.WriteLine("<AllPersons xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");

            var type = typeof(AllPersons);
            var fields = type.GetProperties();
            foreach (var field in fields)
            {
                var prop = field.GetValue(allPersons);
                if(prop!= null)
                {
                    streamWriter.WriteLine("\t <" + field.Name + ">");

                    SerializeList(streamWriter, prop,2);

                    streamWriter.WriteLine("\t </" + field.Name + ">");
                }
            }
            streamWriter.WriteLine("</AllPersons>");
            streamWriter.Close();
        }

        public void DeSerializeList(Type type, string nextline,string listName, string str, StreamReader streamReader, string typeName, object fieldValue)
        {
            while (nextline != listName)
            {

                var elementofListType = type.GetProperty(listName).PropertyType; //Individual(TYPE)
                var obj = Activator.CreateInstance(elementofListType);  //make object individual
                str = streamReader.ReadLine();
                string FieldFullName = GetName(str);

                while (FieldFullName != typeName)
                {
                    string VariableValue = GetVariableValue(str);

                    if(VariableValue =="")
                    {
                        str = streamReader.ReadLine();
                        var NameList = GetName(str);
                        var vloshList = elementofListType.GetProperty(FieldFullName);

                        DeSerializeList(vloshList.PropertyType, NameList, FieldFullName, str, streamReader, NameList, vloshList.GetValue(obj));
                    }
                    else
                    {
                        var FieldOfListType = elementofListType.GetProperty(FieldFullName).PropertyType;

                        object value;
                        if (FieldOfListType != typeof(string))
                        {
                            value = Activator.CreateInstance(FieldOfListType);
                        }

                        try
                        {
                            value = Convert.ChangeType(VariableValue, FieldOfListType);
                        }
                        catch
                        {
                            MessageBox.Show("uncorreect type");
                            return;
                        }

                        elementofListType.GetProperty(FieldFullName).SetValue(obj, value, null); // нету приведения к типу 
                        str = streamReader.ReadLine();
                        FieldFullName = GetName(str);
                    }
                    
                }
                var list = (IList)fieldValue;
                list.Add(obj);
                nextline = streamReader.ReadLine();
                nextline = GetName(nextline);
            }
        }

        public void DeSerialize(String path)
        {
            StreamReader streamReader = new StreamReader(path);
            //allPersons = (AllPersons)xmlSerializer.Deserialize(streamReader);

            allPersons = new AllPersons();
            streamReader.ReadLine();
            streamReader.ReadLine();
            string str = streamReader.ReadLine(); //<<<individuals
            while (str != "</AllPersons>")
            {
                string listName = GetName(str);  //individuals
                var fieldList = typeof(AllPersons).GetProperty(listName);
                var fieldType = fieldList.PropertyType;
                var fieldValue = fieldList.GetValue(allPersons);

                //fieldValue = Activator.CreateInstance(fieldType);

                str = streamReader.ReadLine();   //<<<individual
                string nextline = GetName(str);     //individual
                string typeName = nextline;         //individual

                DeSerializeList(typeof(ListPersons), nextline, listName, str, streamReader, typeName, fieldValue);

                str = streamReader.ReadLine();

            }



            streamReader.Close();
            UpdateListObjects();
        }


        //////////////////////// NEW DESERIALIZE

        private int GetCountTabs(string str)
        {
            int result = 0;
            int i = 0;
            while (str[i]=='\t' && i<str.Length)
            {
                result++;
                i++;
            }
            return result;
        }

        private  bool IsList(StreamReader reader, string currentStr)
        {
            reader.BaseStream.Flush();
            reader.BaseStream.FlushAsync();

            long pos = reader.BaseStream.Position;
            var str = reader.ReadLine();

            reader.BaseStream.Flush();
            reader.BaseStream.FlushAsync();

            Thread.Sleep(500);

            reader.BaseStream.Position = pos;
            reader.BaseStream.Flush();
            reader.BaseStream.FlushAsync();
            Thread.Sleep(500);

            if (GetCountTabs(str) > GetCountTabs(currentStr))
                return true;
            else
                return false;
        }

        public void DeSerializeList2(StreamReader reader, string listName, IList currentList, Type typeElements)
        {
            string currentStr = reader.ReadLine();
            var clearName = GetName(currentStr); //individual
            int j = 0; 

            while(clearName != listName) // listName = Individuals
            {
                var currentField = typeof(ListPersons).GetProperty(listName);//need listname

                object currentObj;

                if (currentField != null)
                {

                    currentObj = Activator.CreateInstance(typeElements);

                    currentList.Add(currentObj);/////////
                    DeSerializeObject2(reader, clearName, currentObj, typeElements);
                    currentStr = reader.ReadLine();
                    clearName = GetName(currentStr);
                }
                else
                {
                    if (currentList.GetType().IsArray)
                    {
                        
                        if(typeElements.GetInterface("IList", true) != null)
                        {
                            int length = ((IList)currentList[0]).Count;
                            currentObj = Activator.CreateInstance(typeElements, length);
                        }
                            
                        else
                            currentObj = Activator.CreateInstance(typeElements);
                    }
                    else
                    {
                        currentObj = Activator.CreateInstance(typeElements);

                    }

                    if (typeElements.GetInterface("IList", true) != null)
                    {
                        Type listItemsType;
                        if (!currentList.GetType().IsArray)
                        {
                            currentList.Add(currentObj);
                            listItemsType = ((IList)currentObj).GetType().GetGenericArguments().Single();
                            DeSerializeList2(reader, clearName, (IList)currentObj, listItemsType);
                        }
                        else 
                        {
                            listItemsType = ((IList)currentObj).GetType().GetElementType();
                            currentList[j] = currentObj;
                            
                            DeSerializeList2(reader, clearName, (IList)currentList[j], listItemsType);
                            ++j;
                        }

                            /////////
                        
                        
                        currentStr = reader.ReadLine();
                        clearName = GetName(currentStr);
                    }
                    else
                    {
                        //currentStr = reader.ReadLine();
                        //clearName = GetName(currentStr);
                        var fieldValueStr = GetVariableValue(currentStr);
                        object fieldValue;
                        int i = 0;
                        while (clearName != listName)
                        {

                            try
                            {
                                fieldValue = Convert.ChangeType(fieldValueStr, typeElements);
                            }
                            catch
                            {
                                fieldValue = null;
                                MessageBox.Show("uncorrect type");
                            }
                            currentObj = Activator.CreateInstance(typeElements);
                            currentObj = fieldValue;
                            
                            if(currentList.GetType().IsArray)
                            {
                                currentList[i] = currentObj;
                                ++i;
                            }
                            else
                                currentList.Add(currentObj);
                            currentStr = reader.ReadLine();
                            clearName = GetName(currentStr);
                            fieldValueStr = GetVariableValue(currentStr);
                        }
                    }
                }

            }
        }

        public void DeSerializeObject2(StreamReader reader, string className, 
                                        object currentObject, Type typeObject)
        {
            string currentStr = reader.ReadLine();
            string VariableValue = GetVariableValue(currentStr);
            currentStr = GetName(currentStr); //currentStr = Individuals

            while (currentStr != className) // className = AllPersons
            {
                var field = typeObject.GetProperty(currentStr);
                var typeField = field.PropertyType;
                var fieldValue = field.GetValue(currentObject); //object individuals
                if (typeField.GetInterface("IList", true) != null) 
                {
                    if(typeField.IsArray)
                    {
                        int size = ((IList)fieldValue).Count;
                        //fieldValue = Activator.CreateInstance(typeField, size);
                    }
                    else
                    {
                        fieldValue = Activator.CreateInstance(typeField); // have to field/set value fieldvalue
                    }

                    Type listItemsType;
                    if (fieldValue.GetType().IsArray)
                    {
                        listItemsType = ((IList)fieldValue).GetType().GetElementType();
                    }
                    else
                    {
                        listItemsType = ((IList)fieldValue).GetType().GetGenericArguments().Single();
                    }
                    
                    DeSerializeList2(reader, currentStr, (IList)fieldValue, listItemsType);
                    field.SetValue(currentObject, (IList)fieldValue);
                }
                else
                {
                    object value;
                    if (typeField != typeof(string))
                    {
                        value = Activator.CreateInstance(typeField);
                    }

                    try
                    {
                        value = Convert.ChangeType(VariableValue, typeField);
                    }
                    catch
                    {
                        MessageBox.Show("uncorreect type");
                        return;
                    }
                    field.SetValue(currentObject, value, null);
                }
                currentStr = reader.ReadLine();
                VariableValue = GetVariableValue(currentStr);
                currentStr = GetName(currentStr); //currentStr = Individuals
            }

        }

        public void Deserialize2(String path)
        {
            StreamReader reader = new StreamReader(path);

            reader.ReadLine();  

            string currentString = reader.ReadLine();
            currentString = GetName(currentString);

            /* Type currentType = typeof(AllPersons);
             currentType.GetProperty(currentString);*/

            //object currentObject = allPersons;

            DeSerializeObject2(reader, currentString, allPersons, typeof(AllPersons));

            reader.Close();

            UpdateListObjects();

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
                var oneType = typeof(ListPersons).GetProperty(name + "s");
                var type = oneType.PropertyType;
                var fields = type.GetProperties();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                int i = 0;
                foreach (var p in fields)
                {
                    listBox2.Items.Add(p.Name);
                    
                    var tempobj = p.GetValue(selectedObject);
                    i++;
                    if (tempobj.GetType().GetInterface("IList", true) != null)
                    {
                        string tempstr = GetStringList((IList)tempobj);
                        listBox3.Items.Add(tempstr);
                    }
                    else
                        listBox3.Items.Add(tempobj);
                }
            }
        }

        private string GetStringList(IList tempobj)
        {
            string result = "  ";
            foreach(var element in tempobj)
            {
                if (element.GetType().GetInterface("IList", true) != null)
                {
                    result += " " + GetStringList((IList)element);
                }
                else
                    result += " " + element.ToString();
            }
            return result;
        }

        private string GetName(string str)
        {
            string result = "";
            int i = 0;
            while (i < str.Length && str[i]!='<')
            {
                ++i;
            }
            ++i;
            while(i < str.Length && str[i]!='>' && str[i] != ' ')
            {

                if (str[i] == '/')
                {
                    ++i;
                    continue;
                }
                result += str[i];
                ++i;
            }
            return result;
        }

        private string GetVariableValue(string str)
        {
            string result = "";
            int i = 0;
            while (i < str.Length && str[i] != '>')
            {
                ++i;
            }
            ++i;
            while (i < str.Length && str[i] != '<')
            {
                result += str[i];
                ++i;
            }
            return result;
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
            //DeSerialize(path);
            Deserialize2(path);
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
