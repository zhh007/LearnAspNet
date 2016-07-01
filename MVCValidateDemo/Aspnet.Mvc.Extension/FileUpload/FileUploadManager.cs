using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aspnet.Mvc.Extension
{
    public class FileUploadManager
    {
        public static void SaveFolder(Guid folder)
        {
            string savePath = System.Web.HttpContext.Current.Request.MapPath(FileUploadHtmlHelper.TempFileSavePath);
            string folderPath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            List<FileUploadItem> list = new List<FileUploadItem>();

            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();

                if (files.Length > 0)
                {
                    string userfilePath = FileUploadHtmlHelper.FileStorePath;
                    if (!Directory.Exists(userfilePath))
                    {
                        Directory.CreateDirectory(userfilePath);
                    }
                    string storePath = DateTime.Now.ToString("yyyy/MM/dd/").Replace("/", "\\") + folder.ToString() + "\\";
                    //var curUser = System.Web.HttpContext.Current.User.Identity.;

                    foreach (FileInfo file in files)
                    {
                        string targetFolder = Path.Combine(userfilePath, storePath);
                        if (!Directory.Exists(targetFolder))
                        {
                            Directory.CreateDirectory(targetFolder);
                        }

                        FileUploadItem info = new FileUploadItem();
                        info.FileName = file.Name;
                        info.FileSize = file.Length;
                        info.Extension = file.Extension;
                        //info.CreatorID = curUser.UserId;
                        //info.CreatorName = curUser.Name;
                        //info.CreatorCode = curUser.UserCode;
                        //info.Type = type;
                        //info.FileStatus = WorkflowFileStatus.Submit;
                        info.StorePath = storePath + file.Name;
                        info.FolderID = folder;
                        list.Add(info);

                        file.CopyTo(Path.Combine(userfilePath, info.StorePath), true);
                    }
                }
            }

            FileUploadManager.Save(list);

            if (dir.Exists)
            {
                dir.Delete(true);
            }
        }

        public static List<FileUploadItem> GetFilesFromDB(Guid folder)
        {
            List<FileUploadItem> list = new List<FileUploadItem>();

            string sql = @"
SELECT ID, FileName, FileSize, Extension, StorePath
FROM WorkflowFiles WITH (NOLOCK)
WHERE FolderID=@FolderID
";

            using (SqlConnection sqlConn = new SqlConnection(SystemConfig.DBLink))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlConn);

                SqlParameter para_FolderID = new SqlParameter("FolderID", SqlDbType.UniqueIdentifier);
                para_FolderID.Value = folder;
                cmd.Parameters.Add(para_FolderID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FileUploadItem info = new FileUploadItem();
                        info.ID = Convert.ToInt32(reader["ID"]);
                        info.FileName = reader["FileName"] as string;
                        info.FileSize = Convert.ToInt32(reader["FileSize"]);
                        info.Extension = reader["Extension"] as string;
                        info.StorePath = reader["StorePath"] as string;
                        info.FolderID = folder;
                        list.Add(info);
                    }
                }

                sqlConn.Close();
            }

            return list;
        }

        public static void Save(List<FileUploadItem> list)
        {
            if (list != null && list.Count > 0)
            {
                using (SqlConnection sqlConn = new SqlConnection(SystemConfig.DBLink))
                {
                    string sql = @"
INSERT INTO [dbo].[WorkflowFiles]
           ([FileName]
           ,[FileSize]
           ,[Extension]
           ,[StorePath]
           ,[CreatorID]
           ,[CreatorName]
           ,[CreatorCode]
           --,[Type]
           --,[FileStatus]
           ,[FolderID]
           ,[CreateTime])
     VALUES
           (@FileName
           ,@FileSize
           ,@Extension
           ,@StorePath
           ,@CreatorID
           ,@CreatorName
           ,@CreatorCode
           --,@Type
           --,@FileStatus
           ,@FolderID
           ,getdate());
";
                    sqlConn.Open();
                    SqlTransaction tran = sqlConn.BeginTransaction();
                    foreach (FileUploadItem item in list)
                    {
                        SqlCommand cmd = new SqlCommand(sql, sqlConn);
                        cmd.Transaction = tran;

                        SqlParameter para_FileName = new SqlParameter("FileName", SqlDbType.NVarChar);
                        para_FileName.Value = item.FileName;
                        cmd.Parameters.Add(para_FileName);

                        SqlParameter para_FileSize = new SqlParameter("FileSize", SqlDbType.Int);
                        para_FileSize.Value = item.FileSize;
                        cmd.Parameters.Add(para_FileSize);

                        SqlParameter para_Extension = new SqlParameter("Extension", SqlDbType.NVarChar);
                        para_Extension.Value = item.Extension;
                        cmd.Parameters.Add(para_Extension);

                        SqlParameter para_StorePath = new SqlParameter("StorePath", SqlDbType.NVarChar);
                        para_StorePath.Value = item.StorePath;
                        cmd.Parameters.Add(para_StorePath);

                        SqlParameter para_CreatorID = new SqlParameter("CreatorID", SqlDbType.UniqueIdentifier);
                        para_CreatorID.Value = item.CreatorID;
                        cmd.Parameters.Add(para_CreatorID);

                        SqlParameter para_CreatorName = new SqlParameter("CreatorName", SqlDbType.NVarChar);
                        para_CreatorName.Value = item.CreatorName;
                        cmd.Parameters.Add(para_CreatorName);

                        SqlParameter para_CreatorCode = new SqlParameter("CreatorCode", SqlDbType.NVarChar);
                        para_CreatorCode.Value = item.CreatorCode;
                        cmd.Parameters.Add(para_CreatorCode);

                        //SqlParameter para_Type = new SqlParameter("Type", SqlDbType.Int);
                        //para_Type.Value = item.Type;
                        //cmd.Parameters.Add(para_Type);

                        //SqlParameter para_FileStatus = new SqlParameter("FileStatus", SqlDbType.Int);
                        //para_FileStatus.Value = item.FileStatus;
                        //cmd.Parameters.Add(para_FileStatus);

                        SqlParameter para_FolderID = new SqlParameter("FolderID", SqlDbType.UniqueIdentifier);
                        para_FolderID.Value = item.FolderID;
                        cmd.Parameters.Add(para_FolderID);

                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    sqlConn.Close();
                }
            }
        }

        public static List<FileUploadItem> GetFiles(Guid folder)
        {
            List<FileUploadItem> list = new List<FileUploadItem>();

            string sql = @"
SELECT ID, FileName, FileSize, Extension
FROM WorkflowFiles WITH (NOLOCK)
WHERE FolderID=@FolderID
";

            using (SqlConnection sqlConn = new SqlConnection(SystemConfig.DBLink))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlConn);

                SqlParameter para_FolderID = new SqlParameter("FolderID", SqlDbType.UniqueIdentifier);
                para_FolderID.Value = folder;
                cmd.Parameters.Add(para_FolderID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FileUploadItem info = new FileUploadItem();
                        info.ID = Convert.ToInt32(reader["ID"]);
                        info.FileName = reader["FileName"] as string;
                        info.FileSize = Convert.ToInt32(reader["FileSize"]);
                        info.Extension = reader["Extension"] as string;
                        info.FolderID = folder;
                        list.Add(info);
                    }
                }

                sqlConn.Close();
            }

            string savePath = System.Web.HttpContext.Current.Request.MapPath(FileUploadHtmlHelper.TempFileSavePath);
            string folderPath = Path.Combine(savePath, folder.ToString());
            DirectoryInfo dir = new DirectoryInfo(folderPath);

            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();

                if (files.Length > 0)
                {
                    //var curUser = System.Web.HttpContext.Current.User.GetCurrentUser();

                    var items = (from p in files
                                 orderby p.CreationTime ascending
                                 select p);

                    foreach (FileInfo file in items)
                    {
                        FileUploadItem info = new FileUploadItem();
                        info.ID = -1;
                        info.FileName = file.Name;
                        info.FileSize = file.Length;
                        info.Extension = file.Extension;
                        info.FolderID = folder;
                        //info.CreatorID = curUser.UserId;
                        //info.CreatorName = curUser.Name;
                        //info.CreatorCode = curUser.UserCode;
                        //info.Type = WorkflowFileType.FromWorkflow;
                        //info.FileStatus = WorkflowFileStatus.Submit;
                        //info.StorePath = storePath + file.Name;
                        //info.FolderID = folder;
                        list.Add(info);

                    }
                }
            }

            return list;
        }

        public static void CopyFromFolder(Guid oldFolder, Guid newFolder)
        {
            if (oldFolder == Guid.Empty || newFolder == Guid.Empty)
            {
                return;
            }

            List<FileUploadItem> oldList = FileUploadManager.GetFilesFromDB(oldFolder);

            string userfilePath = FileUploadHtmlHelper.FileStorePath;
            if (!Directory.Exists(userfilePath))
            {
                Directory.CreateDirectory(userfilePath);
            }
            string storePath = DateTime.Now.ToString("yyyy/MM/dd/").Replace("/", "\\") + newFolder.ToString() + "\\";
            //var curUser = System.Web.HttpContext.Current.User.GetCurrentUser();

            List<FileUploadItem> newList = new List<FileUploadItem>();
            foreach (var file in oldList)
            {
                string targetFolder = Path.Combine(userfilePath, storePath);
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                FileUploadItem info = new FileUploadItem();
                info.FileName = file.FileName;
                info.FileSize = file.FileSize;
                info.Extension = file.Extension;
                //info.CreatorID = curUser.UserId;
                //info.CreatorName = curUser.Name;
                //info.CreatorCode = curUser.UserCode;
                //info.Type = WorkflowFileType.FromWorkflow;
                //info.FileStatus = WorkflowFileStatus.Submit;
                info.StorePath = storePath + file.FileName;
                info.FolderID = newFolder;
                newList.Add(info);

                System.IO.File.Copy(Path.Combine(userfilePath, file.StorePath), Path.Combine(userfilePath, info.StorePath), true);
            }

            FileUploadManager.Save(newList);
        }

        private FileUploadConfigDTO ReadData(SqlDataReader reader)
        {
            FileUploadConfigDTO dto = new FileUploadConfigDTO();

            if (reader["ID"] != DBNull.Value)
            {
                dto.ID = new Guid(reader["ID"].ToString());
            }

            if (reader["Name"] != DBNull.Value)
            {
                dto.Name = reader["Name"].ToString();
            }

            if (reader["SingleFileMaxSize"] != DBNull.Value)
            {
                dto.SingleFileMaxSize = Convert.ToInt32(reader["SingleFileMaxSize"]);
            }

            if (reader["AllFileMaxSize"] != DBNull.Value)
            {
                dto.AllFileMaxSize = Convert.ToInt32(reader["AllFileMaxSize"]);
            }

            if (reader["MaxFileCount"] != DBNull.Value)
            {
                dto.MaxFileCount = Convert.ToInt32(reader["MaxFileCount"]);
            }

            if (reader["MinFileCount"] != DBNull.Value)
            {
                dto.MinFileCount = Convert.ToInt32(reader["MinFileCount"]);
            }

            if (reader["FileExtensions_Include"] != DBNull.Value)
            {
                dto.FileExtensions_Include = reader["FileExtensions_Include"].ToString();
            }

            if (reader["FileExtensions_Exclude"] != DBNull.Value)
            {
                dto.FileExtensions_Exclude = reader["FileExtensions_Exclude"].ToString();
            }

            if (reader["Regex"] != DBNull.Value)
            {
                dto.Regex = reader["Regex"].ToString();
            }

            if (reader["RegexMessage"] != DBNull.Value)
            {
                dto.RegexMessage = reader["RegexMessage"].ToString();
            }

            if (reader["GlobalFlag"] != DBNull.Value)
            {
                dto.GlobalFlag = Convert.ToBoolean(reader["GlobalFlag"]);
            }

            if (reader["MustFiles"] != DBNull.Value)
            {
                dto.MustFiles = reader["MustFiles"].ToString();
            }
            return dto;
        }

        public FileUploadConfigDTO GetGlobalConfig()
        {
            FileUploadConfigDTO dto = null;
            string sql = @"
SELECT [ID]
      ,[Name]
      ,[SingleFileMaxSize]
      ,[AllFileMaxSize]
      ,[MaxFileCount]
      ,[MinFileCount]
      ,[FileExtensions_Include]
      ,[FileExtensions_Exclude]
      ,[Regex]
      ,[RegexMessage]
      ,[DeleteFlag]
      ,[MustFiles]
      ,[GlobalFlag]
  FROM [ConfigFileUploader] WITH (NOLOCK)
WHERE [DeleteFlag]=0 AND [GlobalFlag]=1
";
            using (SqlConnection conn = new SqlConnection(SystemConfig.DBLink))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dto = ReadData(reader);
                    }
                }
                conn.Close();
            }

            return dto;
        }

        public FileUploadConfigDTO GetConfigById(Guid cfgId)
        {
            if (cfgId == null || cfgId == Guid.Empty)
            {
                return null;
            }

            FileUploadConfigDTO dto = null;
            string sql = @"
SELECT [ID]
      ,[Name]
      ,[SingleFileMaxSize]
      ,[AllFileMaxSize]
      ,[MaxFileCount]
      ,[MinFileCount]
      ,[FileExtensions_Include]
      ,[FileExtensions_Exclude]
      ,[Regex]
      ,[RegexMessage]
      ,[DeleteFlag]
      ,[MustFiles]
      ,[GlobalFlag]
  FROM [ConfigFileUploader] WITH (NOLOCK)
WHERE [DeleteFlag]=0 and ID=@ID
";
            using (SqlConnection conn = new SqlConnection(SystemConfig.DBLink))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlParameter para_ID = new SqlParameter("@ID", SqlDbType.UniqueIdentifier);
                para_ID.Value = cfgId;
                cmd.Parameters.Add(para_ID);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dto = ReadData(reader);
                    }
                }
                conn.Close();
            }

            return dto;
        }

        public FileUploadConfigDTO GetConfigByName(string cfgName)
        {
            if (string.IsNullOrEmpty(cfgName))
            {
                return null;
            }

            FileUploadConfigDTO dto = null;
            string sql = @"
SELECT [ID]
      ,[Name]
      ,[SingleFileMaxSize]
      ,[AllFileMaxSize]
      ,[MaxFileCount]
      ,[MinFileCount]
      ,[FileExtensions_Include]
      ,[FileExtensions_Exclude]
      ,[Regex]
      ,[RegexMessage]
      ,[DeleteFlag]
      ,[MustFiles]
      ,[GlobalFlag]
  FROM [ConfigFileUploader] WITH (NOLOCK)
WHERE [DeleteFlag]=0 and (Name=@Name or [GlobalFlag]=1)
";
            using (SqlConnection conn = new SqlConnection(SystemConfig.DBLink))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlParameter para_Name = new SqlParameter("@Name", SqlDbType.NVarChar);
                para_Name.Value = cfgName;
                cmd.Parameters.Add(para_Name);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dto = ReadData(reader);
                    }
                }
                conn.Close();
            }
            return dto;
        }

        /// <summary>
        /// 单个文档最大不能超过(MB)
        /// </summary>
        /// <param name="g"></param>
        /// <param name="_maxFileSizeMB"></param>
        /// <returns></returns>
        public int MergeGolbalMaxFileSizeMB(FileUploadConfigDTO g, int _maxFileSizeMB)
        {
            if (g == null)
            {
                return _maxFileSizeMB;
            }

            int c = 0;
            if (g.SingleFileMaxSize > 0 && _maxFileSizeMB > 0)
            {
                c = Math.Min(g.SingleFileMaxSize, _maxFileSizeMB);
            }
            else
            {
                if (g.SingleFileMaxSize > 0)
                {
                    c = g.SingleFileMaxSize;
                }
                if (_maxFileSizeMB > 0)
                {
                    c = _maxFileSizeMB;
                }
            }

            return c;
        }

        /// <summary>
        /// 总文档大小大小不能超过(MB)
        /// </summary>
        /// <param name="g"></param>
        /// <param name="_maxTotalFileSizeMB"></param>
        /// <returns></returns>
        public int MergeGolbalMaxTotalFileSizeMB(FileUploadConfigDTO g, int _maxTotalFileSizeMB)
        {
            if (g == null)
            {
                return _maxTotalFileSizeMB;
            }

            int c = 0;
            if (g.AllFileMaxSize > 0 && _maxTotalFileSizeMB > 0)
            {
                c = Math.Min(g.AllFileMaxSize, _maxTotalFileSizeMB);
            }
            else
            {
                if (g.AllFileMaxSize > 0)
                {
                    c = g.AllFileMaxSize;
                }
                if (_maxTotalFileSizeMB > 0)
                {
                    c = _maxTotalFileSizeMB;
                }
            }

            return c;
        }

        public string GetGlobalConfigJSON()
        {
            FileUploadConfigDTO g = this.GetGlobalConfig();

            int SingleFileMaxSize = 0;
            int AllFileMaxSize = 0;
            string ExcludeFileExtensions = "[]";

            if (g != null)
            {
                if (g.SingleFileMaxSize > 0)
                {
                    SingleFileMaxSize = g.SingleFileMaxSize;
                }

                if (g.AllFileMaxSize > 0)
                {
                    AllFileMaxSize = g.AllFileMaxSize;
                }

                if (!string.IsNullOrEmpty(g.FileExtensions_Exclude))
                {
                    string[] a = g.FileExtensions_Exclude.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (a != null && a.Length > 0)
                    {
                        ExcludeFileExtensions = "['" + string.Join("','", (from p in a select p.Trim())) + "']";
                    }
                }
            }

            return string.Format("var __gUploadConfig = {{fileSizeLimit:{0}, totalFileSizeLimit:{1}, exclude:{2}}}", SingleFileMaxSize, AllFileMaxSize, ExcludeFileExtensions);
        }
    }

    [Serializable]
    public class FileUploadConfigDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int SingleFileMaxSize { get; set; }
        public int AllFileMaxSize { get; set; }
        public int MaxFileCount { get; set; }
        public int MinFileCount { get; set; }
        public string FileExtensions_Include { get; set; }
        public string FileExtensions_Exclude { get; set; }
        public string Regex { get; set; }
        public string RegexMessage { get; set; }
        public bool GlobalFlag { get; set; }
        public string MustFiles { get; set; }

        public FileUploadConfigDTO()
        {
            this.SingleFileMaxSize = 10;//MB
            this.AllFileMaxSize = 30;//MB
            this.MaxFileCount = 30;
            this.MinFileCount = 0;
        }
    }
}
