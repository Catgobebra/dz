using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;

namespace LimitedSizeStack.UI;

public partial class MainWindow : Window
{
    private readonly TaskListViewModel model;

    public MainWindow()
    {
        InitializeComponent();

        var list = new List<string> { "Составить список дел на сегодня", "Домашка по C#", "Решить задачу 1519" };
        var listModel = new ListModel<string>(list, 20);

        model = new TaskListViewModel(listModel);

        // Инициализация кнопок
        InitializeButtons();

        // Привязка данных
        TasksList.DataContext = model;

        // Обработчики событий
        TasksList.SelectionChanged += OnSelectionChanged;
        TextBox.KeyDown += OnTextBoxKeyDown;

        // Обновление UI
        UpdateAllButtons();
    }

    private void InitializeButtons()
    {
        // Добавить
        ButtonAdd.Content = "Добавить";
        ButtonAdd.Click += (_, __) => AddTask();

        // Отменить
        ButtonUndo.Content = "Отменить";
        ButtonUndo.Click += (_, __) => { if (model.CanUndo) model.Undo(); };

        // Удалить
        ButtonRemove.Content = "Удалить";
        ButtonRemove.Click += (_, __) =>
        {
            if (TasksList.SelectedIndex != -1)
                model.RemoveItem(TasksList.SelectedIndex);
        };

        // Вверх
        ButtonMoveUp.Content = "Вверх";
        ButtonMoveUp.Click += (_, __) =>
        {
            if (TasksList.SelectedIndex != -1)
                model.MoveUpItem(TasksList.SelectedIndex);
        };

        // Вниз
        ButtonMoveDown.Content = "Вниз";
        ButtonMoveDown.Click += (_, __) =>
        {
            if (TasksList.SelectedIndex != -1)
                model.MoveDownItem(TasksList.SelectedIndex);
        };
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        UpdateAllButtons();
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddTask();
            UpdateAllButtons();
            e.Handled = true;
        }
    }

    private void UpdateAllButtons()
    {
        UpdateUndo();
        UpdateMoveButtons();
        ButtonRemove.IsEnabled = TasksList.SelectedIndex != -1;
    }

    private void UpdateUndo()
    {
        ButtonUndo.IsEnabled = model.CanUndo;
    }

    private void UpdateMoveButtons()
    {
        var index = TasksList.SelectedIndex;
        ButtonMoveUp.IsEnabled = index > 0;
        ButtonMoveDown.IsEnabled = index >= 0 && index < model.Items.Length - 1;
    }

    private void AddTask()
    {
        model.AddItem(string.IsNullOrWhiteSpace(TextBox.Text) ? "(empty)" : TextBox.Text);
        TextBox.Text = "";
        UpdateAllButtons();
    }
}