using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcFox.Controllers
{
    public class HeadEditController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            string headFileId = Request.Form["headFileId"];
            string headFolderId = Request.Form["headFolderId"];
            if (!string.IsNullOrEmpty(headFileId) && !string.IsNullOrEmpty(headFolderId))
            {
                string filename = ImageHelper.GetFile(new Guid(headFolderId), new Guid(headFileId));
                byte[] fs = ImageHelper.FileToByteArray(filename);
            }
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

                    try
                    {
                        if (System.IO.File.Exists(rawingPath))
                        {
                            System.IO.File.Delete(rawingPath);
                        }
                        FileData.SaveAs(rawingPath);

                        using (Bitmap rawImage = new Bitmap(FileData.InputStream))
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

                            //if (hResolution > 96.0f && vResolution > 96.0f)
                            //{
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
                            //}
                            //else
                            //{

                            //    //Bitmap thumbnail = ImageHelper.ResizeImage(rawImage, nPercent);

                            //    //if (hResolution == 1.0f)
                            //    //{
                            //    hResolution = 96.0f;
                            //    //}
                            //    //if (vResolution == 1.0f)
                            //    //{
                            //    vResolution = 96.0f;
                            //    //}

                            //    Bitmap thumbnail = ImageHelper.ResizeImage(rawImage, nPercent, hResolution, vResolution);
                            //    nPercent = 1.0f;

                            //    string thumbnailPath = Path.Combine(folderPath, "_thumbnail_" + rawingFileName);
                            //    thumbnail.Save(thumbnailPath);
                            //    //thumbnail.Save(rawingPath);
                            //}
                        }

                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException(e.Message);
                    }

                    //result = string.Format("{{\"success\": true, \"fileid\": '{0}'}}", fileid.ToString());
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = "图片处理错误，请稍候再试！" }, "text/html");
                }
            }
            return Json(new { success = true, fileid = fileId.ToString(), percent = nPercent, spercent = snPercent, hResolution = hResolution, vResolution = vResolution, width = newImageWidth, height = newImageHeight }, "text/html");
        }

        /// <summary>
        /// 上传后下载缩略图
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
        /// 保存草稿
        /// </summary>
        public ActionResult SaveImage(Guid fileid, Guid folder, double percent, double x, double y, double w, double h, int angle, float maxWidth, float maxHeight)
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

            //x = x / percent;
            //y = y / percent;
            //w = w / percent;
            //h = h / percent;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            if (files.Count() == 1)
            {
                string ext = files[0].Extension;
                //string filename = Path.GetFileName(files[0].FullName).Substring(36 + 8);

                //删除旧文件               
                //var tmp_new = dif.GetFiles("_new_" + fileid.ToString() + "*", SearchOption.TopDirectoryOnly);
                //foreach (var item in tmp_new)
                //{
                //    item.Delete();
                //}

                //{formatid}_raw_{filename}.ext
                string raw_filename = "_rawing_" + fileid + ext;
                string raw_filepath = Path.Combine(savePath, string.Format("_rawing_{0}{1}", fileid, ext)); // raw_filename);
                //

                //{formatid}_new_{filename}.ext
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
                using (Bitmap oldimg = new Bitmap(raw_filepath))//files[0].FullName
                {
                    int sourceWidth = oldimg.Width;
                    int sourceHeight = oldimg.Height;

                    //nPercentW = (maxWidth / (float)sourceWidth);
                    //nPercentH = (maxHeight / (float)sourceHeight);

                    //if (nPercentH < nPercentW)
                    //    nPercent = nPercentH;
                    //else
                    //    nPercent = nPercentW;

                    //x = x / nPercent;
                    //y = y / nPercent;
                    //w = w / nPercent;
                    //h = h / nPercent;

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

                        ImageHelper.Resize(bmp, (int)maxWidth, (int)maxHeight, newpath);

                        //EncoderParameters encoderParams = new EncoderParameters();
                        //long[] quality = new long[1];
                        //quality[0] = 92; //压缩比例，决定图片大小的重要因素。
                        //EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                        //encoderParams.Param[0] = encoderParam;

                        //int targetWidth = (int)maxWidth;
                        //int targetHeight = (int)maxHeight;

                        //using (Bitmap newBmp = new Bitmap(targetWidth, targetHeight))
                        //{
                        //    newBmp.SetResolution(oldimg.HorizontalResolution, oldimg.VerticalResolution);
                        //    using (Graphics ng = Graphics.FromImage(newBmp))
                        //    {
                        //        // 用白色清空 
                        //        ng.Clear(Color.White);
                        //        // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                        //        ng.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //        // 指定高质量、低速度呈现。 
                        //        ng.SmoothingMode = SmoothingMode.HighQuality;

                        //        // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
                        //        ng.DrawImage(bmp, new Rectangle(0, 0, targetWidth, targetHeight), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                        //    }

                        //    newBmp.Save(newpath, GetCodecInfo("image/jpeg"), encoderParams);
                        //}

                        //bmp.Save(newpath, GetCodecInfo("image/jpeg"), encoderParams);
                    }
                    //内存不足
                    //Bitmap newimg = oldimg.Clone(rect, oldimg.PixelFormat);
                    //newimg.Save(newpath);
                }

                //string clearfile = Path.Combine(savePath, "_clear.txt");
                //System.IO.File.Delete(clearfile);

                //删除 {formatid}_rawing_{filename}.ext
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

        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
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

            //imageDocumentService.DeleteByOpenID(openid, formatid);
            return Json(new { success = true });
        }

        /// <summary>
        /// 显示草稿图，或真实图片
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

            //ImageDocumentDTO doc = imageDocumentService.GetByOpenID(openid, formatid);
            //if (doc != null)
            //{
            //    return File(doc.FileBody, "application/octet-stream", "show");
            //}

            return Content("");
        }

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
    }

    public class ImageHelper
    {
        public static string GetFile(Guid folder, Guid fileid)
        {
            string savePath = HttpContext.Current.Server.MapPath("~/App_Data/UploadFiles/");
            savePath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dif = new DirectoryInfo(savePath);
            if (dif.Exists)
            {
                FileInfo[] files = dif.GetFiles("_new_" + fileid.ToString() + "*", SearchOption.TopDirectoryOnly);

                if (files.Count() == 1)
                {
                    return files[0].FullName;
                }
            }
            return "";
        }

        public static byte[] FileToByteArray(string fileName)
        {
            byte[] _Buffer = null;
            try
            {
                // Open file for reading
                using (System.IO.FileStream _FileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    // attach filestream to binary reader
                    using (System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream))
                    {
                        // get total byte length of the file
                        long _TotalBytes = new System.IO.FileInfo(fileName).Length;

                        // read entire file into buffer
                        _Buffer = _BinaryReader.ReadBytes((Int32)_TotalBytes);
                    }
                }
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            return _Buffer;
        }

        public static Bitmap ResizeImage(Bitmap imgToResize, float nPercent)
        {
            float sourceWidth = imgToResize.Width;
            float sourceHeight = imgToResize.Height;

            float destWidth = (sourceWidth * nPercent);
            float destHeight = (sourceHeight * nPercent);

            Bitmap b = new Bitmap((int)destWidth, (int)destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;

            RectangleF destRect = new RectangleF(0, 0, destWidth, destHeight);
            RectangleF srcRect = new RectangleF(0, 0, imgToResize.Width, imgToResize.Height);

            g.DrawImage(imgToResize, destRect, srcRect, GraphicsUnit.Pixel);
            g.Dispose();

            return b;
        }

        public static Bitmap ResizeImage(Bitmap imgToResize, float nPercent, float dpiX, float dpiY)
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

        /// <summary>
        /// 将图片进行翻转处理
        /// </summary>
        /// <param name="mybm">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        /// <returns>经过翻转后的图片</returns>
        public static Bitmap RevPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录经过处理后的图片对象
            int x, y, z;//x,y是循环次数,z是用来记录像素点的x坐标的变化的
            Color pixel;

            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值
                    bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图
                }
            }

            return bm;//返回经过翻转后的图片
        }

        public static void Resize(string filename, int width, int height, string savepath)
        {
            using (Bitmap rawImage = new Bitmap(filename))
            {
                float sourceWidth = rawImage.Width;
                float sourceHeight = rawImage.Height;

                float nPercent = 0;
                float nPercentW = (width / (float)sourceWidth);
                float nPercentH = (height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;

                //newImageWidth = sourceWidth * nPercent;
                //newImageHeight = sourceHeight * nPercent;
                float hResolution = rawImage.HorizontalResolution;
                float vResolution = rawImage.VerticalResolution;

                //if (hResolution > 96.0f && vResolution > 96.0f)
                //{
                Bitmap thumbnail = ImageHelper.ResizeImage(rawImage, nPercent, hResolution, vResolution);
                //string thumbnailPath = Path.Combine(folderPath, "_thumbnail_" + fileId.ToString() + ext);

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

                thumbnail.Save(savepath, ici, ep);
            }
        }

        public static void Resize(Bitmap rawImage, int width, int height, string savepath)
        {
            float sourceWidth = rawImage.Width;
            float sourceHeight = rawImage.Height;

            float nPercent = 0;
            float nPercentW = (width / (float)sourceWidth);
            float nPercentH = (height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            //newImageWidth = sourceWidth * nPercent;
            //newImageHeight = sourceHeight * nPercent;
            float hResolution = rawImage.HorizontalResolution;
            float vResolution = rawImage.VerticalResolution;

            //if (hResolution > 96.0f && vResolution > 96.0f)
            //{
            Bitmap thumbnail = ImageHelper.ResizeImage(rawImage, nPercent, hResolution, vResolution);
            //string thumbnailPath = Path.Combine(folderPath, "_thumbnail_" + fileId.ToString() + ext);

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

            thumbnail.Save(savepath, ici, ep);
        }

        /// <summary>
        /// 对图像进行旋转
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="savepath">存储路径</param>
        /// <returns></returns>
        public static System.Drawing.Image Rotate(System.Drawing.Image img, int angle, string savepath)
        {
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w = img.Width;
            int h = img.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //目标位图
            System.Drawing.Bitmap dsImage = new System.Drawing.Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //计算偏移量
            System.Drawing.Point Offset = new System.Drawing.Point((W - w) / 2, (H - h) / 2);

            //构造图像显示区域：让图像的中心与窗口的中心点一致
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(Offset.X, Offset.Y, w, h);
            System.Drawing.Point center = new System.Drawing.Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(img, rect);

            //重至绘图的所有变换
            g.ResetTransform();

            g.Save();
            g.Dispose();

            //保存旋转后的图片
            img.Dispose();
            dsImage.Save(savepath, System.Drawing.Imaging.ImageFormat.Jpeg);

            return dsImage;
        }

        public static System.Drawing.Image Rotate(string filename, int angle, string savepath)
        {
            using (System.Drawing.Image img = System.Drawing.Bitmap.FromFile(filename))
            {
                return Rotate(img, angle, savepath);
            }
        }
    }
}