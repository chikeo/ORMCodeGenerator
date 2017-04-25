using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace InventoryMgt.DAL
{	public class tblProductsDB : DataLayerBase
	{
		#region InserttblProducts
		public static int InserttblProducts(DbTransaction tran,  DateTime createdDate, DateTime modifiedDate, string productCode, string productDescription, double productId, string productName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_InserttblProducts");

			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@productCode", DbType.String, productCode);
			db.AddInParameter(cmd, "@productDescription", DbType.String, productDescription);
			db.AddInParameter(cmd, "@productId", DbType.Double, productId);
			db.AddInParameter(cmd, "@productName", DbType.String, productName);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region DeletetblProducts
		public static int DeletetblProducts(DbTransaction tran, object productId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_DeletetblProducts");

			db.AddInParameter(cmd, "@productId", DbType.Double, productId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region UpdatetblProducts
		public static int UpdatetblProducts(DbTransaction tran,  DateTime createdDate, DateTime modifiedDate, string productCode, string productDescription, double productId, string productName)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_UpdatetblProducts");

			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@productCode", DbType.String, productCode);
			db.AddInParameter(cmd, "@productDescription", DbType.String, productDescription);
			db.AddInParameter(cmd, "@productId", DbType.Double, productId);
			db.AddInParameter(cmd, "@productName", DbType.String, productName);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region GettblProducts
		public static DataTable GettblProducts(DbTransaction tran, object productId)
		{
			DataTable retVal = new DataTable();

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_GettblProducts");

			db.AddInParameter(cmd, "@productId", DbType.Double, productId);

			retVal = DataLayerBase.ExecuteDataSet(db, tran, cmd).Tables[0];

			return retVal;
		}
		#endregion

	}
}