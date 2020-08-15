using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public AntQueen queen;
    public GameObject bootPrefab, stormPrefab, enemyAntPrefab;
    public float bootWeight, stormWeight, enemyAntWeight;
    public float disasterIntervalBase, disasterIntervalNoise, disasterRateBoost;

    private float spawnMinX, spawnMaxX, spawnMinY, spawnMaxY;
    public BoxCollider2D spawnBounds;

    void Start()
    {
        spawnMinX = spawnBounds.bounds.min.x; spawnMinY = spawnBounds.bounds.min.y;
        spawnMaxX = spawnBounds.bounds.max.x; spawnMaxY = spawnBounds.bounds.max.y;

        StartCoroutine(randomDisasterTimer());
    }

    void Update()
    {
        
    }

    private IEnumerator randomDisasterTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds((disasterIntervalBase + Random.Range(-1 * disasterIntervalNoise, disasterIntervalNoise)) * disasterRateBoost);

            float rand = Random.value;

            float x = Random.Range(spawnMinX, spawnMaxX);
            float y = Random.Range(spawnMinY, spawnMaxY);

            //TODO: UI stuff

            if (rand < bootWeight)
            {
                GameObject boot = Instantiate(bootPrefab, new Vector3(x, y, 0), Quaternion.identity);
                Debug.Log("boot!");
                wipeOutNumAnts(0.5f);
            }
            else if (rand < stormWeight)
            {
                GameObject storm = Instantiate(stormPrefab, new Vector3(x, y, 0), Quaternion.identity);
                Debug.Log("storm!");
                wipeOutNumAnts(0.3f);
            }
            else //enemy ants
            {
                GameObject boot = Instantiate(bootPrefab, new Vector3(x, y, 0), Quaternion.identity);
                Debug.Log("enemy ants!");
            }


        }


    }

    public void wipeOutNumAnts(float percentage)
    {
        int num = (int)((queen.numColonyAnts - queen.numAway) * percentage);
        wipeOutNumAnts(num);
    }

    public void wipeOutNumAnts(int number)
    {
        int num = number;
        ArrayList g = queen.groups;
        int count = 0;
        
        while (num > 0)
        {
            if (g.Count == 0)
            {
                queen.numColonyAnts = 0;
                return; //no more ants!
            }
            GameObject lastGroup = (GameObject)g[g.Count - 1];
            if (num < lastGroup.transform.childCount)
            {
                for (int n = 0; n < num; n++)
                {
                    Destroy(lastGroup.transform.GetChild(n).gameObject);
                    count++;
                }

                num = 0; 

                //after this process, the last group still exists
            }
            else
            {
                num -= lastGroup.transform.childCount;
                count += lastGroup.transform.childCount;
                Destroy(lastGroup);
                g.RemoveAt(g.Count - 1); //also removing the group slot from the array
            }
        }

        queen.numColonyAnts -= number; //if reaching here, means there's more ants than required to wipe out


    }


}
