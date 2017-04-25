using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace ORMCodeGenerator
{
    public partial class Form1 : Form
    {
        private readonly string _password = string.Empty;
        private readonly string _userName = "sa";
        Form2 f;
        Label lblDbServerInstance;
        TextBox txtDbServerInstance;
        InputForm f2;
        Collection<string> result;

        public Form1()
        {
            InitializeComponent();
            //string[] strLocalServers = new string[] {"chikeo-pc", "chikeo-pc\\sqlexpress"};
            //Later add code to check if List servers is null and then to accept entry of server instance name from the user if so.
            //If IList<string> servers is null, that means that the host computer is not connected to a local area network.
            IList<string> servers = SqlHelper.GetActiveServers();

            f = new Form2();
            f2 = new InputForm();
            result = new Collection<string>();

            servers = null;//remove later. just used for test.
            if (servers != null)
            {
                Control[] c1 = f2.Controls.Find("lblDbServerInstance", false);
                foreach (Control co in c1)
                {
                    lblDbServerInstance = (Label)co;
                }

                Control[] c2 = f2.Controls.Find("txtDbServerInstance", false);
                foreach (Control co in c2)
                {
                    txtDbServerInstance = (TextBox)co;
                }


                lblDbServerInstance.Visible = false;
                txtDbServerInstance.Visible = false;

                f2.ShowDialog();


            }
            else
            {
                f2.ShowDialog();

                result.Add(f2.StrDbServerInstance);
                servers = result;
            }

            

            int machineNameLength = Environment.MachineName.Length;
            int selectedIndex = -1;

            foreach (string server in servers)
            {
                int index = DBTree.Nodes.Add(new TreeNode(server, getLoadingNodes()));
                if (selectedIndex == -1 && server.Length >= machineNameLength &&
                    server.Substring(0, machineNameLength).ToLower() == Environment.MachineName.ToLower())
                {
                    selectedIndex = index;
                }
            }
            if (selectedIndex > -1)
            {
                DBTree.SelectedNode = DBTree.Nodes[selectedIndex];
            }
        }


        private void DBTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 1)
                return;
            e.Node.Nodes.Clear();
            IList<string> databases = SqlHelper.GetDatabases(e.Node.Text, _userName, _password, true);
            foreach (string database in databases)
            {
                e.Node.Nodes.Add(database);
            }
        }

        private void DBTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool isDatabaseSelected = e.Node.Parent != null;

            if (isDatabaseSelected)
            {
                tableList.DataSource =
                    SqlHelper.GetTables(e.Node.Parent.Text, e.Node.Text, _userName, _password, true);
            }
        }

        private static TreeNode[] getLoadingNodes()
        {
            TreeNode[] nodes = new TreeNode[1];
            nodes[0] = new TreeNode("Loading...");
            return nodes;
        }

        private void tableList_SelectedValueChanged(object sender, EventArgs e)
        {
            columnList.DataSource =
                SqlHelper.GetColumns(DBTree.SelectedNode.Parent.Text, DBTree.SelectedNode.Text, _userName, _password,
                                     true, tableList.SelectedValue.ToString());
        }

        private void tableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //This array shall be used to contain the Column name and the Datatype. Col name = strArrSingleColAndDatatype[0] and 
            //Datatype = strArrSingleColAndDatatype[1].
            string[] strArrSingleColAndDatatype = new string[2];
            string strSelectedTable = tableList.SelectedValue.ToString();
            //MessageBox.Show(strSelectedTable);
            f.StrTableListItem = strSelectedTable;
            f.Text = "Generate code for table :" + f.StrTableListItem;
            IList<string> result = new Collection<string>();
            string resultPrimaryKey = String.Empty;
            string[] strColumnNameList;
            string[] strArrPrimaryKeyCols;

            //Retrieve the column names and column datatypes of the table.
            result = SqlHelper.GetColumns(DBTree.SelectedNode.Parent.Text, DBTree.SelectedNode.Text, _userName, _password,
                                     true, strSelectedTable);

            //retrieve all the indexes in the table so that we can determine/select the primary key.
            resultPrimaryKey = SqlHelper.GetIndexes(SqlHelper.GetActiveConnection(DBTree.SelectedNode.Parent.Text, DBTree.SelectedNode.Text, _userName, _password, true), strSelectedTable, true);

            string[,] strArrColumnAndDatatype = new string[result.Count, 2];

            int i = 0;
            //Enumerate the result collection and extract the data we need.
                    foreach (string s in result)
                    {
                        strArrSingleColAndDatatype = s.Split(',');

            //Store the data separately in each index of the two-dimensional array strArrColumnAndDatatype below.
                        for (int j = 0; j < 2; j++)
                        {
                            strArrColumnAndDatatype[i, j] = strArrSingleColAndDatatype[j];
                        }

                        // Make sure i is incremented only to the last index
                        if (i < (result.Count - 1))
                        {
                            ++i;
                        }
                    }
               

            //Generate 4 Stored Procedure code for Insert, Update, Delete and Select operations respectively.
            //Insert
            StringBuilder sbProcInsert = new StringBuilder();
            sbProcInsert.Append("CREATE PROC proc_Insert");//For a Oracle Database, use "CREATE PROCEDURE proc_Insert"
            strColumnNameList = GenerateStoredProcHeader(strSelectedTable, strArrColumnAndDatatype, ref i, sbProcInsert);
            sbProcInsert.Append("\tINSERT INTO ");
            sbProcInsert.Append(strSelectedTable);
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("\t(");
            sbProcInsert.Append(Environment.NewLine);

            //Writing the column names list to the Stored Procedure definition.
            for (i = 0; i < strColumnNameList.Length; i++)
            {
                if (i == (strColumnNameList.Length - 1))
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append("]");
                    sbProcInsert.Append(Environment.NewLine);
                }
                else
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append("],");
                    sbProcInsert.Append(Environment.NewLine);
                }
            }

            sbProcInsert.Append("\t)");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("VALUES");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("\t(");
            sbProcInsert.Append(Environment.NewLine);

            //Writing the VALUES to the Stored Procedure definition.
            for (i = 0; i < strColumnNameList.Length; i++)
            {
                if (i == (strColumnNameList.Length - 1))
                {
                    sbProcInsert.Append("\t@");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append(Environment.NewLine);
                }
                else
                {
                    sbProcInsert.Append("\t@");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append(",");
                    sbProcInsert.Append(Environment.NewLine);
                }
            }

            sbProcInsert.Append("\t);");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("END");
            sbProcInsert.Append(Environment.NewLine);
            //End of Code generator for Insert segment.

            //Allow a line before writing the update code
            sbProcInsert.Append(Environment.NewLine);

            //Update
            sbProcInsert.Append("CREATE PROC proc_Update");
            strColumnNameList = GenerateStoredProcHeader(strSelectedTable, strArrColumnAndDatatype, ref i, sbProcInsert);
            sbProcInsert.Append("\tUPDATE ");
            sbProcInsert.Append(strSelectedTable);
            sbProcInsert.Append(" SET");
            sbProcInsert.Append(Environment.NewLine);

            for (i = 0; i < strColumnNameList.Length; i++)
            {
                if (i == (strColumnNameList.Length - 1))
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append(Environment.NewLine);
                }
                else
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append(",");
                    sbProcInsert.Append(Environment.NewLine);
                }
            }

            //If this table doesn't have a primary key, then assume the primary key is the first column in the table
            //this is so that we can have a column to filter by for the update query.
            if (resultPrimaryKey.ToUpper() == "NO PRIMARY KEY")
            {
                resultPrimaryKey = strColumnNameList[0];
            }

            sbProcInsert.Append("WHERE ");
            sbProcInsert.Append(Environment.NewLine);

            //Split the array to extract a single column primary key or a multi-column composite primary key.
            strArrPrimaryKeyCols = resultPrimaryKey.Split(',');
            int iMaxElements = strArrPrimaryKeyCols.Length - 1;

            for (i = 0; i < strArrPrimaryKeyCols.Length; i++)
            {
                if (i < iMaxElements)
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append(Environment.NewLine);
                    sbProcInsert.Append("AND ");
                }
                else
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                }
            }
            sbProcInsert.Append(";");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("END");
            sbProcInsert.Append(Environment.NewLine);
            //End of Code generator for Update segment.

            //Allow a line before writing the delete code
            sbProcInsert.Append(Environment.NewLine);

            //Delete
            sbProcInsert.Append("CREATE PROC proc_Delete");
            i = GenerateStoredProcHeaderWithPrimaryKeyOnly(strSelectedTable, strArrPrimaryKeyCols, strArrColumnAndDatatype, i, sbProcInsert);
            //strColumnNameList = GenerateStoredProcHeader(strSelectedTable, strArrColumnAndDatatype, ref i, sbProcInsert);
            sbProcInsert.Append("\tDELETE ");
            sbProcInsert.Append(strSelectedTable);
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("WHERE ");
            sbProcInsert.Append(Environment.NewLine);
            for (i = 0; i < strArrPrimaryKeyCols.Length; i++)
            {
                if (i < iMaxElements)
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append(Environment.NewLine);
                    sbProcInsert.Append("AND ");
                }
                else
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                }
            }
            sbProcInsert.Append(";");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("END");
            sbProcInsert.Append(Environment.NewLine);
            //End of Code generator for Delete code.

            //Allow a line before writing the delete code
            sbProcInsert.Append(Environment.NewLine);

            //Select
            sbProcInsert.Append("CREATE PROC proc_Get");
            i = GenerateStoredProcHeaderWithPrimaryKeyOnly(strSelectedTable, strArrPrimaryKeyCols, strArrColumnAndDatatype, i, sbProcInsert);
            //strColumnNameList = GenerateStoredProcHeader(strSelectedTable, strArrColumnAndDatatype, ref i, sbProcInsert);
            sbProcInsert.Append("\tSELECT ");
            sbProcInsert.Append(Environment.NewLine);
            //Iterate through the columns of the table and list them here.
            for (i = 0; i < strColumnNameList.Length; i++)
            {
                if (i < (strColumnNameList.Length - 1))
                {
                    sbProcInsert.Append(strColumnNameList[i]);
                    sbProcInsert.Append(",");
                    sbProcInsert.Append(Environment.NewLine);
                }
                else
                {
                    sbProcInsert.Append(strColumnNameList[i]);
                }
            }
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("FROM ");
            sbProcInsert.Append(strSelectedTable);
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("WHERE ");
            sbProcInsert.Append(Environment.NewLine);
            for (i = 0; i < strArrPrimaryKeyCols.Length; i++)
            {
                if (i < iMaxElements)
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append(Environment.NewLine);
                    sbProcInsert.Append("AND ");
                }
                else
                {
                    sbProcInsert.Append("\t[");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                    sbProcInsert.Append("] = @");
                    sbProcInsert.Append(strArrPrimaryKeyCols[i]);
                }
            }
            sbProcInsert.Append(";");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("END");
            sbProcInsert.Append(Environment.NewLine);
            //End of Code generator for Select code.

            //Display the results of the Stringbuilder.
            f.StrStoredProcCode = sbProcInsert.ToString();
            //Deallocate sbProcInsert to free up memory
            sbProcInsert.Remove(0, sbProcInsert.Length);
            sbProcInsert = null;

            //Generate 4 Data Access Layer (DAL) code for Insert, Update, Delete and Select operations respectively.
            
            StringBuilder sbDalCode = new StringBuilder();
            sbDalCode.Append("using System;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("using System.Collections.Generic;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("using System.Text;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("using System.Data;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("using System.Data.Common;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("using Microsoft.Practices.EnterpriseLibrary.Data;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("using Microsoft.Practices.EnterpriseLibrary.Data.Sql;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("namespace ");
            sbDalCode.Append(f2.StrProjectName);
            sbDalCode.Append(".DAL");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("{");
            sbDalCode.Append("\tpublic class ");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("DB : DataLayerBase");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t{");
            sbDalCode.Append(Environment.NewLine);

            //Insert
            sbDalCode.Append("\t\t#region Insert");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\tpublic static int Insert");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("(DbTransaction tran, ");

            for (i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (i == (result.Count - 1))
                    {

                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbDalCode.Append(" DateTime ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbDalCode.Append(" int ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            
                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbDalCode.Append(" string ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        }
                    }
                    else
                    {
                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbDalCode.Append(" DateTime ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(",");

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbDalCode.Append(" int ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(",");
                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbDalCode.Append(" string ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(",");
                        }
                    }
                }
            }

            sbDalCode.Append(")");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t{");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tint retVal = -1;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDatabase db = DatabaseFactory.CreateDatabase();");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDbCommand cmd = db.GetStoredProcCommand(\"proc_Insert");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("\");");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);


            for (i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    
                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append("\", DbType.DateTime, ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(");");
                            sbDalCode.Append(Environment.NewLine);
                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append("\", DbType.Int32, ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(");");
                            sbDalCode.Append(Environment.NewLine);
                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append("\", DbType.String, ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(");");
                            sbDalCode.Append(Environment.NewLine);
                        }
                    
                }
            }

            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tretVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\treturn retVal;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t}");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t#endregion");
            sbDalCode.Append(Environment.NewLine);
            //End of DAL Insert method.

            //Append a newline to demarcate between the method above and one below in the source file.
            sbDalCode.Append(Environment.NewLine);

            //NB: the for loops that gets the parameters for the Delete method is the same for the Select method.
            //DAL Delete method.
            sbDalCode.Append("\t\t#region Delete");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\tpublic static int Delete");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("(DbTransaction tran, ");

            for (i = 0; i < strArrPrimaryKeyCols.Length; i++)
            {
                if (i < iMaxElements)
                {
                    sbDalCode.Append("object ");
                    sbDalCode.Append(strArrPrimaryKeyCols[i]);
                    sbDalCode.Append(", ");
                }
                else
                {
                    sbDalCode.Append("object ");
                    sbDalCode.Append(strArrPrimaryKeyCols[i]);
                }
            }
            sbDalCode.Append(")");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t{");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tint retVal = -1;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDatabase db = DatabaseFactory.CreateDatabase();");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDbCommand cmd = db.GetStoredProcCommand(\"proc_Delete");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("\");");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);

            for (int k = 0; k < strArrPrimaryKeyCols.Length; k++)
            {
            for (i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (strArrColumnAndDatatype[i, 0].ToLower().Trim() == strArrPrimaryKeyCols[k].ToLower().Trim())
                    {
                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append("\", DbType.DateTime, ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(");");
                            sbDalCode.Append(Environment.NewLine);
                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append("\", DbType.Int32, ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(");");
                            sbDalCode.Append(Environment.NewLine);
                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append("\", DbType.String, ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(");");
                            sbDalCode.Append(Environment.NewLine);
                        }
                    }
                }
            }
            }

            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tretVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\treturn retVal;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t}");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t#endregion");
            sbDalCode.Append(Environment.NewLine);
            //End of DAL Delete method.

            //Append a newline to demarcate between the method above and one below in the source file.
            sbDalCode.Append(Environment.NewLine);

            //DAL Update Update Method
            sbDalCode.Append("\t\t#region Update");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\tpublic static int Update");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("(DbTransaction tran, ");

            for (i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (i == (result.Count - 1))
                    {

                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbDalCode.Append(" DateTime ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbDalCode.Append(" int ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbDalCode.Append(" string ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        }
                    }
                    else
                    {
                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbDalCode.Append(" DateTime ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(",");

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbDalCode.Append(" int ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(",");
                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbDalCode.Append(" string ");
                            sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbDalCode.Append(",");
                        }
                    }
                }
            }

            sbDalCode.Append(")");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t{");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tint retVal = -1;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDatabase db = DatabaseFactory.CreateDatabase();");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDbCommand cmd = db.GetStoredProcCommand(\"proc_Update");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("\");");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);


            for (i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {

                    if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                    {
                        sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                        sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        sbDalCode.Append("\", DbType.DateTime, ");
                        sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        sbDalCode.Append(");");
                        sbDalCode.Append(Environment.NewLine);
                    }
                    else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                    {
                        sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                        sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        sbDalCode.Append("\", DbType.Int32, ");
                        sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        sbDalCode.Append(");");
                        sbDalCode.Append(Environment.NewLine);
                    }
                    else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                    {
                        sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                        sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        sbDalCode.Append("\", DbType.String, ");
                        sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                        sbDalCode.Append(");");
                        sbDalCode.Append(Environment.NewLine);
                    }

                }
            }

            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tretVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\treturn retVal;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t}");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t#endregion");
            sbDalCode.Append(Environment.NewLine);
            //End of DAL Update method.

            //Append a newline to demarcate between the method above and one below in the source file.
            sbDalCode.Append(Environment.NewLine);

            //DAL Select Method.
            sbDalCode.Append("\t\t#region Get");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\tpublic static DataTable Get");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("(DbTransaction tran, ");

            for (i = 0; i < strArrPrimaryKeyCols.Length; i++)
            {
                if (i < iMaxElements)
                {
                    sbDalCode.Append("object ");
                    sbDalCode.Append(strArrPrimaryKeyCols[i]);
                    sbDalCode.Append(", ");
                }
                else
                {
                    sbDalCode.Append("object ");
                    sbDalCode.Append(strArrPrimaryKeyCols[i]);
                }
            }
            sbDalCode.Append(")");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t{");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDataTable retVal = new DataTable();");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDatabase db = DatabaseFactory.CreateDatabase();");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tDbCommand cmd = db.GetStoredProcCommand(\"proc_Get");
            sbDalCode.Append(strSelectedTable);
            sbDalCode.Append("\");");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);

            for (int k = 0; k < strArrPrimaryKeyCols.Length; k++)
            {
                for (i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (strArrColumnAndDatatype[i, 0].ToLower().Trim() == strArrPrimaryKeyCols[k].ToLower().Trim())
                        {
                            if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                            {
                                sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                                sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                                sbDalCode.Append("\", DbType.DateTime, ");
                                sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                                sbDalCode.Append(");");
                                sbDalCode.Append(Environment.NewLine);
                            }
                            else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                            {
                                sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                                sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                                sbDalCode.Append("\", DbType.Int32, ");
                                sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                                sbDalCode.Append(");");
                                sbDalCode.Append(Environment.NewLine);
                            }
                            else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                            {
                                sbDalCode.Append("\t\t\tdb.AddInParameter(cmd, \"@");
                                sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                                sbDalCode.Append("\", DbType.String, ");
                                sbDalCode.Append(strArrColumnAndDatatype[i, 0]);
                                sbDalCode.Append(");");
                                sbDalCode.Append(Environment.NewLine);
                            }
                        }
                    }
                }
            }

            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\tretVal = DataLayerBase.ExecuteDataSet(db, tran, cmd).Tables[0];");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t\treturn retVal;");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t}");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t\t#endregion");
            sbDalCode.Append(Environment.NewLine);
            //End of DAL Select method.

            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("\t}");
            sbDalCode.Append(Environment.NewLine);
            sbDalCode.Append("}");
            //End of DAL Class and Namespace.

            f.StrDalCode = sbDalCode.ToString();
            //Deallocate sbDalCode to free up memory
            sbDalCode.Remove(0, sbDalCode.Length);
            sbDalCode = null;

            //Generate 4 Business Logic Layer (BLL) code for Insert, Update, Delete and Select operations respectively.
            
            StringBuilder sbBllCode = new StringBuilder();
            sbBllCode.Append("using System;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using System.Collections.Generic;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using System.Text;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using System.Data;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using System.Data.Common;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using Microsoft.Practices.EnterpriseLibrary.Data;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using Microsoft.Practices.EnterpriseLibrary.Data.Sql;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using ");
            sbBllCode.Append(f2.StrProjectName);
            sbBllCode.Append(".DAL;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("using System.Data.SqlClient;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("namespace ");
            sbBllCode.Append(f2.StrProjectName);
            sbBllCode.Append(".BLL");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("[System.ComponentModel.DataObject]");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\tpublic class ");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t{");
            sbBllCode.Append(Environment.NewLine);
            
            //BLL Insert Method Code
            sbBllCode.Append("\t\t#region Insert");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\tpublic static int Insert");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(");

            for (i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (i == (result.Count - 1))
                    {

                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbBllCode.Append(" DateTime ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbBllCode.Append(" int ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbBllCode.Append(" string ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                        }
                    }
                    else
                    {
                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbBllCode.Append(" DateTime ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(",");

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbBllCode.Append(" int ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(",");
                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbBllCode.Append(" string ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(",");
                        }
                    }
                }
            }

            sbBllCode.Append(")");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tint retVal = -1;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tDatabase db = DatabaseFactory.CreateDatabase();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tusing (DbConnection conn = db.CreateConnection())");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tconn.Open();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tDbTransaction tran = conn.BeginTransaction();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\ttry");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\t");
            sbBllCode.Append("retVal = ");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("DB.Insert");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(tran, ");

            for (i = 0; i < result.Count; i++)
            {

                if (i == (result.Count - 1))
                {


                    sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                }
                else
                {

                    sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                    sbBllCode.Append(", ");
                }

            }

            sbBllCode.Append(");");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\ttran.Commit();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tcatch (Exception ex)");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\ttran.Rollback();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\tthrow ex;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tfinally");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\tconn.Close();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\treturn retVal;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t#endregion");

            //End of BLL Insert Method

            //Append a newline to demarcate between the method above and one below in the source file.
            sbBllCode.Append(Environment.NewLine);

            //NB: the for loops that gets the parameters for the Delete method is the same for the Select method.
            //BLL Delete Method Code
            sbBllCode.Append("\t\t#region Delete");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\tpublic static int Delete");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(");

            for (i = 0; i < strArrPrimaryKeyCols.Length; i++)
            {
                if (i < iMaxElements)
                {
                    sbBllCode.Append("object ");
                    sbBllCode.Append(strArrPrimaryKeyCols[i]);
                    sbBllCode.Append(", ");
                }
                else
                {
                    sbBllCode.Append("object ");
                    sbBllCode.Append(strArrPrimaryKeyCols[i]);
                }
            }

            sbBllCode.Append(")");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tint retVal = -1;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tDatabase db = DatabaseFactory.CreateDatabase();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tusing (DbConnection conn = db.CreateConnection())");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tconn.Open();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tDbTransaction tran = conn.BeginTransaction();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\ttry");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\t");
            sbBllCode.Append("retVal = ");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("DB.Delete");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(tran, ");

            for (int k = 0; k < strArrPrimaryKeyCols.Length; k++)
            {
                for (i = 0; i < result.Count; i++)
                {
                    if (strArrColumnAndDatatype[i, 0].ToLower().Trim() == strArrPrimaryKeyCols[k].ToLower().Trim())
                    {
                        if (i == (result.Count - 1))
                        {


                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else
                        {

                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(", ");
                        }
                    }

                }
            }

            /**Remove the comma that appears at end of parameter occasionally*/
            if (sbBllCode.ToString().EndsWith(", "))
            {
                sbBllCode.Remove((sbBllCode.Length - 2), 2);
            }

            sbBllCode.Append(");");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\ttran.Commit();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tcatch (Exception ex)");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\ttran.Rollback();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\tthrow ex;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tfinally");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\tconn.Close();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\treturn retVal;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t#endregion");

            //End of BLL Delete Method

            //Append a newline to demarcate between the method above and one below in the source file.
            sbBllCode.Append(Environment.NewLine);


            //BLL Update Method Code
            sbBllCode.Append("\t\t#region Update");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\tpublic static int Update");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(");

            for (i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (i == (result.Count - 1))
                    {

                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbBllCode.Append(" DateTime ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbBllCode.Append(" int ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbBllCode.Append(" string ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                        }
                    }
                    else
                    {
                        if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime")))//it is a datetime
                        {
                            sbBllCode.Append(" DateTime ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(",");

                        }
                        else if ((j == 1) && (strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")))//it is int
                        {
                            sbBllCode.Append(" int ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(",");
                        }
                        else if ((j == 1) && ((!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("int")) || (!strArrColumnAndDatatype[i, j].ToLower().Trim().StartsWith("datetime"))))//I assume it is a varchar or nvarchar and treat it as string.//it is a string
                        {
                            sbBllCode.Append(" string ");
                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(",");
                        }
                    }
                }
            }

            sbBllCode.Append(")");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tint retVal = -1;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tDatabase db = DatabaseFactory.CreateDatabase();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\tusing (DbConnection conn = db.CreateConnection())");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tconn.Open();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tDbTransaction tran = conn.BeginTransaction();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\ttry");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\t");
            sbBllCode.Append("retVal = ");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("DB.Update");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(tran, ");

            for (i = 0; i < result.Count; i++)
            {

                if (i == (result.Count - 1))
                {


                    sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                }
                else
                {

                    sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                    sbBllCode.Append(", ");
                }

            }

            sbBllCode.Append(");");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\ttran.Commit();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tcatch (Exception ex)");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\ttran.Rollback();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\tthrow ex;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\tfinally");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t{");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t\tconn.Close();");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t\treturn retVal;");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t#endregion");
            //End of BLL Update Method

            //Append a newline to demarcate between the method above and one below in the source file.
            sbBllCode.Append(Environment.NewLine);

            //BLL Select Method Code
            sbBllCode.Append("\t\t#region Get");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\tpublic static DataTable Get");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(");

            for (i = 0; i < strArrPrimaryKeyCols.Length; i++)
            {
                if (i < iMaxElements)
                {
                    sbBllCode.Append("object ");
                    sbBllCode.Append(strArrPrimaryKeyCols[i]);
                    sbBllCode.Append(", ");
                }
                else
                {
                    sbBllCode.Append("object ");
                    sbBllCode.Append(strArrPrimaryKeyCols[i]);
                }
            }

            sbBllCode.Append(")");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t{");
            sbBllCode.Append(Environment.NewLine);
            
            sbBllCode.Append("\t\t\treturn ");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("DB.Get");
            sbBllCode.Append(strSelectedTable);
            sbBllCode.Append("(null, ");

            for (int k = 0; k < strArrPrimaryKeyCols.Length; k++)
            {
                for (i = 0; i < result.Count; i++)
                {
                    if (strArrColumnAndDatatype[i, 0].ToLower().Trim() == strArrPrimaryKeyCols[k].ToLower().Trim())
                    {
                        if (i == (result.Count - 1))
                        {


                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);

                        }
                        else
                        {

                            sbBllCode.Append(strArrColumnAndDatatype[i, 0]);
                            sbBllCode.Append(", ");
                        }
                    }

                }
            }

            /**Remove the comma that appears at end of parameter occasionally*/
            if (sbBllCode.ToString().EndsWith(", "))
            {
                sbBllCode.Remove((sbBllCode.Length - 2), 2);
            }

            sbBllCode.Append(");");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t\t#endregion");

            //End of BLL Select Method

            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("\t}");
            sbBllCode.Append(Environment.NewLine);
            sbBllCode.Append("}");
            //End of BLL Class
            f.StrBllCode = sbBllCode.ToString();

            //Deallocate sbBllCode to free up memory.
            sbBllCode.Remove(0, sbBllCode.Length);
            sbBllCode = null;

            //Display the form with the results.
            f.ShowDialog();
        }

      
        /// <summary>
        /// This method generates stored procedure header that passes only the primary key columns of a table as parameters.
        /// </summary>
        /// <param name="strSelectedTable"></param>
        /// <param name="strArrPrimaryKeyCols"></param>
        /// <param name="strArrColumnAndDatatype"></param>
        /// <param name="i"></param>
        /// <param name="sbProcInsert"></param>
        /// <returns></returns>
        private static int GenerateStoredProcHeaderWithPrimaryKeyOnly(string strSelectedTable, string[] strArrPrimaryKeyCols, string[,] strArrColumnAndDatatype, int i, StringBuilder sbProcInsert)
        {
            sbProcInsert.Append(strSelectedTable);
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("(");
            sbProcInsert.Append(Environment.NewLine);

            //Get the actual length of the strArrColumnAndDatatype array
            int iLenStrArrColumnAndDatatype = strArrColumnAndDatatype.Length / 2;
            int iBoundaryForCommaInsertion = iLenStrArrColumnAndDatatype - 1;
           

            for (int c = 0; c < strArrPrimaryKeyCols.Length; c++)
            {
                for (i = 0; i < iLenStrArrColumnAndDatatype; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (strArrPrimaryKeyCols[c] == strArrColumnAndDatatype[i, j])
                        {
                            if (j == 0)
                            {

                                
                                sbProcInsert.Append("@");
                            }

                            sbProcInsert.Append(strArrColumnAndDatatype[i, j]);
                            
                            sbProcInsert.Append(" ");
                            

                            sbProcInsert.Append(strArrColumnAndDatatype[i, 1]);

                            if (c < (strArrPrimaryKeyCols.Length - 1))
                            {
                                sbProcInsert.Append(",");
                                sbProcInsert.Append(Environment.NewLine);
                            }
                        }
                    }
                }
            }

            //Append newline after last parameter without comma
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append(")");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("AS");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("BEGIN");
            sbProcInsert.Append(Environment.NewLine);
            return i;
        }

        /// <summary>
        /// This method generates a stored procedure header that passess all the column of a table as parameters.
        /// </summary>
        /// <param name="strSelectedTable"></param>
        /// <param name="strArrColumnAndDatatype"></param>
        /// <param name="i"></param>
        /// <param name="sbProcInsert"></param>
        /// <returns></returns>
        private static string[] GenerateStoredProcHeader(string strSelectedTable, string[,] strArrColumnAndDatatype, ref int i, StringBuilder sbProcInsert)
        {
            string[] strColumnNameList;
            sbProcInsert.Append(strSelectedTable);
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("(");
            sbProcInsert.Append(Environment.NewLine);

            //Get the actual length of the strArrColumnAndDatatype array
            int iLenStrArrColumnAndDatatype = strArrColumnAndDatatype.Length / 2;
            int iBoundaryForCommaInsertion = iLenStrArrColumnAndDatatype - 1;
            //Initialising array with the right bounds
            strColumnNameList = new string[iLenStrArrColumnAndDatatype];

            for (i = 0; i < iLenStrArrColumnAndDatatype; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {

                        //Copy out a dimension of the strArrColumnAndDatatype for later use.
                        strColumnNameList[i] = strArrColumnAndDatatype[i, j];
                        sbProcInsert.Append("@");
                    }
                    else if (j == 1)
                    {
                        sbProcInsert.Append(" ");
                    }

                    sbProcInsert.Append(strArrColumnAndDatatype[i, j]);

                    if ((i < iBoundaryForCommaInsertion) && (j == 1))
                    {
                        sbProcInsert.Append(",");
                        sbProcInsert.Append(Environment.NewLine);
                    }

                }
            }

            //Append newline after last parameter without comma
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append(")");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("AS");
            sbProcInsert.Append(Environment.NewLine);
            sbProcInsert.Append("BEGIN");
            sbProcInsert.Append(Environment.NewLine);
            return strColumnNameList;
        }
    }
}