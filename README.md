# 📦 Storage Management App – Document Upload Feature

This project demonstrates how to integrate multiple Azure services to build a simple document management system. It focuses on the **document upload feature**, using a combination of ASP.NET Core, Vue.js, and Azure resources.

## 🚀 Overview

Users can upload various types of documents (e.g., import/export files) through a web interface. The system stores metadata in Azure SQL Database, document types in Azure Table Storage, and the actual files in Azure Blob Storage.

## 🧰 Tech Stack

- **Backend**: ASP.NET Core 8
- **Frontend**: Vue.js 3 + PrimeVue
- **Infrastructure**: Azure Bicep (optional)
- **Azure Services**:
  - Blob Storage – stores uploaded files
  - Table Storage – stores document types
  - SQL Database – stores document metadata
  - App Service – hosts the API
  - Static Web App – serves the Vue SPA
 
## 🗂 Architecture Flow

<img width="516" height="250" alt="explorer_FMZ1LuZdsR" src="https://github.com/user-attachments/assets/75cb2f81-4d6b-4cd7-a297-a59609908244" />

1. User accesses the document management page.
2. SPA requests a SAS token from the API.
3. SPA uploads the document to Blob Storage using the SAS token.
4. SPA sends document metadata to the API.
5. API stores metadata in Azure SQL Database.

## 📦 Prerequisites

- Azure subscription
- Visual Studio or VS Code
- Basic knowledge of Azure services and deployment via portal or Bicep

## 🛠 Local Development

### Backend (Storage API)

```bash
cd upload-doc/app/api
# Add configuration for Blob, Table, and SQL
# Run the API using your IDE
```

### FE (Vue App)

 Update API base URL in HttpService.ts
 
```bash
cd upload-doc/app/webapp
npm install
npm run dev
```

## Azure deployment
- Deploy the API to Azure App Service.
- Verify the API is running.
- Deploy the Vue app to Azure Static Web App.
- Confirm the full application is functional

  
## License

This project is licensed under the MIT License.
