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

    public Canvas abilityCanvas1;
    public Image abilitySkillshot1;

    [Header("Ability 2")]
    public Image abilityImage2;
    public Text abilityText2;
    public KeyCode abilityKey2;
    public float abilityCooldown2 = 5;

    public Canvas abilityCanvas2;
    public Image abilityRangeIndicator2;
    public float maxAbilityDistance2 = 7;

    [Header("Ability 3")]
    public Image abilityImage3;
    public Text abilityText3;
    public KeyCode abilityKey3;
    public float abilityCooldown3 = 5;

    public Canvas abilityCanvas3;
    public Image abilityCone3;

    private bool isAbilityCooldown1 = false;
    private bool isAbilityCooldown2 = false;
    private bool isAbilityCooldown3 = false;

    private float currentAbilityCooldown1;
    private float currentAbilityCooldown2;
    private float currentAbilityCooldown3;

    private Vector3 position;
    private RaycastHit hit;
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;

        abilityText1.text = "";
        abilityText2.text = "";
        abilityText3.text = "";

        abilitySkillshot1.enabled = false;
        abilityRangeIndicator2.enabled = false;
        abilityCone3.enabled = false;

        abilityCanvas1.enabled = false;
        abilityCanvas2.enabled = false;
        abilityCanvas3.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        AbilityInput1();
        AbilityInput2();
        AbilityInput3();

        AbilityCoodown(ref currentAbilityCooldown1, abilityCooldown1, ref isAbilityCooldown1, abilityImage1, abilityText1);
        AbilityCoodown(ref currentAbilityCooldown2, abilityCooldown2, ref isAbilityCooldown2, abilityImage2, abilityText2);
        AbilityCoodown(ref currentAbilityCooldown3, abilityCooldown3, ref isAbilityCooldown3, abilityImage3, abilityText3);

        AbilityCanvas1();
        AbilityCanvas2();
        AbilityCanvas3();
    }

    private void AbilityCanvas1()
    {
        if (abilitySkillshot1.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab1Canvas = Quaternion.LookRotation(position - transform.position);
            ab1Canvas.eulerAngles = new Vector3(0, ab1Canvas.eulerAngles.y, ab1Canvas.eulerAngles.z);

            abilityCanvas1.transform.rotation = Quaternion.Lerp(ab1Canvas,abilityCanvas1.transform.rotation, 0);
        }
    }

    private void AbilityCanvas2()
    {
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                position = hit.point;
            }
        }

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbilityDistance2);

        var newHitPos = transform.position + hitPosDir * distance;
        abilityCanvas2.transform.position = newHitPos;
    }

    private void AbilityCanvas3()
    {
        if (abilityCone3.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab3Canvas = Quaternion.LookRotation(position - transform.position);
            ab3Canvas.eulerAngles = new Vector3(0, ab3Canvas.eulerAngles.y, ab3Canvas.eulerAngles.z);

            abilityCanvas3.transform.rotation = Quaternion.Lerp(ab3Canvas, abilityCanvas3.transform.rotation, 0);
        }
    }

    private void AbilityInput1()
    {
        if (Input.GetKeyDown(abilityKey1)&& !isAbilityCooldown1)
        {
            abilityCanvas1.enabled = true;
            abilitySkillshot1.enabled = true;

            abilityCanvas2.enabled = false;
            abilityRangeIndicator2.enabled = false;

            abilityCanvas3.enabled = false;
            abilityCone3.enabled = false;

            Cursor.visible = true;
        }
        if (abilitySkillshot1.enabled && Input.GetMouseButtonDown(0))
        {
            isAbilityCooldown1 = true;
            currentAbilityCooldown1 = abilityCooldown1;

            abilityCanvas1.enabled = false;
            abilitySkillshot1.enabled = false;
        }
    }

    private void AbilityInput2()
    {
        if (Input.GetKeyDown(abilityKey2)&& !isAbilityCooldown2)
        {
            abilityCanvas1.enabled = false;
            abilitySkillshot1.enabled = false;

            abilityCanvas2.enabled = true;
            abilityRangeIndicator2.enabled = true;

            abilityCanvas3.enabled = false;
            abilityCone3.enabled = false;

            Cursor.visible = false;
        }
        if (abilityCanvas2.enabled && Input.GetMouseButtonDown(0))
        {
            isAbilityCooldown2 = true;
            currentAbilityCooldown2 = abilityCooldown2;

            abilityCanvas2.enabled = false;
            abilityRangeIndicator2.enabled = false;

            Cursor.visible = true;
        }
    }

    private void AbilityInput3()
    {
        if (Input.GetKeyDown(abilityKey3) && !isAbilityCooldown3)
        {
            abilityCanvas1.enabled = false;
            abilitySkillshot1.enabled = false;

            abilityCanvas2.enabled = false;
            abilityRangeIndicator2.enabled = false;

            abilityCanvas3.enabled = true;
            abilityCone3.enabled = true;

            Cursor.visible = true;
        }
        if (abilityCone3.enabled && Input.GetMouseButtonDown(0))
        {
            isAbilityCooldown3 = true;
            currentAbilityCooldown3 = abilityCooldown3;

            abilityCanvas3.enabled = false;
            abilityCone3.enabled = false;
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
