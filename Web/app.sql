CREATE TABLE [dbo].[book] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [title]        VARCHAR (50)  NULL,
    [info]         VARCHAR (MAX) NULL,
    [bookquantity] INT           NULL,
    [price]        INT           NULL,
    [imgfile]      VARCHAR (50)  NULL,
    [cataid]       INT           NULL,
    [author]       VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[orders] (
    [id]        INT  IDENTITY (1, 1) NOT NULL,
    [bookId]    INT  NOT NULL,
    [userid]    INT  NULL,
    [quantity]  INT  NULL,
    [orderdate] DATE DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_orders] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[usersaccounts] (
    [id]   INT          IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (50) NULL,
    [pass] VARCHAR (50) NULL,
    [role] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
