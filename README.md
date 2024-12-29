
# Sonry Socials API for [MineSocial](https://github.com/SonryP/minesocial)

A simple API designed for integrating social network functionalities with Minecraft servers.

## 🚀 Overview
Sonry Socials API is built to enable seamless communication between Minecraft servers and a social network platform. It provides authentication, secure data handling, and user management.

### Key Features:
- Built with **.NET 8.0**.
- Database powered by **PostgreSQL**.
- Secure **authentication system** integrated with a Fabric server-side mod.
- **Bearer token** authentication for API requests.
- Passwords securely stored using **Argon2** hashing.

## 🛠️ Technology Stack
- **Backend:** .NET 8.0
- **Database:** PostgreSQL
- **Authentication:** Bearer Token, Argon2 Password Hashing
- **Integration:** Fabric Server-Side Mod

## 📦 Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/SonryP/minesocial.git
   ```
2. Configure the database connection in `appsettings.json`.
3. Build and run the application:
   ```sh
   dotnet build
   dotnet run
   ```

## ⚙️ appsettings.json Example
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "minedb": ""
  },
  "Jwt": {
    "Key": "",
    "Issuer": "yourissuer.com"
  }
}
```


## 🤝 Contributing
Feel free to contribute by opening an issue or submitting a pull request.

## 📄 License
This project is licensed under the MIT License.


---
Crafted with ❤️ by the Sonry Team.
