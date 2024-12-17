using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.DAL;
using TodoAPI.Model;

namespace TodoAPI.Controllers
{
    /// <summary>
    /// Контроллер с CRUD-методами для TodoItem
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Создание задачи
        /// </summary>
        /// <param name="todoItem">Объект задачи</param>
        /// <response code="201">Успешное создание задачи</response>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem todoItem)
        {
            if (todoItem == null)
                return BadRequest("Invalid data.");

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = todoItem.Id }, todoItem);
        }


        /// <summary>
        /// Получение списка задач с пагинацией 
        /// </summary>
        /// <param name="page">Номер страницы</param>
        /// <param name="size">Количество элементов на странице</param>
        /// <response code="200">Возвращает объект с метаданными пагинации и списком задач на текущей странице.</response>
        /// <response code="400">Введены некорректные параметры запроса</response>
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetPagedTodos(int page = 1, int size = 10)
        {
            if (page < 1 || size < 1)
                return BadRequest("Page number and size must be greater than zero.");
        
            var totalItems = await _context.TodoItems.CountAsync();

            var items = await _context.TodoItems
                .OrderBy(t => t.Id) 
                .Skip((page - 1) * size)
                .Take(size) 
                .ToListAsync();

            // Формируем ответ с метаданными
            var response = new
            {
                Page = page,
                Size = size,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)size),
                Data = items
            };

            return Ok(response);
        }


        /// <summary>
        /// Получить задачу по ID
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <response code="200">Возвращает задачу</response>
        /// <response code="404">Задача не найдена</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
                return NotFound(); 

            return todoItem;
        }

        /// <summary>
        /// Обновление задачи по ID.
        /// </summary>
        /// <param name="id">Идентификатор задачи, которую нужно обновить.</param>
        /// <param name="todoItem">Объект задачи, содержащий обновленные данные.</param>
        /// <response code="400">Id не совпадает с данными в запросе</response>
        /// <response code="404">Задача на найдена</response>
        /// <response code="200">Задача успешно обновлена</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
                return BadRequest("ID mismatch.");

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id">Id задачи</param>
        /// <response code="404">Задача на найдена</response>
        /// <response code="204">Задача успешно удалена</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
                return NotFound();

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
