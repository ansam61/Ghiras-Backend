@echo off
chcp 65001 > nul
title Ghiras Master Launcher
echo ========================================================
echo   🌿 تشغيل كافة مكونات منظومة غراس (API + MVC + Flutter)
echo ========================================================
echo.

echo [1/3] جاري تشغيل API الخادم الخلفي (Ghiras.API)...
start "1. Ghiras API Server" cmd /k "cd /d %~dp0Ghiras.API\Ghiras.API && dotnet run"

timeout /t 4

echo [2/3] جاري تشغيل واجهات الموقع (Ghiras.Web MVC)...
start "2. Ghiras Web MVC" cmd /k "cd /d %~dp0Ghiras.API\Ghiras.Web && dotnet run"

timeout /t 3

echo [3/3] جاري تشغيل تطبيق الجوال الفلاتر (Ghiras.Mobile)...
start "3. Ghiras Flutter Mobile App" cmd /k "cd /d %~dp0ghiras_app && dotnet run"

echo.
echo ========================================================
echo   تم تشغيل جميع مكونات المشروع بنجاح! 🚀
echo ========================================================
pause
