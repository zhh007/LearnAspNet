using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PicUploadValidateAttribute : Attribute
    {
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

        public PicUploadValidateAttribute()
        {

        }
    }
}
