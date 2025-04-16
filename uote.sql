USE [DB_ArchivioPratiche]
GO

/****** Object:  Table [dbo].[ArchivioUote]    Script Date: 31/03/2025 12:53:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArchivioUote]') AND type in (N'U'))
DROP TABLE [dbo].[ArchivioUote]
GO

/****** Object:  Table [dbo].[ArchivioUote]    Script Date: 31/03/2025 12:53:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ArchivioUote](
	[id_Archivio] [bigint] NOT NULL,
	[arch_numPratica] [int] NOT NULL,
	[arch_doppione] [nvarchar](50) NULL,
	[arch_dataIns] [datetime2](7) NOT NULL,
	[arch_datault_intervento] [datetime2](7) NULL,
	[arch_indirizzo] [nvarchar](100) NULL,
	[arch_responsabile] [nvarchar](150) NULL,
	[arch_natoA] [nvarchar](50) NULL,
	[arch_dataNascita] [datetime2](7) NULL,
	[arch_inCarico] [varchar](max) NULL,
	[arch_evasa] [bit] NULL,
	[arch_note] [varchar](max) NULL,
	[arch_tipologia] [varchar](max) NULL,
	[arch_quartiere] [varchar](max) NULL,
	[Suolo_Pubblico] [bit] NULL,
	[arch_vincoli] [bit] NULL,
	[arch_1089] [bit] NULL,
	[arch_demolita] [bit] NULL,
	[arch_allegati] [varchar](max) NULL,
	[arch_matricola] [varchar](50) NULL,
	[arch_sezione] [varchar](50) NULL,
	[arch_foglio] [varchar](50) NULL,
	[arch_particella] [varchar](50) NULL,
	[arch_sub] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


