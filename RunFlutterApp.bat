@echo off
chcp 65001 > nul
title Ghiras Mobile App Launcher (Flutter Standalone)
echo.
echo ========================================================
echo   🌿 تشغيل تطبيق الجوال غراس | Ghiras Flutter App
echo ========================================================
echo.
cd /d "%~dp0ghiras_app"
echo [1/2] جاري جلب الحزم والمكتبات...
call flutter pub get
echo.
echo [2/2] جاري تشغيل التطبيق...
call flutter run -d chrome
pause
