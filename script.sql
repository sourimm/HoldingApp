USE [Plaid]
GO
/****** Object:  Table [dbo].[tblInstance]    Script Date: 3/1/2021 12:14:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblInstance](
	[ConnectionId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[InstanceId] [nchar](100) NOT NULL,
	[InstanceName] [nchar](200) NOT NULL,
	[PublicToken] [nchar](200) NOT NULL,
 CONSTRAINT [PK_tblInstance] PRIMARY KEY CLUSTERED 
(
	[ConnectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 3/1/2021 12:14:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginId] [nchar](100) NOT NULL,
	[Password] [nchar](100) NOT NULL,
	[FName] [nchar](200) NOT NULL,
	[LName] [nchar](200) NOT NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [Plaid] SET  READ_WRITE 
GO
