using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace ORMCodeGenerator
{
    public partial class Form2 : Form
    {
        private string strTableListItem;
        private string strStoredProcCode;

        public string StrStoredProcCode
        {
            get { return strStoredProcCode; }
            set { strStoredProcCode = value; }
        }
        private string strDalCode;

        public string StrDalCode
        {
            get { return strDalCode; }
            set { strDalCode = value; }
        }
        private string strBllCode;

        public string StrBllCode
        {
            get { return strBllCode; }
            set { strBllCode = value; }
        }

        public string StrTableListItem
        {
            get { return strTableListItem; }
            set { strTableListItem = value; }
        }

        NameValueCollection nvcAllAppSettings = ConfigurationManager.AppSettings;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            
            textBox1.Text = this.strStoredProcCode;
            textBox2.Text = this.strDalCode;
            textBox3.Text = this.strBllCode;

           
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //This code is for the Save All Layers to file button.
            FileStream fs = null;
            

            try
            {
                //It saves the generated stored proc source code to a source file.
                if (nvcAllAppSettings["GeneratedCodePath"].EndsWith("\\"))
                {
                    fs = new FileStream(nvcAllAppSettings["GeneratedCodePath"] + this.StrTableListItem + "_storedProc.sql", FileMode.Create, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(nvcAllAppSettings["GeneratedCodePath"] + "\\" + this.StrTableListItem + "_storedProc.sql", FileMode.Create, FileAccess.Write);
                }
                AddText(fs, textBox1.Text);

                //Save the code for the DAL to c# source file
                if (nvcAllAppSettings["GeneratedCodePath"].EndsWith("\\"))
                {
                    fs = new FileStream(nvcAllAppSettings["GeneratedCodePath"] + this.StrTableListItem + "DB.cs", FileMode.Create, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(nvcAllAppSettings["GeneratedCodePath"] + "\\" + this.StrTableListItem + "DB.cs", FileMode.Create, FileAccess.Write);
                }
                AddText(fs, textBox2.Text);

                //Save the code for the BLL to c# source file
                if (nvcAllAppSettings["GeneratedCodePath"].EndsWith("\\"))
                {
                    fs = new FileStream(nvcAllAppSettings["GeneratedCodePath"] + this.StrTableListItem + ".cs", FileMode.Create, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(nvcAllAppSettings["GeneratedCodePath"] + "\\" + this.StrTableListItem + ".cs", FileMode.Create, FileAccess.Write);
                }
                AddText(fs, textBox3.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Application.ProductName);
                return;
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }

            this.button1.Enabled = false;
        }

        private void AddText(FileStream fs, string value) 
        {
             byte[] info = new UTF8Encoding(true).GetBytes(value);
             fs.Write(info, 0, info.Length);
        }

    }
}