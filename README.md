# غِراس — Backend Web API & MVC Dashboard (.NET 10 Clean Architecture)

مرحباً بك في المستودع الرسمي لخدمات الخلفية واللوحة الإدارية والـ RESTful API لمنصة **غِراس (Ghiras Ecosystem)** المبنية بأحدث تقنيات **.NET 10 Web API** ومعمارية **Clean Architecture**.

---

## 🏛️ هيكلية المعمارية والنظام (Clean Architecture Layers)

يتكون المشروع من 4 طبقات رئيسية مفصولة طبقاً لأفضل ممارسات التطوير المعماري:

```
GhirasProject/
├── Ghiras.Core/           # طبقة الكيانات والواجهات الأساسية (Entities & Interfaces)
├── Ghiras.Infrastructure/   # طبقة البيانات والاتصال بقاعدة البيانات (EF Core & SQL Server)
├── Ghiras.API/            # خادم الـ REST API (.NET 10 Controllers & Swagger)
├── Ghiras.Web/            # لوحة التحكم والموقع الإلكتروني (ASP.NET Core 10 MVC)
├── GhirasProject.sln      # حل المشرع الشامل (Solution File)
└── RunAllProjects.bat     # سكربت التشغيل التلقائي للخدمات بنقرة واحدة
```

---

## 🚀 الميزات والتقنيات المستخدمة (Key Features & Tech Stack)

- **الإطار البرمجي**: .NET 10 Framework (C# 14).
- **معمارية النظام**: Clean Architecture + Repository Pattern.
- **قاعدة البيانات**: Microsoft SQL Server المعززة بـ Entity Framework Core 10.
- **الـ API RESTful**: بروتوكولات HTTP متكاملة لعمليات **CRUD** (`GET`, `POST`, `PUT`, `DELETE`).
- **التوثيق**: Swagger OpenAPI 3.0 المدمج في `https://localhost:7267/swagger`.
- **لوحة التحكم**: ASP.NET Core MVC بدعم الثيم المودرن وإدارة المنتجات والنباتات والأقسام.

---

## 🛠️ كيفية التشغيل الاستكشافي (How to Run)

### 1. تشغيل الخدمة الكاملة بضغطة زر واحدة:
- اضغط مرتين على ملف **`RunAllProjects.bat`** في مجلد المشروع الرئيسي.

### 2. التشغيل عبر السطر البرمجي (CLI):
```bash
# تشغيل خادم الـ API
cd Ghiras.API
dotnet run

# تشغيل موقع ولوحة التحكم
cd Ghiras.Web
dotnet run
```

---

## 👤 المسؤول والمطور الرئيسي (Author & Admin)
- **الإسم**: eng: ANSAM JAMEEL
- **البريد الإلكتروني**: `ansam@ghiras.com`
- **التطبيق المرتبط**: [Ghiras Flutter Mobile App Repository](https://github.com/ansam61/Ghiras-Flutter-App)
