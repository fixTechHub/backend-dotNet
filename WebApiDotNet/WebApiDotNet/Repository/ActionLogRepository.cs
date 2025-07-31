using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Data;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class ActionLogRepository : IActionLogRepository
    {
        private readonly IMongoCollection<ActionLog> _actionLogs;

        public ActionLogRepository(MongoDbContext context)
        {
            _actionLogs = context.ActionLogs;
        }

        public async Task<IEnumerable<ActionLog>> GetAllAsync()
        {
            return await _actionLogs.Find(_ => true).ToListAsync();
        }

        public async Task<ActionLog> GetByIdAsync(string id)
        {
            return await _actionLogs.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ActionLog>> GetByUserIdAsync(string userId)
        {
            return await _actionLogs.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<ActionLog>> GetByFilterAsync(ActionLogFilterDto filter)
        {
            var filterBuilder = Builders<ActionLog>.Filter;
            var filterDefinition = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                filterDefinition &= filterBuilder.Eq(x => x.UserId, filter.UserId);
            }

            if (!string.IsNullOrEmpty(filter.ActionType))
            {
                filterDefinition &= filterBuilder.Eq(x => x.ActionType, filter.ActionType);
            }

            if (!string.IsNullOrEmpty(filter.Method))
            {
                filterDefinition &= filterBuilder.Eq(x => x.Method, filter.Method);
            }

            if (!string.IsNullOrEmpty(filter.Route))
            {
                filterDefinition &= filterBuilder.Eq(x => x.Route, filter.Route);
            }

            if (!string.IsNullOrEmpty(filter.Ip))
            {
                filterDefinition &= filterBuilder.Eq(x => x.Ip, filter.Ip);
            }

            if (filter.StatusCode.HasValue)
            {
                filterDefinition &= filterBuilder.Eq(x => x.StatusCode, filter.StatusCode.Value);
            }

            if (filter.FromDate.HasValue)
            {
                filterDefinition &= filterBuilder.Gte(x => x.CreatedAt, filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                filterDefinition &= filterBuilder.Lte(x => x.CreatedAt, filter.ToDate.Value);
            }

            var sortDefinition = Builders<ActionLog>.Sort.Descending(x => x.CreatedAt);
            var skip = (filter.Page - 1) * filter.PageSize;

            return await _actionLogs.Find(filterDefinition)
                .Sort(sortDefinition)
                .Skip(skip)
                .Limit(filter.PageSize)
                .ToListAsync();
        }

        public async Task<ActionLog> CreateAsync(ActionLog actionLog)
        {
            await _actionLogs.InsertOneAsync(actionLog);
            return actionLog;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _actionLogs.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<long> GetCountByFilterAsync(ActionLogFilterDto filter)
        {
            var filterBuilder = Builders<ActionLog>.Filter;
            var filterDefinition = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                filterDefinition &= filterBuilder.Eq(x => x.UserId, filter.UserId);
            }

            if (!string.IsNullOrEmpty(filter.ActionType))
            {
                filterDefinition &= filterBuilder.Eq(x => x.ActionType, filter.ActionType);
            }

            if (!string.IsNullOrEmpty(filter.Method))
            {
                filterDefinition &= filterBuilder.Eq(x => x.Method, filter.Method);
            }

            if (!string.IsNullOrEmpty(filter.Route))
            {
                filterDefinition &= filterBuilder.Eq(x => x.Route, filter.Route);
            }

            if (!string.IsNullOrEmpty(filter.Ip))
            {
                filterDefinition &= filterBuilder.Eq(x => x.Ip, filter.Ip);
            }

            if (filter.StatusCode.HasValue)
            {
                filterDefinition &= filterBuilder.Eq(x => x.StatusCode, filter.StatusCode.Value);
            }

            if (filter.FromDate.HasValue)
            {
                filterDefinition &= filterBuilder.Gte(x => x.CreatedAt, filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                filterDefinition &= filterBuilder.Lte(x => x.CreatedAt, filter.ToDate.Value);
            }

            return await _actionLogs.CountDocumentsAsync(filterDefinition);
        }

        public async Task<IEnumerable<ActionLog>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _actionLogs.Find(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionLog>> GetByActionTypeAsync(string actionType)
        {
            return await _actionLogs.Find(x => x.ActionType == actionType)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionLog>> GetByStatusCodeAsync(int statusCode)
        {
            return await _actionLogs.Find(x => x.StatusCode == statusCode)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
} 