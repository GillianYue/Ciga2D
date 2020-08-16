using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private Vector3 spawnLocation;
    public float moveSpd, movementRange, searchMoveSpd;

    public Vector3 destination;

    public enum Mode { idle, search, guard, entertain };
    public Mode myMode = Mode.idle;

    private IEnumerator currentMoveTo, currentMode;

    public Vector3 cornerNW, cornerNE, cornerSW, cornerSE;
    public AntQueen queen;

    private GameObject myPackage;
    private GatherManager gatherManager;

    public Animator myAnim;
    public Transform mySprite;

    public bool numAwaySetTracker;

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
        if (myMode.Equals(Mode.guard) && !m.Equals(Mode.guard)) queen.numOnGuard--;
        myMode = m;

        if(currentMode!= null) StopCoroutine(currentMode);
        if(currentMoveTo != null) StopCoroutine(currentMoveTo);

        switch (myMode)
        {
            case Mode.idle:
                currentMode = idle();
                StartCoroutine(currentMode); 
                break;
            case Mode.entertain:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                myAnim.Play("antHop");
                currentMode = entertain();
                StartCoroutine(entertain());
                break;

            default: break;
        }
    }

    IEnumerator entertain()
    {
        while (myMode.Equals(Mode.entertain))
        {
            queen.addToSan(1);
            yield return new WaitForSeconds(3);
        }
    }

    //randomly generates amble destination
    IEnumerator idle()
    {
        while (myMode.Equals(Mode.idle))
        {
            bool[] moveDone = new bool[1];
            Vector3 destDelta = Random.insideUnitCircle * movementRange;
            setDestination(
                new Vector3(transform.position.x + destDelta.x, transform.position.y + destDelta.y, 0),
               moveDone);
            yield return new WaitUntil(() => moveDone[0]);
            yield return new WaitForSeconds(Random.Range(0, 2f));
        }

    }

    private IEnumerator moveTo(bool[] don)
    {

        float spd = myMode.Equals(Mode.search) ? searchMoveSpd : moveSpd;
        Vector3 startPos = transform.position;

        //TODO adjust
        Vector3 vectorToTarget = destination - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        if (vectorToTarget.x < 0)
        {
            mySprite.localScale = new Vector3(1, 1, 1);
            float orig = transform.rotation.eulerAngles.z;
            float newZ = ((orig > 0) ? 1 : -1) * 180 - (-1 * orig);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, newZ));
        }
        else mySprite.localScale = new Vector3(-1, 1, 1);


        float waitTime = 0.2f / spd;

        myAnim.SetBool("walk", true);

        for (int i=0; i<=100; i++)
        {
            transform.position = Vector3.Lerp(startPos, destination, (float)i / 100f);
            yield return new WaitForSeconds(waitTime);
            //if (myMode.Equals(Mode.search)) Debug.Log("my position: " + transform.position);
        }

        don[0] = true;
        myAnim.SetBool("walk", false);
    }

    public void spawn(AntQueen q, GatherManager gather)
    {
        spawnLocation = transform.position;
        destination = spawnLocation; //no need to move to
        queen = q;
        gatherManager = gather;

        Vector3 randomDirection = new Vector3(0, 0, Random.Range(-180, 180));
        transform.Rotate(randomDirection);

        StartCoroutine(idle());
    }

    public void guard()
    {
        StartCoroutine(guardCoroutine());
    }

    private IEnumerator guardCoroutine()
    {
        setMode(Mode.guard);
        Vector3 guardDest = Random.insideUnitCircle * 1.5f;
        bool[] moveDone = new bool[1];
        setDestination(guardDest, moveDone);
        yield return new WaitUntil(() => moveDone[0]);

        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Debug.Log("guarding");
        queen.numOnGuard++;
    }

    public void sendForSearch(int level)
    {
        StartCoroutine(crawlOutOfScreen(level));
    }

    private IEnumerator crawlOutOfScreen(int level) //and wait for time
    {
        setMode(Mode.search);

        queen.numAway++;
        numAwaySetTracker = true;

        Vector2 ran = Random.insideUnitCircle;
        Vector3 dest;

        if(transform.position.x > 0 && transform.position.y > 0)
        {
            dest = new Vector3(ran.x + cornerNE.x, ran.y + cornerNE.y, 0);
        }else if(transform.position.x > 0)
        {
            dest = new Vector3(ran.x + cornerSE.x, ran.y + cornerSE.y, 0);
        }else if (transform.position.y > 0)
        {
            dest = new Vector3(ran.x + cornerNW.x, ran.y + cornerNW.y, 0);
        }
        else
        {
            dest = new Vector3(ran.x + cornerSW.x, ran.y + cornerSW.y, 0);
        }

        bool[] moveDone = new bool[1];
        setDestination(dest, moveDone);

        yield return new WaitUntil(() => moveDone[0]);

        yield return new WaitForSeconds((level+1)*3f);
        //chance getting package
        float rand = Random.value;

        switch (level)
        {
            case 0: //normal kind
                if (rand < 0.8f)
                {
                    myPackage = gatherManager.instantiatePackage(mySprite, 0);
                }else if (rand < 0.9f)
                {
                    myPackage = gatherManager.instantiatePackage(mySprite, 1);
                } //else gets nothing and simply returns

                break;
            case 1:
                if (rand < 0.4f)
                {
                    myPackage = gatherManager.instantiatePackage(mySprite, 0);
                }
                else if (rand < 0.8f)
                {
                    myPackage = gatherManager.instantiatePackage(mySprite, 1);
                } //else gets nothing and simply returns
                break;
            case 2:
                if (rand < 0.2f)
                {
                    myPackage = gatherManager.instantiatePackage(mySprite, 0);
                }
                else if (rand < 0.75f)
                {
                    myPackage = gatherManager.instantiatePackage(mySprite, 1);
                }
                else
                {
                    myPackage = gatherManager.instantiatePackage(mySprite, 2);
                }
                break;
            default: break;
        }

        moveDone[0] = false;
        setDestination(spawnLocation, moveDone);
        gatherManager.startReturn();
        yield return new WaitUntil(() => moveDone[0]);

        if (myPackage != null) myPackage.GetComponent<disappear>().slowDisappear(queen);

        queen.numAway--;
        numAwaySetTracker = false;

        setMode(Mode.idle);
    }

    public void setDestination(Vector3 d, bool[] don)
    {
        destination = d;
        if (currentMoveTo != null) StopCoroutine(currentMoveTo);
        currentMoveTo = moveTo(don);
        StartCoroutine(currentMoveTo);
    }

    void OnDestroy()
    {
        if (myMode.Equals(Mode.guard)) queen.numOnGuard--;
        if (numAwaySetTracker) queen.numAway--;
    }

    }
