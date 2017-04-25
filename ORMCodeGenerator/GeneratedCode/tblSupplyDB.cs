using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace InventoryMgt.DAL
{	public class tblSupplyDB : DataLayerBase
	{
		#region InserttblSupply
		public static int InserttblSupply(DbTransaction tran,  DateTime createdDate, DateTime modifiedDate, string supplyDescription, double supplyId, string supplyName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_InserttblSupply");

			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@supplyDescription", DbType.String, supplyDescription);
			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);
			db.AddInParameter(cmd, "@supplyName", DbType.String, supplyName);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region DeletetblSupply
		public static int DeletetblSupply(DbTransaction tran, object supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_DeletetblSupply");

			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region UpdatetblSupply
		public static int UpdatetblSupply(DbTransaction tran,  DateTime createdDate, DateTime modifiedDate, string supplyDescription, double supplyId, string supplyName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_UpdatetblSupply");

			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@supplyDescription", DbType.String, supplyDescription);
			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);
			db.AddInParameter(cmd, "@supplyName", DbType.String, supplyName);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region GettblSupply
		public static DataTable GettblSupply(DbTransaction tran, object supplyId)
		{
			DataTable retVal = new DataTable();

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_GettblSupply");

			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);

			retVal = DataLayerBase.ExecuteDataSet(db, tran, cmd).Tables[0];

			return retVal;
		}
		#endregion

	}
}