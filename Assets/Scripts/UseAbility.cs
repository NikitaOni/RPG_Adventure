using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAbility : MonoBehaviour
{
    [Header("Ability 1")]
    public Image abilityImage1;
    public Text abilityText1;
    public KeyCode abilityKey1;
    public float abilityCooldown1 = 5;

    [Header("Ability 2")]
    public Image abilityImage2;
    public Text abilityText2;
    public KeyCode abilityKey2;
    public float abilityCooldown2 = 5;

    [Header("Ability 3")]
    public Image abilityImage3;
    public Text abilityText3;
    public KeyCode abilityKey3;
    public float abilityCooldown3 = 5;

    private bool isAbilityCooldown1 = false;
    private bool isAbilityCooldown2 = false;
    private bool isAbilityCooldown3 = false;

    private float currentAbilityCooldown1;
    private float currentAbilityCooldown2;
    private float currentAbilityCooldown3;

    // Start is called before the first frame update
    void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;

        abilityText1.text = "";
        abilityText2.text = "";
        abilityText3.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        AbilityInput1();
        AbilityInput2();
        AbilityInput3();

        AbilityCoodown(ref currentAbilityCooldown1, abilityCooldown1, ref isAbilityCooldown1, abilityImage1, abilityText1);
        AbilityCoodown(ref currentAbilityCooldown2, abilityCooldown2, ref isAbilityCooldown2, abilityImage2, abilityText2);
        AbilityCoodown(ref currentAbilityCooldown3, abilityCooldown3, ref isAbilityCooldown3, abilityImage3, abilityText3);
    }

    private void AbilityInput1()
    {
        if (Input.GetKeyDown(abilityKey1)&& !isAbilityCooldown1)
        {
            isAbilityCooldown1 = true;
            currentAbilityCooldown1 = abilityCooldown1;
        }
    }

    private void AbilityInput2()
    {
        if (Input.GetKeyDown(abilityKey2)&& !isAbilityCooldown2)
        {
            isAbilityCooldown2 = true;
            currentAbilityCooldown2 = abilityCooldown2;
        }
    }

    private void AbilityInput3()
    {
        if (Input.GetKeyDown(abilityKey3) && !isAbilityCooldown3)
        {
            isAbilityCooldown3 = true;
            currentAbilityCooldown3 = abilityCooldown3;
        }
    }

    private void AbilityCoodown(ref float currentCooldown, float maxCooldown, ref bool isCooldown, Image skillImage, Text skillText)
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0f)
            {
                isCooldown = false;
                currentCooldown = 0f;

                if (skillImage != null)
                {
                    skillImage.fillAmount = 0f;
                }
                if (skillText != null) 
                {
                    skillText.text = "";
                }
            }
            else
            {
                if (skillImage != null)
                {
                    skillImage.fillAmount = currentCooldown / maxCooldown;
                }
                if (skillText != null)
                {
                    skillText.text = Mathf.Ceil(currentCooldown).ToString();
                }
            }
        }
    }
}
