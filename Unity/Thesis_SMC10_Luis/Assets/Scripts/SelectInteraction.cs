using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script to activate or deactivate the scripts for interaction
/// Only one script can be used at a time
/// 1. Controllers only
/// 2. Head rotation only
/// 3. Controllers and Head rotaiton
/// 4. Eye tracking
/// </summary>

public class SelectInteraction : MonoBehaviour
{


    // ---------------------------------------------------------------------------------------------------//
    // to select type of interaction controll

    public GameObject SelectSource_Controllers;
    public GameObject SelectSource_Head;
    public GameObject SelectSource_EyeTracking;
    public GameObject SelectSource_ControllersnHead;

    // array to store game objects
    private GameObject[] interactionArray;

    // array to select interaction
    private KeyCode[] selectionArray;


    void Awake () {



        // ---------------------------------------------------------------------------------------------------// 
        
        SelectSource_Controllers = GameObject.Find("SelectSource_Controllers");
        SelectSource_Head = GameObject.Find("SelectSource_Head");
        SelectSource_EyeTracking = GameObject.Find("SelectSource_EyeTracking");
        SelectSource_ControllersnHead = GameObject.Find("SelectSource_ControllersnHead");

        SelectSource_Controllers.SetActive(false);
        SelectSource_Head.SetActive(false);
        SelectSource_EyeTracking.SetActive(false);
        SelectSource_ControllersnHead.SetActive(false);

        Debug.Log("Don´t forget to turn on the interaction script with the keyboard numbers!!");
    }

    // Update is called once per frame
    void Update()
    {
        // ---------------------------------------------------------------------------------------------------//
        // Select type of interaction

        // store key codes in array
        selectionArray = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

        // store game objects in interaction array
        interactionArray = new GameObject[] { SelectSource_Controllers, SelectSource_Head, SelectSource_EyeTracking, SelectSource_ControllersnHead };


        // check the state of the game object and active or deactivate accordingly
        for (int i = 0; i < selectionArray.Length; i++)
        {
            if (Input.GetKeyDown(selectionArray[i]) == true && interactionArray[i].active == false)
            {
                Debug.Log("key " + selectionArray[i] + " is pressed"); 
                interactionArray[i].SetActive (true);

                for (int j = 0; j < interactionArray.Length; j++)
                {
                    if (interactionArray[j] != interactionArray[i])
                    {
                        interactionArray[j].SetActive(false);
                    }
                }
            }
        }
    }
}
