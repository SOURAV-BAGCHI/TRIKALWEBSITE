using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Trikal_Website.DATALibrary
{
    public class PArray
    {
        public String PName { get; set; }
        public Object PValue { get; set; }
        public SqlDbType PType { get; set; }
    }

    public class DataConnection
    {
        public List<PArray> PList { get; set; }

        public String ErrorMsg
        {
            get
            {
                return _ErrorMsg;

            }
        }

        private String _ErrorMsg = String.Empty;

        /// <title>makePArray</title>
        /// <summary>Function create an arraylist of SQL parameters.</summary>
        /// <param name="Parameter">The name of the parameter.</param>
        /// <param name="PValue">The value of the paramter.</param>
        /// <param name="SQLType">The SQL Data Input Type.</param>
        /// <example>
        /// <code>
        /// Dim DB as new ClassLibrary.Library.DBConn
        /// Dim ID as integer
        /// Dim myArray As New ArrayList
        /// myArray.add(DB.makePArray("@i_intID", ID, SqlDbType.Int))
        /// </code>
        /// </example>
        /// <returns>Arraylist</returns>
        public PArray makePArray(String Parameter, Object PValue, SqlDbType SQLType)
        {
            PArray parameters = new PArray();
            parameters.PName = Parameter;
            parameters.PValue = PValue;
            parameters.PType = SQLType;
            return parameters;
        }

        /// <title>getData</title>
        /// <summary>Function returning to return a SQL dataset.</summary>
        /// <param name="SPName">The Stored Procedure name.</param>
        /// <param name="myPArray">An arraylist of input parameters and values.</param>
        /// <param name="myConn">The SQL database connection string.</param>
        /// <returns>Dataset</returns>
        public DataSet getData(String SPName, List<PArray> myPArray, String myConn)
        {
            SqlConnection myConnection = new SqlConnection(myConn);

            // Create the command object, passing the SQL string.
            SqlDataAdapter myCommand = new SqlDataAdapter(SPName, myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet DS = new DataSet();

            // Loop the parameter list if not equal to nothing
            // to return individual parameter lists.

            if (myPArray != null)
            {
                foreach (PArray parameter in myPArray)
                {
                    using (var cmd = myCommand.SelectCommand)
                    {
                        cmd.Parameters.Add(new SqlParameter(parameter.PName, parameter.PType));
                        cmd.Parameters[parameter.PName].Value = RenderNull(parameter.PValue, parameter.PType);
                    }
                }
            }

            // Test to see if the database is accessible and output as a Dataset.
            try
            {
                myConnection.Open();

#if DEBUG
                {
                    Debug.WriteLine(String.Format("SQL->     {0}", myCommand.SelectCommand.GetStringForDebugging()));
                }

#endif

                myCommand.Fill(DS, SPName);

            }
            catch (Exception ex)
            {
                _ErrorMsg = ex.Message;
            }
            finally
            {
                // Cleanup command and connection objects.
                myPArray = null;
                myConnection.Close();
                myCommand.Dispose();
                myConnection.Dispose();
            }

            return DS;

        }

        /// <title>GetDataTable</title>
        /// <summary>Function returning to return a SQL dataset.</summary>
        /// <param name="SPName">The Stored Procedure name.</param>
        /// <param name="myPArray">An arraylist of input parameters and values.</param>
        /// <param name="TableID">The index of the table to retrieve data from.</param>
        /// <param name="myConn">The SQL database connection string.</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(String SPName, List<PArray> myPArray, Int32 TableID, String Conn)
        {
            DataSet DS = new DataSet();
            try
            {
                DS = getData(SPName, myPArray, Conn);
                DataTable DT = DS.Tables[TableID];
                return DT;
            }
            catch { return null; }
            finally { DS.Dispose(); }
        }

        /// <title>GetSQLDBType</title>
        /// <summary>Function to return the correct SQLDBType for the given input.</summary>
        /// <param name="ParameterName">The Stored Procedure Parameter Name.</param>
        /// <returns>SqlDbType</returns>
        public SqlDbType GetSQLDBType(String ParameterName)
        {
            SqlDbType SQLType = SqlDbType.VarChar;

            if (ParameterName.Contains("@i_bin"))
            {
                SQLType = SqlDbType.VarBinary;
            }
            else if (ParameterName.Contains("@i_bit"))
            {
                SQLType = SqlDbType.Bit;
            }
            else if (ParameterName.Contains("@i_dce"))
            {
                SQLType = SqlDbType.Decimal;
            }
            else if (ParameterName.Contains("@i_dt"))
            {
                SQLType = SqlDbType.DateTime;
            }
            else if (ParameterName.Contains("@i_int"))
            {
                SQLType = SqlDbType.Int;
            }
            else if (ParameterName.Contains("@i_mny"))
            {
                SQLType = SqlDbType.Money;
            }

            return SQLType;
        }

        /// <title>Add</title>
        /// <summary>Function to return parameter list after adding the current one.</summary>
        /// <param name="ParameterName">The Stored Procedure Parameter Name.</param>
        /// <param name="Value">The Stored Procedure Parameter Value.</param>
        /// <returns>List<PArray</returns>
        public List<PArray> Add(String ParameterName, Object Value)
        {
            PArray newPArray = new PArray();
            newPArray.PName = ParameterName;
            newPArray.PValue = Value;
            newPArray.PType = GetSQLDBType(ParameterName);
            PList.Add(newPArray);
            return PList;
        }

        /// <title>RenderNull</title>
        /// <summary>Function render nulls to the database.</summary>
        /// <param name="Obj">The Stored Procedure object.</param>
        /// <param name="SQLType">The Stored Procedure object data type.</param>
        /// <returns>Object</returns>
        public Object RenderNull(Object Obj, SqlDbType SQLType)
        {
            if (SQLType == SqlDbType.DateTime)
            {
                if (Convert.ToDateTime(Obj) == DateTime.MinValue)
                {
                    Obj = null;
                }
            }

            if (Convert.IsDBNull(Obj) || Obj == null)
            {
                return System.DBNull.Value;
            }
            else
            {
                return Obj;
            }
        }

        /// <title>ConvertNull</title>
        /// <summary>Function to convert nulls for specific input types.</summary>
        /// <param name="input">The input value.</param>
        /// <returns>Object</returns>
        public Object ConvertNull(Object input)
        {
            if (Convert.IsDBNull(input) == true)
            {
                if (input.GetType() == typeof(Boolean))
                {
                    return false;
                }
                else if (input.GetType() == typeof(Int32))
                {
                    return Int32.MinValue;
                }
                else if (input.GetType() == typeof(Double))
                {
                    return Double.MinValue;
                }
                else if (input.GetType() == typeof(DateTime))
                {
                    return DateTime.MinValue;
                }
                else
                    return null;
            }
            else
            {
                return input;
            }
        }

    }

}