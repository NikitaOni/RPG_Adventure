using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthRestore : MonoBehaviour
{
    private CharacterStats myStats;
    private int amountRestore = 25;
    private float cooldown = 5f;

    // for first five second use heal pot 
    private float ColldownCurrent = -5;
    private float DelayBeforeSwitchingOff = 1.5f;
    [SerializeField] private Image cooldownImage;

    public GameObject prefabEffects;
    public Transform projSpawnPoint;

    void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    void UsePotionOfHealth(CharacterStats stats, int heal)
    {
        stats.RestoreDamage(heal);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time - ColldownCurrent > cooldown)
            {
                UsePotionOfHealth(myStats, amountRestore);
                ColldownCurrent = Time.time;
                var healEffect = Instantiate(prefabEffects, projSpawnPoint.position, Quaternion.identity,transform);
                Destroy(healEffect, DelayBeforeSwitchingOff);
            }
        }
        float cooldownPercent = (Time.time - ColldownCurrent) / cooldown;
        //reverse value 
        cooldownImage.fillAmount = 1 - cooldownPercent;
    }
}
