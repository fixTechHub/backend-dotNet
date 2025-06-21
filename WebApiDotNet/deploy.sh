#!/bin/bash

# FixTech Docker Deployment Script

echo "🚀 Starting FixTech Docker Deployment..."

# Check if .env file exists
if [ ! -f .env ]; then
    echo "⚠️  .env file not found. Creating from template..."
    cp env.example .env
    echo "📝 Please edit .env file with your MongoDB Atlas credentials"
    echo "   Then run this script again."
    exit 1
fi

# Function to check if Docker is running
check_docker() {
    if ! docker info > /dev/null 2>&1; then
        echo "❌ Docker is not running. Please start Docker Desktop first."
        exit 1
    fi
}

# Function to clean up
cleanup() {
    echo "🧹 Cleaning up..."
    docker-compose down
    docker system prune -f
}

# Function to build and start services
deploy() {
    echo "🔨 Building and starting services..."
    docker-compose up --build -d
    
    echo "⏳ Waiting for services to start..."
    sleep 10
    
    # Check if services are running
    if docker-compose ps | grep -q "Up"; then
        echo "✅ Services are running!"
        echo ""
        echo "🌐 Access your applications:"
        echo "   Frontend: http://localhost:3000"
        echo "   Backend API: http://localhost:5000/api"
        echo "   Swagger Docs: http://localhost:5000/swagger"
        echo ""
        echo "📊 View logs: docker-compose logs -f"
        echo "🛑 Stop services: docker-compose down"
    else
        echo "❌ Services failed to start. Check logs:"
        docker-compose logs
    fi
}

# Main script
case "$1" in
    "start")
        check_docker
        deploy
        ;;
    "stop")
        echo "🛑 Stopping services..."
        docker-compose down
        ;;
    "restart")
        echo "🔄 Restarting services..."
        docker-compose down
        deploy
        ;;
    "logs")
        docker-compose logs -f
        ;;
    "clean")
        cleanup
        ;;
    "rebuild")
        echo "🔨 Rebuilding services..."
        docker-compose down
        docker-compose build --no-cache
        deploy
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|logs|clean|rebuild}"
        echo ""
        echo "Commands:"
        echo "  start   - Build and start all services"
        echo "  stop    - Stop all services"
        echo "  restart - Restart all services"
        echo "  logs    - View logs"
        echo "  clean   - Clean up containers and images"
        echo "  rebuild - Rebuild without cache and start"
        exit 1
        ;;
esac 