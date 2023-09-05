using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool anwsers;
    Timer timer;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
    }

    public void OnClick()
    {
        Title.Loading("InGame");
    }

    public void O()
    {
        anwsers = true;
        timer.timerTime = 0;
    }
    public void X()
    {
        anwsers = false;
        timer.timerTime = 0;
    }
}
