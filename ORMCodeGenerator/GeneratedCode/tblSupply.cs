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
	public class tblSupply
	{
		#region InserttblSupply
		public static int InserttblSupply( DateTime createdDate, DateTime modifiedDate, string supplyDescription, double supplyId, string supplyName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblSupplyDB.InserttblSupply(tran, createdDate, modifiedDate, supplyDescription, supplyId, supplyName);

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
		#region DeletetblSupply
		public static int DeletetblSupply(object supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblSupplyDB.DeletetblSupply(tran, supplyId);

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
		#region UpdatetblSupply
		public static int UpdatetblSupply( DateTime createdDate, DateTime modifiedDate, string supplyDescription, double supplyId, string supplyName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblSupplyDB.UpdatetblSupply(tran, createdDate, modifiedDate, supplyDescription, supplyId, supplyName);

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
		#region GettblSupply
		public static DataTable GettblSupply(object supplyId)
		{
			return tblSupplyDB.GettblSupply(null, supplyId);
		}
		#endregion
	}
}