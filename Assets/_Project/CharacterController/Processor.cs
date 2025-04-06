using UnityEngine;public interface Processor<T>
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