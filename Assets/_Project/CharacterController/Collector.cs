using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class Collector<T> : MonoBehaviour where T : MonoBehaviour
{
    protected List<T> collection = new();
    public abstract void OnCollect(T item);
    public abstract void OnDrop(T item);
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out T collected))
        {
            collection.Add(collected);
            OnCollect(collected);
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out T collected))
        {
            collection.Remove(collected);
            OnDrop(collected);
        }
    }
    public T GetClosest()
    {
        return GetNearestTo(transform.position);;
    }

    public T GetNearestTo(Vector2 position)
    {
        T closest = default(T);
        float closestDistance = float.MaxValue;
        foreach (var item in collection)
        {
            var distance = Vector2.Distance(position, item.transform.position);
            if (distance < closestDistance)
            {
                closest = item;
                closestDistance = distance;
            }
        }
        return closest;
        
    }
    public int Count => collection.Count;
}