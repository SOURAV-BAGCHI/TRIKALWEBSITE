using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Trikal_Website.DATALibrary
{
    public static class Extensions
    {
        public static String GetStringForDebugging(this SqlCommand cmd)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append(cmd.CommandText);

            foreach (SqlParameter par in cmd.Parameters)
            {
                String value = "";
                switch (par.SqlDbType)
                {
                    case System.Data.SqlDbType.VarChar:
                        value = String.Format("'{0}'", par.SqlValue.ToString());
                        break;
                    case System.Data.SqlDbType.DateTime:
                        value = String.Format("'{0}'", par.SqlValue.ToString());
                        break;
                    case System.Data.SqlDbType.Text:
                        value = String.Format("'{0}'", par.SqlValue.ToString());
                        break;
                    default:
                        value = String.Format("{0}", par.SqlValue.ToString());
                        break;
                }

                strBuilder.Append(String.Format(@" {0}={1},", par.ParameterName, value));
            }

            if (cmd.Parameters.Count > 0)
            {
                strBuilder.Remove(strBuilder.Length - 1, 1);
            }

            return strBuilder.ToString();

        }
    }
}