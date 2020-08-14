using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class AntQueen : MonoBehaviour
{
    // Start is called before the first frame update

    public float sanMax, san;
    public int sanDropRate; //per sec
    public float fertilityRate, fertilityBoost; //rate is inversely proportional with spawn interval time and should range from 0 to 1; boost is percentage multiplied to the total product
    public int fertilityBase; //base is the base number of ants generated per spawn
    public int numColonyAnts;
    public Image sanBar;
    public Text sanBarText;

    public ArrayList colony;
    public GameObject AntPrefab;
    private float spawnMinX, spawnMaxX, spawnMinY, spawnMaxY;
    public BoxCollider2D spawnBounds;


    void Start()
    {
        StartCoroutine(sanNaturalDecline());
        StartCoroutine(spawnAntsNatural());

        colony = new ArrayList(); //arraylist of ants
        spawnMinX = spawnBounds.bounds.min.x; spawnMinY = spawnBounds.bounds.min.y;
        spawnMaxX = spawnBounds.bounds.max.x; spawnMaxY = spawnBounds.bounds.max.y;
    }

    // Update is called once per frame
    void Update()
    {
        float f = sanRate();
        sanBar.fillAmount = f;
        sanBarText.text = f*100 + "%";
    }

    IEnumerator sanNaturalDecline()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            san -= sanDropRate;
            if (san < 0) san = 0; //gameover?
        }
    }

    IEnumerator spawnAntsNatural()
    {
        while (true)
        {
            if (fertilityRate == 0) continue; //wait until not 0
            yield return new WaitForSeconds(0.5f/ (fertilityRate*fertilityBoost)); //at rate of 1, will spawn every 0.1s
            for (int a = 0; a < fertilityBase; a++)
            {
                spawnOneAnt();
            }
        }
    }

    public void setFertilityRate(float rate)
    {
        fertilityRate = rate;
    }

    public void boostFertilityRate(float ratio)
    {
        fertilityRate *= ratio;
    }

    public void setNumColonyAnts(int num)
    {
        numColonyAnts = num;
    }

    public void modifyNumColonyAnts(int deltaNum)
    {
        numColonyAnts += deltaNum;
        if (numColonyAnts < 0) numColonyAnts = 0;
    }

    public float sanRate()
    {
        if (sanMax != 0)
            return san / sanMax;
        else return 0;
    }

    public void spawnOneAnt()
    {
        float x = Random.Range(spawnMinX, spawnMaxX);
        float y = Random.Range(spawnMinY, spawnMaxY);

        GameObject ant = Instantiate(AntPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        colony.Add(ant);

    }


}
