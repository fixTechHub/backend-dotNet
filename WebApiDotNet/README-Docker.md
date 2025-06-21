# Docker Deployment Guide - FixTech Project

## Prerequisites

- Docker Desktop installed
- Docker Compose installed
- MongoDB Atlas account (đã có sẵn)

## Quick Start

### 1. Setup Environment Variables

Copy file `env.example` thành `.env` và cập nhật các giá trị:

```bash
cp env.example .env
```

Chỉnh sửa file `.env` với thông tin MongoDB Atlas của bạn:

```env
# MongoDB Atlas Configuration
MONGODB_CONNECTION_STRING=mongodb+srv://username:password@your-cluster.mongodb.net/?retryWrites=true&w=majority
MONGODB_DATABASE_NAME=fixTech

# Backend Configuration
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80;https://+:443

# Frontend Configuration
VITE_API_BASE_URL=http://localhost:5000/api

# Docker Configuration
DOCKER_BUILDKIT=1
COMPOSE_DOCKER_CLI_BUILD=1
```

### 2. Build and Run with Docker Compose

```bash
# Build và start tất cả services
docker-compose up --build

# Hoặc chạy ở background
docker-compose up --build -d
```

### 3. Access Applications

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000/api
- **Swagger Documentation**: http://localhost:5000/swagger

## Docker Commands

### Build Images
```bash
# Build backend only
docker-compose build backend

# Build frontend only
docker-compose build frontend

# Build all services
docker-compose build
```

### Manage Services
```bash
# Start services
docker-compose up

# Start in background
docker-compose up -d

# Stop services
docker-compose down

# View logs
docker-compose logs

# View logs for specific service
docker-compose logs backend
docker-compose logs frontend

# Restart services
docker-compose restart

# Remove containers and volumes
docker-compose down -v
```

### Development Commands
```bash
# Rebuild and restart specific service
docker-compose up --build backend

# View running containers
docker ps

# Access container shell
docker exec -it fixtech-backend /bin/bash
docker exec -it fixtech-frontend /bin/sh
```

## Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend       │    │   MongoDB       │
│   (React)       │◄──►│   (.NET 8)      │◄──►│   Atlas         │
│   Port: 3000    │    │   Port: 5000    │    │   (Cloud)       │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## Troubleshooting

### Common Issues

1. **Port already in use**
   ```bash
   # Check what's using the port
   netstat -ano | findstr :5000
   netstat -ano | findstr :3000
   
   # Kill process or change ports in docker-compose.yml
   ```

2. **MongoDB connection failed**
   - Kiểm tra connection string trong file `.env`
   - Đảm bảo IP của bạn được whitelist trong MongoDB Atlas
   - Kiểm tra username/password

3. **Frontend can't connect to Backend**
   - Kiểm tra `VITE_API_BASE_URL` trong file `.env`
   - Đảm bảo backend đã start thành công
   - Kiểm tra CORS configuration trong backend

4. **Build failed**
   ```bash
   # Clean Docker cache
   docker system prune -a
   
   # Rebuild without cache
   docker-compose build --no-cache
   ```

### Health Checks

```bash
# Check if services are healthy
docker-compose ps

# Check health status
curl http://localhost:5000/health
curl http://localhost:3000
```

## Production Deployment

### Environment Variables for Production

Tạo file `.env.production` với các giá trị production:

```env
MONGODB_CONNECTION_STRING=mongodb+srv://prod-user:prod-pass@prod-cluster.mongodb.net/?retryWrites=true&w=majority
MONGODB_DATABASE_NAME=fixTech_prod
ASPNETCORE_ENVIRONMENT=Production
VITE_API_BASE_URL=https://your-domain.com/api
```

### Deploy to Production Server

```bash
# Build production images
docker-compose -f docker-compose.yml --env-file .env.production build

# Deploy to production
docker-compose -f docker-compose.yml --env-file .env.production up -d
```

## Development Workflow

1. **Local Development**: Sử dụng `docker-compose up --build` để test
2. **Code Changes**: Rebuild containers khi có thay đổi code
3. **Database**: Sử dụng MongoDB Atlas cho development và production
4. **Collaboration**: Team members có thể pull code và chạy `docker-compose up --build`

## Security Notes

- Không commit file `.env` vào git
- Sử dụng strong passwords cho MongoDB Atlas
- Enable IP whitelist trong MongoDB Atlas
- Consider using Docker secrets cho production 