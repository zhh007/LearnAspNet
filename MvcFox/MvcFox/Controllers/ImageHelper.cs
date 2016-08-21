using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcFox.Controllers
{
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

        public static string GetFile(Guid folder, string filename)
        {
            string savePath = HttpContext.Current.Server.MapPath("~/App_Data/UploadFiles/");
            savePath = Path.Combine(savePath, folder.ToString(), filename);
            return savePath;
        }
    }
}