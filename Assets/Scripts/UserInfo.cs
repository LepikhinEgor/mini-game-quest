using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour {

    private static int userScreenSizeX;
    public static int UserScreenSizeX
    {
        get { return userScreenSizeX; }
    }

    private static int userScreenSizeY;
    public static int UserScreenSizeY
    {
        get { return userScreenSizeY; }
    }

    private void Awake()
    {
        userScreenSizeX = Screen.width;
        userScreenSizeY = Screen.height;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
