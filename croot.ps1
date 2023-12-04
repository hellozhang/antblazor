(Get-Content .\wwwroot\index.html -Encoding UTF8) `
    -replace '<base href="/" />', '<base href="/blazorpagesdemo-pages/" />' |
    Out-File .\wwwroot\index.html -Encoding utf8

pause