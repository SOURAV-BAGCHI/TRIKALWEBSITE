using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Trikal_Website.DATALibrary;
using Trikal_Website.Models.ViewModels;

namespace Trikal_Website.Models
{
    public class UserDetailsModel
    {
        SqlConnection con = null;
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private Trikal_Website.DATALibrary.DataConnection DB;

        private String EmailOrPhone = "@i_EmailOrPhone";
        private String Password = "@i_Password";

        public UserDetailsModel()
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            DB = new DataConnection();
        }


        public UserDetailsViewModel AdminLogin(UserDetailsViewModel UDVMObj)
        {
            DB.PList = new List<PArray>();
            DB.Add(EmailOrPhone, UDVMObj.EmailOrPhone);
            DB.Add(Password, UDVMObj.Password);

            try
            {
                ds = DB.getData(StoredProcedure.usp_AdminDetails_AdminLogin, DB.PList, con.ConnectionString.ToString());
                if(ds.Tables[0].Rows.Count>0)
                {
                    UDVMObj.UserId = Convert.ToString(ds.Tables[0].Rows[0]["USERID"]);
                    UDVMObj.AdminId = Convert.ToString(ds.Tables[0].Rows[0]["ADMINID"]);
                    UDVMObj.Username= Convert.ToString(ds.Tables[0].Rows[0]["USERNAME"]);
                }
            }
            catch(Exception Ex)
            {
                UDVMObj.UserId = String.Empty;
                UDVMObj.AdminId = String.Empty;
                UDVMObj.Username = String.Empty;
            }


            return UDVMObj;
        }
    }
}