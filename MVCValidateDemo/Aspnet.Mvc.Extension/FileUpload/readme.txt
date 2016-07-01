
/****** Object:  Table [dbo].[ConfigFileUploader]    Script Date: 07/01/2016 11:24:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ConfigFileUploader](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SingleFileMaxSize] [int] NOT NULL,
	[AllFileMaxSize] [int] NOT NULL,
	[MaxFileCount] [int] NOT NULL,
	[MinFileCount] [int] NOT NULL,
	[FileExtensions_Include] [nvarchar](max) NULL,
	[FileExtensions_Exclude] [nvarchar](max) NULL,
	[Regex] [nvarchar](max) NULL,
	[RegexMessage] [nvarchar](max) NULL,
	[DeleteFlag] [bit] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateUserName] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [uniqueidentifier] NOT NULL,
	[UpdateUserName] [nvarchar](50) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[MustFiles] [nvarchar](max) NULL,
	[GlobalFlag] [bit] NOT NULL,
 CONSTRAINT [PK_ConfigFileUploader] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单一文档大小（MB）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'SingleFileMaxSize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总文档大小（MB）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'AllFileMaxSize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最多文档数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'MaxFileCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最少文档数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'MinFileCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允许的文件扩展名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'FileExtensions_Include'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除的文件扩展名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'FileExtensions_Exclude'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'正则表达式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'Regex'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未通过正则验证的提示信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'RegexMessage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除（默认0）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'DeleteFlag'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'UpdateUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'必须包含以下文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'MustFiles'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'全局标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigFileUploader', @level2type=N'COLUMN',@level2name=N'GlobalFlag'
GO


/****** Object:  Table [dbo].[WorkflowFiles]    Script Date: 07/01/2016 11:26:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WorkflowFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](200) NOT NULL,
	[FileSize] [int] NOT NULL,
	[Extension] [nvarchar](50) NOT NULL,
	[StorePath] [nvarchar](500) NOT NULL,
	[CreatorID] [uniqueidentifier] NOT NULL,
	[CreatorName] [nvarchar](50) NOT NULL,
	[CreatorCode] [nvarchar](50) NOT NULL,
	[FolderID] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Wo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


