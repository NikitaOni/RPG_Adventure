using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UseDeshAbility : MonoBehaviour
{
    public KeyCode abilityKey = KeyCode.LeftShift;
    public bool canMove = true;


    private NavMeshAgent agent;
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            canMove = false;
            agent.enabled = false;
            anim.SetTrigger("Roll");
        }
    }

    public void EnableNavMesh()
    {
        agent.enabled = true;
        canMove = true;
    }
}
