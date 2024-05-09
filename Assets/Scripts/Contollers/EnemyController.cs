using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public bool IsAlive = true;

    Transform target; 
    NavMeshAgent agent;
    CharacterCombat combat;
    Outline outline;
    Animator animator;
    SpawnLoot loot;
    new CapsuleCollider collider;
    
    private bool IsDestroy = false;


    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
        collider = GetComponent<CapsuleCollider>();
        outline = GetComponent<Outline>();
        animator = GetComponentInChildren<Animator>();
        loot = GetComponent<SpawnLoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    CharacterStats targetStats = target.GetComponent<CharacterStats>();
                    if (targetStats != null)
                    {
                        combat.Attack(targetStats);
                    }
                    FaceTarget();
                }
            }
        }
        else if(!IsDestroy)
        {
            StartCoroutine(Destroy());
        }      
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    IEnumerator Destroy()
    {
        IsDestroy = true;
        collider.enabled = false;
        outline.enabled = false;
        agent.enabled = false;
        animator.SetBool("IsDead", true);
        loot.Loot();
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
