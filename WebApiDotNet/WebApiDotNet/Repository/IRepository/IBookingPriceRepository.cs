using WebApiDotNet.Models;
using System;
using System.Threading.Tasks;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IBookingPriceRepository
    {
        Task<decimal> GetMonthlyRevenueAsync(int year, int month);
        // Có thể thêm các hàm CRUD khác nếu cần
    }
} 