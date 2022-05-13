using Microsoft.AspNetCore.Mvc;

namespace ToDoListMinimalAPI;

public static class ToDoRequests
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("todos", ToDoRequests.GetAll);
        app.MapGet("todos/{id}", ToDoRequests.GetById);
        app.MapPost("todos", ToDoRequests.Create);
        app.MapPut("todos/{id}", ToDoRequests.Update);
        app.MapDelete("todos/{id}", ToDoRequests.Delete);

        return app;
    }

    public static IResult GetAll([FromServices] IToDoService service)
    {
        var todos = service.GetAll();
        return Results.Ok(todos);
    }

    public static IResult GetById([FromServices] IToDoService service, [FromRoute] Guid id)
    {
        var todos = service.GetById(id);

        if (todos == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(todos);
    }

    public static IResult Create([FromServices] IToDoService service, [FromBody] ToDo toDo)
    {
        service.Create(toDo);

        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    public static IResult Update([FromServices] IToDoService service, [FromRoute] Guid id, [FromBody] ToDo toDo)
    {
        var existingToDo = service.GetById(id);

        if (existingToDo == null)
        {
            return Results.NotFound();
        }

        service.Update(toDo);
        return Results.NoContent();
    }

    public static IResult Delete([FromServices] IToDoService service, [FromRoute] Guid id)
    {
        var existingToDo = service.GetById(id);

        if (existingToDo == null)
        {
            return Results.NotFound();
        }

        service.Delete(id);
        return Results.NoContent();
    }

}
