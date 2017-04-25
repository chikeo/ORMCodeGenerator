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
	public class tblProductSupply
	{
		#region InserttblProductSupply
		public static int InserttblProductSupply( string costPrice, DateTime createdDate, string maxSellingPrice, string minSellingPrice, DateTime modifiedDate, double productId, double productSupplyId, string quantityAtHand, string suppliedQuantity, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblProductSupplyDB.InserttblProductSupply(tran, costPrice, createdDate, maxSellingPrice, minSellingPrice, modifiedDate, productId, productSupplyId, quantityAtHand, suppliedQuantity, supplyId);

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
		#region DeletetblProductSupply
		public static int DeletetblProductSupply(object productSupplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblProductSupplyDB.DeletetblProductSupply(tran, productSupplyId);

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
		#region UpdatetblProductSupply
		public static int UpdatetblProductSupply( string costPrice, DateTime createdDate, string maxSellingPrice, string minSellingPrice, DateTime modifiedDate, double productId, double productSupplyId, string quantityAtHand, string suppliedQuantity, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblProductSupplyDB.UpdatetblProductSupply(tran, costPrice, createdDate, maxSellingPrice, minSellingPrice, modifiedDate, productId, productSupplyId, quantityAtHand, suppliedQuantity, supplyId);

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
		#region GettblProductSupply
		public static DataTable GettblProductSupply(object productSupplyId)
		{
			return tblProductSupplyDB.GettblProductSupply(null, productSupplyId);
		}
		#endregion
	}
}