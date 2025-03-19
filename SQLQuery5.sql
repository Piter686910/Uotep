USE [DB_ArchivioPratiche]
GO

/****** Object:  Table [dbo].[PRINCIPALE1]    Script Date: 14/03/2025 07:00:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRINCIPALE1](
	[Nr_Protocollo] [int] NULL,
	[Sigla] [varchar](50) NULL,
	[DataArrivo] [datetime] NULL,
	[Provenienza] [varchar](max) NULL,
	[Tipologia_atto] [varchar](max) NULL,
	[Giudice] [varchar](50) NULL,
	[TipoProvvedimentoAG] [varchar](max) NULL,
	[ProcedimentoPen] [varchar](50) NULL,
	[Nominativo] [varchar](max) NULL,
	[Indirizzo] [varchar](max) NULL,
	[Evasa] [bit] NULL,
	[EvasaData] [datetime] NULL,
	[Inviata] [varchar](50) NULL,
	[DataInvio] [datetime] NULL,
	[Scaturito] [varchar](50) NULL,
	[Accertatori] [varchar](max) NULL,
	[DataCarico] [datetime] NULL,
	[nr_Pratica] [varchar](50) NULL,
	[Quartiere] [varchar](50) NULL,
	[Note] [varchar](max) NULL,
	[Anno] [varchar](50) NULL,
	[Giorno] [varchar](50) NULL,
	[Rif_Prot_Gen] [varchar](50) NULL,
	[Matricola] [varchar](50) NULL,
	[DataInserimento] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


