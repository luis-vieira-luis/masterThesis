using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     // ------------------------------------------------------------------------------------------ //
// Situation #4
// Choose source using controller buttons
// Use controller to change listener directivity

// ------------------------------------------------------------------------------------------ //

/// This scrips is to be attached to the hand controller.
/// The number selected will select the source number and activate the controll for it
/// </summary>

public class SourceSelection_Controllers : MonoBehaviour
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

private bool[] angleInRangeArray;
private float[] azArray;

private float coordenateX;
private float[] coordenateXArray;
private float coordenateY;
private float[] coordenateYArray;



// ------------------------------------------------------------------------------------------ //

private int sourceNumber;
private int numberOfSources = 7;

// ------------------------------------------------------------------------------------------ //

public int selectedSource;

// ------------------------------------------------------------------------------------------ //

private GameObject[] sourceArray;

private Material sourceColor;

// ------------------------------------------------------------------------------------------ //

TouchpadInput touchPad;

public GameObject controller;

private float fingerX;
private float fingerY;
private float[] fingerCoordernatesArray;

private float fingerPressedX;
private float fingerPressedY;


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

        // get public components from TouchpadInput.cs
        touchPad = GetComponent<TouchpadInput>();

}
// Use this for initialization
void Start()
{
        //numberOfSources = GetComponent<AngleSource> ().numberOfSources;
        sourceArray = new GameObject[] { Source1, Source2, Source3, Source4, Source5, Source6, Source7 };

        for (int idx = 0; idx < sourceArray.Length; idx++)
        {
                sourceArray[idx].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        // initialize array
        fingerCoordernatesArray = new float[2];

        // initialise array az
        azArray = new float[7];

        coordenateXArray = new float[7];
        coordenateYArray = new float[7];
}

IEnumerator waitThreeSeconds()
{
        yield return new WaitForSeconds(3);
        Debug.Log("we are waiting 3 seconds");
}
// Update is called once per frame
void Update()
{
        // store in an array the bolean variables for each source, regarding if the are in the range of the listener
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

        // store in an array the values of the relative angle between sources and listener
        azArray = new float[]
        {
                Source1.gameObject.GetComponent<AngleSource>().azimuth,
                Source2.gameObject.GetComponent<AngleSource>().azimuth,
                Source3.gameObject.GetComponent<AngleSource>().azimuth,
                Source4.gameObject.GetComponent<AngleSource>().azimuth,
                Source5.gameObject.GetComponent<AngleSource>().azimuth,
                Source6.gameObject.GetComponent<AngleSource>().azimuth,
                Source7.gameObject.GetComponent<AngleSource>().azimuth
        };

        // Debug.Log(azArray.Length);
        //Debug.Log(azArray[0] + " " + azArray[1] + " " + azArray[2]);


        for (int k = 0; k < sourceArray.Length; k++)
        {
                // take the relative angle (az) between source and listener for each source

                coordenateX = Mathf.Cos((azArray[k] * Mathf.Deg2Rad));
                coordenateY = Mathf.Sin((azArray[k] * Mathf.Deg2Rad));
                coordenateXArray[k] = coordenateX;
                coordenateYArray[k] = coordenateY;

        }

        // ------------------------------------------------------------------------------------------ //
        // Select Source script

        // find which source is in the range of the listener and if it is, angleInRangeArray(i) is true
        for (int i = 0; i < angleInRangeArray.Length; i++)
        {
                if (angleInRangeArray[i] == true)
                {
                        // ------------------------------------------------------------------------------------------- //
                        // using trackpad controller

                        // fetch finger x and y coordenates from TouchpadInput.cs and round them to 3 decimals
                        fingerX = Mathf.Round(controller.gameObject.GetComponent<TouchpadInput>().fingerCoordenatesX * 100f) / 100f;
                        fingerY = Mathf.Round(controller.gameObject.GetComponent<TouchpadInput>().fingerCoordenatesY * 100f) / 100f;

                        fingerCoordernatesArray = new float[] { fingerX, fingerY };

                        // ------------------------------------------------------------------------------------------- //
                        // if the coordenates of the source's relative position to the head matches the finger coordenates in the controller
                        // change color of that source to yellow, else keep the original color

                        for (int sourceMatch = 0; sourceMatch < azArray.Length; sourceMatch++)
                        {
                                if (fingerPressedX == 0)
                                {
                                        if (fingerCoordernatesArray[0] >= -0.2f + coordenateXArray[sourceMatch] && fingerCoordernatesArray[0] <= 0.2f + coordenateXArray[sourceMatch] && fingerCoordernatesArray[1] >= -0.2f + coordenateYArray[sourceMatch] && fingerCoordernatesArray[1] <= 0.2f + coordenateYArray[sourceMatch])
                                        {
                                                // change the color of the source selected to yellow
                                                sourceArray[sourceMatch].gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                                        }
                                        else
                                        {
                                                // if the source is not selected then change color to blue
                                                sourceArray[sourceMatch].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;

                                                // for all other sources out of range, define color as blue and directivity alpha to 0.2
                                                for (int n = 0; n < numberOfSources; n++)
                                                {
                                                        if (sourceArray[n] != sourceArray[sourceMatch])
                                                        {
                                                                // change color to blue
                                                                sourceArray[n].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                                                                // for use with oculus controllers OVRInput.GetDown(OVRInput.Button.Four)
                                                                sourceArray[n].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 0.2f;
                                                        }
                                                }
                                        }
                                }
                        }

                        // ------------------------------------------------------------------------------------------- //
                        // if trackpad is pressed on the top center part, increase directivity values
                        if (fingerPressedX < 0.3f && fingerPressedX > -0.3f && fingerPressedY > 0.75f && fingerPressedY < 0.99f)
                        {
                                // increase value of source alpha directivity by 0.1
                                sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha += 0.1f;
                        } else if (fingerPressedX < 0.3F && fingerPressedX > -0.3f && fingerPressedY < -0.75f && fingerPressedY > -0.99f)
                        {
                          // if trackpad is pressed on the bottom center part, decrease directivity value
                          sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha -= 0.1f;
                        }











                        // ------------------------------------------------------------------------------------------ //
                        // Selected Source will turn yellow and directivity can be controlled
                        if (Input.GetKeyDown(KeyCode.J) && selectedSource < numberOfSources)
                        {
                                // to go up the sources' list
                                selectedSource += 1;
                                // source selected will show as yellow
                                sourceArray[selectedSource].gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;

                                // for use with oculus controllers OVRInput.GetDown(OVRInput.Button.Four)
                                sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 1;

                                // for all other sources out of range, define color as blue and directivity alpha to 0.2
                                for (int n = 0; n < numberOfSources; n++)
                                {
                                        if (sourceArray[n] != sourceArray[selectedSource])
                                        {
                                                // change color to blue
                                                sourceArray[n].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                                                // for use with oculus controllers OVRInput.GetDown(OVRInput.Button.Four)
                                                sourceArray[n].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 0.2f;
                                        }
                                }
                        }
                        else if (Input.GetKeyDown(KeyCode.K) && selectedSource > 0)
                        {
                                // to go down the sources' list
                                selectedSource -= 1;
                                // source selected will show as yellow
                                sourceArray[selectedSource].gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;


                                // for all other sources out of range, define color as blue and directivity alpha to 0.2
                                for (int j = 0; j < numberOfSources; j++)
                                {
                                        if (sourceArray[j] != sourceArray[selectedSource])
                                        {
                                                // cange color to blue
                                                sourceArray[j].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                                                // for use with oculus controllers OVRInput.GetDown(OVRInput.Button.Four)
                                                sourceArray[j].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 0.2f;
                                        }
                                }
                        }














                        // ------------------------------------------------------------------------------------------ //
                        // Change directivity script ((keys I and U)

                        // increase source directivity alpha value
                        if (Input.GetKeyDown(KeyCode.I) && sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha <= 1)
                        {
                                // for use with oculus controllers OVRInput.GetDown(OVRInput.Button.Four)
                                sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha += 0.05f;
                                Debug.Log("We are rising the source directivity to " + sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha);

                                if (sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha > 1)
                                {
                                        sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 1;
                                }

                        }
                        // decrease source directivity alpha value
                        else if (Input.GetKeyDown(KeyCode.U) && sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha >= 0)
                        {
                                // for use with oculus controllers OVRInput.GetDown(OVRInput.Button.Four)
                                sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha -= 0.05f;
                                Debug.Log("We are lowering the source directivity to " + sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha);

                                if (sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha < 0)
                                {
                                        sourceArray[i].gameObject.GetComponent<ResonanceAudioSource>().directivityAlpha = 0;
                                }
                        }

                        // ------------------------------------------------------------------------------------------ //
                        // Change sound pressure (gainDb) script (keys N and M)

                        // increase source sound pressure
                        if (Input.GetKeyDown(KeyCode.M))
                        {
                                sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().gainDb += 0.05f;

                        }
                        // decrease source sound pressure
                        else if (Input.GetKeyDown(KeyCode.N))
                        {
                                sourceArray[selectedSource].gameObject.GetComponent<ResonanceAudioSource>().gainDb -= 0.05f;

                        }
                }
        }
}
}
