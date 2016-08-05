using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web;
using System.Threading;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web.Mvc.Html;

namespace Aspnet.Mvc.Extension
{
    public static class PicUploadHtmlHelper
    {
        public static MvcHtmlString PicUpload<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            where TProperty : PicUploadModel
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            PicUploadModel model = metadata.Model as PicUploadModel;

            if(model == null)
            {
                throw new Exception(string.Format("{0}未初始化。", metadata.PropertyName));
            }

            if (model.Folder == null || model.Folder == Guid.Empty)
            {
                model.Folder = Guid.NewGuid();
            }

            string htmlId = string.Format("{0}-{1}", metadata.ContainerType.Name, metadata.PropertyName);
            string btnId = string.Format("btn{0}", htmlId);
            int max = 9;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\r\n<div class='speed-panel picupload' id='{0}'>", htmlId);
            sb.AppendFormat("\r\n<input type='hidden' name='{0}' value='{1}' />\r\n", htmlId, model.Folder);

            int index = 0;
            foreach (var item in model.Files)
            {
                if (item.State == UploadState.Delete)
                    continue;
                sb.AppendLine("<div class='speed-main'>");
                sb.AppendLine("<div class='speed-img'>");
                sb.AppendFormat("\t<a href='{0}' data-lightbox='roadtrip'>\r\n", !string.IsNullOrEmpty(item.ThumbUrl) ? item.ThumbUrl : item.FileUrl);
                sb.AppendFormat("\t<img src='{0}' />\r\n", !string.IsNullOrEmpty(item.ThumbUrl) ? item.ThumbUrl : item.FileUrl);
                sb.AppendLine("\t</a>");
                sb.AppendLine("</div>");

                sb.AppendLine("<div class='speed-del'>");
                sb.AppendFormat("\t<a href='javascript:;' onclick='deletePic(this, \"{0}\")'>", htmlId);
                sb.Append("<i class='fa fa-minus-circle fa-lg'></i>");
                sb.Append("</a>");
                sb.AppendLine("</div>");

                sb.AppendFormat("<input type='hidden' class='state' name='{0}' value='{1}' />\r\n", string.Format("{0}State{1}", htmlId, index), item.State == UploadState.New ? 1 : 0);
                sb.AppendFormat("<input type='hidden' class='filename' name='{0}' value='{1}' />\r\n", string.Format("{0}FileName{1}", htmlId, index), item.FileName);
                sb.AppendFormat("<input type='hidden' class='fileurl' name='{0}' value='{1}' />\r\n", string.Format("{0}FileUrl{1}", htmlId, index), item.FileUrl);
                sb.AppendFormat("<input type='hidden' class='thumbname' name='{0}' value='{1}' />\r\n", string.Format("{0}ThumbName{1}", htmlId, index), item.ThumbName);
                sb.AppendFormat("<input type='hidden' class='thumburl' name='{0}' value='{1}' />\r\n", string.Format("{0}ThumbUrl{1}", htmlId, index), item.ThumbUrl);

                sb.AppendLine("</div>");
                index++;
            }

            //upload button
            sb.Append("<div class='speed-main'");
            if (index >= max - 1)
            {
                sb.Append(" style='display:none;'");
            }
            sb.AppendLine("id='" + btnId + "'>");
            sb.AppendLine("<a href='#'><div class='lent'></div></a>");
            sb.AppendLine("</div>");

            //delete files
            index = 0;
            foreach (var item in model.Files)
            {
                if (item.State != UploadState.Delete)
                    continue;
                sb.AppendFormat("<input type='hidden' class='delete_file' name='{0}' value='{1}' />\r\n", string.Format("{0}Delete{1}", htmlId, index), item.FileName);
            }

            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("$(function () {");

            sb.AppendFormat("init9PicUploader('{0}', '{1}', '{2}', '{3}', {4});"
                , model.Folder, btnId, VirtualPathUtility.ToAbsolute("~/__picuploader/PicUpload")
                , htmlId, max);

            sb.AppendLine("\r\n});");
            sb.AppendLine("</script>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
}
}
