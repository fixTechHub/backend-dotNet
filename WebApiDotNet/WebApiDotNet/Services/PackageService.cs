using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _repository;
        private readonly IMapper _mapper;

        public PackageService(IPackageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PackageDto>> GetAllAsync()
        {
            var packages = await _repository.GetAllAsync();
            return _mapper.Map<List<PackageDto>>(packages);
        }

        public async Task<PackageDto?> GetByIdAsync(string id)
        {
            var package = await _repository.GetByIdAsync(id);
            return package == null ? null : _mapper.Map<PackageDto>(package);
        }

        public async Task<PackageDto> CreateAsync(CreatePackageDto createPackageDto)
        {
            var package = _mapper.Map<Package>(createPackageDto);
            var createdPackage = await _repository.CreateAsync(package);
            return _mapper.Map<PackageDto>(createdPackage);
        }

        public async Task<PackageDto?> UpdateAsync(string id, UpdatePackageDto updatePackageDto)
        {
            var existingPackage = await _repository.GetByIdAsync(id);
            if (existingPackage == null)
                return null;

            // Update only non-null properties
            if (updatePackageDto.Name != null)
                existingPackage.Name = updatePackageDto.Name;
            if (updatePackageDto.Price.HasValue)
                existingPackage.Price = updatePackageDto.Price.Value;
            if (updatePackageDto.Description != null)
                existingPackage.Description = updatePackageDto.Description;
            if (updatePackageDto.Benefits != null)
                existingPackage.Benefits = updatePackageDto.Benefits;
            if (updatePackageDto.IsActive.HasValue)
                existingPackage.IsActive = updatePackageDto.IsActive.Value;

            var updatedPackage = await _repository.UpdateAsync(id, existingPackage);
            return updatedPackage == null ? null : _mapper.Map<PackageDto>(updatedPackage);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<PackageDto>> GetActivePackagesAsync()
        {
            var packages = await _repository.GetActivePackagesAsync();
            return _mapper.Map<List<PackageDto>>(packages);
        }

        public async Task<int> CountByMonthAsync(int year, int month)
        {
            return await _repository.CountByMonthAsync(year, month);
        }
    }
}