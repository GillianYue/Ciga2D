using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextUpdate : MonoBehaviour
{
    // Start is called before the first frame update

    public Text myText;
    public Slider myself;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = myself.value + "";
    }
}
