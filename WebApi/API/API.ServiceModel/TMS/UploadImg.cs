﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;
using ServiceStack.Common.Web;
using System.Drawing;
using ServiceStack;
using System.Drawing.Imaging;
using WebApi.ServiceModel.Utils;
using System.IO;

namespace WebApi.ServiceModel.TMS
{
    [Route("/tms/upload/img", "Post")]                      //img?JobNo= & FileName= & Extension=
    [Route("/tms/upload/img", "Options")]			//img?FileName= & Extension=
    public class UploadImg: IReturn<CommonResponse>
    {
        public string JobNo { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Base64 { get; set; }
        public Stream RequestStream { get; set; }
    }
    public class UploadImg_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int upload(UploadImg request)
        {
            int i = -1;
            string filePath = "";
            if (!string.IsNullOrEmpty(request.JobNo))
            {
                try
                {
                    using (var db = DbConnectionFactory.OpenDbConnection())
                    {
                        string strSQL = "Select Top 1 BackupPath From Saco1";
                        List<Saco1> saco1 = db.Select<Saco1>(strSQL);
                        if (saco1.Count > 0)
                        {
                            filePath = saco1[0].BackupPath+"\\SysFreight\\" + "\\csbk1\\" + request.JobNo;        /*2016018 File path */          
                          //  filePath = "E:\\"+"\\Sysfreight\\" + "\\csbk1\\" + request.JobNo;
                        }
                    }
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string resultFile = Path.Combine(filePath, request.FileName);
                    if (File.Exists(resultFile))
                    {
                        File.Delete(resultFile);
                    }
                    //Image img = System.Drawing.Image.FromStream(request.RequestStream);
                    //img.Save(System.IO.Path.GetTempPath() + "\\" + request.FileName, ImageFormat.Jpeg);		
                    if (!string.IsNullOrEmpty(request.Base64))
                    {
                        string strBase64 = request.Base64;
                        string[] base64s = strBase64.Split(',');
                        if (base64s.Length > 0)
                        {
                            byte[] arr = Convert.FromBase64String(base64s[1]);
                            using (MemoryStream ms = new MemoryStream(arr))
                            {
                                Bitmap bmp = new Bitmap(ms);
                                bmp.Save(resultFile, System.Drawing.Imaging.ImageFormat.Png);
                                //bmp.Save(txtFileName + ".bmp", ImageFormat.Bmp);
                                //bmp.Save(txtFileName + ".gif", ImageFormat.Gif);
                                //bmp.Save(txtFileName + ".png", ImageFormat.Png);
                                i = 0;
                            }
                        }
                    }
                    if (i.Equals(0))
                    {
                        using (var db = DbConnectionFactory.OpenDbConnection())
                        {
                            i = db.Update<Csbk1>(
                                            new
                                            {
                                                AttachmentFlag = "Y"
                                            },
                                            p => p.JobNo == request.JobNo
                            );
                        }
                    }
                }
                catch { throw; }
            }
            return i;
        }
    }

}
