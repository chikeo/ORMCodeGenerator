using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace InventoryMgt.DAL
{	public class tblExpensesDB : DataLayerBase
	{
		#region InserttblExpenses
		public static int InserttblExpenses(DbTransaction tran,  DateTime createdDate, string expenseAmount, string expenseDescription, double expenseId, string expenseName, DateTime modifiedDate)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_InserttblExpenses");

			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@expenseAmount", DbType.String, expenseAmount);
			db.AddInParameter(cmd, "@expenseDescription", DbType.String, expenseDescription);
			db.AddInParameter(cmd, "@expenseId", DbType.Double, expenseId);
			db.AddInParameter(cmd, "@expenseName", DbType.String, expenseName);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region DeletetblExpenses
		public static int DeletetblExpenses(DbTransaction tran, object expenseId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_DeletetblExpenses");

			db.AddInParameter(cmd, "@expenseId", DbType.Double, expenseId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region UpdatetblExpenses
		public static int UpdatetblExpenses(DbTransaction tran,  DateTime createdDate, string expenseAmount, string expenseDescription, double expenseId, string expenseName, DateTime modifiedDate)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_UpdatetblExpenses");

			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@expenseAmount", DbType.String, expenseAmount);
			db.AddInParameter(cmd, "@expenseDescription", DbType.String, expenseDescription);
			db.AddInParameter(cmd, "@expenseId", DbType.Double, expenseId);
			db.AddInParameter(cmd, "@expenseName", DbType.String, expenseName);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region GettblExpenses
		public static DataTable GettblExpenses(DbTransaction tran, object expenseId)
		{
			DataTable retVal = new DataTable();

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_GettblExpenses");

			db.AddInParameter(cmd, "@expenseId", DbType.Double, expenseId);

			retVal = DataLayerBase.ExecuteDataSet(db, tran, cmd).Tables[0];

			return retVal;
		}
		#endregion

	}
}