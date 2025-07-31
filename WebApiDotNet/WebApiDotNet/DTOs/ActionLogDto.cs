using System;

namespace WebApiDotNet.DTOs
{
    public class ActionLogDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ActionType { get; set; }
        public string Method { get; set; }
        public string Route { get; set; }
        public object Params { get; set; }
        public object Query { get; set; }
        public object Body { get; set; }
        public int StatusCode { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateActionLogDto
    {
        public string UserId { get; set; }
        public string ActionType { get; set; }
        public string Method { get; set; }
        public string Route { get; set; }
        public object Params { get; set; }
        public object Query { get; set; }
        public object Body { get; set; }
        public int StatusCode { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string Description { get; set; }
    }

    public class ActionLogFilterDto
    {
        public string? UserId { get; set; }
        public string? ActionType { get; set; }
        public string? Method { get; set; }
        public string? Route { get; set; }
        public string? Ip { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 