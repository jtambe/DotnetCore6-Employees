# DotnetCore6-Employees

This is .Net core 6 ASP.net MVC web application that Adds Employees in Company Database's Employees table with Either Employee or Manager as Role
This application has used Db first approach with Ef Core


Important points to notice:
1. Application was created with Company Database used from local database editing tools within Visual Studio.
2. DB only has a single table named 'Employee' and has only 5 columns viz. Id, Name, Address, Role, ManagerId
3. APIs are provided for CRUD operations for Employee
4. Default role for an employee is Employee. You can assign the role of Manager to employee at time of creation
5. If there is any employee with Manager as role in DB, dropdown will display the list of managers to choose from and assign it to an employee
6. In case manager employee is deleted from table, the DB is updated for associated employees to mark their manager as null
7. In case a manager level employee is assigned a role of Employee, it also updates associated employees' managerIds as null

Following are some of the commands that came in handy when dealing with localDB in Db first approach
```
sqllocaldb stop ProjectModels
sqllocaldb stop MSSQLLocalDB
sqllocaldb start ProjectModels
sqllocaldb start MSSQLLocalDB

dotnet ef migrations add UpdateDatabase
dotnet ef database update
dotnet ef dbcontext scaffold "DBConnectionString" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models -f
```
