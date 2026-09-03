-- 1. إضافة جدول صور النباتات (PlantImages) إذا لم يكن موجوداً
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PlantImages')
BEGIN
    CREATE TABLE dbo.PlantImages (
        ImageId INT IDENTITY(1,1) PRIMARY KEY,
        PlantId INT NOT NULL,
        ImageUrl NVARCHAR(MAX) NOT NULL,
        FileName NVARCHAR(MAX) NULL,
        IsPrimary BIT NOT NULL DEFAULT 1,
        UploadedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_PlantImages_Plants FOREIGN KEY (PlantId) REFERENCES dbo.Plants(PlantId) ON DELETE CASCADE
    );
END

-- 2. إضافة قسم تجريبي
IF NOT EXISTS (SELECT * FROM dbo.PlantCategories WHERE CategoryName = N'نباتات داخلية')
BEGIN
    INSERT INTO dbo.PlantCategories (CategoryName, Description, CreatedAt)
    VALUES (N'نباتات داخلية', N'نباتات زينة للمنازل والمكاتب', GETDATE());
END