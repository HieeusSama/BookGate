IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Auths] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [FullName] nvarchar(max) NULL,
    [Role] int NOT NULL,
    CONSTRAINT [PK_Auths] PRIMARY KEY ([Id])
);

CREATE TABLE [OrderStatuses] (
    [StatusId] nvarchar(450) NOT NULL,
    [StatusName] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_OrderStatuses] PRIMARY KEY ([StatusId])
);

CREATE TABLE [Publishers] (
    [PublisherId] nvarchar(450) NOT NULL,
    [PublisherName] nvarchar(255) NOT NULL,
    [Address] nvarchar(500) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    CONSTRAINT [PK_Publishers] PRIMARY KEY ([PublisherId])
);

CREATE TABLE [Orders] (
    [OrderId] nvarchar(450) NOT NULL,
    [Id] int NOT NULL,
    [StatusId] nvarchar(450) NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    [CompletedDate] datetime2 NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [ShippingFee] decimal(18,2) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [Ward] nvarchar(100) NOT NULL,
    [StreetAddress] nvarchar(255) NOT NULL,
    [ReceiverPhone] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_Orders_Auths_Id] FOREIGN KEY ([Id]) REFERENCES [Auths] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_OrderStatuses_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [OrderStatuses] ([StatusId]) ON DELETE CASCADE
);

CREATE TABLE [Books] (
    [BookId] nvarchar(450) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Author] nvarchar(max) NOT NULL,
    [Quantity] int NOT NULL,
    [PublisherId] nvarchar(450) NOT NULL,
    [PublicationDate] datetime2 NOT NULL,
    [Genre] nvarchar(max) NOT NULL,
    [FileUrl] nvarchar(max) NULL,
    [PurchasePrice] decimal(18,2) NOT NULL,
    [SellingPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([BookId]),
    CONSTRAINT [FK_Books_Publishers_PublisherId] FOREIGN KEY ([PublisherId]) REFERENCES [Publishers] ([PublisherId]) ON DELETE CASCADE
);

CREATE TABLE [CartItems] (
    [CartItemId] nvarchar(450) NOT NULL,
    [Id] int NOT NULL,
    [BookId] nvarchar(450) NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([CartItemId]),
    CONSTRAINT [FK_CartItems_Auths_Id] FOREIGN KEY ([Id]) REFERENCES [Auths] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE CASCADE
);

CREATE TABLE [OrderDetails] (
    [OrderDetailId] nvarchar(450) NOT NULL,
    [OrderId] nvarchar(450) NOT NULL,
    [BookId] nvarchar(450) NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([OrderDetailId]),
    CONSTRAINT [FK_OrderDetails_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StatusId', N'StatusName') AND [object_id] = OBJECT_ID(N'[OrderStatuses]'))
    SET IDENTITY_INSERT [OrderStatuses] ON;
INSERT INTO [OrderStatuses] ([StatusId], [StatusName])
VALUES (N'CANCELLED', N'Đã hủy'),
(N'COMPLETED', N'Đã hoàn thành'),
(N'PENDING', N'Chờ xử lý'),
(N'SHIPPING', N'Đang giao hàng');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StatusId', N'StatusName') AND [object_id] = OBJECT_ID(N'[OrderStatuses]'))
    SET IDENTITY_INSERT [OrderStatuses] OFF;

CREATE INDEX [IX_Books_PublisherId] ON [Books] ([PublisherId]);

CREATE INDEX [IX_CartItems_BookId] ON [CartItems] ([BookId]);

CREATE INDEX [IX_CartItems_Id] ON [CartItems] ([Id]);

CREATE INDEX [IX_OrderDetails_BookId] ON [OrderDetails] ([BookId]);

CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);

CREATE INDEX [IX_Orders_Id] ON [Orders] ([Id]);

CREATE INDEX [IX_Orders_StatusId] ON [Orders] ([StatusId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260313110517_TenSuaDoiCuaBan', N'10.0.3');

COMMIT;
GO

BEGIN TRANSACTION;
UPDATE [OrderStatuses] SET [StatusName] = N'Hoàn thành'
WHERE [StatusId] = N'COMPLETED';
SELECT @@ROWCOUNT;


UPDATE [OrderStatuses] SET [StatusName] = N'Chờ xác nhận'
WHERE [StatusId] = N'PENDING';
SELECT @@ROWCOUNT;


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StatusId', N'StatusName') AND [object_id] = OBJECT_ID(N'[OrderStatuses]'))
    SET IDENTITY_INSERT [OrderStatuses] ON;
INSERT INTO [OrderStatuses] ([StatusId], [StatusName])
VALUES (N'AWAITING_SHIPMENT', N'Chờ giao hàng');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StatusId', N'StatusName') AND [object_id] = OBJECT_ID(N'[OrderStatuses]'))
    SET IDENTITY_INSERT [OrderStatuses] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260326063638_add_Migration', N'10.0.3');

COMMIT;
GO

