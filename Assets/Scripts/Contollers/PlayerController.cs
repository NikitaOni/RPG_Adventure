using UnityEngine.EventSystems;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Drawing;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public UseDeshAbility desh;
    public Interactable focus;
    public bool castingSpell;
    private int movementMaskGroundNumber = 6;
    private int interactMaskGroundNumber = 9;

    Camera cam;
    PlayerMotor motor;
    
    [SerializeField] private ParticleSystem clickEffect;

    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        desh = GetComponent<UseDeshAbility>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0) && !castingSpell)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject.layer == movementMaskGroundNumber)
                {
                    RemoveFocus();
                    if (clickEffect != null)
                    {
                        Destroy(Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation), 5f);
                    }
                    motor.MoveToPoint(hit.point);
                } else if (hit.collider.gameObject.layer == interactMaskGroundNumber && desh.canMove)
                {
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            SetFocus(interactable);
                        }
                    }
                }
            }  
        }
    }

    private void SetFocus(Interactable newFocus)
    {
        if(newFocus != focus)
        {
            if(focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    private void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();
    }
}
