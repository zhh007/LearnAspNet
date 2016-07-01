using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension.Controllers
{
    public class FileUploadController : Controller
    {
        //public AccountIdentity CurrentUser
        //{
        //    get
        //    {
        //        AccountIdentity curUser = HttpContext.User.GetCurrentUser();
        //        return curUser;
        //    }
        //}

        /// <summary>
        /// 上传到临时文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="sizeLimit"></param>
        /// <returns></returns>
        [HttpPost]
        //[LogException]
        public ActionResult Upload(Guid folder, long sizeLimit, long? totalSizeLimit)
        {
            HttpFileCollectionBase files = Request.Files;

            if (files.Count == 0)
            {
                return Json(new { success = false, error = "无效请求！" }, "text/html");
            }

            //if (files.Count > 1)
            //{
            //    return Json(new { success = false, error = "每次只能上传一个文件！" }, "text/html");
            //}

            //HttpPostedFileBase FileData = files[0];

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase item = files[i];
                    if (item.ContentLength == 0)
                    {
                        return Json(new { success = false, error = "文件内容不能为空！" }, "text/html");
                    }

                    if (sizeLimit > 0 && item.ContentLength > sizeLimit)
                    {
                        string validateMessage = string.Format("{0}文件太大, 最大支持{1}。", item.FileName, FileUploadHtmlHelper.GetFileSizeFormat(sizeLimit));
                        return Json(new { success = false, error = validateMessage, message = validateMessage }, "text/html");
                    }

                    if (totalSizeLimit > 0)
                    {
                        var oldFiles = FileUploadManager.GetFiles(folder);
                        long curTotalFileSize = item.ContentLength;
                        foreach (var of in oldFiles)
                        {
                            curTotalFileSize += of.FileSize;
                        }

                        if (curTotalFileSize > totalSizeLimit)
                        {
                            string validateMessage = string.Format("总文件大小不能超过{0}。", FileUploadHtmlHelper.GetFileSizeFormat(totalSizeLimit.Value));
                            return Json(new { success = false, error = validateMessage, message = validateMessage }, "text/html");
                        }
                    }
                }

                string savePath = Request.MapPath(FileUploadHtmlHelper.TempFileSavePath);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string folderPath = Path.Combine(savePath, folder.ToString());
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase item = files[i];
                    int filesize = item.ContentLength;
                    string contentType = item.ContentType;
                    string filename = Path.GetFileName(item.FileName);
                    string ext = Path.GetExtension(item.FileName);

                    string rawingPath = Path.Combine(folderPath, filename);
                    if (System.IO.File.Exists(rawingPath))
                    {
                        System.IO.File.Delete(rawingPath);
                    }
                    item.SaveAs(rawingPath);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("文件上传出错(" + folder.ToString() + ")", ex);
                return Json(new { success = false, error = "处理出错，请稍候再试！" }, "text/html");
            }

            return Json(new { success = true }, "text/html");
        }

        //[LogException]
        public ActionResult Download(int? id, string fn, Guid? folder)
        {
            if (!id.HasValue)
            {
                return new EmptyResult();
            }
            try
            {
                string fileName = Server.UrlDecode(fn);
                byte[] fs = null;

                if ((id == 0 || id == -1) && folder.HasValue)
                {
                    string savePath = Request.MapPath(FileUploadHtmlHelper.TempFileSavePath);
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }

                    string folderPath = Path.Combine(savePath, folder.ToString());
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string rawingPath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(rawingPath))
                    {
                        fs = System.IO.File.ReadAllBytes(rawingPath);
                        //System.IO.File.Delete(rawingPath);
                    }
                }
                else
                {
                    string storepath = GetAttachmentFilePath(id.Value, ref fileName);
                    fs = System.IO.File.ReadAllBytes(storepath);
                }

                //IE7, IE8, IE9, IE10, Chrome 13, Opera 11, FF5, Safari 5.
                string contentDisposition;
                if (Request.Browser.Browser == "IE" && (Request.Browser.Version == "7.0" || Request.Browser.Version == "8.0"))
                    contentDisposition = "attachment; filename=" + Uri.EscapeDataString(fileName);
                else if (Request.Browser.Browser == "Safari")
                    contentDisposition = "attachment; filename=" + fileName;
                else
                    contentDisposition = "attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName);
                Response.AddHeader("Content-Disposition", contentDisposition);

                return File(fs, "application/octet-stream");
            }
            catch (Exception ex)
            {
                //LogHelper.Error("文件下载出错(" + id.ToString() + ")", ex);
                return Content("文件下载出错，请稍候再试！");
            }
        }

        //[LogException]
        public ActionResult DeleteFile(Guid folder, string fileName, int? fileID)
        {
            try
            {
                //删除临时文件
                string savePath = Request.MapPath(FileUploadHtmlHelper.TempFileSavePath);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string folderPath = Path.Combine(savePath, folder.ToString());
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string rawingPath = Path.Combine(folderPath, fileName);
                if (System.IO.File.Exists(rawingPath))
                {
                    System.IO.File.Delete(rawingPath);
                }

                //删除正式库文件
                if (fileID.HasValue && fileID != 0 && fileID != -1)
                {
                    string userfilePath = FileUploadHtmlHelper.FileStorePath;
                    string storePath = string.Empty;
                    using (SqlConnection sqlConn = new SqlConnection(SystemConfig.DBLink))
                    {
                        string sql = @"
SELECT [StorePath] FROM [dbo].[WorkflowFiles] WITH (NOLOCK) where ID=@ID
DELETE FROM [dbo].[WorkflowFiles] WHERE ID=@ID"
;

                        sqlConn.Open();

                        SqlCommand cmd = new SqlCommand(sql, sqlConn);

                        SqlParameter para_ID = new SqlParameter("ID", SqlDbType.Int);
                        para_ID.Value = fileID.Value;
                        cmd.Parameters.Add(para_ID);

                        storePath = cmd.ExecuteScalar().ToString();

                        sqlConn.Close();
                    }

                    string filepath = Path.Combine(userfilePath, storePath);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error("文件删除出错(" + folder.ToString() + "," + fileName + ")", ex);
                return Json(new { success = false, error = "文件删除出错，请稍候再试！" }, "text/html");
            }

            return Json(new { success = true }, "text/html");
        }

        private string GetAttachmentFilePath(int id, ref string fileName)
        {
            string storePath = "";
            string userfilePath = FileUploadHtmlHelper.FileStorePath;
            string sql = @"SELECT [FileName], [StorePath] FROM [dbo].[WorkflowFiles] WITH (NOLOCK) where ID=@ID";
            using (SqlConnection sqlConn = new SqlConnection(SystemConfig.DBLink))
            {
                SqlCommand cmd = new SqlCommand(sql, sqlConn);

                SqlParameter para_ID = new SqlParameter("@ID", SqlDbType.Int);
                para_ID.Value = id;
                cmd.Parameters.Add(para_ID);

                sqlConn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader[0] != null)
                        {
                            fileName = reader[0].ToString();
                        }
                        if (reader[1] != null)
                        {
                            storePath = reader[1].ToString();
                        }
                    }
                }
                sqlConn.Close();
            }

            return Path.Combine(userfilePath, storePath);
        }
    }
}
