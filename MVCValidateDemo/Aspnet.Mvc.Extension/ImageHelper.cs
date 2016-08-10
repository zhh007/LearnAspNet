using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension
{
    public static class ImageHelper
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

        public static byte[] ToBytes(Image image)
        {
            using (var ms = new MemoryStream())
            {
                try
                {
                    image.Save(ms, image.RawFormat);
                }
                catch (System.ArgumentNullException)
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 裁剪
        /// </summary>
        public static byte[] Crop(Image img, int width, int height, int startX, int startY, System.Drawing.Imaging.ImageFormat format)
        {
            //check the image height against our desired image height
            if (img.Height < height)
            {
                height = img.Height;
            }

            if (img.Width < width)
            {
                width = img.Width;
            }

            //create a bitmap window for cropping
            using (Bitmap bmPhoto = new Bitmap(width, height))//, PixelFormat.Format24bppRgb
            {
                //bmPhoto.SetResolution(72, 72);

                //create a new graphics object from our image and set properties
                using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
                {
                    grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    //now do the crop
                    grPhoto.DrawImage(img, new Rectangle(0, 0, width, height), startX, startY, width, height, GraphicsUnit.Pixel);
                }

                using (var mm = new MemoryStream())
                {
                    bmPhoto.Save(mm, format);
                    return mm.ToArray();
                    //outimage = GetImageFromStream(mm);//Image.FromStream(mm);
                }
            }
        }

        ///// <summary>
        ///// 文件压缩（png格式）
        ///// </summary>
        //public static byte[] Resize(Image img, int width, int height, System.Drawing.Imaging.ImageFormat format)
        //{
        //    Image resized = null;
        //    try
        //    {
        //        resized = BuildThumbnail(img, width, height);
        //        //if (width > height)
        //        //{
        //        //    resized = BuildThumbnail(img, width, width);
        //        //}
        //        //else
        //        //{
        //        //    resized = BuildThumbnail(img, height, height);
        //        //}
        //        return Crop(resized, width, height, 0, 0, format);
        //    }
        //    finally
        //    {
        //        resized.Dispose();
        //    }
        //}

        /// <summary>
        /// 大图生成缩略图，小图直接返回原图
        /// </summary>
        public static Image Resize(Image img, int width, int height)
        {
            int oWidth = img.Width;
            int oHeight = img.Height;
            if (oWidth > width || oHeight > height)
            {
                //The flips are in here to prevent any embedded image thumbnails -- usually from cameras
                //from displaying as the thumbnail image later, in other words, we want a clean
                //resize, not a grainy one.
                img.RotateFlip(RotateFlipType.Rotate180FlipX);
                img.RotateFlip(RotateFlipType.Rotate180FlipX);

                float ratio = 0;
                if (width == height)
                {
                    if (oWidth < oHeight)
                    {
                        ratio = (float)oHeight / (float)oWidth;
                        oHeight = height;
                        oWidth = Convert.ToInt32(Math.Round((float)oHeight / ratio));
                    }
                    else
                    {
                        ratio = (float)oWidth / (float)oHeight;
                        oWidth = width;
                        oHeight = Convert.ToInt32(Math.Round((float)oWidth / ratio));
                    }
                }
                else if (width > height)
                {
                    ratio = (float)oHeight / (float)oWidth;
                    oHeight = height;
                    oWidth = Convert.ToInt32(Math.Round((float)oHeight / ratio));
                }
                else
                {
                    ratio = (float)oWidth / (float)oHeight;
                    oWidth = width;
                    oHeight = Convert.ToInt32(Math.Round((float)oWidth / ratio));
                }

                //return the resized image
                return img.GetThumbnailImage(oWidth, oHeight, null, IntPtr.Zero);
            }
            //return the original resized image
            return img;
        }

        public static Image FromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    return FromStream(ms);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get an image from a file without locking the file
        /// </summary>
        public static Image FromFile(string fileName)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return FromStream(stream);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get an image from stream without locking the stream
        /// </summary>
        public static Image FromStream(Stream stream)
        {
            try
            {
                using (var image = Image.FromStream(stream))
                {
                    var srcBitmap = image as Bitmap;
                    var destBitmap = new Bitmap(image.Width, image.Height, image.PixelFormat);
                    if (IsImageIndexed(image) && srcBitmap != null)
                    {
                        Rectangle srcArea = new Rectangle(0, 0, image.Width, image.Height);
                        BitmapData srcData = srcBitmap.LockBits(srcArea, ImageLockMode.ReadOnly, image.PixelFormat);
                        Rectangle destArea = new Rectangle(0, 0, image.Width, image.Height);
                        BitmapData destData = destBitmap.LockBits(destArea, ImageLockMode.WriteOnly, image.PixelFormat);

                        IntPtr srcPtr = srcData.Scan0;
                        IntPtr destPtr = destData.Scan0;
                        byte[] buffer = new byte[srcData.Stride * image.Height];
                        Marshal.Copy(srcPtr, buffer, 0, buffer.Length);
                        Marshal.Copy(buffer, 0, destPtr, buffer.Length);
                        srcBitmap.UnlockBits(srcData);
                        destBitmap.UnlockBits(destData);
                        destBitmap.Palette = srcBitmap.Palette;
                    }
                    else
                    {
                        using (var graphics = Graphics.FromImage(destBitmap))
                        {
                            graphics.DrawImage(image, 0, 0);
                        }
                    }
                    return destBitmap;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Determines if an image is indexed (with a color palette)
        /// </summary>
        private static bool IsImageIndexed(Image image)
        {
            switch (image.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Indexed:
                    return true;
            }
            return false;
        }
    }
}
