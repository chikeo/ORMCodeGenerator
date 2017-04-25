/*
 * Created by: Malovic, Nikola
 * Created: 11/19/2007
 */
/*
 * Modified by: Oji, Chikelue
 * Modified: 15/10/2008 in dd/MM/YYYY format
 */
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System;
using System.Text;
using System.Windows.Forms;

namespace ORMCodeGenerator
{
    public class SqlHelper
    {
        public static IList<string> GetActiveServers()
        {
            Collection<string> result = new Collection<string>();

            try
            {
                SqlDataSourceEnumerator instanceEnumerator = SqlDataSourceEnumerator.Instance;
                DataTable instancesTable = instanceEnumerator.GetDataSources();
                foreach (DataRow row in instancesTable.Rows)
                {
                    if (!string.IsNullOrEmpty(row["InstanceName"].ToString()))
                        result.Add(string.Format(@"{0}\{1}", row["ServerName"], row["InstanceName"]));
                    else
                        result.Add(row["ServerName"].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Application.ProductName);
            }
            return result;
        }


        public static IList<string> GetDatabases(string serverName, string userId, string password,
                                                 bool windowsAuthentication)
        {

            Collection<string> result = new Collection<string>();

            try
            {

                
                using (
                    SqlConnection connection =
                        GetActiveConnection(serverName, string.Empty, userId, password, windowsAuthentication))
                {
                    connection.Open();
                    DataTable dt = connection.GetSchema(SqlClientMetaDataCollectionNames.Databases);
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(string.Format("{0}", row[0]));
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " +  ex.Message, Application.ProductName);
            }

            return result;
        }


        public static IList<string> GetTables(string serverName, string databaseName, string userId, string password,
                                              bool windowsAuthentication)
        {
            string[] restrictions = new string[4];
            restrictions[0] = databaseName; // database/catalog name   
            restrictions[1] = "dbo"; // owner/schema name   
            restrictions[2] = null; // table name   
            restrictions[3] = "BASE TABLE"; // table type    
            Collection<string> result = new Collection<string>();

            try
            {
                using (
                    SqlConnection connection =
                        GetActiveConnection(serverName, databaseName, userId, password, windowsAuthentication))
                {
                    connection.Open();
                    DataTable dt = connection.GetSchema(SqlClientMetaDataCollectionNames.Tables, restrictions);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!row[2].ToString().StartsWith("sys"))
                            result.Add(string.Format(@"{0}", row[2]));
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Application.ProductName);
            }

            return result;
        }


        public static IList<string> GetColumns(
            string serverName, string databaseName, string userId,
            string password, bool windowsAuthentication, string tableName)
        {
            SqlConnection connection =
                GetActiveConnection(serverName, databaseName, userId, 
                                    password, windowsAuthentication);

            string[] restrictions = new string[3];
            restrictions[0] = connection.Database; // database/catalog name      
            restrictions[1] = "dbo"; // owner/schema name      
            restrictions[2] = tableName; // table name      
            IList<string> result = new Collection<string>();


            using (connection)
            {

                try
                {
                    connection.Open();
                    DataTable columns = connection.GetSchema(SqlClientMetaDataCollectionNames.Columns, restrictions);
                    foreach (DataRow row in columns.Rows)
                    {
                        string columnName = row[3].ToString();
                        string columnDataType = row[7].ToString();
                        if (columnDataType.IndexOf("char") > -1)
                        {
                            // row[8] - CHARACTER_MAXIMUM_LENGTH    
                            columnDataType = string.Format("{0}({1})", columnDataType, row[8]);
                        }
                        if (columnDataType.IndexOf("decimal") > -1)
                        {
                            // row[10] - CHARACTER_OCTET_LENGTH    
                            // row[11] - NUMERIC_PRECISION    
                            columnDataType = string.Format("{0}({1},{2})", columnDataType, row[10], row[11]);
                        }
                        result.Add(string.Format("{0},{1}", columnName, columnDataType));
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " +  ex.Message, Application.ProductName);
                }

                return result;
            }
        }


        private static IList<string> GetIndexes(SqlConnection connection, string tableName)
        {
            string[] restrictions = new string[3];
            restrictions[0] = connection.Database; // database/catalog name      
            restrictions[1] = "dbo"; // owner/schema name      
            restrictions[2] = tableName; // table name      
            IList<string> result = new Collection<string>();
            using (connection)
            {
                try
                {
                    connection.Open();
                    DataTable columns = connection.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, restrictions);
                    foreach (DataRow row in columns.Rows)
                    {
                        string columnName = row["column_name"].ToString();
                        string indexName = row["index_name"].ToString();
                        bool isPrimaryKey = row["constraint_name"].ToString().StartsWith("PK");
                        result.Add(string.Format("Index:{0}, on column:{1}, PK:{2}", indexName, columnName, isPrimaryKey));
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, Application.ProductName);
                }
                return result;
            }
        }

        public static string GetIndexes(SqlConnection connection, string tableName, bool isGetPrimaryKey)
        {
            string[] restrictions = new string[3];
            restrictions[0] = connection.Database; // database/catalog name      
            restrictions[1] = "dbo"; // owner/schema name      
            restrictions[2] = tableName; // table name      
            string result = String.Empty;
            int iCounter = 0;

            using (connection)
            {
                try
                {
                    connection.Open();
                    DataTable columns = connection.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, restrictions);
                    foreach (DataRow row in columns.Rows)
                    {
                        iCounter++; //Increment the counter to keep track of position in the recordset
                        string columnName = row["column_name"].ToString();
                        string indexName = row["index_name"].ToString();
                        bool isPrimaryKey = row["constraint_name"].ToString().StartsWith("PK");

                        if (columns.Rows.Count >= 1)
                        {
                            if (isPrimaryKey == true)
                            {

                                if (iCounter < columns.Rows.Count)
                                {
                                    result += columnName + ",";
                                }
                                else
                                {
                                    result += columnName;
                                }
                                //break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, Application.ProductName);
                }

                //Evaluate the result to be return to the caller.
                if (result == String.Empty)
                {
                    return "NO PRIMARY KEY";
                }
                else
                {
                    return result;
                }
            }
        }

        public static SqlParameter[] DiscoverStoredProcedureParameters(SqlConnection sqlConnection,
                                                                       string storedProcedureName)
        {


            SqlCommand cmd = new SqlCommand(storedProcedureName, sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            using (sqlConnection)
            {

                try
                {
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(cmd);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, Application.ProductName);
                }
            }
            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(discoveredParameters, 0);
            return discoveredParameters;
        }


        public static SqlConnection GetActiveConnection(string serverName, string databaseName, string userName,
                                                         string password, bool useIntegratedSecurity)
        {
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
            connBuilder.DataSource = serverName;
            connBuilder.InitialCatalog = databaseName;
            connBuilder.IntegratedSecurity = useIntegratedSecurity;
            connBuilder.UserID = userName;
            connBuilder.Password = password;
            return new SqlConnection(connBuilder.ConnectionString);
        }
    }
}