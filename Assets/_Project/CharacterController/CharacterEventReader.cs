using System;
using UnityEngine;

public class CharacterEventReader : MonoBehaviour
{
    [SerializeField] private CharacterProcessController controller;

    [SerializeField] private CharacterArmState armsUpStationary;
    [SerializeField] private CharacterArmState armsUpWiggle;
    [SerializeField] private CharacterArmState armsThrowing;
    [SerializeField] private CharacterArmState armsDownStationary;
    [SerializeField] private CharacterArmState armsSwinging;
    [SerializeField] private CharacterArmState armsDownPumpUp;
    
    [SerializeField] private CharacterLegState legsPlanted;
    [SerializeField] private CharacterLegState legsStationary;
    [SerializeField] private CharacterLegState legsRunning;
    [SerializeField] private CharacterLegState legsTakeOff;
    
    private ICharacterEventStateBase currentArmState;
    private ICharacterEventStateBase currentLegState;

    private CharacterDataSettings data;
    private void Start()
    {
        data = controller.GetData();
        data.jump.OnJumpEvent += OnJump;
        data.locomotion.OnZeroedVelocityEvent += OnHalt;
        data.attack.OnStartAttackEvent += OnAttackStart;
        data.attack.OnEndAttackEvent += OnAttackEnd;
    }

    private void OnDisable()
    {
        data.jump.OnJumpEvent -= OnJump;
        data.locomotion.OnZeroedVelocityEvent -= OnHalt;
        data.attack.OnStartAttackEvent -= OnAttackStart;
        data.attack.OnEndAttackEvent -= OnAttackEnd;
    }


    private void OnJump()
    {
        jumping = true;
    }

    private void OnHalt()
    {
        stopped = true;
    }

    private void OnAttackStart()
    {
        throwing = true;
    }

    private void OnAttackEnd()
    {
        throwing = false;
    }


    private bool throwing;
    private bool movingHorizontally;
    private bool jumping;
    private bool rising;
    private bool falling;
    private bool stopped;
    
    private void UpdateFlags()
    {
        if (movingHorizontally) stopped = false;
        jumping = false;
        movingHorizontally = Mathf.Abs(data.locomotion.Velocity.Value.x) > 0.01f;
        rising = data.locomotion.Velocity.Value.y > 0.01f;
        falling = data.locomotion.Velocity.Value.y < -0.01f && !data.collision.GroundContact.Contact;
    }

    private void CalcArms()
    {
        if(throwing) SwitchArmState(armsThrowing);
        else
        {
            if(jumping) SwitchArmState(armsDownPumpUp);
            else
            {
                if (falling) SwitchArmState(armsUpWiggle);
                else
                {
                    if(rising) SwitchArmState(armsUpStationary);
                    else
                    {
                        if(stopped) SwitchArmState(armsDownStationary);
                        else SwitchArmState(armsSwinging);
                    }
                }
            }
        }
    }

    private void CalcLegs()
    {
        if(jumping) SwitchLegState(legsTakeOff);
        else
        {
            if(rising) SwitchLegState(legsStationary);
            else
            {
                if(movingHorizontally) SwitchLegState(legsRunning);
                else
                {
                    if(throwing) SwitchLegState(legsPlanted);
                    else SwitchLegState(legsStationary);
                }
            }
        }
    }
    private void CalculateState()
    {
        CalcArms();
        CalcLegs();
    }



    private void Update()
    {
        if(currentArmState != null) currentArmState.Tick(data);
        if(currentLegState != null) currentLegState.Tick(data);
        CalculateState();
        UpdateFlags();
    }
    

    private void SwitchArmState(CharacterArmState state)
    {
        if (currentArmState == state) return;
        if (currentArmState != null) currentArmState.Exit();
        currentArmState = state;
        if (currentArmState != null) currentArmState.Enter();
    }
    private void SwitchLegState(CharacterLegState state)
    {
        if (currentLegState == state) return;
        if (currentLegState != null) currentLegState.Exit();
        currentLegState = state;
        if (currentLegState != null) currentLegState.Enter();
    }

    
}

public interface ICharacterEventStateBase
{
    public void Exit();

    public void Enter();

    public void Tick(CharacterDataSettings data);
}
public abstract class CharacterLimbPoser : MonoBehaviour
{
    [SerializeField] protected Transform leftHand;
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform leftFoot;
    [SerializeField] protected Transform rightFoot;
}
public abstract class CharacterArmState : CharacterLimbPoser, ICharacterEventStateBase
{
    [SerializeField] protected Vector3 leftPreset;
    [SerializeField] protected Vector3 rightPreset;
    public abstract void Exit();

    public abstract void Enter();

    public abstract void Tick(CharacterDataSettings data);

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftPreset + transform.position, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rightPreset + transform.position, 0.1f);
    }
}
public abstract class CharacterLegState : CharacterLimbPoser, ICharacterEventStateBase
{
    [SerializeField] protected Vector3 leftPreset;
    [SerializeField] protected Vector3 rightPreset;
    public abstract void Exit();

    public abstract void Enter();

    public abstract void Tick(CharacterDataSettings data);
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftPreset + transform.position, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rightPreset + transform.position, 0.1f);
    }
}