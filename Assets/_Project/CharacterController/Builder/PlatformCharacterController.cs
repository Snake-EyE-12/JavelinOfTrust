using System;
using UnityEngine;

public class PlatformCharacterController : MonoBehaviour, ICharacterCreator
{
    private CharacterProcessor chain;
    
    private void BuildProcess()
    {
        
    }
    private void Awake()
    {
        BuildProcess();
    }
}