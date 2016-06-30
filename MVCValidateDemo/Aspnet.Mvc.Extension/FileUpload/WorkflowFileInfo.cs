using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspnet.Mvc.Extension
{
    public class WorkflowFileInfo
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string Extension { get; set; }
        public string StorePath { get; set; }
        public Guid CreatorID { get; set; }
        public string CreatorName { get; set; }
        public string CreatorCode { get; set; }
        public WorkflowFileType Type { get; set; }
        public WorkflowFileStatus FileStatus { get; set; }
        public Guid FolderID { get; set; }
        public DateTime CreateTime { get; set; }

        public string FileSizeFormat
        {
            get
            {
                return FileUploadHelper.GetFileSizeFormat(this.FileSize);
            }
        }

        public string FileClass
        {
            get
            {
                return FileUploadHelper.GetFileClass(this.FileName);
            }
        }
    }

    public enum WorkflowFileType
    {
        None = 0,
        /// <summary>
        /// 来自代办事项
        /// </summary>
        FromTodo = 1,
        /// <summary>
        /// 来自反馈
        /// </summary>
        FromFeedback = 2,
        /// <summary>
        /// 来自消息中心
        /// </summary>
        FromMessage = 3,
        /// <summary>
        /// 来自工作流
        /// </summary>
        FromWorkflow = 4
    }

    public enum WorkflowFileStatus
    {
        /// <summary>
        /// 未提交
        /// </summary>
        UnSubmit = 0,
        /// <summary>
        /// 已提交
        /// </summary>
        Submit = 1,
    }
}
