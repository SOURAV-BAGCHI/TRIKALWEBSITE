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
    public class BrochureRequestDetailsModel
    {
        SqlConnection con = null;
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private Trikal_Website.DATALibrary.DataConnection DB;

        private String BrochureId = "@i_BrochureId";
        private String Name = "@i_Name";
        private String Email = "@i_Email";
        private String FKAdminId = "@i_FKAdminId";
        private String MarkAsSpam = "@i_MarkAsSpam";
        private String MailSendBit = "@i_MailSendBit";
        private String ListType = "@i_ListType";


        public BrochureRequestDetailsModel()
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            DB = new DataConnection();
        }

        public BrochureRequestDetailsViewModel BrochureRequestDetailsInsertUpdate(BrochureRequestDetailsViewModel BRDVMObj)
        {
            DB.PList = new List<PArray>();
            if(!String.IsNullOrEmpty(BRDVMObj.BrochureRequestId))
            {
                DB.Add(BrochureId, BRDVMObj.BrochureRequestId);
            }
            if(!String.IsNullOrEmpty(BRDVMObj.Name))
            {
                DB.Add(Name, BRDVMObj.Name);
            }
            if(!String.IsNullOrEmpty(BRDVMObj.Email))
            {
                DB.Add(Email, BRDVMObj.Email);
            }
            if(!String.IsNullOrEmpty(BRDVMObj.LastUpdateByAdminId))
            {
                DB.Add(FKAdminId, BRDVMObj.LastUpdateByAdminId);
            }
            DB.Add(MarkAsSpam, BRDVMObj.MarkAsSpam);
            DB.Add(MailSendBit, BRDVMObj.SendBit);

            try
            {
                ds = DB.getData(StoredProcedure.usp_BrochureRequestDetails_InsertUpdate, DB.PList, con.ConnectionString.ToString());
                if(ds.Tables[0].Rows.Count>0)
                {
                    BRDVMObj.Message= Convert.ToString(ds.Tables[0].Rows[0]["MESSAGE"]);
                    BRDVMObj.Success = Convert.ToInt32(ds.Tables[0].Rows[0]["SUCCESS"]);
                    BRDVMObj.Name = Convert.ToString(ds.Tables[0].Rows[0]["NAME"]);
                    BRDVMObj.Email = Convert.ToString(ds.Tables[0].Rows[0]["EMAIL"]);
                }
            }
            catch(Exception Ex)
            {
                BRDVMObj.Message = Convert.ToString(Ex.Message);
                BRDVMObj.Success = 0;
            }

            return BRDVMObj;
        }

        public List<BrochureRequestDetailsViewModel> GetBrochureRequestDetails(BrochureRequestDetailsViewModel BRDVMObj)
        {
            List<BrochureRequestDetailsViewModel> BRDVMObjList = new List<BrochureRequestDetailsViewModel>();
            DB.PList = new List<PArray>();
            DB.Add(ListType, BRDVMObj.ListType);

            try
            {
                ds = DB.getData(StoredProcedure.usp_BrochureRequestDetails_Get, DB.PList, con.ConnectionString.ToString());
                if(ds.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BrochureRequestDetailsViewModel BRDVMObjTemp = new BrochureRequestDetailsViewModel();
                        BRDVMObjTemp.Name = Convert.ToString(dr["NAME"]);
                        BRDVMObjTemp.Email = Convert.ToString(dr["EMAIL"]);
                        BRDVMObjTemp.BrochureRequestId = Convert.ToString(dr["REQUESTID"]);
                        BRDVMObjTemp.LastUpdateDate = Convert.ToDateTime(dr["LASTUPDATEDATE"]).ToString("dddd, dd MMMM yyyy");
                        BRDVMObjTemp.MarkAsSpam = Convert.ToBoolean(dr["MARKASSPAM"]);
                        BRDVMObjTemp.SendBit = Convert.ToBoolean(dr["SENDBIT"]);

                        BRDVMObjList.Add(BRDVMObjTemp);
                    }
                }
            }
            catch(Exception Ex)
            {

            }

            return BRDVMObjList;
        }

        public Int32 GetBrochureRequestCount()
        {
            Int32 RequestCount = 0;
            DB.PList = new List<PArray>();

            try
            {
                ds = DB.getData(StoredProcedure.usp_BrochureRequestDetails_RequestCount, DB.PList, con.ConnectionString.ToString());
                if(ds.Tables[0].Rows.Count>0)
                {
                    RequestCount = Convert.ToInt32(ds.Tables[0].Rows[0]["REQUESTCOUNT"]);
                }
            }
            catch(Exception Ex)
            {
                RequestCount = 0;
            }

            return RequestCount;
        }
    }
}