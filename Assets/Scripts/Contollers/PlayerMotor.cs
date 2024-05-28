using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    Transform target;
    public NavMeshAgent agent;
    public float rotateSpeedMovement = 0.05f;
    public UseDeshAbility desh;


    private float rotateVelocity;
    private HighLightManager highLightManager;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        highLightManager = GetComponent<HighLightManager>();
        desh = GetComponent<UseDeshAbility>();
    }

    private void Update()
    {
        if (target != null && desh.canMove)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        if (desh.canMove)
        {
            agent.SetDestination(point);

            highLightManager.DeselectHighlight();

            Quaternion rotationToLookAt = Quaternion.LookRotation(point - transform.position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement );

            Debug.Log(rotationY);

            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
    }

    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;
        agent.updateRotation = false;

        target = newTarget.interactionTransform;
        highLightManager.SelectedHighlight();
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;

        target = null;
    }

    public void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
