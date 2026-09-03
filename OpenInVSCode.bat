@echo off
chcp 65001 > nul
title Open Ghiras Flutter App in VS Code
echo.
echo ========================================================
echo   🌿 فتح مشروع غراس الفلاتر في VS Code (تلوين وسرعة)
echo ========================================================
echo.
cd /d "%~dp0ghiras_app"
start code .
echo تم فتح المشروع بنجاح في VS Code!
