using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Merge
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private CharacterAnimationVisual activeVisual;
        [SerializeField] private Transform head;
        [SerializeField] private Transform body;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;

        [SerializeField] private CharacterAnimationVisual idle;
        [SerializeField] private CharacterAnimationVisual jump;

        [HideInInspector] public CharacterData data;

        [SerializeField] private float speed;

        private void UpdateState()
        {
            if (data.inJump) activeVisual = jump;
            else activeVisual = idle;
        }
        private void Update()
        {
            UpdateState();
            if (activeVisual != null)
            {
                head.position = Vector2.MoveTowards(
                    head.position, 
                    (Vector2)transform.position + activeVisual._headPos, 
                    speed * Time.deltaTime
                );
                body.position = Vector2.MoveTowards(
                    body.position, 
                    (Vector2)transform.position + activeVisual._bodyPos, 
                    speed * Time.deltaTime
                );
                leftHand.position = Vector2.MoveTowards(
                    leftHand.position, 
                    (Vector2)transform.position + activeVisual._leftHandPos, 
                    speed * Time.deltaTime
                );
                rightHand.position = Vector2.MoveTowards(
                    rightHand.position, 
                    (Vector2)transform.position + activeVisual._rightHandPos, 
                    speed * Time.deltaTime
                );
                leftFoot.position = Vector2.MoveTowards(
                    leftFoot.position, 
                    (Vector2)transform.position + activeVisual._leftFootPos, 
                    speed * Time.deltaTime
                );
                rightFoot.position = Vector2.MoveTowards(
                    rightFoot.position, 
                    (Vector2)transform.position + activeVisual._rightFootPos, 
                    speed * Time.deltaTime
                );
            }
        }

    }
}