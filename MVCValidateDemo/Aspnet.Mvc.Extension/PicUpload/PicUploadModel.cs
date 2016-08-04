using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension
{
    [Serializable]
    public class PicUploadModel
    {
        public Guid Folder { get; set; }

        public List<PicUploadItem> Files { get; set; }

        public PicUploadModel()
        {
            Files = new List<PicUploadItem>();
        }
    }

    public class PicUploadItem
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public string ThumbName { get; set; }
        public string ThumbPath { get; set; }
        public string ThumbUrl { get; set; }
    }
}
