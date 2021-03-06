﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Aspnet.Mvc.Extension
{
    public static class FileUploadHtmlHelper
    {
        internal readonly static string TempFileSavePath = "~/App_Data/UploadFiles/";

        public readonly static string FileStorePath = Path.Combine(SystemConfig.FileStoreRootPath, "UploadFiles");

        internal static readonly string[] ext_doc = new string[] { ".doc", ".docx" };
        internal static readonly string[] ext_xls = new string[] { ".xls", ".xlsx" };
        internal static readonly string[] ext_ppt = new string[] { ".ppt", ".pptx" };
        internal static readonly string[] ext_zip = new string[] { ".zip", ".rar" };
        internal static readonly string[] ext_pdf = new string[] { ".pdf" };
        internal static readonly string[] ext_pic = new string[] { ".jpg", ".png", ".jpeg", ".gif", ".ico" };

        public readonly static int defaultMinFilesCount = 0;
        public readonly static int defaultMaxFilesCount = 30;
        public readonly static int defaultMaxFileSizeMB = 10;
        public readonly static int defaultMaxTotalFileSizeMB = 30;

        public static string GetFileClass(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename);
            if (ext_doc.Contains(ext))
            {
                return "f_doc";
            }
            else if (ext_xls.Contains(ext))
            {
                return "f_xls";
            }
            else if (ext_ppt.Contains(ext))
            {
                return "f_ppt";
            }
            else if (ext_zip.Contains(ext))
            {
                return "f_zip";
            }
            else if (ext_pdf.Contains(ext))
            {
                return "f_pdf";
            }
            else if (ext_pic.Contains(ext))
            {
                return "f_pic";
            }
            else
            {
                return "f_file";
            }
        }

        public static string GetFileSizeFormat(long filesize)
        {
            if (filesize == 0) return "0kB";
            int i = -1;
            double bytes = filesize;
            do
            {
                bytes = bytes / 1024;
                i++;
            } while (bytes > 99);

            return Math.Round(Math.Max(bytes, 0.1), 1).ToString() + (new string[] { "kB", "MB", "GB", "TB", "PB", "EB" })[i];
        }

        private static double ChinaRound(double value, int decimals)
        {
            if (value < 0)
            {
                return Math.Round(value + 5 / Math.Pow(10, decimals + 1), decimals, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
            }
        }

        public static MvcHtmlString FileUpload<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression, string text = "添加文件", bool enabled = true
            , FileUploadDisplayType displayModel = FileUploadDisplayType.Edit)
            where TProperty : FileUploadModel
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            FileUploadModel model = metadata.Model as FileUploadModel;

            if (model == null)
            {
                throw new Exception(string.Format("{0}未初始化。", metadata.PropertyName));
            }

            if (model.Folder == null || model.Folder == Guid.Empty)
            {
                model.Folder = Guid.NewGuid();
            }

            FileUploadConfigDTO _config = null;
            int MinFilesCount = defaultMinFilesCount;
            int MaxFilesCount = defaultMaxFilesCount;
            int MaxFileSizeMB = defaultMaxFileSizeMB;
            int MaxTotalFileSizeMB = defaultMaxTotalFileSizeMB;
            //string ExcludeFileExtensions = string.Empty;//全局配置中才有
            string IncludeFileExtensions = string.Empty;
            string Regex = string.Empty;
            string RegexMessage = string.Empty;
            string[] MustFiles = null;
            FileUploadValidateAttribute fatt = Helper.GetAttribute<FileUploadValidateAttribute>(metadata.ContainerType, metadata.PropertyName);//GetFileUploadValidateAttribute(metadata.ContainerType, metadata.PropertyName);
            if (fatt != null)
            {
                MinFilesCount = fatt.MinFilesCount;
                MaxFilesCount = fatt.MaxFilesCount;
                MaxFileSizeMB = fatt.MaxFileSizeMB;
                MaxTotalFileSizeMB = fatt.MaxTotalFileSizeMB;
                IncludeFileExtensions = fatt.IncludeFileExtensions;
                Regex = fatt.Regex;
                RegexMessage = fatt.RegexMessage;
                MustFiles = fatt.MustFiles;
                FileUploadManager manager = new FileUploadManager();
                _config = manager.GetConfigByName(fatt.ConfigName);
            }
            if (_config != null)
            {
                MaxFileSizeMB = _config.SingleFileMaxSize;
                MaxTotalFileSizeMB = _config.AllFileMaxSize;
                MinFilesCount = _config.MinFileCount;
                MaxFilesCount = _config.MaxFileCount;
                IncludeFileExtensions = _config.FileExtensions_Include;
                Regex = _config.Regex;
                RegexMessage = _config.RegexMessage;
                if (!string.IsNullOrEmpty(_config.MustFiles))
                {
                    MustFiles = _config.MustFiles.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            RequiredAttribute reqAtt = Helper.GetAttribute<RequiredAttribute>(metadata.ContainerType, metadata.PropertyName); //GetRequiredAttribute(metadata.ContainerType, metadata.PropertyName);
            DisplayAttribute displayAtt = Helper.GetAttribute<DisplayAttribute>(metadata.ContainerType, metadata.PropertyName);//GetDisplayAttribute(metadata.ContainerType, metadata.PropertyName);
            string fieldName = "";
            if (displayAtt != null)
            {
                fieldName = displayAtt.Name;
            }

            string htmlId = string.Format("{0}-{1}", metadata.ContainerType.Name, metadata.PropertyName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\r\n<div>");
            sb.AppendFormat("<input type='hidden' id='{0}' name='{0}' value='{1}'", htmlId, model.Folder);

            if (displayModel == FileUploadDisplayType.Edit && enabled)
            {
                sb.Append(" data-val='true'");
                if (reqAtt != null || MinFilesCount > 0)
                {
                    int count = 1;
                    if (MinFilesCount > 0)
                    {
                        count = MinFilesCount;
                    }
                    sb.AppendFormat(" data-val-fuRequired='{0}至少要上传{1}个文件!' MinFilesCount='{1}'", fieldName, count);
                }
                if (MaxFilesCount > 0)
                {
                    sb.AppendFormat(" data-val-fuMaxFilesCount='{0}最多上传{1}个文件!' MaxFilesCount='{1}'", fieldName, MaxFilesCount);
                }

                if (MustFiles != null && MustFiles.Length > 0)
                {
                    string tt = "[]";
                    tt = "['" + string.Join("','", (from p in MustFiles select p.Trim().Replace("*", ".*"))) + "']";
                    if (tt.Length > 2)
                    {
                        sb.AppendFormat(" data-val-fuMustFiles=\"必须包含以下文件：{0}\" MustFiles=\"{1}\"", string.Join("\r\n", MustFiles), tt);
                    }
                }
            }

            sb.Append(" />");

            sb.AppendFormat("<div id='{0}-upload'></div>", htmlId);
            sb.AppendFormat("<ul id='{0}-upload-list' class='qq-upload-list'>", htmlId);

            var files = model.Files;//FileUploadController.GetFiles(model.Folder);
            foreach (var item in files)
            {
                if (enabled && displayModel == FileUploadDisplayType.Edit)
                {
                    sb.AppendFormat("<li class=' qq-upload-success'><span class='{5} qq-upload-file'>{0}</span><span class='qq-upload-size' style='display: inline;'>{1}</span><span class='qq-upload-cancel-s' onclick=\"EFDeleteUploadFile(this, '{4}', '{2}', '{0}', '{3}')\"></span></li>"
                        , item.FileName, item.FileSizeFormat, model.Folder, item.ID
                        , VirtualPathUtility.ToAbsolute("~/__fileuploader/DeleteFile")
                        , item.FileClass);
                }
                else
                {
                    string downloadPath = "";
                    if (item.ID > 0)
                    {
                        downloadPath = string.Format("{0}?id={1}"
                            , VirtualPathUtility.ToAbsolute("~/__fileuploader/Download"), item.ID);
                    }
                    else
                    {
                        downloadPath = string.Format("{0}?id={1}&fn={2}&folder={3}"
                            , VirtualPathUtility.ToAbsolute("~/__fileuploader/Download"), item.ID
                            , item.FileName, model.Folder);
                    }

                    sb.AppendFormat("<li class=' qq-upload-success'><span><a class='blue1 {2}' href='{3}' target='_blank'>{0}</a> <span>({1})</span></span></li>"
                        , item.FileName, item.FileSizeFormat, item.FileClass
                        , downloadPath);
                }
            }

            sb.Append("</ul>");

            sb.AppendLine("</div>");

            if (enabled && displayModel == FileUploadDisplayType.Edit)
            {
                sb.AppendLine("<script type='text/javascript'>");
                sb.AppendLine("$(function () {");

                sb.Append("EFConfigInitUploader({");
                sb.AppendFormat("buttonId:'{0}', buttonText:'{1}', uploadHandler:'{2}', deleteHandler:'{3}', fileSizeLimit:{4}, totalFileSizeLimit:{5}"
                    , htmlId
                    , text
                    , VirtualPathUtility.ToAbsolute("~/__fileuploader/Upload")
                    , VirtualPathUtility.ToAbsolute("~/__fileuploader/DeleteFile")
                    , (MaxFileSizeMB > 0) ? MaxFileSizeMB * 1024 * 1024 : 0
                    , (MaxTotalFileSizeMB > 0) ? MaxTotalFileSizeMB * 1024 * 1024 : 0);
                sb.AppendFormat(", regex:'{0}', regexMessage:'{1}'", Regex, RegexMessage);

                string inc = "[]";
                if (!string.IsNullOrEmpty(IncludeFileExtensions))
                {
                    string[] a = IncludeFileExtensions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (a != null && a.Length > 0)
                    {
                        inc = "['" + string.Join("','", (from p in a select p.Trim())) + "']";
                    }
                }
                sb.AppendFormat(", include:{0}", inc);

                sb.Append(", exclude:[]");
                sb.Append("});");

                sb.AppendLine("});");
                sb.AppendLine("</script>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString FileUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression)
            where TProperty : FileUploadModel
        {
            return htmlHelper.FileUploadValidationMessageFor(expression, null, null);
        }

        public static MvcHtmlString FileUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression, string validationMessage)
            where TProperty : FileUploadModel
        {
            return htmlHelper.FileUploadValidationMessageFor(expression, validationMessage, null);
        }

        public static MvcHtmlString FileUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes)
            where TProperty : FileUploadModel
        {
            return htmlHelper.FileUploadValidationMessageFor(expression, validationMessage, htmlAttributes, null);
        }

        public static MvcHtmlString FileUploadValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes, string tag)
            where TProperty : FileUploadModel
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            FileUploadModel model = metadata.Model as FileUploadModel;

            if (model == null)
            {
                throw new Exception(string.Format("{0}未初始化。", metadata.PropertyName));
            }

            string htmlId = string.Format("{0}-{1}", metadata.ContainerType.Name, metadata.PropertyName);

            return htmlHelper.ValidationMessage(htmlId, validationMessage, htmlAttributes, tag);
        }

    }
}
