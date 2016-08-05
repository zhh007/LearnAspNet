using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Aspnet.Mvc.Extension
{
    public class PicUploadManager
    {
        public static readonly string FileStoreRootPath = ConfigurationManager.AppSettings["System:FileStoreRootPath"];

        public static readonly string PhotoPath = Path.Combine(PicUploadManager.FileStoreRootPath, "Photos");

        public static readonly string DomainUrl = ConfigurationManager.AppSettings["System:DomainUrl"];

        //private static string SavePhoto(string imagePath)
        //{
        //    string photourl = "";
        //    if (File.Exists(imagePath))
        //    {
        //        if (!Directory.Exists(PicUploadManager.PhotoPath))
        //        {
        //            Directory.CreateDirectory(PicUploadManager.PhotoPath);
        //        }

        //        string fpath = Path.Combine(PicUploadManager.PhotoPath, DateTime.Now.Year.ToString("0000"));
        //        if (!Directory.Exists(fpath))
        //        {
        //            Directory.CreateDirectory(fpath);
        //        }

        //        fpath = Path.Combine(fpath, DateTime.Now.Month.ToString("00"));
        //        if (!Directory.Exists(fpath))
        //        {
        //            Directory.CreateDirectory(fpath);
        //        }

        //        fpath = Path.Combine(fpath, DateTime.Now.Day.ToString("00"));
        //        if (!Directory.Exists(fpath))
        //        {
        //            Directory.CreateDirectory(fpath);
        //        }

        //        string fid = Guid.NewGuid().ToString();
        //        string ext = Path.GetExtension(imagePath);
        //        string filePath = Path.Combine(fpath, string.Format("{0}{1}", fid, ext));
        //        if (File.Exists(filePath))
        //        {
        //            File.Delete(filePath);
        //        }

        //        File.Copy(imagePath, filePath);

        //        //using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        //        //{
        //        //    fs.Write(bytes, 0, bytes.Length);
        //        //    fs.Close();
        //        //}

        //        photourl = string.Format("Photos/{0:yyyy}/{0:MM}/{0:dd}/{1}{2}", DateTime.Now, fid, ext);
        //    }

        //    return photourl;
        //}

        public static string GetPicPath(Guid folderid, string filename)
        {
            string savePath = HostingEnvironment.MapPath("~/App_Data/UploadFiles/");
            string folderPath = Path.Combine(savePath, folderid.ToString());
            string rawingPath = Path.Combine(folderPath, filename);
            return rawingPath;
        }

        //public static string SavePhoto(Guid folderid, string filename)
        //{
        //    string path = GetPhotoPath(folderid, filename);
        //    return SavePhoto(path);
        //}
    }
}
