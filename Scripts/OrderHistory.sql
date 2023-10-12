USE [SampleApp]
GO

/****** Object:  Table [dbo].[OrderHistory]    Script Date: 12.10.2023 14:22:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[Changes] [nvarchar](150) NOT NULL,
	[ChangeDate] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_OrderHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrderHistory_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO

ALTER TABLE [dbo].[OrderHistory] CHECK CONSTRAINT [FK_OrderHistory_Order]
GO


