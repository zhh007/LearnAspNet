using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    [Serializable]
    [ModelBinder(typeof(FileUploadModelBinder))]
    public class FileUploadModel
    {
        public Guid Folder { get; set; }

        public List<FileUploadItem> Files { get; set; }

        public FileUploadModel()
        {
            Files = new List<FileUploadItem>();
        }

        public void Save()
        {
            FileUploadHtmlHelper.SaveFolder(this.Folder);
        }
    }

    public class FileUploadItem
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string Extension { get; set; }
        public string StorePath { get; set; }
        public Guid CreatorID { get; set; }
        public string CreatorName { get; set; }
        public string CreatorCode { get; set; }
        public Guid FolderID { get; set; }
        public DateTime CreateTime { get; set; }

        public string FileSizeFormat
        {
            get
            {
                return FileUploadHtmlHelper.GetFileSizeFormat(this.FileSize);
            }
        }

        public string FileClass
        {
            get
            {
                return FileUploadHtmlHelper.GetFileClass(this.FileName);
            }
        }
    }

    public enum FileUploadDisplayType
    {
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 0,
        /// <summary>
        /// 显示
        /// </summary>
        View = 1
    }
}
