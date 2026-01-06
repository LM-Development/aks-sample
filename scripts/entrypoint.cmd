@echo off

IF "%1"=="-v" (
    .\RecordingBot.Console.exe -v
    exit /b 0
)

:: --- Ensure the VC_redist is installed for the Microsoft.Skype.Bots.Media Library ---
echo Setup: Starting VC_redist
.\VC_redist.x64.exe /quiet /norestart

echo Setup: Converting certificate
powershell.exe C:\Program` Files\OpenSSL\bin\openssl.exe pkcs12 -export -out C:\bot\certificate.pfx -passout pass: -inkey C:\certs\tls.key -in C:\certs\tls.crt

echo Setup: Installing certificate
certutil -f -p "" -importpfx certificate.pfx
powershell.exe "(Get-PfxCertificate -FilePath certificate.pfx).Thumbprint" > thumbprint
set /p AzureSettings__CertificateThumbprint= < thumbprint
del thumbprint
del certificate.pfx

echo Setup: Done
echo ---------------------

:: --- Running bot ---
.\RecordingBot.Console.exe
