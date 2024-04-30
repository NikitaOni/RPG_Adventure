using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthRestore : MonoBehaviour
{
    CharacterStats myStats;
    int amountRestore = 25;

    [SerializeField]private Image cooldownImage;
    float cooldown = 5f;

    // for first five second use heal pot 
    float ColldownCurrent = -5; 



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
            }
        }
        float cooldownPercent = (Time.time - ColldownCurrent) / cooldown;
        
        //reverse value 
        cooldownImage.fillAmount = 1 - cooldownPercent;

    }
}
