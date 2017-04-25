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
	public class tblExpenses
	{
		#region InserttblExpenses
		public static int InserttblExpenses( DateTime createdDate, string expenseAmount, string expenseDescription, double expenseId, string expenseName, DateTime modifiedDate)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblExpensesDB.InserttblExpenses(tran, createdDate, expenseAmount, expenseDescription, expenseId, expenseName, modifiedDate);

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
		#region DeletetblExpenses
		public static int DeletetblExpenses(object expenseId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblExpensesDB.DeletetblExpenses(tran, expenseId);

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
		#region UpdatetblExpenses
		public static int UpdatetblExpenses( DateTime createdDate, string expenseAmount, string expenseDescription, double expenseId, string expenseName, DateTime modifiedDate)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			using (DbConnection conn = db.CreateConnection())
			{
				conn.Open();
				DbTransaction tran = conn.BeginTransaction();
				try
				{
					retVal = tblExpensesDB.UpdatetblExpenses(tran, createdDate, expenseAmount, expenseDescription, expenseId, expenseName, modifiedDate);

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
		#region GettblExpenses
		public static DataTable GettblExpenses(object expenseId)
		{
			return tblExpensesDB.GettblExpenses(null, expenseId);
		}
		#endregion
	}
}