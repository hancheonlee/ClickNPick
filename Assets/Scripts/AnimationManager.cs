using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationManager : MonoBehaviour
{
    public enum animationState
    {
        Idle, Forward, Backward
    }

    public animationState currentState;
    private Vector3 lastPosition;
    private NavMeshAgent agent;
    private Animator anim;
    public bool walking;

    [SerializeField] private bool male03;
    [SerializeField] private bool male04;
    [SerializeField] private bool female03;
    [SerializeField] private bool female04;
    [SerializeField] private bool cat01;
    [SerializeField] private bool cat02;
    [SerializeField] private bool dog01;
    [SerializeField] private bool dog02;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = animationState.Idle;
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 0)
        {
            walking = true;
        }
        else if (agent.velocity.magnitude == 0)
        {
            walking = false;
            currentState = animationState.Idle;
        }

        if (currentState == animationState.Forward)
        {
            if (male03)
            {
                anim.Play("Male03_Walking");
            }
            else if (male04)
            {
                anim.Play("Male04_Walking");
            }
            else if (female03)
            {
                anim.Play("Female03_Walking");
            }
            else if (female04)
            {
                anim.Play("Female04_Walking");
            }
            else if (cat01)
            {
                anim.Play("Cat01_Walking");
            }
            else if (cat02)
            {
                anim.Play("Cat02_Walking");
            }
            else if (dog01)
            {
                anim.Play("Dog01_Walking");
            }
            else if (dog02)
            {
                anim.Play("Dog02_Walking");
            }
        }
        else if (currentState == animationState.Backward)
        {
            if (male03)
            {
                anim.Play("Male03_Walking2");
            }
            else if (male04)
            {
                anim.Play("Male04_Walking2");
            }
            else if (female03)
            {
                anim.Play("Female03_Walking2");
            }
            else if (female04)
            {
                anim.Play("Female04_Walking2");
            }
            else if (cat01)
            {
                anim.Play("Cat01_Walking2");
            }
            else if (cat02)
            {
                anim.Play("Cat02_Walking2");
            }
            else if (dog01)
            {
                anim.Play("Dog01_Walking2");
            }
            else if (dog02)
            {
                anim.Play("Dog02_Walking2");
            }
        }
        else
        {
            if (male03)
            {
                anim.Play("Male03_Idle");
            }
            else if (male04)
            {
                anim.Play("Male04_Idle");
            }
            else if (female03)
            {
                anim.Play("Female03_Idle");
            }
            else if (female04)
            {
                anim.Play("Female04_Idle");
            }
            else if (cat01)
            {
                anim.Play("Cat01_Idle");
            }
            else if (cat02)
            {
                anim.Play("Cat02_Idle");
            }
            else if (dog01)
            {
                anim.Play("Dog01_Idle");
            }
            else if (dog02)
            {
                anim.Play("Dog02_Idle");
            }
        }

        Vector3 direction = transform.position - lastPosition;
        lastPosition = transform.position;

        if (direction.x < 0 && direction.y < 0)
        {
            currentState = animationState.Forward;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            currentState = animationState.Forward;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0 && direction.y > 0)
        {
            currentState = animationState.Backward;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0 && direction.y > 0)
        {
            currentState = animationState.Backward;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
