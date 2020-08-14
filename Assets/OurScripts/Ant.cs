using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private Vector3 spawnLocation;
    public float moveSpd, movementRange;

    public Vector3 destination;
    public bool goingSomewhere;

    public enum Mode { idle, search };
    public Mode myMode = Mode.idle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (myMode) {
            case Mode.idle:
        if (Vector3.Distance(destination, transform.position) > movementRange * 0.1f)
        {
            //has a destination to reach
            goingSomewhere = true;
            transform.position = Vector3.Lerp(transform.position, destination, moveSpd + Random.Range(-0.01f, 0.01f));
        }
        else
        {
            goingSomewhere = false;
        }
                break;





            default: break;
         }
    }

    IEnumerator idle()
    {
        while (myMode.Equals(Mode.idle))
        {
            yield return new WaitUntil(() => !goingSomewhere);
            yield return new WaitForSeconds(Random.Range(0, 2f));
            Vector3 destDelta = Random.insideUnitCircle * movementRange;
            destination = new Vector3(transform.position.x + destDelta.x, transform.position.y + destDelta.y, 0);
        }
    }

    public void spawn(Vector3 spwnLoc)
    {
        spawnLocation = spwnLoc;
        destination = spwnLoc;
        StartCoroutine(idle());
    }
}
