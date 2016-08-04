using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace MVCValidateDemo.Controllers
{
    internal class Util
    {
        internal static float bigMaxHight = 1080;
        internal static float bigMaxWidth = 1080;

        public static readonly string FileStoreRootPath = ConfigurationManager.AppSettings["System:FileStoreRootPath"];

        public static readonly string PhotoPath = Path.Combine(Util.FileStoreRootPath, "Photos");

        public static readonly string DomainUrl = ConfigurationManager.AppSettings["System:DomainUrl"];
    }

    public class PicFileInfo
    {
        public string Folder { get; set; }
        public string FileName { get; set; }
        public string ThumbPath { get; set; }
    }

    public class PicHelper
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="thumbPath">缩略图路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>   
        public static void GenerateThumb(string imagePath, string thumbPath, int width, int height, string mode)
        {
            Image image = Image.FromFile(imagePath);

            string extension = imagePath.Substring(imagePath.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
            ImageFormat imageFormat = null;
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }

            int toWidth = width > 0 ? width : image.Width;
            int toHeight = height > 0 ? height : image.Height;

            int x = 0;
            int y = 0;
            int ow = image.Width;
            int oh = image.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）           
                    break;
                case "W"://指定宽，高按比例             
                    toHeight = image.Height * width / image.Width;
                    break;
                case "H"://指定高，宽按比例
                    toWidth = image.Width * height / image.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）           
                    if ((double)image.Width / (double)image.Height > (double)toWidth / (double)toHeight)
                    {
                        oh = image.Height;
                        ow = image.Height * toWidth / toHeight;
                        y = 0;
                        x = (image.Width - ow) / 2;
                    }
                    else
                    {
                        ow = image.Width;
                        oh = image.Width * height / toWidth;
                        x = 0;
                        y = (image.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp
            Image bitmap = new Bitmap(toWidth, toHeight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(image,
                        new Rectangle(0, 0, toWidth, toHeight),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumbPath, imageFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (bitmap != null)
                    bitmap.Dispose();
                if (image != null)
                    image.Dispose();
            }
        }

        private static string SavePhoto(string imagePath)
        {
            string photourl = "";
            if (File.Exists(imagePath))
            {
                if (!Directory.Exists(Util.PhotoPath))
                {
                    Directory.CreateDirectory(Util.PhotoPath);
                }

                string fpath = Path.Combine(Util.PhotoPath, DateTime.Now.Year.ToString("0000"));
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }

                fpath = Path.Combine(fpath, DateTime.Now.Month.ToString("00"));
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }

                fpath = Path.Combine(fpath, DateTime.Now.Day.ToString("00"));
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }

                string fid = Guid.NewGuid().ToString();
                string ext = Path.GetExtension(imagePath);
                string filePath = Path.Combine(fpath, string.Format("{0}{1}", fid, ext));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.Copy(imagePath, filePath);

                //using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                //{
                //    fs.Write(bytes, 0, bytes.Length);
                //    fs.Close();
                //}

                photourl = string.Format("Photos/{0:yyyy}/{0:MM}/{0:dd}/{1}{2}", DateTime.Now, fid, ext);
            }

            return photourl;
        }

        public static string GetPhotoPath(Guid folderid, string filename)
        {
            string savePath = HostingEnvironment.MapPath("~/App_Data/UploadFiles/");
            string folderPath = Path.Combine(savePath, folderid.ToString());
            string rawingPath = Path.Combine(folderPath, filename);
            return rawingPath;
        }

        public static string SavePhoto(Guid folderid, string filename)
        {
            string path = GetPhotoPath(folderid, filename);
            return SavePhoto(path);
        }

        public static string GetPhotoPath(string url)
        {
            var s = url.Split(new char[] { '/' });
            string sub = string.Join("\\", s);
            return Path.Combine(Util.FileStoreRootPath, sub);
        }

        /// <summary>
        /// 九宫格图片上传
        /// </summary>
        /// <param name="request"></param>
        /// <param name="folder"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static List<PicFileInfo> Build(HttpRequestBase request, Guid? folder, string prefix)
        {
            List<PicFileInfo> photos = new List<PicFileInfo>();

            int rowIndex = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(request.Form[prefix + "FileName" + rowIndex.ToString()]))
                {
                    break;
                }

                string rowFileName = request[prefix + "FileName" + rowIndex.ToString()];
                PicFileInfo pfinfo = new PicFileInfo();
                pfinfo.FileName = rowFileName;
                if (rowFileName != "raw")
                {
                    pfinfo.ThumbPath = PicHelper.SavePhoto(folder.GetValueOrDefault(), rowFileName);
                }
                photos.Add(pfinfo);
                rowIndex++;
            }

            return photos;
        }

        public static string GetUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Format("{0}/qw.png", Util.DomainUrl.TrimEnd('/'));
            }
            if (url.StartsWith("http://"))
            {
                return url;
            }
            if (HttpContext.Current != null && HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                var s = string.Format("~/Help/ShowPic2?url={0}", url);
                return VirtualPathUtility.ToAbsolute(s);
            }
            return string.Format("{0}/{2}", Util.DomainUrl.TrimEnd('/'), url.TrimStart('/'));
        }
    }
}