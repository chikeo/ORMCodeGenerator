/****** Object:  Table [dbo].[tblTest]    Script Date: 01/21/2009 11:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTest](
	[TestName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[TestDate] [datetime] NOT NULL,
	[TestDescription] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_tblTest] PRIMARY KEY CLUSTERED 
(
	[TestName] ASC,
	[TestDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
