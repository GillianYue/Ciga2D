using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  Also controls other types of missions of ants
 */
public class GatherManager : MonoBehaviour
{
    public Slider sendSlider;
    public AntQueen queen;

    public GameObject[] packagePrefabs; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendForExplore(int level)
    {
        int num = (int) sendSlider.value;

        ArrayList g = queen.groups;

        int groupCount = 1;
        while (num > 0)
        {
            if (g.Count == 0 || g.Count - groupCount < 0)
            {
                Debug.Log("not enough free ants in groups");
                return; //no more ants!
            }
            
            GameObject lastGroup = (GameObject)g[g.Count - groupCount];


                for (int n = 1; n < lastGroup.transform.childCount+1; n++)
                {
                    Ant a = lastGroup.transform.GetChild(lastGroup.transform.childCount - n).GetComponent<Ant>();
                    if (!a.myMode.Equals(Ant.Mode.search))
                    {
                        a.sendForSearch(level);
                        num--;
                    if (num <= 0) return;
                    }
                }
                groupCount++;
  
        }

    }

    public void assignGuard()
    {
        int num = (int)sendSlider.value;

        ArrayList g = queen.groups;

        int groupCount = 1;
        while (num > 0)
        {
            if (g.Count == 0 || g.Count - groupCount < 0)
            {
                Debug.Log("not enough free ants in groups");
                return; //no more ants!
            }

            GameObject lastGroup = (GameObject)g[g.Count - groupCount];


            for (int n = 1; n < lastGroup.transform.childCount + 1; n++)
            {
                Ant a = lastGroup.transform.GetChild(lastGroup.transform.childCount - n).GetComponent<Ant>();
                if (!a.myMode.Equals(Ant.Mode.search))
                {
                    a.guard();
                    num--;
                    if (num <= 0) return;
                }
            }
            groupCount++;

        }
    }


    public GameObject instantiatePackage(Transform parent, int type) //packageType
    {
        GameObject p = Instantiate(packagePrefabs[type], parent.position + new Vector3(0, 0, 0), Quaternion.identity);
        p.transform.parent = parent;
        p.transform.rotation = new Quaternion(0, 0, 0, 0);

        return p;
    }
}
