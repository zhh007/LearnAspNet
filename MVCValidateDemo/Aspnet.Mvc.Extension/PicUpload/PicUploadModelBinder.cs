using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;

namespace Aspnet.Mvc.Extension
{
    public class PicUploadModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model == null)
            {
                return null;
            }
            var modelType = model.GetType();

            foreach (var property in modelType.GetProperties())
            {
                if (property.PropertyType == typeof(PicUploadModel))
                {
                    PicUploadModel o = property.GetValue(model, null) as PicUploadModel;
                    if (o != null)
                    {
                        ModelState modelState = bindingContext.ModelState[property.Name];
                        if (modelState == null)
                        {
                            modelState = new ModelState();
                            bindingContext.ModelState[property.Name] = modelState;
                        }

                        string htmlId = string.Format("{0}-{1}", modelType.Name, property.Name);

                        string v = controllerContext.HttpContext.Request[htmlId];
                        o.Folder = new Guid(v);
                        o.Files = Build(controllerContext.HttpContext.Request, o.Folder, htmlId);
                    }
                }
            }

            return model;
        }

        public List<PicUploadItem> Build(HttpRequestBase request, Guid folder, string prefix)
        {
            List<PicUploadItem> photos = new List<PicUploadItem>();

            int rowIndex = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(request.Form[prefix + "FileName" + rowIndex.ToString()]))
                {
                    break;
                }

                string fileName = request[prefix + "FileName" + rowIndex.ToString()];
                string thumbName = request[prefix + "ThumbName" + rowIndex.ToString()];

                PicUploadItem pfinfo = new PicUploadItem();
                pfinfo.FileName = fileName;
                pfinfo.ThumbName = thumbName;
                if (fileName != "raw")
                {
                    pfinfo.State = UploadState.New;
                    pfinfo.FilePath = PicUploadManager.SavePhoto(folder, fileName);
                    pfinfo.ThumbPath = PicUploadManager.SavePhoto(folder, thumbName);
                }
                else
                {
                    pfinfo.State = UploadState.None;
                }
                photos.Add(pfinfo);
                rowIndex++;
            }

            rowIndex = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(request.Form[prefix + "Delete" + rowIndex.ToString()]))
                {
                    break;
                }

                string fileName = request[prefix + "Delete" + rowIndex.ToString()];

                PicUploadItem pfinfo = new PicUploadItem();
                pfinfo.FileName = fileName;
                pfinfo.State = UploadState.Delete;
                photos.Add(pfinfo);
                rowIndex++;
            }

            return photos;
        }
    }
}
