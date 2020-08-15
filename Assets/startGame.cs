using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class startGame : MonoBehaviour
{
    public GameController gameController;
    public PlayableDirector play;

    void Start()
    {
        StartCoroutine(startGameCall());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator startGameCall()
    {
        yield return new WaitUntil(() => (play.state != PlayState.Playing));
        gameController.StartGame();
    }
}
