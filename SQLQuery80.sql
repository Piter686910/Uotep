
	[ID_giudice] [int] IDENTITY(1,1) NOT NULL,
	[id_provenienza] [int] IDENTITY(1,1) NOT NULL,
	[ID_quartiere] [int] IDENTITY(1,1) NOT NULL,
	[id_scaturito] [int] IDENTITY(1,1) NOT NULL,
	id_tipo_nota
	id_tipo_nota_ag

	--inviata
	USE [DB_ArchivioPratiche]
GO

/****** Object:  Table [dbo].[Inviati]    Script Date: 09/02/2025 20:02:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Inviati](
	[id_inviata] [int] IDENTITY(1,1) NOT NULL,
	[inviata] [varchar](max) NULL,
 CONSTRAINT [PK_Inviati] PRIMARY KEY CLUSTERED 
(
	[id_inviata] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
---------------------------------------


