using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace TestProject.DAL
{
    public abstract class DataLayerBase
    {
        #region ExecuteDataSet
        public static DataSet ExecuteDataSet(Database db, DbTransaction tran, DbCommand cmd)
        {
            //declare variables
            DataSet ret = null;

            //check whether DBTransaction is null
            if (tran == null)
            {
                ret = db.ExecuteDataSet(cmd);
            }
            else
            {
                ret = db.ExecuteDataSet(cmd, tran);
            }

            return ret;

        }
        #endregion


        #region ExecuteNonQuery
        public static int ExecuteNonQuery(Database db, DbTransaction tran, DbCommand cmd)
        {
            //declare variable
            int retVal = 0;

            if (tran == null)
            {
                retVal = db.ExecuteNonQuery(cmd);

            }
            else
            {
                retVal = db.ExecuteNonQuery(cmd, tran);
            }

            return retVal;
        }
        #endregion
    }
}
