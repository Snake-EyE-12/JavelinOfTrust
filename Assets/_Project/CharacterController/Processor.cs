using System.Collections.Generic;
using UnityEngine;
public interface Processor<T>
{
    public void Process(T data);
    public Processor<T> SetNext(Processor<T> next);
}
public abstract class BaseProcessor<T> : Processor<T>
{
    public Processor<T> nextProcessor;
    public virtual Processor<T> SetNext(Processor<T> next) => nextProcessor = next;
    public virtual void Process(T data) => nextProcessor?.Process(data);
}
public abstract class DiscreteProcessor<T> : BaseProcessor<T>
{
    protected List<Processor<T>> processors = new List<Processor<T>>();
    public virtual Processor<T> SetNext(Processor<T> next)
    {
        processors.Add(next);
        return this;
    }
    public sealed override void Process(T data)
    {
        processors.ForEach((x) => x.Process(data));
    }
    public abstract void Operate(T data);
}