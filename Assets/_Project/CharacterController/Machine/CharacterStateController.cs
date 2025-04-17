using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStateController : MonoBehaviour, ICharacterStateController
{
    public ICharacterSettingsData CharacterSettingsData { get; set; }
    public ICharacterState CurrentState { get; set; }
    
    [SerializeField] private InterfaceReference<ICharacterState>[] states;

    [SerializeField] private FeatureItemContainer features = new (
        new CharacterModule[]
        {
            new SlopeControl(),
            new TerminalVelocity()
        });

    private void Awake()
    {
        CurrentState = null; //Get Initial State
    }

    private void Update()
    {
        foreach (var state in states)
        {
            state.Value.Upkeep(this);
        }
        if(CurrentState != null) CurrentState.Tick(this);
        
    }
    

    public void ChangeState(ICharacterState newState)
    {
        if(CurrentState == null || CurrentState.CanTransitionTo(newState)) MakeTransition(newState);
    }

    private void MakeTransition(ICharacterState newState)
    {
        if(CurrentState != null) CurrentState.Exit(this);
        CurrentState = newState;
        if(CurrentState != null) CurrentState.Enter(this);
    }
}

public interface ICharacterStateController
{
    public ICharacterSettingsData CharacterSettingsData { get; set; }
    public void ChangeState(ICharacterState newState);
}
public interface ICharacterState
{
    public void Tick(ICharacterStateController controller);
    public void Enter(ICharacterStateController controller);
    public void Exit(ICharacterStateController controller);
    public bool CanTransitionTo(ICharacterState state);
    public void Upkeep(ICharacterStateController controller);
}

public interface ICharacterSettingsData
{
    
}


public abstract class CharacterModule
{
    public bool active;
    public string Name => GetType().Name;
    protected CharacterModuleProcessor chain;
    public abstract void BuildChain();
    public abstract void Process(ICharacterSettingsData data);
}

[Serializable]
public class SlopeControl : CharacterModule
{
    public override void BuildChain()
    {
        //chain = null;
        //chain.SetNext(null);

    }
    public override void Process(ICharacterSettingsData data)
    {
        chain.Process(data);
    }
}
[Serializable]
public class TerminalVelocity : CharacterModule
{
    public override void BuildChain()
    {
        //chain = null;
        //chain.SetNext(null);

    }
    public override void Process(ICharacterSettingsData data)
    {
        chain.Process(data);
    }
}

public class CharacterModuleProcessor : BaseProcessor<ICharacterSettingsData>
{
}

[Serializable]
public class FeatureItemContainer
{
    [SerializeReference] private CharacterModule[] modules;
    public FeatureItemContainer(CharacterModule[] modules)
    {
        this.modules = modules;
    }

    public CharacterModule[] Modules => modules;

    public void Process(ICharacterSettingsData data)
    {
        foreach (var module in modules)
        {
            module.Process(data);
        }
    }
}

public abstract class CharacterBaseState : MonoBehaviour, ICharacterState
{
    protected abstract void OnEnter(ICharacterStateController controller);
    protected abstract void OnUpdate(ICharacterStateController controller);
    protected abstract void OnExit(ICharacterStateController controller);

    [SerializeField] private UnityEvent onEnter, onExit;
    public void Tick(ICharacterStateController controller)
    {
        OnUpdate(controller);
    }

    public void Enter(ICharacterStateController controller)
    {
        OnEnter(controller);
        onEnter.Invoke();
    }

    public void Exit(ICharacterStateController controller)
    {
        OnExit(controller);
        onExit.Invoke();
    }

    [SerializeField] private InterfaceReference<ICharacterState>[] possibleTransitions;
    public bool CanTransitionTo(ICharacterState state)
    {
        foreach (var transition in possibleTransitions)
        {
            if (transition.Value == state) return true;
        }
        return false;
    }

    public abstract void Upkeep(ICharacterStateController controller);
}