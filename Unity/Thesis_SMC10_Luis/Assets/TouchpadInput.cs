﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadInput : MonoBehaviour {


	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;

	private SteamVR_TrackedController controller;

	public float fingerCoordenatesX;
    public float fingerCoordenatesY;

    public float fingerOnPressCoordenateX;
    public float fingerOnPressCoordenateY;

    // ------------------------------------------------------------------------------------------ //


    // Use this for initialization
    void Start () {
	
		trackedObject = GetComponent<SteamVR_TrackedObject> ();
		controller = GetComponent<SteamVR_TrackedController> ();
		controller.PadClicked += Controller_PadClicked;

	}

	// only when the trackpad is pressed
	void Controller_PadClicked (object sender, ClickedEventArgs e)
	{

		if (device.GetAxis ().x != 0 || device.GetAxis ().y != 0) 
		{
		
			//Debug.Log (device.GetAxis ().x + " " + device.GetAxis ().y);
            fingerOnPressCoordenateX = device.GetAxis().x; 
            fingerOnPressCoordenateY = device.GetAxis().y;
        }
	}
	
	// Update is called once per frame
	void Update () {

		device = SteamVR_Controller.Input ((int)trackedObject.index);


        // ------------------------------------------------------------------------------------------ //
        // track users' finger when hovering the trackpad and store in array
        fingerCoordenatesX = device.GetAxis().x;
        fingerCoordenatesY = device.GetAxis().y;



    }
}
