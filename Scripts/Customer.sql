USE [SampleApp]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 12.10.2023 14:22:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Number] [int] NOT NULL,
	[CustomValidationField] [nvarchar](100) NOT NULL,
	[PhotoString] [nvarchar](max) NULL,
	[PhotoBinary] [varbinary](max) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


