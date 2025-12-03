using SQLite;
namespace CetTodoApp.Data;

public class TodoItem
{
    [PrimaryKey, AutoIncrement]
    public int Id {get; set;}
    public string? Title {get; set;}
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate {get; set;}
    public bool IsComplete { get; set; }
    public bool IsOverdue => !IsComplete && DueDate < DateTime.Now;

    public TodoItem()
    {
        CreatedDate = DateTime.Now;
        IsComplete = false;
        Title = "";
    }
    
}