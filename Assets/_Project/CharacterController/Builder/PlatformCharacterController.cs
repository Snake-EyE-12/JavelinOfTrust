using System;
using UnityEngine;

public class PlatformCharacterController : MonoBehaviour, ICharacterCreator
{
    [SerializeField] private CharacterControlsData data;
    private PlatformCharacterProcessor chain;
    
    private void BuildProcess()
    {
        chain = new PCP_VelocityApplicator();
    }
    private void Awake()
    {
        BuildProcess();
    }
}