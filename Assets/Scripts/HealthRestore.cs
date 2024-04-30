using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRestore : MonoBehaviour
{
    CharacterStats myStats;
    int amountRestore = 25;

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
            UsePotionOfHealth(myStats, amountRestore);
        }
    }
}
