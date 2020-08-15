using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnt : MonoBehaviour
{
    private Vector3 spawnLocation;
    public float moveSpd;

    public Vector3 destination;

    public Vector3 cornerNW, cornerNE, cornerSW, cornerSE;

    public Animator myAnim;
    public Transform mySprite;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


    }

    private IEnumerator moveTo(bool[] don)
    {

        float spd = moveSpd;
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

        for (int i = 0; i <= 100; i++)
        {
            transform.position = Vector3.Lerp(startPos, destination, (float)i / 100f);
            yield return new WaitForSeconds(waitTime);
        }

        don[0] = true;
        myAnim.SetBool("walk", false);
        myAnim.SetBool("attack", true);
    }

    public void spawn()
    {
        spawnLocation = transform.position;
        destination = spawnLocation; //no need to move to

        Vector3 randomDirection = new Vector3(0, 0, Random.Range(-180, 180));
        transform.Rotate(randomDirection);

    }


    public void setDestination(Vector3 d, bool[] don)
    {
        destination = d;
        StartCoroutine(moveTo(don));
    }
}
