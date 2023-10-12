USE [SampleApp]
GO

/****** Object:  Table [dbo].[Order]    Script Date: 12.10.2023 14:22:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Order](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[CustomerId] [bigint] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO

ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO


