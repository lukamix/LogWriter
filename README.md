# LogWriter
If you want to use this Service you must have Visual Studio with .Net Framework
Download this project, Run with VS Studio

1,Search “Command Prompt” and run as administrator
2,Fire the below command in the command prompt and press ENTER.
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319 
3,Now Go to your project source folder > bin > Debug and copy the full path of your Windows Service exe file.
4,Installing a Windows Service
Open the command prompt and fire the below command and press ENTER.
Syntax:
InstallUtil.exe + Your copied path + \your service name + .exe

Our path
InstallUtil.exe C:\Users\PV\source\repos\MyFirstService\MyFirstService\bin\Debug\MyFirstService.exe
Check the status of a Windows Service
Open services by following the below steps:
Press Window key + R.
Type services.msc
Find your Service and check if it Running,
If you want it to run with System, just Right Click -> Properties and make sure Startup type is Automatic!
