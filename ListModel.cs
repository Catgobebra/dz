using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    private readonly List<TItem> items;
    private readonly int undoLimit;
    private readonly LimitedSizeStack<ICommand<TItem>> historyOperation;

    public IReadOnlyList<TItem> Items => items.AsReadOnly();
    public int UndoLimit => undoLimit;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        this.items = items;
        this.undoLimit = undoLimit;
        this.historyOperation = new LimitedSizeStack<ICommand<TItem>>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        var command = new AddCommand<TItem>(items, item);
        command.Execute();
        historyOperation.Push(command);
    }

    public void RemoveItem(int index)
    {
        var command = new RemoveCommand<TItem>(items, index);
        command.Execute();
        historyOperation.Push(command);
    }

    public bool CanUndo()
    {
        return historyOperation.Count > 0;
    }

    public void Undo()
    {
        var command = historyOperation.Pop();
        command.Undo();
    }

    public void MoveUp(int index)
    {
        if (index <= 0 || index >= items.Count) return;
        var command = new MoveUpCommand<TItem>(items, index);
        command.Execute();
        historyOperation.Push(command);
    }

    public void MoveDown(int index)
    {
        if (index < 0 || index >= items.Count - 1) return;
        var command = new MoveDownCommand<TItem>(items, index);
        command.Execute();
        historyOperation.Push(command);
    }
}

public interface ICommand<T>
{
    void Execute();
    void Undo();
}

public class AddCommand<T> : ICommand<T>
{
    private readonly List<T> items;
    private readonly T item;
    private int index;

    public AddCommand(List<T> items, T item)
    {
        this.items = items;
        this.item = item;
    }

    public void Execute()
    {
        items.Add(item);
        index = items.Count - 1;
    }

    public void Undo()
    {
        items.RemoveAt(index);
    }
}

public class RemoveCommand<T> : ICommand<T>
{
    private readonly List<T> items;
    private readonly int index;
    private T removedItem;

    public RemoveCommand(List<T> items, int index)
    {
        this.items = items;
        this.index = index;
    }

    public void Execute()
    {
        removedItem = items[index];
        items.RemoveAt(index);
    }

    public void Undo()
    {
        items.Insert(index, removedItem);
    }
}
public class MoveUpCommand<T> : ICommand<T>
{
    private readonly List<T> items;
    private readonly int originalIndex;

    public MoveUpCommand(List<T> items, int index)
    {
        this.items = items;
        originalIndex = index;
    }

    public void Execute()
    {
        if (originalIndex > 0)
        {
            (items[originalIndex], items[originalIndex - 1]) =
                (items[originalIndex - 1], items[originalIndex]);
        }
    }

    public void Undo()
    {
        if (originalIndex > 0)
        {
            (items[originalIndex - 1], items[originalIndex]) =
                (items[originalIndex], items[originalIndex - 1]);
        }
    }
}

public class MoveDownCommand<T> : ICommand<T>
{
    private readonly List<T> items;
    private readonly int originalIndex;

    public MoveDownCommand(List<T> items, int index)
    {
        this.items = items;
        originalIndex = index;
    }

    public void Execute()
    {
        if (originalIndex < items.Count - 1)
        {
            (items[originalIndex], items[originalIndex + 1]) =
                (items[originalIndex + 1], items[originalIndex]);
        }
    }

    public void Undo()
    {
        if (originalIndex < items.Count - 1)
        {
            (items[originalIndex + 1], items[originalIndex]) =
                (items[originalIndex], items[originalIndex + 1]);
        }
    }
}