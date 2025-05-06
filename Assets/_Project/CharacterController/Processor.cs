using System.Collections.Generic;
using CharacterProcess;
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
public abstract class OperationalProcessor<T>
{
    public abstract void Operate(T data);
}

public class OperationalController<T>
{
    private List<OperationalProcessor<T>> processors = new ();
    public OperationalController<T> SetNext(OperationalProcessor<T> next)
    {
        processors.Add(next);
        return this;
    }

    public void Process(T data)
    {
        processors.ForEach((x) => x.Operate(data));
    }
}