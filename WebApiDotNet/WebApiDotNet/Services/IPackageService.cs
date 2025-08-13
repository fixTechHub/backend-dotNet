using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IPackageService
    {
        Task<List<PackageDto>> GetAllAsync();
        Task<PackageDto?> GetByIdAsync(string id);
        Task<PackageDto> CreateAsync(CreatePackageDto createPackageDto);
        Task<PackageDto?> UpdateAsync(string id, UpdatePackageDto updatePackageDto);
        Task<bool> DeleteAsync(string id);
        Task<List<PackageDto>> GetActivePackagesAsync();
        Task<int> CountByMonthAsync(int year, int month);
    }
}