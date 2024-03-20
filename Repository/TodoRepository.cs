using TodoProto;

namespace dotnet_webgrpc.Repository;

public class TodoRepository(ILogger<TodoRepository> logger): ITodoRepository
{
    private Todos _todos = new Todos
    {
        Todos_ =
        {
            new Todo()
            {
                Completed = true,
                Id = Guid.NewGuid().ToString(),
                Title = "Learn gRPC"
            }
        }
    };
    
    public Todo AddTodo(Todo todo)
    {
        todo.Id = Guid.NewGuid().ToString();
        logger.LogInformation("Adding todo with id {TodoId}", todo.Id);
        _todos.Todos_.Add(todo);
        return todo;
    }

    public Todos GetTodos()
    {
        return _todos;
    }

    public Todo? GetTodoById(string id)
    {
        return _todos.Todos_.FirstOrDefault(x => x.Id == id);
    }

    public Todo UpdateTodo(Todo todo)
    {
        var todoToUpdate = _todos.Todos_.FirstOrDefault(x => x.Id == todo.Id);
        if (todoToUpdate == null) return todo;
        todoToUpdate.Title = todo.Title;
        todoToUpdate.Completed = todo.Completed;

        return todo;
    }

    public bool DeleteTodoById(string id)
    {
        var todo = _todos.Todos_.FirstOrDefault(x => x.Id == id);
        if (todo == null) return false;
        _todos.Todos_.Remove(todo);
        return true;
    }

    public Todos ToggleIsCompleted(string id)
    {
        var todo = _todos.Todos_.FirstOrDefault(x => x.Id == id);
        if (todo != null)
        {
            todo.Completed = !todo.Completed;
        }

        return _todos;
    }
    
}

public interface ITodoRepository
{
    Todo AddTodo(Todo todo);
    Todos GetTodos();
    Todo? GetTodoById(string id);
    Todo UpdateTodo(Todo todo);
    
    bool DeleteTodoById(string id);
    
    
}