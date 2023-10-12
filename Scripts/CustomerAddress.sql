USE [SampleApp]
GO

/****** Object:  Table [dbo].[CustomersAddress]    Script Date: 12.10.2023 14:38:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomersAddress](
	[CustomersId] [bigint] NOT NULL,
	[AddressId] [bigint] NOT NULL,
 CONSTRAINT [PK_CustomersAddress] PRIMARY KEY CLUSTERED 
(
	[CustomersId] ASC,
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CustomersAddress]  WITH CHECK ADD  CONSTRAINT [FK_CustomersAddress_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
GO

ALTER TABLE [dbo].[CustomersAddress] CHECK CONSTRAINT [FK_CustomersAddress_Address]
GO

ALTER TABLE [dbo].[CustomersAddress]  WITH CHECK ADD  CONSTRAINT [FK_CustomersAddress_Customer] FOREIGN KEY([CustomersId])
REFERENCES [dbo].[Customer] ([Id])
GO

ALTER TABLE [dbo].[CustomersAddress] CHECK CONSTRAINT [FK_CustomersAddress_Customer]
GO


