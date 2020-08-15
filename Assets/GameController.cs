using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public AntQueen queen;
    public DisasterManager disasterManager;
    public GatherManager gatherManager;

    public GameObject[] hideOnStart, hideAfterTimeline;
    public bool timeline;
    void Start()
    {
        if (timeline)
            foreach (GameObject g in hideOnStart)
            {
                g.SetActive(false);
            }
        else
            StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        foreach (GameObject g in hideOnStart)
        {
            g.SetActive(true);
        }

        foreach (GameObject g in hideAfterTimeline)
        {
            g.SetActive(false);
        }

        queen.startGame();
        disasterManager.startGame();
    }
}
