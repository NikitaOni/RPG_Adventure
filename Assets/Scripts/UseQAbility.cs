using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UseQAbility : MonoBehaviour
{
    [Header("Ability values")]
    public KeyCode abilityKey = KeyCode.Q;
    public float cooldown = 10f;
    //public float manaCost = 20f;

    [Header("Projectile")]
    public GameObject projPrefab;
    public Transform projSpawnPoint;

    [Header("UI Elements 2d")]
    public Image abilityImageMain;
    public Image abilityImageGreyed;
    public Text abilityText;

    [Header("UI Elements 3D")]
    public Image indicatorRangeCircle;
    public float maxAbilityDistance;

    private bool isCooldown = false;
    private float currentCooldown;
    private GameObject targetedEnemy; 
    private Animator anim;
    private PlayerMotor moveScript;
    private PlayerController playerController;
    //private ManaSystem manaSystem;
    private bool indicatorActive = false;
    private bool isPreparingToCast = false;
    RaycastHit hit;

    private void Awake()
    {
        CacheComponents();
        InitializeUI();
    }

    private void Update()
    {
        HandleInput();
        UpdateCooldown();
        UpdateUI();
        if (isPreparingToCast)
        {
            RotateTowardsTarget();
        }
    }

    private void CacheComponents()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        moveScript = GetComponent<PlayerMotor>();
        indicatorRangeCircle ??= GetComponentInChildren<Image>();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(abilityKey) && !isCooldown)
        {
            ToggleIndicator();
        }
        if (indicatorActive && Input.GetMouseButtonDown(0) && !isCooldown)
        {
            indicatorRangeCircle.enabled = false; 
            TryCastFireball();  
        }
        
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, maxAbilityDistance);
    }

    private void TryCastFireball()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
        {
            targetedEnemy = hit.collider.gameObject;
            isPreparingToCast = true;
            
            MoveTowerdsTarget();
        } else
        {
            ToggleIndicator(false);
        }
    }

    private void MoveTowerdsTarget()
    {
        if (targetedEnemy == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetedEnemy.transform.position);
        if(distanceToTarget > maxAbilityDistance)
        {
            moveScript.agent.SetDestination(targetedEnemy.transform.position);
            moveScript.agent.stoppingDistance = maxAbilityDistance - 1f;
        }
    }

    private void RotateTowardsTarget()
    {
        if (targetedEnemy == null) return;

        Vector3 targetDirection = targetedEnemy.transform.position - transform.position;
        targetDirection.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (Quaternion.Angle(transform.rotation, lookRotation) < 1f)
        {
            isPreparingToCast = false;
            AttemptCastFireball();
        }
    }

    private void AttemptCastFireball()
    {
        if (!isCooldown && !isPreparingToCast)
        {
            anim.SetTrigger("Annie");
            //manaSystem.UseAbility(manaCost);
            StartCooldown();
        }
    }

    public void LaunchProjectile()
    {
        if (projPrefab != null && projSpawnPoint != null)
        {
            Vector3 targetDirection = targetedEnemy.transform.position - transform.position;
            var projectileInstance = Instantiate(projPrefab, projSpawnPoint.position, Quaternion.identity).GetComponent<Projectile>();
            projectileInstance.SetTarget(targetedEnemy);
            playerController.castingSpell = false;
            ClearTarget();
        }
    }

    private void ClearTarget()
    {
        targetedEnemy = null;
        ToggleIndicator(false);
        if (moveScript.agent != null)
        {
            moveScript.agent.ResetPath();
            moveScript.agent.stoppingDistance = 0f;
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        currentCooldown = cooldown;
    }

    private void UpdateCooldown()
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;
            isCooldown = currentCooldown > 0;
        }
    }

    private void UpdateUI()
    {
        if (abilityImageGreyed)
        {
            abilityImageGreyed.color = isCooldown ? Color.grey : Color.white;
            abilityImageGreyed.fillAmount = isCooldown ? currentCooldown / cooldown : 0;
            //abilityImageMain.color = manaSystem.CanAffordAbility(manaCost) ? Color.white : Color.red;
        }
        abilityText.text = isCooldown ? Mathf.Ceil(currentCooldown).ToString() : "";
    }

    private void InitializeUI()
    {
        if (abilityImageGreyed) abilityImageGreyed.color = Color.white;
        abilityText.text = "";
        ToggleIndicator(false);
    }

    private void ToggleIndicator(bool state = true)
    {
        playerController.castingSpell = state;
        indicatorActive = state ? !indicatorActive : false;
        indicatorRangeCircle.enabled = indicatorActive;   
    }
}
