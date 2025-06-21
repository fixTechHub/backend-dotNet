# 🚀 Quick Start - Deploy FixTech với Docker

## Bước 1: Chuẩn bị
1. **Cài đặt Docker Desktop** (nếu chưa có)
2. **Start Docker Desktop**
3. **Mở terminal/PowerShell** tại thư mục gốc của project

## Bước 2: Setup Environment
```bash
# Copy file environment template
cp env.example .env

# Chỉnh sửa file .env với thông tin MongoDB Atlas của bạn
# (Mở file .env và cập nhật MONGODB_CONNECTION_STRING)
```

## Bước 3: Deploy
### Windows (PowerShell):
```powershell
# Chạy script PowerShell
.\deploy.ps1 start
```

### Linux/Mac (Bash):
```bash
# Chạy script Bash
chmod +x deploy.sh
./deploy.sh start
```

### Hoặc chạy trực tiếp:
```bash
# Build và start tất cả services
docker-compose up --build -d
```

## Bước 4: Truy cập ứng dụng
- 🌐 **Frontend**: http://localhost:3000
- 🔧 **Backend API**: http://localhost:5000/api
- 📚 **Swagger Docs**: http://localhost:5000/swagger

## Các lệnh hữu ích

### Xem logs:
```bash
docker-compose logs -f
```

### Dừng services:
```bash
docker-compose down
```

### Restart services:
```bash
docker-compose restart
```

### Rebuild (khi có thay đổi code):
```bash
docker-compose up --build
```

## Troubleshooting

### Nếu gặp lỗi port đã được sử dụng:
```bash
# Kiểm tra process đang sử dụng port
netstat -ano | findstr :5000
netstat -ano | findstr :3000

# Kill process hoặc thay đổi port trong docker-compose.yml
```

### Nếu MongoDB connection failed:
- Kiểm tra connection string trong file `.env`
- Đảm bảo IP của bạn được whitelist trong MongoDB Atlas
- Kiểm tra username/password

### Nếu build failed:
```bash
# Clean cache và rebuild
docker system prune -a
docker-compose build --no-cache
```

## 🎉 Chúc mừng! 
Ứng dụng của bạn đã được deploy thành công và sẵn sàng để team members khác truy cập! 