USE [DB_ArchivioPratiche]
GO

/****** Object:  Table [dbo].[PRINCIPALE]    Script Date: 14/03/2025 06:59:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRINCIPALE](
	[Nr_Protocollo] [int] NULL,
	[Sigla] [nvarchar](50) NULL,
	[DataArrivo] [datetime] NULL,
	[Provenienza] [nvarchar](max) NULL,
	[Tipologia_atto] [nvarchar](max) NULL,
	[Giudice] [nvarchar](50) NULL,
	[TipoProvvedimentoAG] [nvarchar](max) NULL,
	[ProcedimentoPen] [nvarchar](50) NULL,
	[Nominativo] [nvarchar](max) NULL,
	[Indirizzo] [nvarchar](max) NULL,
	[via] [nvarchar](max) NULL,
	[Evasa] [bit] NULL,
	[EvasaData] [datetime] NULL,
	[Inviata] [nvarchar](50) NULL,
	[DataInvio] [datetime] NULL,
	[Scaturito] [nvarchar](50) NULL,
	[Accertatori] [nvarchar](max) NULL,
	[DataCarico] [datetime] NULL,
	[nr_Pratica] [nvarchar](50) NULL,
	[Quartiere] [nvarchar](50) NULL,
	[Note] [nvarchar](max) NULL,
	[Anno] [nvarchar](50) NULL,
	[Giorno] [nvarchar](50) NULL,
	[Rif_Prot_Gen] [nvarchar](50) NULL,
	[matricola] [nchar](10) NULL,
	[datainserimento] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


