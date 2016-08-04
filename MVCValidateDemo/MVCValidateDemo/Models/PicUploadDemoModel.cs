using MVCValidateDemo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCValidateDemo.Models
{
    public class PicUploadDemoModel
    {
        public Guid PhotoFolderId { get; set; }
        public List<PicFileInfo> Photos { get; set; }
        public PicUploadDemoModel()
        {
            this.Photos = new List<PicFileInfo>();
        }
    }
}