# Financial Report API

API này cung cấp các endpoint để lấy thông tin báo cáo tài chính từ Booking và Technician.

## Các Endpoint

### 1. Tổng quan tài chính
```
GET /api/FinancialReport/summary
```
Trả về tổng quan tài chính bao gồm:
- TotalRevenue: Tổng FinalPrice từ tất cả booking
- TotalHoldingAmount: Tổng HoldingAmount từ tất cả booking
- TotalCommissionAmount: Tổng CommissionAmount từ tất cả booking
- TotalTechnicianEarning: Tổng TechnicianEarning từ tất cả booking
- TotalWithdrawn: Tổng TotalWithdrawn từ tất cả technician

### 2. Danh sách booking với thông tin tài chính
```
GET /api/FinancialReport/bookings
```
Trả về danh sách tất cả booking với thông tin tài chính chi tiết.

### 3. Danh sách technician với thông tin tài chính tổng hợp
```
GET /api/FinancialReport/technicians/summary
```
Trả về danh sách tất cả technician với thông tin tài chính tổng hợp.

### 4. Chi tiết tài chính của một technician
```
GET /api/FinancialReport/technicians/{technicianId}/details
```
Trả về chi tiết tài chính của một technician cụ thể, bao gồm danh sách booking của technician đó.

### 5. Danh sách booking của một technician
```
GET /api/FinancialReport/technicians/{technicianId}/bookings
```
Trả về danh sách booking của một technician cụ thể.

### 6. Các endpoint tổng hợp riêng lẻ

#### Tổng doanh thu
```
GET /api/FinancialReport/total-revenue
```

#### Tổng HoldingAmount
```
GET /api/FinancialReport/total-holding-amount
```

#### Tổng CommissionAmount
```
GET /api/FinancialReport/total-commission-amount
```

#### Tổng TechnicianEarning
```
GET /api/FinancialReport/total-technician-earning
```

#### Tổng TotalWithdrawn
```
GET /api/FinancialReport/total-withdrawn
```

## Cấu trúc dữ liệu

### FinancialSummaryDto
```json
{
  "totalRevenue": 1000000,
  "totalHoldingAmount": 200000,
  "totalCommissionAmount": 100000,
  "totalTechnicianEarning": 700000,
  "totalWithdrawn": 500000
}
```

### BookingFinancialDto
```json
{
  "id": "booking_id",
  "bookingCode": "BK001",
  "customerId": "customer_id",
  "technicianId": "technician_id",
  "serviceId": "service_id",
  "finalPrice": 100000,
  "holdingAmount": 20000,
  "commissionAmount": 10000,
  "technicianEarning": 70000,
  "createdAt": "2024-01-01T00:00:00Z",
  "status": "DONE",
  "paymentStatus": "PAID"
}
```

### TechnicianFinancialSummaryDto
```json
{
  "technicianId": "technician_id",
  "technicianName": "Nguyễn Văn A",
  "totalEarning": 500000,
  "totalCommissionPaid": 50000,
  "totalHoldingAmount": 100000,
  "totalWithdrawn": 300000,
  "totalBookings": 10
}
```

### TechnicianFinancialDto
```json
{
  "id": "technician_id",
  "userId": "user_id",
  "fullName": "Nguyễn Văn A",
  "email": "nguyenvana@example.com",
  "phone": "0123456789",
  "totalEarning": 500000,
  "totalCommissionPaid": 50000,
  "totalHoldingAmount": 100000,
  "totalWithdrawn": 300000,
  "bookings": [
    {
      "id": "booking_id",
      "bookingCode": "BK001",
      "finalPrice": 100000,
      "holdingAmount": 20000,
      "commissionAmount": 10000,
      "technicianEarning": 70000
    }
  ]
}
```

## Logic tính toán

1. **Doanh thu (TotalRevenue)**: Tổng FinalPrice của tất cả booking
2. **HoldingAmount**: Tổng HoldingAmount của tất cả booking = Tổng TotalHoldingAmount của tất cả technician
3. **CommissionAmount**: Tổng CommissionAmount của tất cả booking = Tổng TotalCommissionPaid của tất cả technician
4. **TechnicianEarning**: Tổng TechnicianEarning của tất cả booking
5. **TotalWithdrawn**: Tổng TotalWithdrawn của tất cả technician

## Sử dụng trong Frontend

### 1. Lấy tổng quan tài chính
```javascript
const response = await fetch('/api/FinancialReport/summary');
const data = await response.json();
const summary = data.data;
```

### 2. Lấy danh sách booking
```javascript
const response = await fetch('/api/FinancialReport/bookings');
const data = await response.json();
const bookings = data.data;
```

### 3. Lấy danh sách technician
```javascript
const response = await fetch('/api/FinancialReport/technicians/summary');
const data = await response.json();
const technicians = data.data;
```

### 4. Lấy chi tiết technician
```javascript
const technicianId = 'technician_id';
const response = await fetch(`/api/FinancialReport/technicians/${technicianId}/details`);
const data = await response.json();
const technician = data.data;
```

## Lưu ý

- Tất cả các endpoint đều trả về response với format `{ success: boolean, data: any, message?: string }`
- Các giá trị tiền tệ được trả về dưới dạng số (double)
- Các trường thời gian được trả về dưới dạng ISO 8601 string
- Các enum được trả về dưới dạng string 