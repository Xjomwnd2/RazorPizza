#!/bin/bash

# Razor Pizza Project Setup Script
# This script creates the complete project structure for the Razor Pizza application

echo "🍕 Razor Pizza - Project Setup Script"
echo "======================================"
echo ""

# Check if .NET SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET 8.0 SDK first."
    echo "Download from: https://dotnet.microsoft.com/download"
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"
echo ""

# Create project directory
PROJECT_NAME="RazorPizza"
if [ -d "$PROJECT_NAME" ]; then
    echo "⚠️  Directory $PROJECT_NAME already exists."
    read -p "Do you want to continue and overwrite? (y/n): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
    rm -rf "$PROJECT_NAME"
fi

echo "📁 Creating project structure..."
mkdir -p $PROJECT_NAME
cd $PROJECT_NAME

# Create solution
echo "🔧 Creating solution..."
dotnet new sln -n RazorPizza

# Create directory structure
mkdir -p src tests docs

# Create projects
echo "🏗️  Creating Web project..."
dotnet new blazorserver -n RazorPizza.Web -o src/RazorPizza.Web

echo "🏗️  Creating Data project..."
dotnet new classlib -n RazorPizza.Data -o src/RazorPizza.Data

echo "🏗️  Creating Models project..."
dotnet new classlib -n RazorPizza.Models -o src/RazorPizza.Models

echo "🏗️  Creating Services project..."
dotnet new classlib -n RazorPizza.Services -o src/RazorPizza.Services

echo "🧪 Creating Test project..."
dotnet new xunit -n RazorPizza.Tests -o tests/RazorPizza.Tests

# Add projects to solution
echo "📦 Adding projects to solution..."
dotnet sln add src/RazorPizza.Web/RazorPizza.Web.csproj
dotnet sln add src/RazorPizza.Data/RazorPizza.Data.csproj
dotnet sln add src/RazorPizza.Models/RazorPizza.Models.csproj
dotnet sln add src/RazorPizza.Services/RazorPizza.Services.csproj
dotnet sln add tests/RazorPizza.Tests/RazorPizza.Tests.csproj

# Add project references
echo "🔗 Setting up project references..."

# Web project references
cd src/RazorPizza.Web
dotnet add reference ../RazorPizza.Data/RazorPizza.Data.csproj
dotnet add reference ../RazorPizza.Models/RazorPizza.Models.csproj
dotnet add reference ../RazorPizza.Services/RazorPizza.Services.csproj

# Data project references
cd ../RazorPizza.Data
dotnet add reference ../RazorPizza.Models/RazorPizza.Models.csproj

# Services project references
cd ../RazorPizza.Services
dotnet add reference ../RazorPizza.Data/RazorPizza.Data.csproj
dotnet add reference ../RazorPizza.Models/RazorPizza.Models.csproj

# Test project references
cd ../../tests/RazorPizza.Tests
dotnet add reference ../../src/RazorPizza.Web/RazorPizza.Web.csproj
dotnet add reference ../../src/RazorPizza.Services/RazorPizza.Services.csproj

# Go back to project root
cd ../..

# Install NuGet packages
echo "📦 Installing NuGet packages..."

# Web project packages
echo "  Installing packages for Web project..."
cd src/RazorPizza.Web
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.UI --version 8.0.0

# Data project packages
echo "  Installing packages for Data project..."
cd ../RazorPizza.Data
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0

# Test project packages
echo "  Installing packages for Test project..."
cd ../../tests/RazorPizza.Tests
dotnet add package Moq --version 4.20.70
dotnet add package FluentAssertions --version 6.12.0
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.0

cd ../..

# Create documentation files
echo "📝 Creating documentation..."
cat > README.md << 'EOF'
# 🍕 Razor Pizza - Team-14 .NET Blazor Application

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server / SQL Server Express (or SQLite for development)
- Visual Studio 2022 / VS Code

### Setup
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update --project src/RazorPizza.Web`
4. Run application: `dotnet run --project src/RazorPizza.Web`

### Default Users
- **Admin:** admin@razorpizza.com / Admin@123
- **Customer:** customer@test.com / Customer@123

## Project Structure
- `src/RazorPizza.Web` - Blazor Server application
- `src/RazorPizza.Data` - Data access layer with EF Core
- `src/RazorPizza.Models` - Domain models
- `src/RazorPizza.Services` - Business logic
- `tests/RazorPizza.Tests` - Unit and integration tests

## Features
- 🍕 Pizza menu with customization
- 🛒 Shopping cart
- 👤 User authentication
- 📦 Order management
- 👑 Admin dashboard
- 💳 Checkout process

## Tech Stack
- .NET 8.0
- Blazor Server
- Entity Framework Core
- ASP.NET Core Identity
- Bootstrap 5
- SQL Server

For detailed documentation, see the `/docs` folder.
EOF

# Create .gitignore
echo "🔒 Creating .gitignore..."
cat > .gitignore << 'EOF'
## Ignore Visual Studio temporary files, build results, and files generated by popular Visual Studio add-ons.

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio cache/options directory
.vs/

# Visual Studio Code
.vscode/
*.code-workspace

# Database files
*.mdf
*.ldf
*.ndf
*.db
*.db-shm
*.db-wal
*.sqlite
*.sqlite3

# User secrets
secrets.json
appsettings.Development.json

# OS generated files
.DS_Store
Thumbs.db
EOF

# Restore all packages
echo "🔄 Restoring packages..."
dotnet restore

# Build solution
echo "🔨 Building solution..."
dotnet build

echo ""
echo "✅ Project setup complete!"
echo ""
echo "📋 Next Steps:"
echo "1. Update connection string in src/RazorPizza.Web/appsettings.json"
echo "2. Run migrations:"
echo "   cd src/RazorPizza.Web"
echo "   dotnet ef migrations add InitialCreate --project ../RazorPizza.Data"
echo "   dotnet ef database update"
echo ""
echo "3. Run the application:"
echo "   dotnet run --project src/RazorPizza.Web"
echo ""
echo "4. Access the app at: https://localhost:5001"
echo ""
echo "🍕 Happy coding!"