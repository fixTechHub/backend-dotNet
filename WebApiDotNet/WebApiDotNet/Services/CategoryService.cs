using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using System;
using System.Linq;
using AutoMapper;

namespace WebApiDotNet.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(categories.ToList());
        }

        public async Task<List<CategoryDto>> GetDeletedAsync()
        {
            var categories = await _categoryRepository.GetDeletedAsync();
            return _mapper.Map<List<CategoryDto>>(categories.ToList());
        }

        public async Task<CategoryDto> GetByIdAsync(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            var created = await _categoryRepository.CreateAsync(category);
            return _mapper.Map<CategoryDto>(created);
        }

        public async Task<CategoryDto> UpdateAsync(string id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) throw new Exception("Không tìm thấy danh mục");
            _mapper.Map(dto, category);
            category.UpdatedAt = DateTime.UtcNow;
            var updated = await _categoryRepository.UpdateAsync(id, category);
            return _mapper.Map<CategoryDto>(updated);
        }

        public async Task DeleteAsync(string id)
        {
            if (!await _categoryRepository.ExistsAsync(id))
                throw new Exception("Không tìm thấy danh mục");
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task RestoreAsync(string id)
        {
            if (!await _categoryRepository.ExistsAsync(id))
                throw new Exception("Không tìm thấy danh mục đã xóa");
            await _categoryRepository.RestoreAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _categoryRepository.ExistsAsync(id);
        }
    }
}

