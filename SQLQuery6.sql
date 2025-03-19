USE [DB_ArchivioPratiche]
GO

/****** Object:  Table [dbo].[PRINCIPALE2]    Script Date: 14/03/2025 07:00:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRINCIPALE2](
	[Nr_Protocollo] [bigint] NULL,
	[Sigla] [varchar](50) NOT NULL,
	[DataArrivo] [varchar](50) NULL,
	[Provenienza] [varchar](50) NULL,
	[Tipologia_atto] [varchar](50) NULL,
	[Giudice] [varchar](50) NULL,
	[TipoProvvedimentoAG] [varchar](50) NULL,
	[ProcedimentoPen] [varchar](50) NULL,
	[Nominativo] [varchar](50) NULL,
	[Indirizzo] [varchar](50) NULL,
	[via] [varchar](50) NULL,
	[Evasa] [bit] NULL,
	[EvasaData] [varchar](max) NULL,
	[Inviata] [varchar](50) NULL,
	[DataInvio] [varchar](50) NULL,
	[Scaturito] [varchar](50) NULL,
	[Accertatori] [varchar](50) NULL,
	[DataCarico] [varchar](50) NULL,
	[nr_Pratica] [varchar](50) NULL,
	[Quartiere] [varchar](50) NULL,
	[Note] [varchar](max) NULL,
	[Anno] [varchar](50) NULL,
	[Giorno] [varchar](50) NULL,
	[Rif_Prot_Gen] [varchar](50) NULL,
	[Matricola] [varchar](50) NULL,
	[DataInserimento] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


