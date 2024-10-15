@echo off

REM Navegar até o diretório do projeto
cd /d "C:\Users\User\OneDrive\Documentos\GitHub\VisualSoftwareTeste"

REM Restaurar dependências, compilar e executar o projeto
dotnet restore
dotnet build
start /b dotnet run
for /f "tokens=2" %%i in ('tasklist ^| findstr dotnet.exe') do set DotnetPID=%%i

REM Aguardar alguns segundos para simular execução
timeout /t 10 /nobreak >nul

REM Forçar a finalização do processo dotnet capturado
echo Finalizando a execução do projeto...
taskkill /pid %DotnetPID% /f

echo Execução do projeto finalizada com sucesso.

REM Aguardar um tempo máximo para a janela permanecer aberta (ex: 5 segundos)
echo A janela irá fechar em 30 segundos...
echo Os logs da execução ficarão salvos na pasta /logs do projeto.
timeout /t 30 /nobreak >nul

REM Encerrar a janela do terminal
exit