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
        Debug.Log("sending " + num + " out ");

        ArrayList g = queen.groups;

        int groupCount = 1;
        while (num != 0)
        {
            if (g.Count == 0 || g.Count - groupCount < 0)
            {
                return; //no more ants!
            }
            GameObject lastGroup = (GameObject)g[g.Count - groupCount];
            if (num < lastGroup.transform.childCount)
            {
                for (int n = 1; n < num+1; n++)
                {
                   lastGroup.transform.GetChild(lastGroup.transform.childCount - n).GetComponent<Ant>().sendForSearch(level);
                }

                num = 0;
                groupCount++;
            }
            else
            {
                num -= lastGroup.transform.childCount;

                for (int n = 1; n < lastGroup.transform.childCount+1; n++)
                {
                    lastGroup.transform.GetChild(lastGroup.transform.childCount - n).GetComponent<Ant>().sendForSearch(level);
                }
                groupCount++;
            }
        }

         
    }
}
