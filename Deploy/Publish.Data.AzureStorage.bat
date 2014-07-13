@echo off
powershell -NoProfile -ExecutionPolicy unrestricted -Command "& {.\publish.ps1 -PackageName 'DefiantCode.Data.AzureStorage'; exit $error.Count}"
pause