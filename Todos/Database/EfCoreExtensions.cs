using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todos.Database.Models;

namespace Todos.Database
{
    public static class EfCoreExtensions
    {
        public static async Task WrapInTransaction(this TodosDbContext todosDbContext, Action action, CancellationToken cancellationToken)
        {
            await using var transaction = await todosDbContext.Database.BeginTransactionAsync(cancellationToken);
            action();
            await todosDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        public static IQueryable<TodoList> TodoListsWithItems(this TodosDbContext todosDbContext)
        {
            return todosDbContext.TodoLists.Include(list => list.TodoItems);
        }
    }
}