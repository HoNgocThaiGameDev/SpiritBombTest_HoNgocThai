using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> where T : class, new()
{
    private readonly Stack<T> objectStack;
    private readonly HashSet<T> storedObjects;
    private readonly Action<T> resetAction;
    private readonly Action<T> onetimeInitAction;
    private readonly Action<T> releaseAction;

    public ObjectPooling(
        int initialBufferSize,
        Action<T> resetAction = null,
        Action<T> onetimeInitAction = null,
        Action<T> releaseAction = null)
    {
        objectStack = new Stack<T>(initialBufferSize);
        storedObjects = new HashSet<T>();
        this.resetAction = resetAction;
        this.onetimeInitAction = onetimeInitAction;
        this.releaseAction = releaseAction ?? DeactivateUnityObject;
    }

    public void InitBuffer(int initialBufferSize)
    {
        if (onetimeInitAction == null)
            throw new InvalidOperationException("Pool cannot preload without an initialization action.");

        for (int i = 0; i < initialBufferSize; i++)
            onetimeInitAction(null);
    }

    public T New()
    {
        if (objectStack.Count == 0)
        {
            if (onetimeInitAction == null)
                throw new InvalidOperationException("Pool is empty and has no initialization action.");

            onetimeInitAction(null);
        }

        if (objectStack.Count == 0)
            throw new InvalidOperationException("Pool initialization did not store an object.");

        T item = objectStack.Pop();
        storedObjects.Remove(item);
        if (resetAction != null)
            resetAction(item);

        return item;
    }

    public void Store(T obj)
    {
        if (obj == null || !storedObjects.Add(obj))
            return;

        releaseAction(obj);
        objectStack.Push(obj);
    }

    public int Count
    {
        get { return objectStack.Count; }
    }

    private static void DeactivateUnityObject(T obj)
    {
        Component component = obj as Component;
        if (component != null && component.gameObject.activeSelf)
            component.gameObject.SetActive(false);
    }
}
