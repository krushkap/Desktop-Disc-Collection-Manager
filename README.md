# Desktop-Disc-Collection-Manager
A desktop application for cataloging a home media collection, built with C#, WPF (.NET), and MS SQL Server.
<img width="851" height="449" alt="image" src="https://github.com/user-attachments/assets/8892ee0a-cd8c-4473-b435-4bacf1bdf754" />

🚀 How to Run
Prerequisites
Visual Studio 2022 (with .NET Desktop workload)
MS SQL Server (Express version is fine)
SQL Server Management Studio (SSMS)
Steps:
Clone the repository:

git clone https://github.com/krushkap/Desktop-Disc-Collection-Manager.git
Set up the database:

Open SSMS and create a new database (e.g., MyDiscCollectionDB).
Run the scripts from the /Database folder:
create_tables.sql (to build the schema)
populate_data.sql (to add sample data)
Configure the connection:

Open the project in Visual Studio.
In App.config, update the connectionString with your server name and database name.
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=MyDiscCollectionDB;Integrated Security=True" />
</connectionStrings>
Build and run:

Press F5 in Visual Studio to start the application.
