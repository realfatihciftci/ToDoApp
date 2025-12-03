using CetTodoApp.Data;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
    private readonly TodoDatabase _database;
    public MainPage(TodoDatabase database)
    {
        InitializeComponent();
        _database = database;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshListView();
    }

    private async void AddButton_OnClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TodoTitleEntry.Text))
        {
            await DisplayAlert("Error", "Title is empty", "Ok");
            return;
        }

        var newItem = new TodoItem
        {
            Title = TodoTitleEntry.Text,
            DueDate = DueDate.Date,
            CreatedDate = DateTime.Now,
        };
        await _database.SaveItemAsync(newItem);

        TodoTitleEntry.Text = string.Empty;
        DueDate.Date = DateTime.Now;

        await RefreshListView();
    }
    
    private async Task RefreshListView()
    {
        var allItems = await _database.GetItemsAsync();

        // Filtreleme ve sıralama
        TasksListView.ItemsSource = allItems
            .Where(x => !x.IsComplete || (x.IsComplete && x.DueDate > DateTime.Now.AddDays(-1)))
            .OrderBy(x => x.IsComplete)
            .ThenBy(x => x.DueDate)
            .ToList();
    }

    private async void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is TodoItem item)
        {
            item.IsComplete = !item.IsComplete;
            await _database.SaveItemAsync(item);
            await RefreshListView();
            TasksListView.SelectedItem = null;
        }
    }
}