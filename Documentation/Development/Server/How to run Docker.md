I tried to implement docker compose for this project backend.

For now, here is tutorial how to launch SQL Server on docker.
- Enter SQLMS and disable my localhost server!!!!! This is very important because without that it will not work.
- Run docker container via powershell 
	```docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!@#" -p 1433:1433 --name sqlserver --hostname sqlserver -d mcr.microsoft.com/mssql/server:2019-latest```
- Now you need to remove 'Services' folder with images on on local machine becuase without that server will not be seeded thus, no migration will occur thus. Database will not be created.