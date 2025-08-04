using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;
using WebApiDotNet.Attributes;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[RequireAdmin]
    public class ActionLogController : ControllerBase
    {
        private readonly ActionLogService _actionLogService;

        public ActionLogController(ActionLogService actionLogService)
        {
            _actionLogService = actionLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActionLogDto>>> GetAll([FromQuery] ActionLogFilterDto filter)
        {
            try
            {
                var actionLogs = await _actionLogService.GetByFilterAsync(filter);
                var actionLogDtos = new List<ActionLogDto>();

                foreach (var log in actionLogs)
                {
                    actionLogDtos.Add(new ActionLogDto
                    {
                        Id = log.Id,
                        UserId = log.UserId,
                        ActionType = log.ActionType,
                        Method = log.Method,
                        Route = log.Route,
                        Params = log.Params,
                        Query = log.Query,
                        Body = log.Body,
                        StatusCode = log.StatusCode,
                        Ip = log.Ip,
                        UserAgent = log.UserAgent,
                        Description = log.Description,
                        CreatedAt = log.CreatedAt
                    });
                }

                return Ok(actionLogDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActionLogDto>> GetById(string id)
        {
            try
            {
                var actionLog = await _actionLogService.GetByIdAsync(id);
                if (actionLog == null)
                {
                    return NotFound(new { message = "Action log not found" });
                }

                var actionLogDto = new ActionLogDto
                {
                    Id = actionLog.Id,
                    UserId = actionLog.UserId,
                    ActionType = actionLog.ActionType,
                    Method = actionLog.Method,
                    Route = actionLog.Route,
                    Params = actionLog.Params,
                    Query = actionLog.Query,
                    Body = actionLog.Body,
                    StatusCode = actionLog.StatusCode,
                    Ip = actionLog.Ip,
                    UserAgent = actionLog.UserAgent,
                    Description = actionLog.Description,
                    CreatedAt = actionLog.CreatedAt
                };

                return Ok(actionLogDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ActionLogDto>>> GetByUserId(string userId)
        {
            try
            {
                var actionLogs = await _actionLogService.GetByUserIdAsync(userId);
                var actionLogDtos = new List<ActionLogDto>();

                foreach (var log in actionLogs)
                {
                    actionLogDtos.Add(new ActionLogDto
                    {
                        Id = log.Id,
                        UserId = log.UserId,
                        ActionType = log.ActionType,
                        Method = log.Method,
                        Route = log.Route,
                        Params = log.Params,
                        Query = log.Query,
                        Body = log.Body,
                        StatusCode = log.StatusCode,
                        Ip = log.Ip,
                        UserAgent = log.UserAgent,
                        Description = log.Description,
                        CreatedAt = log.CreatedAt
                    });
                }

                return Ok(actionLogDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("action-type/{actionType}")]
        public async Task<ActionResult<IEnumerable<ActionLogDto>>> GetByActionType(string actionType)
        {
            try
            {
                var actionLogs = await _actionLogService.GetByActionTypeAsync(actionType);
                var actionLogDtos = new List<ActionLogDto>();

                foreach (var log in actionLogs)
                {
                    actionLogDtos.Add(new ActionLogDto
                    {
                        Id = log.Id,
                        UserId = log.UserId,
                        ActionType = log.ActionType,
                        Method = log.Method,
                        Route = log.Route,
                        Params = log.Params,
                        Query = log.Query,
                        Body = log.Body,
                        StatusCode = log.StatusCode,
                        Ip = log.Ip,
                        UserAgent = log.UserAgent,
                        Description = log.Description,
                        CreatedAt = log.CreatedAt
                    });
                }

                return Ok(actionLogDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("status-code/{statusCode}")]
        public async Task<ActionResult<IEnumerable<ActionLogDto>>> GetByStatusCode(int statusCode)
        {
            try
            {
                var actionLogs = await _actionLogService.GetByStatusCodeAsync(statusCode);
                var actionLogDtos = new List<ActionLogDto>();

                foreach (var log in actionLogs)
                {
                    actionLogDtos.Add(new ActionLogDto
                    {
                        Id = log.Id,
                        UserId = log.UserId,
                        ActionType = log.ActionType,
                        Method = log.Method,
                        Route = log.Route,
                        Params = log.Params,
                        Query = log.Query,
                        Body = log.Body,
                        StatusCode = log.StatusCode,
                        Ip = log.Ip,
                        UserAgent = log.UserAgent,
                        Description = log.Description,
                        CreatedAt = log.CreatedAt
                    });
                }

                return Ok(actionLogDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<ActionLogDto>>> GetByDateRange([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            try
            {
                var actionLogs = await _actionLogService.GetByDateRangeAsync(fromDate, toDate);
                var actionLogDtos = new List<ActionLogDto>();

                foreach (var log in actionLogs)
                {
                    actionLogDtos.Add(new ActionLogDto
                    {
                        Id = log.Id,
                        UserId = log.UserId,
                        ActionType = log.ActionType,
                        Method = log.Method,
                        Route = log.Route,
                        Params = log.Params,
                        Query = log.Query,
                        Body = log.Body,
                        StatusCode = log.StatusCode,
                        Ip = log.Ip,
                        UserAgent = log.UserAgent,
                        Description = log.Description,
                        CreatedAt = log.CreatedAt
                    });
                }

                return Ok(actionLogDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ActionLogDto>> Create([FromBody] CreateActionLogDto createDto)
        {
            try
            {
                var actionLog = await _actionLogService.CreateAsync(createDto);
                var actionLogDto = new ActionLogDto
                {
                    Id = actionLog.Id,
                    UserId = actionLog.UserId,
                    ActionType = actionLog.ActionType,
                    Method = actionLog.Method,
                    Route = actionLog.Route,
                    Params = actionLog.Params,
                    Query = actionLog.Query,
                    Body = actionLog.Body,
                    StatusCode = actionLog.StatusCode,
                    Ip = actionLog.Ip,
                    UserAgent = actionLog.UserAgent,
                    Description = actionLog.Description,
                    CreatedAt = actionLog.CreatedAt
                };

                return CreatedAtAction(nameof(GetById), new { id = actionLog.Id }, actionLogDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var result = await _actionLogService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Action log not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<long>> GetCount([FromQuery] ActionLogFilterDto filter)
        {
            try
            {
                var count = await _actionLogService.GetCountByFilterAsync(filter);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
} 