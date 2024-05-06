using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    [SerializeField] private List<GameObject> loot = new List<GameObject>();
    [SerializeField]
    [Range(1,99)]
    private int minNumber = 7;
    [Range(2, 100)]
    private int maxNumber = 20;
    [SerializeField]
    private Transform spawnPoint;
    private bool hasBeenCollected = false;

    private void OnValidate()
    {
        if (minNumber > maxNumber)
        {
            maxNumber = minNumber + 1;
        }
    }

    public void Loot()
    {
        hasBeenCollected = true;
        int number = Random.Range(minNumber, maxNumber);
        StartCoroutine(CreateLoot(number));
    }

    IEnumerator CreateLoot(int number)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < number; i++)
        {
            GameObject tempLoot = Instantiate(loot[Random.Range(0, loot.Count)]);
            tempLoot.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
