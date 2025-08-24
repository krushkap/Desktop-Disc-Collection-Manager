# Desktop-Disc-Collection-Manager
A desktop application for cataloging a home media collection, built with C#, WPF (.NET), and MS SQL Server.
<img width="851" height="449" alt="image" src="https://github.com/user-attachments/assets/8892ee0a-cd8c-4473-b435-4bacf1bdf754" />

# 🚀 How to Run

## 📌 Prerequisites
- Visual Studio 2022 (с установленным **.NET Desktop workload**)
- MS SQL Server (Express версии достаточно)
- SQL Server Management Studio (**SSMS**)

---

## ⚡ Steps

### 1. Clone the repository
```bash
git clone https://github.com/krushkap/Desktop-Disc-Collection-Manager.git
````

### 2. Set up the database

1. Откройте **SSMS** и создайте новую базу данных (например: `MyDiscCollectionDB`).
2. Выполните скрипты из папки `/Database`:

   * `create_tables.sql` – создаёт схему БД
   * `populate_data.sql` – добавляет тестовые данные

### 3. Configure the connection

1. Откройте проект в **Visual Studio**.
2. В файле `App.config` обновите `connectionString`, указав ваш сервер и имя базы данных:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=MyDiscCollectionDB;Integrated Security=True" />
</connectionStrings>
```

### 4. Build and run

Нажмите **F5** в Visual Studio, чтобы запустить приложение.
