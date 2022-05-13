namespace ToDoListMinimalAPI;

public class ToDo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; }
    public bool IsCompleted { get; set; }
    public string WhenAddedTime { get; set; } = DateTime.Now.ToLongDateString();
}
