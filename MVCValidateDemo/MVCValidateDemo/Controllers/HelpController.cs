using Aspnet.Mvc.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.Controllers
{
    public class HelpController : Controller
    {
        /// <summary>
        /// 九宫格图片上传
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="thumbWidth"></param>
        /// <param name="thumbHeight"></param>
        /// <param name="sizeLimit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PicUpload_9(Guid folder, int thumbWidth, int thumbHeight, int sizeLimit)
        {
            HttpFileCollectionBase files = Request.Files;

            if (files.Count == 0)
            {
                return Json(new { success = false }, "text/html");
            }

            if (files.Count > 1)
            {
                return Json(new { success = false }, "text/html");
                //return Content("每次只能上传一个文件！");
            }

            if (files[0].ContentLength == 0)
            {
                return Json(new { success = false }, "text/html");
                //return Content("文件内容不能为空！");
            }

            if (files[0].ContentLength > sizeLimit)
            {
                string validateMessage = "文件超出规定大小。";
                return Json(new { success = false, error = validateMessage, message = validateMessage }, "text/html");
            }

            string rawFileName = "";//原图名
            string thumbFileName = "";//缩略图名            
            string rawPath = "";
            string thumbPath = "";
            string thumbUrl = "";// Url.Content(string.Format("~/Help/DownloadThumbnail_9?folder={0}&fileid={1}", folder, fileId));
            string rawUrl = "";

            Guid fileId = Guid.NewGuid();
            HttpPostedFileBase FileData = files[0];
            if (FileData != null)
            {
                try
                {
                    int filesize = FileData.ContentLength;
                    string contentType = FileData.ContentType;
                    string filename = Path.GetFileName(FileData.FileName);
                    string ext = Path.GetExtension(FileData.FileName);

                    rawFileName = fileId.ToString() + ext;
                    thumbFileName = "_thumb_" + fileId.ToString() + ext;

                    string savePath = Request.MapPath("~/App_Data/UploadFiles/");
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }

                    string folderPath = Path.Combine(savePath, folder.ToString());
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    rawPath = Path.Combine(folderPath, rawFileName);
                    thumbPath = Path.Combine(folderPath, thumbFileName);

                    FileData.SaveAs(rawPath);

                    // 生成缩略图
                    ImageHelper.GenerateThumb(rawPath, thumbPath, thumbWidth, thumbHeight, "Cut");
                }
                catch (Exception)
                {
                    return Json(new { success = false, error = "图片处理错误，请稍候再试！" }, "text/html");
                }
            }

            //rawUrl = string.Format("~/Help/ShowPic?folderid={0}&filename={1}", folder, rawFileName);
            thumbUrl = string.Format("~/Help/ShowPic?folderid={0}&filename={1}", folder, thumbFileName);

            return Json(new { success = true, filename = rawFileName, thumburl = Url.Content(thumbUrl) }, "text/html");
        }

        /// <summary>
        /// 九宫格图片缩略图
        /// </summary>
        public ActionResult DownloadThumbnail_9(Guid folder, Guid fileid)
        {
            string savePath = Request.MapPath("~/App_Data/UploadFiles/");
            savePath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dif = new DirectoryInfo(savePath);
            FileInfo[] files = dif.GetFiles("_newTmp_" + fileid.ToString() + "*", SearchOption.TopDirectoryOnly);

            if (files.Count() == 1)
            {
                return File(files[0].FullName, "application/octet-stream", "thumbnail");
            }

            return Content("");
        }

        private static Bitmap ResizeImage(Bitmap imgToResize, float nPercent, float dpiX, float dpiY)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            b.SetResolution(dpiX, dpiY);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        public ActionResult ShowPic(Guid folderid, string filename)
        {
            string savePath = Request.MapPath("~/App_Data/UploadFiles/");
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string folderPath = Path.Combine(savePath, folderid.ToString());
            string rawingPath = Path.Combine(folderPath, filename);

            return File(rawingPath, "image/jpeg");
        }
        public ActionResult ShowPic2(string url)
        {
            string path = PicHelper.GetPhotoPath(url);
            return File(path, "image/jpeg");
        }
    }
}