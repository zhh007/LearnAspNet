using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension.Controllers
{
    public class PicUploadController : Controller
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="thumbWidth"></param>
        /// <param name="thumbHeight"></param>
        /// <param name="sizeLimit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(Guid folder, int thumbWidth, int thumbHeight, int sizeLimit)
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

            string fileName = "";//原图名
            string filePath = "";
            string fileUrl = "";
            string thumbName = "";//缩略图名
            string thumbPath = "";
            string thumbUrl = "";

            Guid fileId = Guid.NewGuid();
            HttpPostedFileBase FileData = files[0];
            if (FileData != null)
            {
                try
                {
                    int filesize = FileData.ContentLength;
                    string contentType = FileData.ContentType;
                    //string filename = Path.GetFileName(FileData.FileName);
                    string ext = Path.GetExtension(FileData.FileName);

                    fileName = fileId.ToString() + ext;
                    thumbName = "_thumb_" + fileId.ToString() + ext;

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

                    filePath = Path.Combine(folderPath, fileName);
                    thumbPath = Path.Combine(folderPath, thumbName);

                    FileData.SaveAs(filePath);

                    // 生成缩略图
                    ImageHelper.GenerateThumb(filePath, thumbPath, thumbWidth, thumbHeight, "Cut");
                }
                catch (Exception)
                {
                    return Json(new { success = false, error = "图片处理错误，请稍候再试！" }, "text/html");
                }
            }

            fileUrl = string.Format("~/Help/ShowPic?folderid={0}&filename={1}", folder, fileName);
            thumbUrl = string.Format("~/Help/ShowPic?folderid={0}&filename={1}", folder, thumbName);

            return Json(new
            {
                success = true,
                filename = fileName,
                fileurl = Url.Content(fileUrl),
                thumbname = thumbName,
                thumburl = Url.Content(thumbUrl)
            }, "text/html");
        }

        [HttpPost]
        public ActionResult Delete(Guid folder, string filename)
        {
            string filePath = PicUploadHelper.GetPicPath(folder, filename);
            string thumbPath = PicUploadHelper.GetPicPath(folder, "_thumb_" + filename);
            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            if(System.IO.File.Exists(thumbPath))
            {
                System.IO.File.Delete(thumbPath);
            }
            return Json(new { success = true, message = "删除成功。" });
        }
    }
}
