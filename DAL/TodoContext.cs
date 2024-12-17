using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoAPI.Model;

namespace TodoAPI.DAL
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
    }
}
