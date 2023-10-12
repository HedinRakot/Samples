USE [SampleApp]
GO

/****** Object:  Table [dbo].[Address]    Script Date: 12.10.2023 14:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Address](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Street] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Housenumber] [nvarchar](50) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


