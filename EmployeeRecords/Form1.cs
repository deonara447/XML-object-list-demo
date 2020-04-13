using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

/// <summary>
/// This program was created to demonstrate using an XML file
/// to read information when the program starts into a data list
/// of objects. When the program closes is saves the current contents 
/// of the data list back to the XML file. 
/// </summary>


namespace EmployeeRecords
{
    public partial class mainForm : Form
    {
        List<Employee> employeeDB = new List<Employee>();

        public mainForm()
        {
            InitializeComponent();
            loadDB(); // load data from XML file with program starts
        }

        private void addButton_Click(object sender, EventArgs e)
        {   
            int newID = Convert.ToInt16(idInput.Text);
            string newFN = fnInput.Text;
            string newLN = lnInput.Text;
            string newDate = dateInput.Text;
            string newSalary = salaryInput.Text;

            Employee newEmp = new Employee(newID, newFN, newLN, newDate, newSalary);
            employeeDB.Add(newEmp);

            outputLabel.Text = "new employee added successfully";

            //using lambda expression to sort object list by employee id
            employeeDB = employeeDB.OrderBy(emp => emp.id).ToList();

            ClearLabels();
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            outputLabel.Text = "";
            
            foreach(Employee emp in employeeDB)
            {
                outputLabel.Text += emp.id + " " + emp.lastName + " " + emp.firstName +
                    " " + emp.date + " " + emp.salary + "\n";
            }

        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveDB();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {

            int searchID = Convert.ToInt16(idInput.Text);

            int index = employeeDB.FindIndex(emp => emp.id == searchID);

            if (index >= 0)
            {
                employeeDB.RemoveAt(index);
                outputLabel.Text = "Employee " + searchID + " removed";
            }
            else
            {
                outputLabel.Text = "Employee ID not found";
            }

            ClearLabels();

            /*
             * the following code does the same operation as the code above, however
             * it does a linear search for the employee in question instead of
             * using a lambda expression
             */

            //foreach (Employee emp in employeeDB)
            //{
            //    if (emp.id == searchID)
            //    {
            //        outputLabel.Text = "Employee " + searchID + " removed";

            //        employeeDB.Remove(emp);
            //        ClearLabels();
            //        return; 
            //    }
            //}

            //outputLabel.Text = "Employee ID not found";
            //ClearLabels();
        }

        public void loadDB()
        {
            int newID;
            string newFN, newLN, newDate, newSalary;

            XmlReader reader = XmlReader.Create("Resources/employeeData.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    newID = Convert.ToInt16(reader.ReadString());

                    reader.ReadToNextSibling("firstName");
                    newFN = reader.ReadString();

                    reader.ReadToNextSibling("lastName");
                    newLN = reader.ReadString();

                    reader.ReadToNextSibling("startDate");
                    newDate = reader.ReadString();

                    reader.ReadToNextSibling("salary");
                    newSalary = reader.ReadString();

                    Employee newEmp = new Employee(newID, newFN, newLN, newDate, newSalary);
                    employeeDB.Add(newEmp);
                }
            }

            reader.Close();
        }

        public void saveDB()
        {
            XmlWriter writer = XmlWriter.Create("Resources/employeeData.xml", null);

            writer.WriteStartElement("Employees");

            foreach (Employee e in employeeDB)
            {
                writer.WriteStartElement("Employee");

                writer.WriteElementString("id", Convert.ToString(e.id));
                writer.WriteElementString("firstName", e.firstName);
                writer.WriteElementString("lastName", e.lastName);
                writer.WriteElementString("startDate", e.date);
                writer.WriteElementString("salary", e.salary);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.Close();
        }

        private void ClearLabels()
        {
            idInput.Text = "";
            fnInput.Text = "";
            lnInput.Text = "";
            dateInput.Text = "";
            salaryInput.Text = "";
        }
    }
}
