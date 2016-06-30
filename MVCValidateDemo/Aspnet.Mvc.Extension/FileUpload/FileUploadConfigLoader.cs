using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Aspnet.Mvc.Extension
{
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

    public class FileUploadConfigLoader
    {
        private static readonly string EFormConnectionString = ConfigurationManager.ConnectionStrings["ENTERPRISES_EFormContext"].ConnectionString;
        
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
            using (SqlConnection conn = new SqlConnection(EFormConnectionString))
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
            using (SqlConnection conn = new SqlConnection(EFormConnectionString))
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
            using (SqlConnection conn = new SqlConnection(EFormConnectionString))
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
}
