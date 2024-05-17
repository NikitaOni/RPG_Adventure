using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseWAbility : MonoBehaviour
{
    [Header("Ability values")]
    public KeyCode abilityKey = KeyCode.W;
    public float cooldown = 10f;
    private float duration = 5f;

    [Header("UI Elements 2d")]
    public Image abilityImageMain;
    public Image abilityImageGreyed;
    public Text abilityText;

    public GameObject prefabEffects;
    public Transform projSpawnPoint;

    private PlayerStats myStats;
    private float colldownCurrent = 0;
    private float colldownForText;
    private bool abilityActive = false;
    private float delayBeforeSwitchingOff = 10f;

    void Start()
    {
        myStats = GetComponent<PlayerStats>();
        abilityImageGreyed.fillAmount = 0;
        colldownCurrent = -cooldown;
        abilityText.text = "";
        colldownForText = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            if (Time.time - colldownCurrent > cooldown)
            {
                myStats.DamgageIncreace();
                colldownCurrent = Time.time;
                colldownForText = cooldown;
                var healEffect = Instantiate(prefabEffects, projSpawnPoint.position, Quaternion.identity, transform);
                Destroy(healEffect, delayBeforeSwitchingOff);                
            }
        }
        float cooldownPercent = (Time.time - colldownCurrent) / cooldown;

        if (Time.time - colldownCurrent > duration)
        {
            myStats.DamgageRedaction();
        }

        //reverse value 
        abilityImageGreyed.fillAmount = 1 - cooldownPercent;
        if (colldownForText<=0)
        {
            abilityText.text = "";
        }
        else
        {
            colldownForText -= Time.deltaTime;
            Debug.Log(Time.deltaTime);
            abilityText.text = Mathf.Ceil(colldownForText).ToString();
        }     
    }
}
