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

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetByIdAsync(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var now = DateTime.UtcNow;
            var category = new Category
            {
                CategoryName = dto.CategoryName,
                Icon = dto.Icon,
                IsActive = dto.IsActive,
                CreatedAt = now,
                UpdatedAt = now
            };
            var created = await _categoryRepository.CreateAsync(category);
            return _mapper.Map<CategoryDto>(created);
        }

        public async Task<bool> UpdateAsync(string id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;
            category.CategoryName = dto.CategoryName;
            category.Icon = dto.Icon;
            category.IsActive = dto.IsActive;
            category.UpdatedAt = DateTime.UtcNow;
            return await _categoryRepository.UpdateAsync(id, category);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
    }
}

