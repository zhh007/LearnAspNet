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
        {
            if (typeof(TProperty) != typeof(PicUploadModel))
            {
                throw new Exception("上传控件只能绑定PicUploadModel类。");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            PicUploadModel model = metadata.Model as PicUploadModel;

            if (model.Folder == null || model.Folder == Guid.Empty)
            {
                model.Folder = Guid.NewGuid();
            }

            string htmlId = string.Format("{0}-{1}", metadata.ContainerType.Name, metadata.PropertyName);
            string btnId = string.Format("btn{0}", htmlId);
            int max = 9;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\r\n<div class='speed-panel' id='{0}'>", htmlId);

            int index = 0;
            foreach (var item in model.Files)
            {
                sb.AppendLine("<div class='speed-main'>");
                sb.AppendLine("<div class='speed-img'>");
                sb.AppendFormat("<a href='{0}' data-lightbox='roadtrip'>\r\n", !string.IsNullOrEmpty(item.ThumbUrl) ? item.ThumbUrl : item.FileUrl);
                sb.AppendFormat("<img src='{0}' />\r\n", !string.IsNullOrEmpty(item.ThumbUrl) ? item.ThumbUrl : item.FileUrl);
                sb.AppendLine("</a>");
                sb.AppendLine("</div>");

                sb.AppendLine("<div class='speed-del'>");
                sb.AppendFormat("<a href='javascript:;' onclick='deletePic(this, \"{0}\")'>", htmlId);
                sb.AppendLine("<i class='fa fa-minus-circle fa-lg'></i>");
                sb.AppendLine("</a>");

                sb.AppendFormat("<input type='hidden' class='filename' value='raw' name='{0}' />", string.Format("{0}FileName{1}", htmlId, index));

                sb.AppendLine("</div>");
                index++;
            }

            sb.Append("<div class='speed-main'");
            if (index >= max - 1)
            {
                sb.Append(" style='display:none;'");
            }
            sb.AppendLine("id='" + btnId + "'>");
            sb.AppendLine("<a href='#'><div class='lent'></div></a>");
            sb.AppendLine("</div>");

            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("$(function () {");

            sb.AppendFormat("init9PicUploader('{0}', '{1}', '{2}', '{3}', {4});"
                , model.Folder, btnId, VirtualPathUtility.ToAbsolute("~/__picuploader/PicUpload")
                , htmlId, max);

            sb.AppendLine("\r\n});");
            sb.AppendLine("</script>");

            return MvcHtmlString.Create(sb.ToString());
        }
}
}
