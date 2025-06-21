# FixTech Docker Deployment Script for Windows

param(
    [Parameter(Position=0)]
    [ValidateSet("start", "stop", "restart", "logs", "clean", "rebuild")]
    [string]$Command = "start"
)

Write-Host "🚀 Starting FixTech Docker Deployment..." -ForegroundColor Green

# Check if .env file exists
if (-not (Test-Path ".env")) {
    Write-Host "⚠️  .env file not found. Creating from template..." -ForegroundColor Yellow
    Copy-Item "env.example" ".env"
    Write-Host "📝 Please edit .env file with your MongoDB Atlas credentials" -ForegroundColor Yellow
    Write-Host "   Then run this script again." -ForegroundColor Yellow
    exit 1
}

# Function to check if Docker is running
function Test-Docker {
    try {
        docker info | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

# Function to clean up
function Clear-Docker {
    Write-Host "🧹 Cleaning up..." -ForegroundColor Blue
    docker-compose down
    docker system prune -f
}

# Function to build and start services
function Start-Services {
    Write-Host "🔨 Building and starting services..." -ForegroundColor Blue
    docker-compose up --build -d
    
    Write-Host "⏳ Waiting for services to start..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
    
    # Check if services are running
    $services = docker-compose ps
    if ($services -match "Up") {
        Write-Host "✅ Services are running!" -ForegroundColor Green
        Write-Host ""
        Write-Host "🌐 Access your applications:" -ForegroundColor Cyan
        Write-Host "   Frontend: http://localhost:3000" -ForegroundColor White
        Write-Host "   Backend API: http://localhost:5000/api" -ForegroundColor White
        Write-Host "   Swagger Docs: http://localhost:5000/swagger" -ForegroundColor White
        Write-Host ""
        Write-Host "📊 View logs: docker-compose logs -f" -ForegroundColor Gray
        Write-Host "🛑 Stop services: docker-compose down" -ForegroundColor Gray
    }
    else {
        Write-Host "❌ Services failed to start. Check logs:" -ForegroundColor Red
        docker-compose logs
    }
}

# Main script
switch ($Command) {
    "start" {
        if (-not (Test-Docker)) {
            Write-Host "❌ Docker is not running. Please start Docker Desktop first." -ForegroundColor Red
            exit 1
        }
        Start-Services
    }
    "stop" {
        Write-Host "🛑 Stopping services..." -ForegroundColor Blue
        docker-compose down
    }
    "restart" {
        Write-Host "🔄 Restarting services..." -ForegroundColor Blue
        docker-compose down
        Start-Services
    }
    "logs" {
        docker-compose logs -f
    }
    "clean" {
        Clear-Docker
    }
    "rebuild" {
        Write-Host "🔨 Rebuilding services..." -ForegroundColor Blue
        docker-compose down
        docker-compose build --no-cache
        Start-Services
    }
}

Write-Host "✅ Deployment script completed!" -ForegroundColor Green 