namespace WebApiDotNet.DTOs
{
    public class PackageDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public List<string> Benefits { get; set; } = new();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreatePackageDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public List<string> Benefits { get; set; } = new();
        public bool IsActive { get; set; } = true;
    }

    public class UpdatePackageDto
    {
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        public List<string>? Benefits { get; set; }
        public bool? IsActive { get; set; }
    }
}