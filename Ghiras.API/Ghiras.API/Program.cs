using Ghiras.Infrastructure;
using Ghiras.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. إضافة المتحكمات وإعدادات الـ JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// 2. تسجيل خدمات البنية التحتية (DbContext والمستودعات) عبر Clean Architecture
builder.Services.AddInfrastructureServices(builder.Configuration);

// 3. إعداد Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. إعداد السياسات (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 5. التثبت التلقائي من إنشاء قاعدة البيانات والجداول (بما فيها جدول PlantImages)
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();

        // التأكد التلقائي من وجود جدول PlantImages في قاعدة البيانات
        dbContext.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PlantImages')
            BEGIN
                CREATE TABLE [dbo].[PlantImages] (
                    [ImageId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    [PlantId] INT NOT NULL,
                    [ImageUrl] NVARCHAR(MAX) NOT NULL,
                    [IsPrimary] BIT NOT NULL DEFAULT 1,
                    [UploadedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
                    CONSTRAINT [FK_PlantImages_Plants_PlantId] FOREIGN KEY ([PlantId]) REFERENCES [dbo].[Plants] ([PlantId]) ON DELETE CASCADE
                );
            END
        ");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Database Init Warning]: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ghiras API v1");
    });
}

// app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();