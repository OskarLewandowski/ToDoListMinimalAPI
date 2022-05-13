using FluentValidation;

namespace ToDoListMinimalAPI;

public class ToDoValidator : AbstractValidator<ToDo>
{
    public ToDoValidator()
    {
        RuleFor(r => r.Value)
            .NotEmpty()
            .MinimumLength(5)
            .WithMessage("Value of todo must be at least 5 characters");
    }
}
