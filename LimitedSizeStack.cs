using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private LinkedList<T> Stack = new();
    private readonly int Limit;

    public LimitedSizeStack(int undoLimit)
    {
        this.Limit = undoLimit;
    }

    public void Push(T addItem)
    {
        if (Limit == 0) return;

        if (Count >= Limit)
            Stack.RemoveLast();

        Stack.AddFirst(addItem);
    }

    public T Pop()
    {
        if (Stack.Count == 0)
            throw new InvalidOperationException("Stack empty");

        var removedItem = Stack.First!;
        Stack.RemoveFirst();
        return removedItem.Value;
    }

    public int Count => Stack.Count;
}