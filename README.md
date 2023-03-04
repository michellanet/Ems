# Employment Management System

# For test runs use the following username and passwords
  Admin Username: harryjake password: root
  
  Employee Username: mike password:root
  
  NB: An admin can create a new employee profile, but the new employee can only be recognised by the system when a shift has also been assigned to the new employee.
  
# Project Description
An employee scheduling system to management employees’ schedule and create interaction between managers/supervisors and employees for making leave and day-off requests and reviewing these requests. Also, this system provides report for employee to stay cautious of their performance level. 


# Database Component
Employee table – [columns]
Id (Primary Key)
FirstName
LastName
PhoneNo
HomeAddress
Email
dob
JobTitle
privilege
password
username

Shift table – [columns] 
Id (Primary Key)
Date
Attendance
EmployeeId


Leave table – [columns] 
Id (Primary Key)
StartDate
EndDate
Status
EmployeeId

DayOff table – [columns]
Id (Primary Key)
Date
Status
EmployeeId

# Connection String
  <connectionStrings>
    <add name="EmployeeManagementContext" connectionString="Data Source=(localdb)\MSSQLLocalDB;AttachDBFilename=|DataDirectory|\ems.mdf;Initial Catalog=EMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;" providerName="System.Data.SqlClient"/>
  </connectionStrings>
  
  # Database
  Database resides in the app data as ems.mdf




