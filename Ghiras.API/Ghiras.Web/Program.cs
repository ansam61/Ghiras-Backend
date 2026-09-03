var builder = WebApplication.CreateBuilder(args);

// إضافة خدمات الـ Controllers والـ Views
builder.Services.AddControllersWithViews();

// تسجيل HttpClient الموجه لـ API الخاص بـ Ghiras
builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();