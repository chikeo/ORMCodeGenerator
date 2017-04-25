using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace InventoryMgt.DAL
{	public class tblProductSupplyDB : DataLayerBase
	{
		#region InserttblProductSupply
		public static int InserttblProductSupply(DbTransaction tran,  string costPrice, DateTime createdDate, string maxSellingPrice, string minSellingPrice, DateTime modifiedDate, double productId, double productSupplyId, string quantityAtHand, string suppliedQuantity, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_InserttblProductSupply");

			db.AddInParameter(cmd, "@costPrice", DbType.String, costPrice);
			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@maxSellingPrice", DbType.String, maxSellingPrice);
			db.AddInParameter(cmd, "@minSellingPrice", DbType.String, minSellingPrice);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@productId", DbType.Double, productId);
			db.AddInParameter(cmd, "@productSupplyId", DbType.Double, productSupplyId);
			db.AddInParameter(cmd, "@quantityAtHand", DbType.String, quantityAtHand);
			db.AddInParameter(cmd, "@suppliedQuantity", DbType.String, suppliedQuantity);
			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region DeletetblProductSupply
		public static int DeletetblProductSupply(DbTransaction tran, object productSupplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_DeletetblProductSupply");

			db.AddInParameter(cmd, "@productSupplyId", DbType.Double, productSupplyId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region UpdatetblProductSupply
		public static int UpdatetblProductSupply(DbTransaction tran,  string costPrice, DateTime createdDate, string maxSellingPrice, string minSellingPrice, DateTime modifiedDate, double productId, double productSupplyId, string quantityAtHand, string suppliedQuantity, double supplyId)
		{
			int retVal = -1;

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_UpdatetblProductSupply");

			db.AddInParameter(cmd, "@costPrice", DbType.String, costPrice);
			db.AddInParameter(cmd, "@createdDate", DbType.DateTime, createdDate);
			db.AddInParameter(cmd, "@maxSellingPrice", DbType.String, maxSellingPrice);
			db.AddInParameter(cmd, "@minSellingPrice", DbType.String, minSellingPrice);
			db.AddInParameter(cmd, "@modifiedDate", DbType.DateTime, modifiedDate);
			db.AddInParameter(cmd, "@productId", DbType.Double, productId);
			db.AddInParameter(cmd, "@productSupplyId", DbType.Double, productSupplyId);
			db.AddInParameter(cmd, "@quantityAtHand", DbType.String, quantityAtHand);
			db.AddInParameter(cmd, "@suppliedQuantity", DbType.String, suppliedQuantity);
			db.AddInParameter(cmd, "@supplyId", DbType.Double, supplyId);

			retVal = DataLayerBase.ExecuteNonQuery(db, tran, cmd);

			return retVal;
		}
		#endregion

		#region GettblProductSupply
		public static DataTable GettblProductSupply(DbTransaction tran, object productSupplyId)
		{
			DataTable retVal = new DataTable();

			Database db = DatabaseFactory.CreateDatabase();

			DbCommand cmd = db.GetStoredProcCommand("proc_GettblProductSupply");

			db.AddInParameter(cmd, "@productSupplyId", DbType.Double, productSupplyId);

			retVal = DataLayerBase.ExecuteDataSet(db, tran, cmd).Tables[0];

			return retVal;
		}
		#endregion

	}
}