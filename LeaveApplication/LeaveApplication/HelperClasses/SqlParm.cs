using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace LeaveApplication.HelperClasses
{
    public class SqlParm
    {
        List<SqlParameter> Parm = new List<SqlParameter>();
        public void Add(string ParmName,Object Value)
        {
            Parm.Add(new SqlParameter() { ParameterName = ParmName, Value = Value });
        }
        public List<SqlParameter> GetParmList()
        {if (Parm.Count == 0)
                return null;
            return Parm;
        }
    }
}