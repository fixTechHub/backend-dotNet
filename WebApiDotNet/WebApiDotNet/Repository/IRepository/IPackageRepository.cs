using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IPackageRepository
    {
        Task<List<Package>> GetAllAsync();
        Task<Package?> GetByIdAsync(string id);
        Task<Package> CreateAsync(Package package);
        Task<Package?> UpdateAsync(string id, Package package);
        Task<bool> DeleteAsync(string id);
        Task<List<Package>> GetActivePackagesAsync();
        Task<int> CountByMonthAsync(int year, int month);
    }
}