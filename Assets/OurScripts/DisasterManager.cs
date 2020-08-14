using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public AntQueen queen;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void wipeOutNumAnts(int number)
    {
        int num = number;
        ArrayList g = queen.groups;

        
        while (num != 0)
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
                    Destroy(lastGroup.transform.GetChild(lastGroup.transform.childCount - 1).gameObject);
                }

                num = 0; 

                //after this process, the last group still exists
            }
            else
            {
                num -= lastGroup.transform.childCount;
                Destroy(lastGroup);
                g.RemoveAt(g.Count - 1); //also removing the group slot from the array
            }
        }

        queen.numColonyAnts -= number; //if reaching here, means there's more ants than required to wipe out

        Debug.Log("supposedly wiped out " + number + " ants ");

        //TODO: UI stuff
    }
}
