using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IActionLogRepository
    {
        Task<IEnumerable<ActionLog>> GetAllAsync();
        Task<ActionLog> GetByIdAsync(string id);
        Task<IEnumerable<ActionLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<ActionLog>> GetByFilterAsync(ActionLogFilterDto filter);
        Task<ActionLog> CreateAsync(ActionLog actionLog);
        Task<bool> DeleteAsync(string id);
        Task<long> GetCountByFilterAsync(ActionLogFilterDto filter);
        Task<IEnumerable<ActionLog>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ActionLog>> GetByActionTypeAsync(string actionType);
        Task<IEnumerable<ActionLog>> GetByStatusCodeAsync(int statusCode);
    }
} 