using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Aspnet.Mvc.Extension
{
    public class FileUploadComponentModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            var modelType = model.GetType();


            foreach (var property in modelType.GetProperties())
            {
                if (property.PropertyType == typeof(FileUploadComponentModel))
                {
                    FileUploadComponentModel o = property.GetValue(model, null) as FileUploadComponentModel;
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
                        o.Files = FileUploadHelper.GetFiles(o.Folder);

                        FileUploadConfigDTO _config = null;
                        FileUploadConfigDTO _gConfig = null;
                        int MinFilesCount = FileUploadHelper.defaultMinFilesCount;
                        int MaxFilesCount = FileUploadHelper.defaultMaxFilesCount;
                        int MaxFileSizeMB = FileUploadHelper.defaultMaxFileSizeMB;
                        int MaxTotalFileSizeMB = FileUploadHelper.defaultMaxTotalFileSizeMB;
                        string ExcludeFileExtensions = string.Empty;//全局配置中才有
                        string IncludeFileExtensions = string.Empty;
                        string Regex = string.Empty;
                        string RegexMessage = string.Empty;
                        string[] MustFiles = null;
                        FileUploadValidateAttribute fatt = FileUploadHelper.GetFileUploadValidateAttribute(modelType, property.Name);
                        FileUploadConfigLoader cfgLoader = new FileUploadConfigLoader();
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
                            _config = cfgLoader.GetConfigByName(fatt.ConfigName);
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
                        _gConfig = cfgLoader.GetGlobalConfig();
                        if (_gConfig != null)
                        {
                            MaxFileSizeMB = cfgLoader.MergeGolbalMaxFileSizeMB(_gConfig, MaxFileSizeMB);
                            MaxTotalFileSizeMB = cfgLoader.MergeGolbalMaxTotalFileSizeMB(_gConfig, MaxTotalFileSizeMB);
                            ExcludeFileExtensions = _gConfig.FileExtensions_Exclude;
                        }

                        RequiredAttribute reqAtt = FileUploadHelper.GetRequiredAttribute(modelType, property.Name);
                        DisplayAttribute displayAtt = FileUploadHelper.GetDisplayAttribute(modelType, property.Name);
                        string fieldName = "";
                        if (displayAtt != null)
                        {
                            fieldName = displayAtt.Name;
                        }

                        if (MinFilesCount < 1 && reqAtt != null)
                        {
                            MinFilesCount = 1;
                        }

                        if (MinFilesCount > 0 && (o.Files == null || o.Files.Count < MinFilesCount))
                        {
                            modelState.Errors.Add(string.Format("{0}至少要上传{1}个文件!", fieldName, MinFilesCount));
                        }

                        if (MaxFilesCount > 0 && o.Files != null && o.Files.Count > MaxFilesCount)
                        {
                            modelState.Errors.Add(string.Format("{0}最多上传{1}个文件!", fieldName, MaxFilesCount));
                        }

                        if (o.Files != null && o.Files.Count > 0)
                        {
                            if (MaxFileSizeMB > 0)
                            {
                                foreach (var file in o.Files)
                                {
                                    if (file.FileSize > MaxFileSizeMB * 1024 * 1024)
                                    {
                                        modelState.Errors.Add(string.Format("{0} 文件太大, 最大支持 {1}。", file.FileName, FileUploadHelper.GetFileSizeFormat(MaxFileSizeMB)));
                                    }
                                }
                            }

                            if (MaxTotalFileSizeMB > 0)
                            {
                                long total = 0;
                                foreach (var file in o.Files)
                                {
                                    total += file.FileSize;
                                }
                                if (total > MaxTotalFileSizeMB * 1024 * 1024)
                                {
                                    modelState.Errors.Add(string.Format("{0} 总文件大小不能超过 {1}。", fieldName, FileUploadHelper.GetFileSizeFormat(MaxTotalFileSizeMB)));
                                }
                            }

                            //全局配置禁止上传的文件扩展名
                            if (!string.IsNullOrEmpty(ExcludeFileExtensions))
                            {
                                List<string> extList = new List<string>();
                                string[] a = ExcludeFileExtensions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                if (a != null && a.Length > 0)
                                {
                                    extList = (from p in a select p.ToLower().Trim()).ToList();
                                }
                                if (extList != null && extList.Count > 0)
                                {
                                    foreach (var file in o.Files)
                                    {
                                        string t = file.Extension.ToLower().Trim().TrimStart(new char[] { '.' });
                                        if (extList.Contains(t))
                                        {
                                            modelState.Errors.Add(string.Format("{0} 该扩展名不允许上传。", file.FileName));
                                        }
                                    }
                                }
                            }

                            //允许的扩展名
                            if (!string.IsNullOrEmpty(IncludeFileExtensions))
                            {
                                List<string> extList = new List<string>();
                                string[] a = IncludeFileExtensions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                if (a != null && a.Length > 0)
                                {
                                    extList = (from p in a select p.ToLower().Trim()).ToList();
                                }
                                if (extList != null && extList.Count > 0)
                                {
                                    foreach (var file in o.Files)
                                    {
                                        string t = file.Extension.ToLower().Trim().TrimStart(new char[] { '.' });
                                        if (!extList.Contains(t))
                                        {
                                            modelState.Errors.Add(string.Format("文件扩展名必须为: {0}。", IncludeFileExtensions));
                                            break;
                                        }
                                    }
                                }
                            }

                            //正则验证
                            if (!string.IsNullOrEmpty(Regex))
                            {
                                foreach (var file in o.Files)
                                {
                                    try
                                    {
                                        if (!System.Text.RegularExpressions.Regex.IsMatch(file.FileName, Regex, RegexOptions.Singleline))
                                        {
                                            if (string.IsNullOrEmpty(RegexMessage))
                                            {
                                                RegexMessage = "正则验证失败。";
                                            }
                                            modelState.Errors.Add(string.Format("{0} {1}", file.FileName, RegexMessage));
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.Error("正则验证出错, " + Regex, ex);
                                    }
                                }
                            }

                            //必须包含以下文件
                            if (MustFiles != null && MustFiles.Length > 0)
                            {
                                foreach (var item in MustFiles)
                                {
                                    bool check = false;
                                    foreach (var file in o.Files)
                                    {
                                        try
                                        {
                                            if (System.Text.RegularExpressions.Regex.IsMatch(file.FileName, item.Trim().Replace("*", ".*"), RegexOptions.Singleline))
                                            {
                                                check = true;
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.Error("正则验证出错, " + Regex, ex);
                                        }
                                    }
                                    if (!check)
                                    {
                                        modelState.Errors.Add(string.Format("必须包含以下文件：{0}", string.Join("\r\n", MustFiles)));
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return model;
        }
    }
}
