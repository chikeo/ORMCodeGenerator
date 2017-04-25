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
	public class tblSales
	{
		#region InserttblSales
		public static int InserttblSales( string actualSellingPrice, string costPrice, DateTime createdDate, DateTime modifiedDate, string packType, double productId, string profit, string quantity, double saleId, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblSalesDB.InserttblSales(tran, actualSellingPrice, costPrice, createdDate, modifiedDate, packType, productId, profit, quantity, saleId, supplyId);

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
		#region DeletetblSales
		public static int DeletetblSales(object saleId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblSalesDB.DeletetblSales(tran, saleId);

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
		#region UpdatetblSales
		public static int UpdatetblSales( string actualSellingPrice, string costPrice, DateTime createdDate, DateTime modifiedDate, string packType, double productId, string profit, string quantity, double saleId, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblSalesDB.UpdatetblSales(tran, actualSellingPrice, costPrice, createdDate, modifiedDate, packType, productId, profit, quantity, saleId, supplyId);

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
		#region GettblSales
		public static DataTable GettblSales(object saleId)
		{
			return tblSalesDB.GettblSales(null, saleId);
		}
		#endregion
	}
}