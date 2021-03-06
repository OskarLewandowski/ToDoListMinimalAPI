using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoListMinimalAPI;

public static class ToDoRequests
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("todos", ToDoRequests.GetAll)
            .Produces<List<ToDo>>()
            .WithTags("To Dos")
            .RequireAuthorization();

        app.MapGet("todos/{id}", ToDoRequests.GetById)
            .Produces<ToDo>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("To Dos")
            .AllowAnonymous();

        app.MapPost("todos", ToDoRequests.Create)
            .Produces<ToDo>(StatusCodes.Status201Created)
            .Accepts<ToDo>("application/json")
            .WithTags("To Dos")
            .WithValidator<ToDo>();

        app.MapPut("todos/{id}", ToDoRequests.Update)
            .Produces<ToDo>(StatusCodes.Status204NoContent)
            .Produces<ToDo>(StatusCodes.Status404NotFound)
            .Accepts<ToDo>("application/json")
            .WithTags("To Dos")
            .WithValidator<ToDo>();

        app.MapDelete("todos/{id}", ToDoRequests.Delete)
            .Produces<ToDo>(StatusCodes.Status204NoContent)
            .Produces<ToDo>(StatusCodes.Status404NotFound)
            .WithTags("To Dos");

        //.ExcludeFromDescription();


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

    [Authorize]
    public static IResult Create([FromServices] IToDoService service, [FromBody] ToDo toDo)
    {

        service.Create(toDo);

        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    [AllowAnonymous]
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
