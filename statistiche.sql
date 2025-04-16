USE [DB_ArchivioPratiche]
GO

/****** Object:  Table [dbo].[statistiche]    Script Date: 24/03/2025 21:27:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[statistiche](
	[id_statisctiche] [bigint] IDENTITY(1,1) NOT NULL,
	[mese] [nchar](10) NOT NULL,
	[anno] [int] NOT NULL,
	[relazioni] [bigint] NULL,
	[ponteggi] [bigint] NULL,
	[dpi] [bigint] NULL,
	[esposti_ricevuti] [bigint] NULL,
	[esposti_evasi] [bigint] NULL,
	[ripristino_tot_par] [bigint] NULL,
	[controlli_scia] [bigint] NULL,
	[contr_cant_daily] [bigint] NULL,
	[cnr] [bigint] NULL,
	[annotazioni] [bigint] NULL,
	[notifiche] [bigint] NULL,
	[sequestri] [bigint] NULL,
	[riapp_sigilli] [bigint] NULL,
	[deleghe_ricevute] [bigint] NULL,
	[deleghe_esitate] [bigint] NULL,
	[cnr_annotazioni] [bigint] NULL,
	[interrogazioni] [bigint] NULL,
	[denunce_uff] [nchar](10) NULL,
	[convalide] [bigint] NULL,
	[demolizioni] [bigint] NULL,
	[violazione_sigilli] [bigint] NULL,
	[dissequestri] [bigint] NULL,
	[dissequestri_temp] [bigint] NULL,
	[rimozione_sigilli] [bigint] NULL,
	[controlli_42_04] [bigint] NULL,
	[contr_cant_suolo_pubb] [bigint] NULL,
	[contr_lavori_edili] [bigint] NULL,
	[contr_cant] [bigint] NULL,
	[contr_nato_da_esposti] [bigint] NULL,
 CONSTRAINT [PK_statistiche] PRIMARY KEY CLUSTERED 
(
	[id_statisctiche] ASC,
	[mese] ASC,
	[anno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


