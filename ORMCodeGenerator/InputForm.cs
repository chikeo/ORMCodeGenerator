using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;

namespace ORMCodeGenerator
{
    public partial class InputForm : Form
    {
        string strProjectName;

        public string StrProjectName
        {
            get { return strProjectName; }
            set { strProjectName = value; }
        }
        string strDbServerInstance;

        public string StrDbServerInstance
        {
            get { return strDbServerInstance; }
            set { strDbServerInstance = value; }
        }
        
        // Creating access to an instance of the Config file
        NameValueCollection nvcAllAppSettings = ConfigurationManager.AppSettings;

        public InputForm()
        {
            InitializeComponent();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((txtProjectName.Text == String.Empty) || ((txtDbServerInstance.Visible == true) && (txtDbServerInstance.Text == String.Empty)))
            {
                MessageBox.Show("You must enter a Project name and/or a Database Server Instance", Application.ProductName);
                return;
            }

            this.strProjectName = txtProjectName.Text;
            this.strDbServerInstance = txtDbServerInstance.Text;
            this.Visible = false;
            

            //Search for BaseProjectName within the DataLayerBase file and replace it with the current Project's name
            string fName = System.IO.Directory.GetCurrentDirectory().Remove(System.IO.Directory.GetCurrentDirectory().LastIndexOf("\\bin")) + @"\DataLayerBase.cs";//path to text file
            StreamReader dataLayerBaseFileReader = new StreamReader(fName);
            StreamWriter dataLayerBaseFileWriter;

            if (nvcAllAppSettings["GeneratedCodePath"].EndsWith("\\"))
            {
                dataLayerBaseFileWriter = new StreamWriter(nvcAllAppSettings["GeneratedCodePath"] + "DataLayerBase.cs");
            }
            else
            {
                dataLayerBaseFileWriter = new StreamWriter(nvcAllAppSettings["GeneratedCodePath"] + @"\DataLayerBase.cs");
            }
            string allRead = String.Empty;

            //Read from the template
            try
            {

                
                allRead = dataLayerBaseFileReader.ReadToEnd();//Reads the whole text file to the end
                dataLayerBaseFileReader.Close(); //Closes the text file after it is fully read.
                dataLayerBaseFileReader.Dispose();
                

                //Replace the placeholder text "BaseProjectName" with f.StrProjectName
                allRead = allRead.Replace("BaseProjectName", StrProjectName);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName);
            }
            finally
            {
                dataLayerBaseFileReader.Close(); //Closes the text file after it is fully read.
                dataLayerBaseFileReader.Dispose();
            }


            //Write the changes on the template to the GeneratedCode Folder
            try
            {


                dataLayerBaseFileWriter.Write(allRead);
                dataLayerBaseFileWriter.Flush();
                dataLayerBaseFileWriter.Close();
                dataLayerBaseFileWriter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName);
            }
            finally
            {
                dataLayerBaseFileWriter.Close();
                dataLayerBaseFileWriter.Dispose();
            }

        }

        private void InputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((txtProjectName.Text == String.Empty) || ((txtDbServerInstance.Visible == true) && (txtDbServerInstance.Text == String.Empty)))
            {
                MessageBox.Show("You must enter a Project name and/or a Database Server Instance", Application.ProductName);
                e.Cancel = true;
                return;
            }
        }
    }
}