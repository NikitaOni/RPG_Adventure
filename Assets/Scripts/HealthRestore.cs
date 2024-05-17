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
    private float colldownCurrent = -5;
    private float delayBeforeSwitchingOff = 1.5f;
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
            if (Time.time - colldownCurrent > cooldown)
            {
                UsePotionOfHealth(myStats, amountRestore);
                colldownCurrent = Time.time;
                var healEffect = Instantiate(prefabEffects, projSpawnPoint.position, Quaternion.identity,transform);
                Destroy(healEffect, delayBeforeSwitchingOff);
            }
        }
        float cooldownPercent = (Time.time - colldownCurrent) / cooldown;
        //reverse value 
        cooldownImage.fillAmount = 1 - cooldownPercent;
    }
}
