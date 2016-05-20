﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.TMS
{
    [Route("/tms/csbk1/sps", "Get")]  // sps?RecordCount=
    [Route("/tms/csbk1/update", "Get")] //update?CompletedFlag=
    [Route("/tms/csbk1/confirm", "Get")] //update?BookingNo=
    [Route("/tms/csbk1", "Get")]      //csbk1?BookingNo=
    [Route("/tms/csbk2", "Get")]      //Csbk2?BookingNo=
    public class Csbk : IReturn<CommonResponse>
    {
        public string RecordCount { get; set; }
        public string BookingNo { get; set; }
        public string CompletedFlag { get; set; }
        public string DriverCode { get; set; }

    }
    public class Csbk_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public List<Csbk1> Get_csbk1_List(Csbk request)
        {
            List<Csbk1> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    int count = 0;
                    if (!string.IsNullOrEmpty(request.RecordCount))
                    {
                        count = int.Parse(request.RecordCount);
                    }
                    string strWhere = "";
                    if (!string.IsNullOrEmpty(request.BookingNo))
                    {

                        strWhere = "Where BookingNo='" + request.BookingNo + "'";
                        //var strSQL = "select Csbk1.TrxNo,Csbk1.BookingNo,Csbk1.StatusCode,isnull(Csbk1.JobNo, '') as JobNo,isnull(Csbk1.BookingCustomerCode, '') as BookingCustomerCode,isnull(Csbk1.CollectionTimeStart, '') as CollectionTimeStart,isnull(Csbk1.CollectionTimeEnd, '') as CollectionTimeEnd ,sum(Csbk2.Pcs) as Pcs,isnull(rcbp1.BusinessPartyCode, '') as BusinessPartyCode,isnull(Rcbp1.PostalCode, '') as PostalCode,isnull(Rcbp1.BusinessPartyName, '') as BusinessPartyName,isnull(Rcbp1.Address1, '') as Address1,isnull(Rcbp1.Address2, '') as Address2,isnull(Rcbp1.Address3, '') as Address3,isnull(Rcbp1.Address4, '') as Address4 ,isnull(Csbk1.CompletedFlag,'') AS CompletedFlag" +
                        //  " from Csbk1 left join Csbk2 on Csbk1.TrxNo = Csbk2.TrxNo  left" +
                        //  " join rcbp1 on Csbk1.BookingCustomerCode = rcbp1.BusinessPartyCode " + strWhere +
                        //  " group  by Csbk1.jobno,rcbp1.BusinessPartyCode,Csbk1.BookingNo,Csbk1.StatusCode,Rcbp1.PostalCode,Rcbp1.BusinessPartyName,Rcbp1.Address1,Rcbp1.Address2,Rcbp1.Address3,Rcbp1.Address4,Csbk1.BookingCustomerCode,Csbk1.CollectionTimeStart,Csbk1.CollectionTimeEnd,Csbk1.CompletedFlag,Csbk1.TrxNo";
                        var strSQL = "select Csbk1.TrxNo," +
          "isnull((Select top 1 TimeFrom From Todr2 Where District like '%' + Rcbp1.DistrictCode + '%' and  day in ( case when DatePart(W, csbk1.DeliveryDate) = 1 then 'SUN'" +
           " when DatePart(W, csbk1.DeliveryDate) = 2 then 'MON'" +
          "  when DatePart(W, csbk1.DeliveryDate) = 3 then 'TUE'" +
           " when DatePart(W, csbk1.DeliveryDate) = 4 then 'WED'" +
          "  when DatePart(W, csbk1.DeliveryDate) = 5 then 'THU'" +
          "  when DatePart(W, csbk1.DeliveryDate) = 6 then 'FRI'" +
          "  when DatePart(W, csbk1.DeliveryDate) = 7 then 'SAT'" +
         " end)),'') AS TimeFrom," +
         "isnull((Select top 1 TimeTo From Todr2 Where District like '%' + Rcbp1.DistrictCode + '%' and  day in ( case when DatePart(W, csbk1.DeliveryDate) = 1 then 'SUN'" +

         "  when DatePart(W, csbk1.DeliveryDate) = 2 then 'MON'" +

         "  when DatePart(W, csbk1.DeliveryDate) = 3 then 'TUE'" +

         "  when DatePart(W, csbk1.DeliveryDate) = 4 then 'WED'" +

        "   when DatePart(W, csbk1.DeliveryDate) = 5 then 'THU'" +

         "  when DatePart(W, csbk1.DeliveryDate) = 6 then 'FRI'" +

         "  when DatePart(W, csbk1.DeliveryDate) = 7 then 'SAT'" +

        " end )),'') AS TimeTo," +
         "Csbk1.BookingNo,Csbk1.StatusCode,isnull(Csbk1.JobNo, '') as JobNo,isnull(Csbk1.BookingCustomerCode, '') as BookingCustomerCode,isnull(Csbk1.CollectionTimeStart, '') as CollectionTimeStart,isnull(Csbk1.CollectionTimeEnd, '') as CollectionTimeEnd ,sum(Csbk2.Pcs) as Pcs,isnull(rcbp1.BusinessPartyCode, '') as BusinessPartyCode,isnull(Rcbp1.PostalCode, '') as PostalCode,isnull(Rcbp1.BusinessPartyName, '') as BusinessPartyName,isnull(Rcbp1.Address1, '') as Address1,isnull(Rcbp1.Address2, '') as Address2,isnull(Rcbp1.Address3, '') as Address3,isnull(Rcbp1.Address4, '') as Address4 ,isnull(Csbk1.CompletedFlag, '') AS CompletedFlag" +
                       "  from Csbk1 left join Csbk2 on Csbk1.TrxNo = Csbk2.TrxNo  left" +
                       "  join rcbp1 on Csbk1.BookingCustomerCode = rcbp1.BusinessPartyCode    "+ strWhere + 
                       "  group  by Csbk1.jobno,rcbp1.BusinessPartyCode,Csbk1.BookingNo,Csbk1.StatusCode,Rcbp1.PostalCode,Rcbp1.BusinessPartyName,Rcbp1.Address1,Rcbp1.Address2,Rcbp1.Address3,Rcbp1.Address4,Csbk1.BookingCustomerCode,Csbk1.CollectionTimeStart,Csbk1.CollectionTimeEnd,Csbk1.CompletedFlag,Csbk1.TrxNo ,Rcbp1.DistrictCode ,csbk1.DeliveryDate";
                        Result = db.Select<Csbk1>(strSQL);




                    }

                }

            }
            catch { throw; }
            return Result;

        }

        public List<Csbk1> Get_csbk1_SpsList(Csbk request)
        {
            List<Csbk1> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    int count = 0;
                    if (!string.IsNullOrEmpty(request.RecordCount))
                    {
                        count = int.Parse(request.RecordCount);
                    }
                    string strWhere = "";
                    if (!string.IsNullOrEmpty(request.BookingNo))
                    {

                        strWhere = "Where BookingNo='" + request.BookingNo + "'";

                    }
                    var strSQL = "SELECT t1.BookingNo,JobNo,CustomerCode,CustomerName,CustomerRefNo,DeliveryEndDateTime,TotalPcs,Toaddress1,Toaddress2,Toaddress3,Toaddress4,UOMCode" +
                         " FROM csbk1 t1," +
                         "(SELECT TOP " + (count + 20) + "row_number() OVER(ORDER BY bookingNo ASC) n, bookingNo FROM csbk1  " + strWhere + " ) t2 " +
                         "WHERE t1.bookingNo = t2.bookingNo AND StatusCode<> 'DEL' AND t2.n >" + count +
                         "ORDER BY t2.n ASC";
                    Result = db.Select<Csbk1>(strSQL);

                }

            }
            catch { throw; }
            return Result;

        }

        public List<Csbk2> Get_Csbk2_List(Csbk request)
        {
            List<Csbk2> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    if (!string.IsNullOrEmpty(request.BookingNo))
                    {

                        var strSQL = "select Csbk1.BookingNo, Csbk1.JobNo,Csbk1.TrxNo,Csbk1.StatusCode as StatusCode,Csbk1.ItemNo,Csbk2.BoxCode,Csbk2.Pcs,Csbk1.DepositAmt,Csbk1.DiscountAmt,Csbk2.UnitRate,'' as CollectedPcs  ,Csbk1.CollectedAmt      from Csbk2 left join Csbk1 on Csbk2.trxno = Csbk1.trxno where BookingNo ='" + request.BookingNo + "'";
                        Result = db.Select<Csbk2>(strSQL);


                    }
                }
            }
            catch { throw; }
            return Result;

        }
        public int update_csbk1(Csbk request)
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Update<Csbk1>(
                                    new
                                    {
                                        CompletedFlag = request.CompletedFlag

                                    },
                                    p => p.BookingNo == request.BookingNo
                    );
                }
            }
            catch { throw; }
            return Result;
        }

        public int confirm_csbk1(Csbk request)
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Update<Csbk1>(
                                    new
                                    {
                                        CompletedFlag = "Y"

                                    },
                                    p => p.BookingNo == request.BookingNo
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
