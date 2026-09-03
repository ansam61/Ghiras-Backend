@echo off
chcp 65001 > nul
echo ===================================================
echo   جاري تشغيل مشروع غراس (Ghiras API + Ghiras Web MVC)
echo ===================================================
echo.

echo 1. جاري تشغيل خلفية النظام Ghiras.API...
start "Ghiras API Backend" cmd /k "cd /d %~dp0Ghiras.API\Ghiras.API && dotnet run"

timeout /t 4

echo 2. جاري تشغيل واجهة الـ MVC Ghiras.Web...
start "Ghiras Web MVC Frontend" cmd /k "cd /d %~dp0Ghiras.API\Ghiras.Web && dotnet run"

echo.
echo تم تشغيل المشروعين بنجاح!
pause