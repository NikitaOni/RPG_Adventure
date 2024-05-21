using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseEAbility : MonoBehaviour
{
    [Header("Ability values")]
    public KeyCode abilityKey = KeyCode.E;
    public float cooldown = 10f;
    private int amountRestore = 25;

    [Header("UI Elements 2d")]
    public Image abilityImageMain;
    public Image abilityImageGreyed;
    public Text abilityText;

    public GameObject prefabEffects;
    public Transform projSpawnPoint;

    private PlayerStats myStats;
    private float colldownCurrent = 0;
    private float colldownForText;
    private float delayBeforeSwitchingOff = 10f;


    void Start()
    {
        myStats = GetComponent<PlayerStats>();
        abilityImageGreyed.fillAmount = 0;
        colldownCurrent = -cooldown;
        abilityText.text = "";
        colldownForText = 0;
    }

    void UsePotionOfHealth(CharacterStats stats, int heal)
    {
        stats.RestoreDamage(heal);
    }
    void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            if (Time.time - colldownCurrent > cooldown)
            {
                UsePotionOfHealth(myStats, amountRestore);
                colldownCurrent = Time.time;
                colldownForText = cooldown;
                var healEffect = Instantiate(prefabEffects, projSpawnPoint.position, Quaternion.identity,transform);
                Destroy(healEffect, delayBeforeSwitchingOff);
            }
        }
        float cooldownPercent = (Time.time - colldownCurrent) / cooldown;
        //reverse value 
        abilityImageGreyed.fillAmount = 1 - cooldownPercent;
        if (colldownForText <= 0)
        {
            abilityText.text = "";
        }
        else
        {
            colldownForText -= Time.deltaTime;
            abilityText.text = Mathf.Ceil(colldownForText).ToString();
        }
    }
}
