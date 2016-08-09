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
        public readonly static int defaultMinFilesCount = 0;
        public readonly static int defaultMaxFilesCount = 30;
        public readonly static int defaultMaxFileSizeMB = 10;

        public static MvcHtmlString PicUpload<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression)
            where TProperty : PicUploadModel
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            PicUploadModel model = metadata.Model as PicUploadModel;

            if (model == null)
            {
                throw new Exception(string.Format("{0}未初始化。", metadata.PropertyName));
            }

            if (model.Folder == null || model.Folder == Guid.Empty)
            {
                model.Folder = Guid.NewGuid();
            }

            //string partialFieldName = ExpressionHelper.GetExpressionText(expression);
            //string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(partialFieldName);
            string fieldDisplayName = "";
            int MinFilesCount = defaultMinFilesCount;
            int MaxFilesCount = defaultMaxFilesCount;
            int MaxFileSizeMB = defaultMaxFileSizeMB;
            PicUploadValidateAttribute patt = Helper.GetAttribute<PicUploadValidateAttribute>(metadata.ContainerType, metadata.PropertyName);
            if (patt != null)
            {
                MinFilesCount = patt.MinFilesCount;
                MaxFilesCount = patt.MaxFilesCount;
                MaxFileSizeMB = patt.MaxFileSizeMB;
            }

            RequiredAttribute reqAtt = Helper.GetAttribute<RequiredAttribute>(metadata.ContainerType, metadata.PropertyName);
            DisplayAttribute displayAtt = Helper.GetAttribute<DisplayAttribute>(metadata.ContainerType, metadata.PropertyName);
            if (displayAtt != null)
            {
                fieldDisplayName = displayAtt.Name;
            }

            string inputId = string.Format("{0}-{1}", metadata.ContainerType.Name, metadata.PropertyName);
            string boxId = string.Format("box{0}", inputId);
            string btnId = string.Format("btn{0}", inputId);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\r\n<div class='pp-panel picupload' id='{0}'>", boxId);
            sb.AppendFormat("\r\n<input type='hidden' id='{0}' name='{0}' value='{1}'", inputId, model.Folder);
            sb.Append(" data-val='true'");
            if (reqAtt != null || MinFilesCount > 0)
            {
                int count = 1;
                if (MinFilesCount > 0)
                {
                    count = MinFilesCount;
                }
                sb.AppendFormat(" data-val-puRequired='{0}至少要上传{1}个图片。' MinFilesCount='{1}'", fieldDisplayName, count);
            }
            if (MaxFilesCount > 0)
            {
                sb.AppendFormat(" data-val-puMaxFilesCount='{0}最多上传{1}个图片。' MaxFilesCount='{1}'", fieldDisplayName, MaxFilesCount);
            }
            sb.Append(" />\r\n");

            int index = 0;
            foreach (var item in model.Files)
            {
                if (item.State == UploadState.Delete)
                    continue;
                sb.AppendLine("<div class='pp-box'>");
                sb.AppendLine("<div class='pp-img'>");
                sb.AppendFormat("\t<a href='{0}' data-lightbox='roadtrip'>\r\n", !string.IsNullOrEmpty(item.ThumbUrl) ? item.ThumbUrl : item.FileUrl);
                sb.AppendFormat("\t<img src='{0}' />\r\n", !string.IsNullOrEmpty(item.ThumbUrl) ? item.ThumbUrl : item.FileUrl);
                sb.AppendLine("\t</a>");
                sb.AppendLine("</div>");

                sb.Append("<div class='btn-del'>");
                sb.Append("<a href='javascript:;' class='del'><i class='fa fa-minus-circle fa-lg'></i></a>");
                sb.AppendLine("</div>");

                sb.AppendFormat("<input type='hidden' class='state' name='{0}' value='{1}' />\r\n", string.Format("{0}State{1}", inputId, index), item.State == UploadState.New ? 1 : 0);
                sb.AppendFormat("<input type='hidden' class='filename' name='{0}' value='{1}' />\r\n", string.Format("{0}FileName{1}", inputId, index), item.FileName);
                sb.AppendFormat("<input type='hidden' class='fileurl' name='{0}' value='{1}' />\r\n", string.Format("{0}FileUrl{1}", inputId, index), item.FileUrl);
                sb.AppendFormat("<input type='hidden' class='thumbname' name='{0}' value='{1}' />\r\n", string.Format("{0}ThumbName{1}", inputId, index), item.ThumbName);
                sb.AppendFormat("<input type='hidden' class='thumburl' name='{0}' value='{1}' />\r\n", string.Format("{0}ThumbUrl{1}", inputId, index), item.ThumbUrl);

                sb.AppendLine("</div>");
                index++;
            }

            //upload button
            sb.Append("<div class='pp-box'");
            if (MaxFilesCount > 0 && index + 1 >= MaxFilesCount)
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
                sb.AppendFormat("<input type='hidden' class='delete_file' name='{0}' value='{1}' />\r\n", string.Format("{0}Delete{1}", inputId, index), item.FileName);
            }

            sb.AppendLine("<div style='clear:both'></div>");
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("$(function () {");

            sb.AppendFormat("$.picupload('{0}', '{1}', '{2}', {3});"
                , VirtualPathUtility.ToAbsolute("~/__picuploader/Upload")
                , VirtualPathUtility.ToAbsolute("~/__picuploader/Delete")
                , inputId, MaxFilesCount);

            sb.AppendLine("\r\n});");
            sb.AppendLine("</script>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString PicUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression)
            where TProperty : PicUploadModel
        {
            return htmlHelper.PicUploadValidationMessageFor(expression, null, null);
        }

        public static MvcHtmlString FileUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression, string validationMessage)
            where TProperty : PicUploadModel
        {
            return htmlHelper.PicUploadValidationMessageFor(expression, validationMessage, null);
        }

        public static MvcHtmlString PicUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes)
            where TProperty : PicUploadModel
        {
            return htmlHelper.PicUploadValidationMessageFor(expression, validationMessage, htmlAttributes, null);
        }

        public static MvcHtmlString PicUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes, string tag)
            where TProperty : PicUploadModel
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            PicUploadModel model = metadata.Model as PicUploadModel;

            if (model == null)
            {
                throw new Exception(string.Format("{0}未初始化。", metadata.PropertyName));
            }

            string htmlId = string.Format("{0}-{1}", metadata.ContainerType.Name, metadata.PropertyName);

            return htmlHelper.ValidationMessage(htmlId, validationMessage, htmlAttributes, tag);
        }

    }
}
