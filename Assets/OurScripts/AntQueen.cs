using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AntQueen : MonoBehaviour
{

    public float sanMax, san;
    public int sanDropRate; //per sec
    public float fertilityRate, fertilityBoost; //rate is inversely proportional with spawn interval time and should range from 0 to 1; boost is percentage multiplied to the total product
    public int fertilityBase; //base is the base number of ants generated per spawn
    public int numColonyAnts;
    public Image sanBar;
    public Text sanPercentageTxt, sanMaxTxt, numAntsTxt, numAntsNestTxt;

    public ArrayList groups; //each group is an empty gameObject being the parent of maximum 10 children ants (for ease of deleting)
    public GameObject AntPrefab;
    private float spawnMinX, spawnMaxX, spawnMinY, spawnMaxY;
    public BoxCollider2D spawnBounds;
    public Slider sendSlider;

    public int numAway;

    public GatherManager gatherManager;
    public DisasterManager disasterManager;

    void Start()
    {
        StartCoroutine(sanNaturalDecline());
        StartCoroutine(spawnAntsNatural());

        groups = new ArrayList();
        spawnMinX = spawnBounds.bounds.min.x; spawnMinY = spawnBounds.bounds.min.y;
        spawnMaxX = spawnBounds.bounds.max.x; spawnMaxY = spawnBounds.bounds.max.y;

        sendSlider.minValue = 1;
    }

    void Update()
    {
        float f = sanRate();
        sanBar.fillAmount = f;
        sanPercentageTxt.text = f*100 + "%";
        sanMaxTxt.text = (int) sanMax + "";
        numAntsTxt.text = numColonyAnts + "";
        numAntsNestTxt.text = numColonyAnts - numAway + "";

        sendSlider.maxValue = numColonyAnts;


        this.transform.position = Vector3.up * Mathf.Cos(4*Time.time) * 0.1f;
    }

    IEnumerator sanNaturalDecline()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            san -= sanDropRate;
            if (san < 0)
            {
                san = 0;
                sanZeroPunishment();
            }
        }
    }

    IEnumerator spawnAntsNatural()
    {
        while (true)
        {
            if (fertilityRate == 0)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            } //wait until not 0
            yield return new WaitForSeconds(0.5f/ (fertilityRate*fertilityBoost)); //at rate of 1, will spawn every 0.1s
            for (int a = 0; a < fertilityBase; a++)
            {
                spawnOneAnt();
            }
        }
    }

    public void addToSan(int boost)
    {
        if (san + boost > sanMax) san = sanMax;
        else san = san + boost;
    }

    public void setSanMax(int newMax)
    {
        sanMax = newMax;
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

        GameObject antObj = Instantiate(AntPrefab, new Vector3(x, y, 0), Quaternion.identity);
        Ant ant = antObj.GetComponent<Ant>();
        ant.spawn(this, gatherManager);
        numColonyAnts++;

        if (groups.Count != 0) {
            GameObject lastGroup = (GameObject)groups[groups.Count - 1];
            if (lastGroup.transform.childCount >= 10)
            {
                GameObject n = new GameObject();
                antObj.transform.parent = n.transform;
                groups.Add(n); //empty new group
            }
            else
            {
                antObj.transform.parent = lastGroup.transform;
            }
        }
        else
        {
            GameObject n = new GameObject();
            antObj.transform.parent = n.transform;
            groups.Add(n); //empty new group
        }

    }

    public void sanZeroPunishment()
    {
        sanMax /= 2;
        numColonyAnts = (int) (sanMax / 3);
        //TODO UI stuff here
    }

    
}
