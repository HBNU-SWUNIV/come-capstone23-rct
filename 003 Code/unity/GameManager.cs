using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update\
    public static GameManager instance;

    public WebRTCTest1 webrtc;

    private void Start() => webrtc.Printing();

    // Update is called once per frame
    void Update()
    {
        
    }
}
