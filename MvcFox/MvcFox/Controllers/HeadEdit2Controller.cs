﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcFox.Controllers
{
    /// <summary>
    /// 比例固定,头像编辑，上传后添加空白内容
    /// </summary>
    public class HeadEdit2Controller : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Guid folder, float maxWidth, float maxHeight, int sizeLimit)
        {
            HttpFileCollectionBase files = Request.Files;

            if (files.Count == 0)
            {
                return Json(new { success = false }, "text/html");
            }

            if (files.Count > 1)
            {
                return Json(new { success = false }, "text/html");
            }

            if (files[0].ContentLength == 0)
            {
                return Json(new { success = false }, "text/html");
            }

            if (files[0].ContentLength > sizeLimit)
            {
                string validateMessage = "文件超出规定大小。";
                return Json(new { success = false, error = validateMessage, message = validateMessage }, "text/html");
            }

            string rawingFileName = "";
            HttpPostedFileBase FileData = files[0];
            float snPercent = 0;//服务端比例
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            float hResolution = 0f, vResolution = 0f;//图像的水平分辨率和垂直分辨率，单位：像素/英寸
            float newImageWidth = 0, newImageHeight = 0;//缩放后图片大小
            Guid fileId = Guid.NewGuid();
            if (FileData != null)
            {
                try
                {
                    int filesize = FileData.ContentLength;
                    string contentType = FileData.ContentType;
                    string filename = Path.GetFileName(FileData.FileName);
                    string ext = Path.GetExtension(FileData.FileName);

                    rawingFileName = "_raw_" + fileId.ToString() + ext;

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

                    string rawingPath = Path.Combine(folderPath, rawingFileName);
                    string newTmpFileName = "_newTmp_" + fileId.ToString() + ext;
                    newTmpFileName = Path.Combine(folderPath, newTmpFileName);
                    try
                    {
                        if (System.IO.File.Exists(rawingPath))
                        {
                            System.IO.File.Delete(rawingPath);
                        }

                        //处理图片
                        if (System.IO.File.Exists(newTmpFileName))
                        {
                            System.IO.File.Delete(newTmpFileName);
                        }
                        FileData.SaveAs(newTmpFileName);
                        Prc(newTmpFileName, rawingPath, maxWidth, maxHeight);

                        using (Bitmap rawImage = new Bitmap(rawingPath))
                        {
                            float sourceWidth = rawImage.Width;
                            float sourceHeight = rawImage.Height;

                            nPercentW = (maxWidth / (float)sourceWidth);
                            nPercentH = (maxHeight / (float)sourceHeight);

                            if (nPercentH < nPercentW)
                                nPercent = nPercentH;
                            else
                                nPercent = nPercentW;

                            snPercent = nPercent;

                            newImageWidth = sourceWidth * nPercent;
                            newImageHeight = sourceHeight * nPercent;
                            hResolution = rawImage.HorizontalResolution;
                            vResolution = rawImage.VerticalResolution;

                            Bitmap thumbnail = ImageHelper.ResizeImage(rawImage, nPercent, hResolution, vResolution);
                            string thumbnailPath = Path.Combine(folderPath, "_thumbnail_" + fileId.ToString() + ext);

                            //关键质量控制
                            //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                            ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                            ImageCodecInfo ici = null;
                            foreach (ImageCodecInfo i in icis)
                            {
                                if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                                {
                                    ici = i;
                                }
                            }
                            EncoderParameters ep = new EncoderParameters(1);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                            thumbnail.Save(thumbnailPath, ici, ep);
                        }

                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException(e.Message);
                    }
                }
                catch (Exception)
                {
                    return Json(new { success = false, error = "图片处理错误，请稍候再试！" }, "text/html");
                }
            }
            return Json(new { success = true, fileid = fileId.ToString(), percent = nPercent, spercent = snPercent, hResolution = hResolution, vResolution = vResolution, width = newImageWidth, height = newImageHeight }, "text/html");
        }

        /// <summary>
        /// 图片旋转
        /// </summary>
        public ActionResult Rotate(Guid folder, Guid fileid, int angle, int maxWidth, int maxHeight)
        {
            string savePath = Request.MapPath("~/App_Data/UploadFiles/");
            DirectoryInfo dif = new DirectoryInfo(Path.Combine(savePath, folder.ToString()));
            FileInfo[] files = dif.GetFiles("_raw_" + fileid.ToString() + "*", SearchOption.TopDirectoryOnly);

            int up_width = 0;
            int up_height = 0;
            float nPercent = 0;
            if (files.Count() == 1)
            {
                string rawPath = files[0].FullName;
                string spath = string.Format("{0}\\{1}\\_thumbnail_{2}{3}", savePath, folder, fileid, files[0].Extension);
                string tmp = string.Format("{0}\\{1}\\_tmp_{2}{3}", savePath, folder, fileid, files[0].Extension);

                if (System.IO.File.Exists(spath))
                {
                    System.IO.File.Delete(spath);
                }

                ImageHelper.Rotate(rawPath, angle, tmp);

                using (Bitmap tbmp = new Bitmap(tmp))
                {
                    float sourceWidth = tbmp.Width;
                    float sourceHeight = tbmp.Height;

                    float nPercentW = (maxWidth / (float)sourceWidth);
                    float nPercentH = (maxHeight / (float)sourceHeight);

                    if (nPercentH < nPercentW)
                        nPercent = nPercentH;
                    else
                        nPercent = nPercentW;
                }

                ImageHelper.Resize(tmp, maxWidth, maxHeight, spath);

                System.IO.File.Delete(tmp);

                using (Bitmap bmp = new Bitmap(spath))
                {
                    up_width = bmp.Width;
                    up_height = bmp.Height;
                }
            }
            return Json(new { success = true, width = up_width, height = up_height, percent = nPercent });
        }

        /// <summary>
        /// 显示缩略图
        /// </summary>
        public ActionResult DownloadThumbnail(Guid folder, Guid fileid)
        {
            string savePath = Request.MapPath("~/App_Data/UploadFiles/");
            savePath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dif = new DirectoryInfo(savePath);
            FileInfo[] files = dif.GetFiles("_thumbnail_" + fileid.ToString() + "*", SearchOption.TopDirectoryOnly);

            if (files.Count() == 1)
            {
                return File(files[0].FullName, "application/octet-stream", "thumbnail");
            }

            return Content("");
        }

        /// <summary>
        /// 图片截取
        /// </summary>
        public ActionResult SaveImage(Guid fileid, Guid folder, double percent, double x, double y, double w, double h, int angle, float maxWidth, float maxHeight, int _double)
        {
            string savePath = Request.MapPath("~/App_Data/UploadFiles/");
            savePath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dif = new DirectoryInfo(savePath);
            FileInfo[] files = dif.GetFiles("_raw_" + fileid.ToString() + "*", SearchOption.TopDirectoryOnly);

            if (x < 0)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 0;
            }

            if (files.Count() == 1)
            {
                string ext = files[0].Extension;
                string raw_filename = "_rawing_" + fileid + ext;
                string raw_filepath = Path.Combine(savePath, string.Format("_rawing_{0}{1}", fileid, ext)); // raw_filename);
                string newName = "_new_" + fileid + ext;
                string newpath = Path.Combine(savePath, newName);

                if (angle != 0)
                {
                    ImageHelper.Rotate(files[0].FullName, angle, raw_filepath);
                }
                else
                {
                    files[0].CopyTo(raw_filepath);
                }
                using (Bitmap oldimg = new Bitmap(raw_filepath))
                {
                    int sourceWidth = oldimg.Width;
                    int sourceHeight = oldimg.Height;

                    if (w > sourceWidth)
                    {
                        w = sourceWidth;
                    }
                    if (h > sourceHeight)
                    {
                        h = sourceHeight;
                    }
                    Rectangle rect = new Rectangle((int)x, (int)y, (int)w, (int)h);

                    using (Bitmap bmp = new Bitmap((int)w, (int)h))
                    {
                        bmp.SetResolution(oldimg.HorizontalResolution, oldimg.VerticalResolution);
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            // 用白色清空 
                            g.Clear(Color.White);
                            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            // 指定高质量、低速度呈现。 
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
                            g.DrawImage(oldimg, new Rectangle(0, 0, (int)w, (int)h), rect, GraphicsUnit.Pixel);
                        }

                        if (_double > 0)
                        {
                            ImageHelper.Resize(bmp, (int)maxWidth * 2, (int)maxHeight * 2, newpath);
                        }
                        else
                        {
                            ImageHelper.Resize(bmp, (int)maxWidth, (int)maxHeight, newpath);
                        }
                    }
                }

                try
                {
                    System.IO.File.Delete(raw_filepath);
                }
                catch (Exception)
                {

                }

                return Json(new { success = true, folder = folder });
            }

            return Json(new { success = false });
        }

        /// <summary>
        /// 显示图片
        /// </summary>
        public ActionResult ShowImage(Guid folder, Guid fileid)
        {
            string savePath = Request.MapPath("~/App_Data/UploadFiles/");
            savePath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dif = new DirectoryInfo(savePath);
            if (dif.Exists)
            {
                FileInfo[] files = dif.GetFiles("_new_" + fileid.ToString() + "*", SearchOption.TopDirectoryOnly);

                if (files.Count() == 1)
                {
                    return File(files[0].FullName, "application/octet-stream", "draft");
                }
            }

            return Content("");
        }

        public ActionResult ClearImage(Guid openid, Guid folder, int count)
        {
            string savePath = Request.MapPath("~/App_Data/UploadFiles/");
            savePath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dif = new DirectoryInfo(savePath);
            if (!dif.Exists)
            {
                dif.Create();
            }

            FileInfo[] files = dif.GetFiles("_*", SearchOption.TopDirectoryOnly);
            foreach (var item in files)
            {
                item.Delete();
            }

            if (count > 0)
            {
                string clearfile = Path.Combine(savePath, "_clear.txt");
                System.IO.File.AppendAllText(clearfile, "abc");
            }

            return Json(new { success = true });
        }

        private void Prc(string sourceFilePath, string targetFile, float maxWidth, float maxHeight)
        {
            using (Bitmap rawImage = new Bitmap(sourceFilePath))
            {
                int sourceWidth = rawImage.Width;
                int sourceHeight = rawImage.Height;

                float sPercent = (float)sourceWidth / (float)sourceHeight;
                float mPercent = (float)maxWidth / (float)maxHeight;

                //Console.WriteLine("{0}/{1}={2}", sourceWidth, sourceHeight, sPercent);
                //Console.WriteLine("{0}/{1}={2}", maxWidth, maxHeight, mPercent);

                int newW = 0;
                int newH = 0;

                if (sPercent > mPercent)
                {
                    newW = sourceWidth;
                    newH = (int)(sourceWidth / mPercent);
                }
                else
                {
                    newW = (int)(sourceHeight * mPercent);
                    newH = sourceHeight;
                }

                //Console.WriteLine("{0}/{1}={2}", newW, newH, (float)newW / (float)newH);

                using (Bitmap b = new Bitmap(newW, newH))
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 指定高质量、低速度呈现。 
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.Clear(Color.White);

                    RectangleF srcRect = new RectangleF(0, 0, sourceWidth, sourceHeight);
                    RectangleF destRect = new RectangleF(0, 0, newW, newH);

                    if (sPercent > mPercent)
                    {
                        destRect = new RectangleF(0, (newH - sourceHeight) / 2, sourceWidth, sourceHeight);
                    }
                    else
                    {
                        destRect = new RectangleF((newW - sourceWidth) / 2, 0, sourceWidth, sourceHeight);
                    }

                    g.DrawImage(rawImage, destRect, srcRect, GraphicsUnit.Pixel);
                    g.Dispose();

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                    b.Save(targetFile, ici, ep);
                }
            }
        }

    }

}
