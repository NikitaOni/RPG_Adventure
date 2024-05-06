using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;

    Transform target; 
    NavMeshAgent agent;
    CharacterCombat combat;
    public bool IsAlive = true;
    new CapsuleCollider collider;
    Outline outline;
    Animator animator;
    SpawnLoot loot;


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
        else
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
        collider.enabled = false;
        outline.enabled = false;
        agent.enabled = false;
        animator.SetBool("IsDead", true);
        loot.Loot();
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
