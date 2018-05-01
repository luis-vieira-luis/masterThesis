using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Use script to control listener and sources' directivity with head rotation
/// Choose target with head angle when looking into a certain direction for more than 3 seconds
/// Use Head angle with the source to change source directivity
/// </summary>
public class SourceSelection_Head : MonoBehaviour { 

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

    public int selectedSource;
    private int sourcesRight;
    private int sourcesLeft;
    private string sourceName;


    // ------------------------------------------------------------------------------------------ //

    public GameObject[] sourceArray;

    private Material sourceColor;


    void Awake()
    {
        // get public componenets from AngleSource.cs
        angleSource = GetComponent<AngleSource>();

        // get public components from ResonanceAudioSource.cs
        resonanceSource = GetComponent<ResonanceAudioSource>();

        // fetch game objects 
        Source1 = GameObject.Find("Source1");
        Source2 = GameObject.Find("Source2");
        Source3 = GameObject.Find("Source3");
        Source4 = GameObject.Find("Source4");
        Source5 = GameObject.Find("Source5");
        Source6 = GameObject.Find("Source6");
        Source7 = GameObject.Find("Source7");

    }


    void Start()
    {
        //numberOfSources = GetComponent<AngleSource> ().numberOfSources;
        sourceArray = new GameObject[] { Source1, Source2, Source3, Source4, Source5, Source6, Source7 };
    }


	void Update () {

        // create an array with the boolean variables for all sources to understand if they are in the range of listening
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

        for (int i = 0; i < angleInRangeArray.Length; i++) {

            if (angleInRangeArray [i] == true) {

                // change color of the selected source to green
                sourceArray [i].gameObject.GetComponent<MeshRenderer> ().material.color = Color.green;

                // fetch relative azimuth angle between source and listener
                az = sourceArray [i].gameObject.GetComponent<AngleSource> ().azimuth;


                if (az > -12f && az < 12f)
                {
                    // if source is still in range but no in the range of [-12 : 12], then change listener Directivity Alpha and source sound pressure
                    sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().listenerDirectivityAlpha = 0.2f;
                    sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().gainDb = 0;

                    // for all other sources out of the range, change sound pressure to -6 dB and listenerDirectivityAlpha to 0.9
                    for(int j = 0; j < angleInRangeArray.Length; j++) {

                        if (sourceArray[j] != sourceArray[i])
                        {
                            sourceArray[j].gameObject.GetComponent<ResonanceAudioSource>().gainDb = -6;
                            sourceArray[j].gameObject.GetComponent<ResonanceAudioSource>().listenerDirectivityAlpha = 0.9f;
                        }
                    }
                }

                else if (az < -12 || az > 12)
                {
                    // if source is still in range but no in the range of [-12 : 12], then change listener Directivity Alpha and source sound pressure
                    sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().listenerDirectivityAlpha = 0.5f;
                    sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().gainDb = -3;

                    // for all other sources out of the range, change sound pressure to -6 dB and listenerDirectivityAlpha to 0.9
                    for(int j = 0; j < angleInRangeArray.Length; j++) {

                        if (sourceArray[j] != sourceArray[i])
                        {
                            sourceArray[j].gameObject.GetComponent<ResonanceAudioSource>().gainDb = -6;
                            sourceArray[j].gameObject.GetComponent<ResonanceAudioSource>().listenerDirectivityAlpha = 0.9f;
                        }
                    }
                }
                 

            } else if (angleInRangeArray [i] == false) {
                
                // when source is not in range any longer, change color to blue
                sourceArray [i].gameObject.GetComponent<MeshRenderer> ().material.color = Color.blue;
            }
        }
	}
}
