using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Situation #2
/// Choose target with head angle when looking over 2 seconds
/// Control directivity of the listener with the VR controller A; Control directivity of the source with the VR controller B
/// </summary>

public class SourceSelection_ControllersnHead : MonoBehaviour
{


    // define sources as gameObjects

    private GameObject Source1;
    private GameObject Source2;
    private GameObject Source3;
    private GameObject Source4;
    private GameObject Source5;
    private GameObject Source6;
    private GameObject Source7;


    // ------------------------------------------------------------------------------------------ //

    AngleSource angleSource;
    ResonanceAudioSource resonanceSource;

    private bool angleInRange;
    private bool[] angleInRangeArray;
    public float az;

    // ------------------------------------------------------------------------------------------ //

    private int numberOfSources = 7;
    public int selectedSource;
    private int sourcesRight;
    private int sourcesLeft;
    private string sourceName;


    // ------------------------------------------------------------------------------------------ //

    public GameObject[] sourceArray;

    private Material sourceColor;



    void Awake()
    {

        // fetch game objects
        Source1 = GameObject.Find("Source1");
        Source2 = GameObject.Find("Source2");
        Source3 = GameObject.Find("Source3");
        Source4 = GameObject.Find("Source4");
        Source5 = GameObject.Find("Source5");
        Source6 = GameObject.Find("Source6");
        Source7 = GameObject.Find("Source7");


        // get public componenets from AngleSource.cs
        angleSource = GetComponent<AngleSource>();

        // get public components from ResonanceAudioSource.cs
        resonanceSource = GetComponent<ResonanceAudioSource>();

    }

    void Start()
    {
        //numberOfSources = GetComponent<AngleSource> ().numberOfSources;
        sourceArray = new GameObject[] { Source1, Source2, Source3, Source4, Source5, Source6, Source7 };
    }

    void Update()
    {
        // define boolean array with inTheRange variables from the different sources
        angleInRangeArray = new bool[]
        {
            Source1.gameObject.GetComponent<AngleSource>().inTheRange,
            Source2.gameObject.GetComponent<AngleSource>().inTheRange,
            Source3.gameObject.GetComponent<AngleSource>().inTheRange,
            Source4.gameObject.GetComponent<AngleSource>().inTheRange,
            Source5.gameObject.GetComponent<AngleSource>().inTheRange,
            Source6.gameObject.GetComponent<AngleSource>().inTheRange,
            Source7.gameObject.GetComponent<AngleSource>().inTheRange,

        };

        // ------------------------------------------------------------------------------------------ //
        // evalute in all the array of AngleInRange which source is in range
        // when in range, change the gain to 4 dB and the listener directivity alpha to 0.4f (wider sound propagation)
        // the other sources, out of range, should reduce their gain to -3dB 
        for (int i = 0; i < angleInRangeArray.Length; i++)
        {

            if (angleInRangeArray[i] == true)
            {
                // change color of the selected source to green
                sourceArray[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.green;

                // define source in range sound pressure and listener directivity alpha values
                sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().gainDb = 4;

                if (Input.GetKeyDown(KeyCode.K) && sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha <= 1)
                {
                    sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha += 0.05f;

                    if (sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha > 1) {
                        sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 1;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.J) && sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha >= 0)
                {
                    sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha -= 0.05f;

                    if (sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha < 0)
                    {
                        sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 0;
                    }
                }

               

                // for all other sources out of range, change color to blue and reduce sound pressure to -6 and listener directivity alpha to 0.9
                for (int j = 0; j < numberOfSources; j++)
                {

                    if (sourceArray[j] != sourceArray[i])
                    {
                        sourceArray[j].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                        // for use with oculus controllers OVRInput.GetDown(OVRInput.Button.Four)
                        sourceArray[j].gameObject.GetComponent<ResonanceAudioSource>().gainDb = -6;
                        sourceArray[j].gameObject.GetComponent<ResonanceAudioSource>().listenerDirectivityAlpha = 0.9f;

                    }
                }   
            } 
        }
    }
}
