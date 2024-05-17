using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    private Transform originalTarget;
    public float speed = 0.2f;
    public float destroyAfterSeconds = 5f;

    public UnityEvent onEnemyKilled;
    //private PlayerStats playerStats;
    void Start()
    {
        originalTarget = target.transform;
        Destroy(gameObject, destroyAfterSeconds);
        //playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }        
    }

    public void MoveTowardsTarget()
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        targetDirection.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null && ReferenceEquals(other.gameObject, target.gameObject))
        {
            ApplyDamageAndDestroy(target.gameObject);
        } else if (originalTarget != null && ReferenceEquals(other.gameObject, target.gameObject))
        {
            ApplyDamageAndDestroy(originalTarget.gameObject);
        }
    }

    private void ApplyDamageAndDestroy(GameObject targetObject)
    {
        CharacterStats targetStats = targetObject.GetComponent<CharacterStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(2);

            if (targetStats.currentHealth <= 0)
            {
                Destroy(targetObject);
                onEnemyKilled?.Invoke();
            }
        }

        Destroy(gameObject);
    }
}
