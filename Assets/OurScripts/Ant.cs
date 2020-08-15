using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private Vector3 spawnLocation;
    public float moveSpd, movementRange, searchMoveSpd;

    public Vector3 destination;
    public bool goingSomewhere;

    public enum Mode { idle, search };
    public Mode myMode = Mode.idle;

    private IEnumerator currentMoveTo, currentMode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void setMode(Mode m)
    {
        myMode = m;
        if(currentMode!= null) StopCoroutine(currentMode);
        if(currentMoveTo != null) StopCoroutine(currentMoveTo);
        if(currentMoveTo != null) StopCoroutine(currentMoveTo);
        goingSomewhere = false;

        switch (myMode)
        {
            case Mode.idle:
                currentMode = idle();
                StartCoroutine(currentMode); 
                break;


            default: break;
        }
    }

    //randomly generates amble destination
    IEnumerator idle()
    {
        while (myMode.Equals(Mode.idle))
        {
            yield return new WaitUntil(() => !goingSomewhere);
            yield return new WaitForSeconds(Random.Range(0, 2f));
            Vector3 destDelta = Random.insideUnitCircle * movementRange;
            setDestination(new Vector3(transform.position.x + destDelta.x, transform.position.y + destDelta.y, 0));

        }

    }

    private IEnumerator moveTo()
    {

        float spd = myMode.Equals(Mode.search) ? searchMoveSpd : moveSpd;
        Vector3 startPos = transform.position;

/*        if (myMode.Equals(Mode.search))
            Debug.Log("trying to move from " + startPos + " to " + destination);*/

        //TODO adjust
        Vector3 vectorToTarget = destination - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;


        float waitTime = 0.2f / spd;

        for (int i=0; i<=100; i++)
        {
            transform.position = Vector3.Lerp(startPos, destination, (float)i / 100f);
            yield return new WaitForSeconds(waitTime);
        }

        goingSomewhere = false;
    }

    public void spawn()
    {
        spawnLocation = transform.position;
        destination = spawnLocation; //no need to move to

        Vector3 randomDirection = new Vector3(0, 0, Random.Range(-180, 180));
        transform.Rotate(randomDirection);

        StartCoroutine(idle());
    }

    public void sendForSearch(int level)
    {
        StartCoroutine(crawlOutOfScreen());
    }

    private IEnumerator crawlOutOfScreen() //and wait for time
    {
        setMode(Mode.search);

        setDestination(new Vector3(1.82f, 0.56f, 0));
        yield return new WaitUntil(() => !goingSomewhere);

        yield return new WaitForSeconds(3f);
        setDestination(spawnLocation);
        yield return new WaitUntil(() => !goingSomewhere);

        setMode(Mode.idle);
    }

    public void setDestination(Vector3 d)
    {
        goingSomewhere = true;
        destination = d;
        if (currentMoveTo != null) StopCoroutine(currentMoveTo);
        currentMoveTo = moveTo();
        StartCoroutine(currentMoveTo);
    }
}
