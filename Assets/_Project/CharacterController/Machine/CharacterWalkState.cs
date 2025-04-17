using System;
using UnityEngine;

public class CharacterWalkState : CharacterBaseState
{
    [SerializeField] private FeatureItemContainer features = new (
    new CharacterModule[]
    {
        new SlopeControl(),
        new TerminalVelocity()
    });
    protected override void OnEnter(ICharacterStateController controller)
    {
        throw new NotImplementedException();
    }

    protected override void OnUpdate(ICharacterStateController controller)
    {
        features.Process(controller.CharacterSettingsData);
    }

    protected override void OnExit(ICharacterStateController controller)
    {
        throw new NotImplementedException();
    }

    public override void Upkeep(ICharacterStateController controller)
    {
        throw new NotImplementedException();
    }
}