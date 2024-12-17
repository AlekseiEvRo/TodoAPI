namespace TodoAPI.Model
{
    /// <summary>
    /// Объект задачи
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        /// <example>Купить молоко</example>
        public string? Title { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        /// <example>Купить молоко в магазине на углу</example>
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Статус завершения задачи
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
