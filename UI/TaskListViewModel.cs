using ReactiveUI;
using System.Linq;

namespace LimitedSizeStack.UI;

public class TaskListViewModel : ReactiveObject
{
    public string[] Items => items;
    public bool CanUndo => model.CanUndo();

    // Добавляем свойства для кнопок
    public bool CanMoveUp => SelectedIndex > 0;
    public bool CanMoveDown => SelectedIndex >= 0 && SelectedIndex < model.Items.Count - 1;

    private int _selectedIndex = -1;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedIndex, value);
            this.RaisePropertyChanged(nameof(CanMoveUp));
            this.RaisePropertyChanged(nameof(CanMoveDown));
        }
    }

    private readonly ListModel<string> model;
    private string[] items;

    public TaskListViewModel(ListModel<string> listModel)
    {
        model = listModel;
        Update();
    }

    public void AddItem(string item)
    {
        model.AddItem(item);
        Update();
    }

    public void RemoveItem(int index)
    {
        model.RemoveItem(index);
        Update();
    }

    public void Undo()
    {
        model.Undo();
        Update();
    }

    // Добавляем методы перемещения
    public void MoveUpItem(int index)
    {
        model.MoveUp(index);
        Update();
        SelectedIndex = index > 0 ? index - 1 : 0;
    }

    public void MoveDownItem(int index)
    {
        model.MoveDown(index);
        Update();
        SelectedIndex = index < model.Items.Count - 1 ? index + 1 : model.Items.Count - 1;
    }

    private void Update()
    {
        this.RaiseAndSetIfChanged(ref items, model.Items.ToArray(), nameof(Items));
        this.RaisePropertyChanged(nameof(CanUndo));
        this.RaisePropertyChanged(nameof(CanMoveUp));
        this.RaisePropertyChanged(nameof(CanMoveDown));
    }
}