# Sử dụng SDK image để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Sao chép tất cả vào thư mục source.
# Đây là cách đơn giản nhất để xử lý cấu trúc thư mục lồng nhau của bạn.
COPY . .

# Chạy lệnh restore và publish từ đúng thư mục chứa file .csproj
# Đường dẫn này phải chính xác với cấu trúc repo của bạn
WORKDIR /source/WebApiDotNet/WebApiDotNet
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

# Sử dụng ASP.NET runtime image để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app ./

# Cổng mà ASP.NET Core sẽ chạy bên trong container
EXPOSE 8080

# Đặt biến môi trường để ASP.NET Core lắng nghe trên tất cả các địa chỉ IP
ENV ASPNETCORE_URLS=http://+:8080

# Lệnh để khởi chạy ứng dụng
# Tên 'WebApiDotNet.dll' phải khớp với tên project của bạn
ENTRYPOINT ["dotnet", "WebApiDotNet.dll"]