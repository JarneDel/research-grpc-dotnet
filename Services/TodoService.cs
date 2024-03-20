using dotnet_webgrpc.Repository;
using Grpc.Core;
using TodoProto;

namespace dotnet_webgrpc.Services;

public class TodoService(ILogger<TodoService> logger, ITodoRepository todoRepository)
    : TodoProto.TodoService.TodoServiceBase
{
    public override Task<Todos> GetTodos(Empty request, ServerCallContext context)
    {
        logger.LogInformation("GetTodos called");
        return Task.FromResult(todoRepository.GetTodos());
    }

    public override Task<Todo> AddTodo(newTodo request, ServerCallContext context)
    {
        logger.LogInformation("AddTodo called");

        var todo = new Todo
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            Completed = false
        };

        var result = Task.FromResult(todoRepository.AddTodo(todo));

        return result;
    }

    public override Task<Todo> UpdateTodo(updateTodo request, ServerCallContext context)
    {
        var updatedTodo = new Todo
        {
            Completed = request.Completed,
            Title = request.Title,
            Id = request.Id
        };
        return Task.FromResult(todoRepository.UpdateTodo(updatedTodo));
    }

    public override Task<Empty> DeleteTodo(Todo request, ServerCallContext context)
    {
        return todoRepository.DeleteTodoById(request.Id)
            ? Task.FromResult(new Empty())
            : throw new RpcException(new Status(StatusCode.NotFound, "Todo not found"));
    }

    public override Task<Todo> GetTodo(getTodo request, ServerCallContext context)
    {
        var todo = todoRepository.GetTodoById(request.Id);
        if (todo == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Todo not found"));
        }
        return Task.FromResult(todo);
    }
    
}