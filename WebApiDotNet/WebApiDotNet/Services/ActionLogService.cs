using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class ActionLogService
    {
        private readonly IActionLogRepository _actionLogRepository;

        public ActionLogService(IActionLogRepository actionLogRepository)
        {
            _actionLogRepository = actionLogRepository;
        }

        public async Task<IEnumerable<ActionLog>> GetAllAsync()
        {
            return await _actionLogRepository.GetAllAsync();
        }

        public async Task<ActionLog> GetByIdAsync(string id)
        {
            return await _actionLogRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ActionLog>> GetByUserIdAsync(string userId)
        {
            return await _actionLogRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<ActionLog>> GetByFilterAsync(ActionLogFilterDto filter)
        {
            return await _actionLogRepository.GetByFilterAsync(filter);
        }

        public async Task<ActionLog> CreateAsync(CreateActionLogDto createDto)
        {
            var actionLog = new ActionLog
            {
                UserId = createDto.UserId,
                ActionType = createDto.ActionType,
                Method = createDto.Method,
                Route = createDto.Route,
                Params = createDto.Params,
                Query = createDto.Query,
                Body = createDto.Body,
                StatusCode = createDto.StatusCode,
                Ip = createDto.Ip,
                UserAgent = createDto.UserAgent,
                Description = createDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            return await _actionLogRepository.CreateAsync(actionLog);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _actionLogRepository.DeleteAsync(id);
        }

        public async Task<long> GetCountByFilterAsync(ActionLogFilterDto filter)
        {
            return await _actionLogRepository.GetCountByFilterAsync(filter);
        }

        public async Task<IEnumerable<ActionLog>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _actionLogRepository.GetByDateRangeAsync(fromDate, toDate);
        }

        public async Task<IEnumerable<ActionLog>> GetByActionTypeAsync(string actionType)
        {
            return await _actionLogRepository.GetByActionTypeAsync(actionType);
        }

        public async Task<IEnumerable<ActionLog>> GetByStatusCodeAsync(int statusCode)
        {
            return await _actionLogRepository.GetByStatusCodeAsync(statusCode);
        }

        // Helper method để log action
        public async Task LogActionAsync(string userId, string actionType, string method, string route, 
            object parameters = null, object query = null, object body = null, 
            int statusCode = 200, string ip = null, string userAgent = null, string description = null)
        {
            var createDto = new CreateActionLogDto
            {
                UserId = userId,
                ActionType = actionType,
                Method = method,
                Route = route,
                Params = parameters,
                Query = query,
                Body = body,
                StatusCode = statusCode,
                Ip = ip,
                UserAgent = userAgent,
                Description = description
            };

            await CreateAsync(createDto);
        }
    }
} 