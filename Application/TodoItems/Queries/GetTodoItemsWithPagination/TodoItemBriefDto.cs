using Lucky9.Application.Common.Mappings;
using Lucky9.Domain.Entities;

namespace Lucky9.Application.TodoItems.Queries.GetTodoItemsWithPagination;
public class TodoItemBriefDto : IMapFrom<TodoItem>
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
}
