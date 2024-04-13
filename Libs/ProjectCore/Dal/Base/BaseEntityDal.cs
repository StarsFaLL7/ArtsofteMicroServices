using System.ComponentModel.DataAnnotations;

namespace ProjectCore.Dal.Base;

/// <summary>
/// Базовая сущность для работы с сущностями в бд
/// </summary>
/// <typeparam name="T">тип идентификатор</typeparam>
public record BaseEntityDal<T>
{
    /// <summary>
    /// уникальный идентфиикатор сущности
    /// </summary>
    [Key]
    public required T Id { get; init; }
}