using SQLite;
namespace CetTodoApp.Data;

public class TodoDatabase
{
    private SQLiteAsyncConnection _database;

    private const string DbName = "TodoDatabase.db3";
    private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);

    private async Task Init()
    {
        if (_database != null)
            return;
        
        _database = new SQLiteAsyncConnection(DbPath);
        await _database.CreateTableAsync<TodoItem>();
    }
    
    public async Task<List<TodoItem>> GetItemsAsync()
    {
        await Init();
        return await _database.Table<TodoItem>().ToListAsync();
    }

    public async Task<int> SaveItemAsync(TodoItem item)
    {
        await Init();
        if (item.Id != 0)
            return await _database.UpdateAsync(item);
        else
            return await _database.InsertAsync(item);
    }

    public async Task<int> DeleteItemAsync(TodoItem item)
    {
        await Init();
        return await _database.DeleteAsync(item);
    }
}