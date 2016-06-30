using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    [Serializable]
    [ModelBinder(typeof(FileUploadComponentModelBinder))]
    public class FileUploadComponentModel
    {
        public Guid Folder { get; set; }

        public List<WorkflowFileInfo> Files { get; set; }

        public FileUploadComponentModel()
        {
            Files = new List<WorkflowFileInfo>();
        }

        public void Save()
        {
            FileUploadHelper.SaveFolder(this.Folder);
        }
    }
}
