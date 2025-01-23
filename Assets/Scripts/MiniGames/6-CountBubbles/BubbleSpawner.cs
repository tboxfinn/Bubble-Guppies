using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public int bubblesToGenerate = 10;
    public List<Material> bubbleMaterials;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateBubbles()
    {
        for (int i = 0; i < bubblesToGenerate; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            GameObject bubble = Instantiate(bubblePrefab, randomPosition, Quaternion.identity, transform);
            Material randomMaterial = bubbleMaterials[Random.Range(0, bubbleMaterials.Count)];
            bubble.GetComponent<Renderer>().material = randomMaterial;
        }
    }
}
