using Lucky9.Application.Common.Mappings;
using Lucky9.Domain.Entities;

namespace Lucky9.Application.TodoLists.Queries.ExportTodos;
public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; init; }

    public bool Done { get; init; }
}
