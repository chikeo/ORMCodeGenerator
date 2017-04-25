using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using InventoryMgt.DAL;
using System.Data.SqlClient;

namespace InventoryMgt.BLL
{
[System.ComponentModel.DataObject]
	public class tblProducts
	{
		#region InserttblProducts
		public static int InserttblProducts( DateTime createdDate, DateTime modifiedDate, string productCode, string productDescription, double productId, string productName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblProductsDB.InserttblProducts(tran, createdDate, modifiedDate, productCode, productDescription, productId, productName);

					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
				finally
				{
					conn.Close();
				}
			}

			return retVal;

		}
		#endregion
		#region DeletetblProducts
		public static int DeletetblProducts(object productId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblProductsDB.DeletetblProducts(tran, productId);

					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
				finally
				{
					conn.Close();
				}
			}

			return retVal;

		}
		#endregion
		#region UpdatetblProducts
		public static int UpdatetblProducts( DateTime createdDate, DateTime modifiedDate, string productCode, string productDescription, double productId, string productName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblProductsDB.UpdatetblProducts(tran, createdDate, modifiedDate, productCode, productDescription, productId, productName);

					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
				finally
				{
					conn.Close();
				}
			}

			return retVal;

		}
		#endregion
		#region GettblProducts
		public static DataTable GettblProducts(object productId)
		{
			return tblProductsDB.GettblProducts(null, productId);
		}
		#endregion
	}
}