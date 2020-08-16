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

    private float[] aSpawnMinX, aSpawnMaxX, aSpawnMinY, aSpawnMaxY;
    public BoxCollider2D[] EnemyAntSpawnBounds;
    public GameObject enemyAntsParent;
    public int enemySpawnNumber, enemySpawnNoise;

    public GameObject enemyNote, disasterNote;
    public Animator camAnim;

    void Start()
    {
        spawnMinX = spawnBounds.bounds.min.x; spawnMinY = spawnBounds.bounds.min.y;
        spawnMaxX = spawnBounds.bounds.max.x; spawnMaxY = spawnBounds.bounds.max.y;

        aSpawnMinX = new float[4];
        aSpawnMinY = new float[4];
        aSpawnMaxX = new float[4];
        aSpawnMaxY = new float[4];

        for (int c=0; c<4; c++)
        {
            aSpawnMinX[c] = EnemyAntSpawnBounds[c].bounds.min.x; aSpawnMinY[c] = EnemyAntSpawnBounds[c].bounds.min.y;
            aSpawnMaxX[c] = EnemyAntSpawnBounds[c].bounds.max.x; aSpawnMaxY[c] = EnemyAntSpawnBounds[c].bounds.max.y;
        }

        enemyNote.SetActive(false);
        disasterNote.SetActive(false);
    }

    void Update()
    {
        
    }

    public void startGame()
    {
        StartCoroutine(randomDisasterTimer());
    }

    private IEnumerator randomDisasterTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds((disasterIntervalBase + Random.Range(-1 * disasterIntervalNoise, disasterIntervalNoise)) * disasterRateBoost);

            float rand = Random.value;

            //TODO: UI stuff

            if (rand < bootWeight)
            {
                StartCoroutine(effectThenDisaster(0));
            }
            else if (rand < stormWeight)
            {
                StartCoroutine(effectThenDisaster(1));
            }
            else //enemy ants
            {
                StartCoroutine(effectThenDisaster(2));
               
            }


        }

    }

    public IEnumerator effectThenDisaster(int index)
    {

        float x = Random.Range(spawnMinX, spawnMaxX);
        float y = Random.Range(spawnMinY, spawnMaxY);

        switch (index)
        {
            case 0:
                disasterNote.SetActive(true);
                disasterNote.GetComponent<Animator>().Play("enemyNote");
                disasterNote.GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(1.5f);
                disasterNote.SetActive(false);

                GameObject boot = Instantiate(bootPrefab, new Vector3(x, y, 0), Quaternion.identity);
                camAnim.SetTrigger("vert"); //cam shake

                wipeOutNumAnts(0.5f);
                break;

            case 1:
                disasterNote.SetActive(true);
                disasterNote.GetComponent<Animator>().Play("enemyNote");
                disasterNote.GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(1.5f);
                disasterNote.SetActive(false);

                GameObject storm = Instantiate(stormPrefab, new Vector3(x, y, 0), Quaternion.identity);
                camAnim.SetTrigger("horiz");
                wipeOutNumAnts(0.3f);
                break;

            case 2:
                enemyNote.SetActive(true);
                enemyNote.GetComponent<Animator>().Play("enemyNote");
                enemyNote.GetComponent<AudioSource>().Play(); 
                yield return new WaitForSeconds(1.5f);
                enemyNote.SetActive(false);
                queen.animForSeconds(3, 3.0f);

                spawnEnemyAntWave();
                break;


        }
    }

    public void wipeOutNumAnts(float percentage)
    {
        int num = (int)((queen.numColonyAnts) * percentage);
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

    public void spawnEnemyAntWave()
    {
        StartCoroutine(spawnWave());
    }

    private IEnumerator spawnWave()
    {
        GameObject group = Instantiate(new GameObject());
        group.transform.parent = enemyAntsParent.transform;

        for (int a = 0; a < enemySpawnNumber + Random.Range(-enemySpawnNoise, enemySpawnNoise); a++)
        {
            spawnOneEnemyAnt(group.transform);
        }

        yield return new WaitForSeconds(10);
        Destroy(group);
    }


    public void spawnOneEnemyAnt(Transform parent)
    {
        int index = (int) Random.Range(0, 3.99f);

        float x = Random.Range(aSpawnMinX[index], aSpawnMaxX[index]);
        float y = Random.Range(aSpawnMinY[index], aSpawnMaxY[index]);

        GameObject antObj = Instantiate(enemyAntPrefab, new Vector3(x, y, 0), Quaternion.identity);

        EnemyAnt ant = antObj.GetComponent<EnemyAnt>();
        ant.spawn();
        bool[] walkDone = new bool[1];
        ant.setDestination(queen.randomCavePoint(), walkDone);

        ant.transform.parent = parent;

    }


}
