using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace InventoryMgt.DAL
{	public class tblSalesDB : DataLayerBase
	{
		#region InserttblSales
		public static int InserttblSales(DbTransaction tran,  string actualSellingPrice, string costPrice, DateTime createdDate, DateTime modifiedDate, string packType, double productId, string profit, string quantity, double saleId, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_InserttblSales");

			db.AddInParameter(cmd, "@actualSellingPrice", DbType.String, actualSellingPrice);
			db.AddInParameter(cmd, "@costPrice", DbType.String, costPrice);
			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@packType", DbType.String, packType);
			db.AddInParameter(cmd, "@productId", DbType.Double, productId);
			db.AddInParameter(cmd, "@profit", DbType.String, profit);
			db.AddInParameter(cmd, "@quantity", DbType.String, quantity);
			db.AddInParameter(cmd, "@saleId", DbType.Double, saleId);
			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region DeletetblSales
		public static int DeletetblSales(DbTransaction tran, object saleId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_DeletetblSales");

			db.AddInParameter(cmd, "@saleId", DbType.Double, saleId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region UpdatetblSales
		public static int UpdatetblSales(DbTransaction tran,  string actualSellingPrice, string costPrice, DateTime createdDate, DateTime modifiedDate, string packType, double productId, string profit, string quantity, double saleId, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_UpdatetblSales");

			db.AddInParameter(cmd, "@actualSellingPrice", DbType.String, actualSellingPrice);
			db.AddInParameter(cmd, "@costPrice", DbType.String, costPrice);
			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@packType", DbType.String, packType);
			db.AddInParameter(cmd, "@productId", DbType.Double, productId);
			db.AddInParameter(cmd, "@profit", DbType.String, profit);
			db.AddInParameter(cmd, "@quantity", DbType.String, quantity);
			db.AddInParameter(cmd, "@saleId", DbType.Double, saleId);
			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region GettblSales
		public static DataTable GettblSales(DbTransaction tran, object saleId)
		{
			DataTable retVal = new DataTable();

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_GettblSales");

			db.AddInParameter(cmd, "@saleId", DbType.Double, saleId);

			retVal = DataLayerBase.ExecuteDataSet(db, tran, cmd).Tables[0];

			return retVal;
		}
		#endregion

	}
}