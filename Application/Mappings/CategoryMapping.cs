using Application.DTOs;
using Core.Entities;

namespace Application.Mappings;

public static class CategoryMapping
{
    public static CategoryDto ToDto(this Category c) => new() { Id = c.Id, Name = c.Name };

    public static Category ToEntity(this CreateCategoryDto dto) => new() { Name = dto.Name };
}
