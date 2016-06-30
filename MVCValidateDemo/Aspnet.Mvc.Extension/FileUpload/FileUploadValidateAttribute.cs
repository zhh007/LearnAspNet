using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspnet.Mvc.Extension
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileUploadValidateAttribute : Attribute
    {
        public FileUploadValidateAttribute()
        {
            MaxTotalFileSizeMB = FileUploadHelper.defaultMaxTotalFileSizeMB;
            MaxFileSizeMB = FileUploadHelper.defaultMaxFileSizeMB;//5MB
            MinFilesCount = FileUploadHelper.defaultMinFilesCount;
            MaxFilesCount = FileUploadHelper.defaultMaxFilesCount;
        }

        /// <summary>
        /// 总文件大小（MB）
        /// </summary>
        public int MaxTotalFileSizeMB { get; set; }

        /// <summary>
        /// 单文件大小（MB）
        /// </summary>
        public int MaxFileSizeMB { get; set; }

        /// <summary>
        /// 至少上传几个文件
        /// </summary>
        public int MinFilesCount { get; set; }

        /// <summary>
        /// 至多上传几个文件
        /// </summary>
        public int MaxFilesCount { get; set; }

        /// <summary>
        /// 允许的扩展名，如zip,rar,png
        /// </summary>
        public string IncludeFileExtensions { get; set; }
        
        /// <summary>
        /// 正则验证
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// 正则验证提示消息
        /// </summary>
        public string RegexMessage { get; set; }

        /// <summary>
        /// 必须包含的文件，如身份证*，户口本*
        /// </summary>
        public string[] MustFiles { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName { get; set; }
    }
}
