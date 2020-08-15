using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappear : MonoBehaviour
{
    public SpriteRenderer s;
    public int mySanBoost;
    public bool autoDestroy;

    void Start()
    {

        if(autoDestroy) StartCoroutine(longTempDestroy());
    }

    void Update()
    {
        
    }

    public void slowDisappear(AntQueen queen)
    {
        //TODO UI notify
        queen.addToSan(mySanBoost);

        StartCoroutine(slowlyDisappear());
    }

    private IEnumerator slowlyDisappear()
    {
        for(float o = 0; o<1; o += 0.02f)
        {
            s.color = new Color(s.color.r, s.color.g, s.color.b, 1 - o);
            yield return new WaitForSeconds(0.03f);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator longTempDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    public void simpleDisappear()
    {
        StartCoroutine(slowlyDisappear());
    }
}
