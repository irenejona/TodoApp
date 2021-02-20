using Microsoft.EntityFrameworkCore;
using Todos.Database.Models;

namespace Todos.Database
{
    public class TodosDbContext : DbContext
    {
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        
        public TodosDbContext(DbContextOptions<TodosDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoList>(list =>
            {
                list.HasKey(l => l.Id);
                list.Property(l => l.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                list.Property(l => l.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<TodoItem>(item =>
            {
                item.HasKey(i => i.Id);
                item.Property(i => i.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                item.Property(i => i.Name)
                    .IsRequired();

                item.Property(i => i.Description)
                    .IsRequired(false);

                item.HasOne(i => i.TodoList)
                    .WithMany(l => l.TodoItems)
                    .HasForeignKey(i => i.TodoListId)
                    .IsRequired();
            });
        }
    }
}