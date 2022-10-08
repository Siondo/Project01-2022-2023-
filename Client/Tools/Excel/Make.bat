@echo off
cd %~dp0
ExportConfig.exe>log.txt
@echo %~dp0