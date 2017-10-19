CREATE TABLE [dbo].[Books] (
    [BookID]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Author]      NVARCHAR (100) NOT NULL,
    [Publishing]  NVARCHAR (100) NOT NULL,
    [Year]        INT            NOT NULL,
    [Image]       NVARCHAR (100) NULL,
    [Rating]      INT            NOT NULL,
    [Description] NVARCHAR (500) NOT NULL,
    [Category]    NVARCHAR (50)  NOT NULL,
	[ImageData]		VARBINARY(MAX)	NULL,
	[ImageMimeType]	VARCHAR(50)		NULL
    PRIMARY KEY CLUSTERED ([BookID] ASC)
);

