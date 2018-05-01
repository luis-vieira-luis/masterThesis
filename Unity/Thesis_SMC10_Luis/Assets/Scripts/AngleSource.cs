using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleSource : MonoBehaviour
{

    // ---------------------------------------------------------------------------------------------------//
    //public OSC osc;
    public Transform listener;
    // gameObject cameraRig

    private Vector3 distanceVector;
    // vector of coordenate distances between listener and source

    private float zDistance;
    // z coordenate of distance vector between source and listener
    private float distanceListenrSource;
    // distance value between source and listener
    private float theta;
    // angle between front vector in oculus headset and source

    private float distanceListenrSourceXY;

    public GameObject head;
    private float headRotation;
    public float azimuth;

    // ---------------------------------------------------------------------------------------------------//
    // For Circle displacement

    public int numberOfSources;
    public int sourceNumber;
    public float radius;

    private float positionAngleDeg;
    private float positionAngleRad;

    // ---------------------------------------------------------------------------------------------------//
    // For the virtual azimuth range (poor man's choice)

    public float maxLimit;
    // maximum azimuth angle
    public float minLimit;
    // minimum azimuth angle
    private Vector3 virtualLoudspeakerPos;
    // virtual loudspeaker with the coordenates for the updated position to send via OSC to MAX/MSP

    public bool inTheRange;
   


    // Use this for initialization
    void Start()
    {
        // ---------------------------------------------------------------------------------------------------// 
        // display sources in a circle position

        positionAngleDeg = (360 / numberOfSources) * (sourceNumber - 1);
        positionAngleRad = (positionAngleDeg) * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(positionAngleRad);
        float y = 0;
        float z = radius * Mathf.Sin(positionAngleRad);

        this.gameObject.transform.position = new Vector3(x, y, z);





        /*
        // send position of the sources one time at the beginning of the scene
        OscMessage message = new OscMessage();

        message.address = "/" + gameObject.name;
        message.values.Add(transform.position.x);
        message.values.Add(transform.position.y);
        message.values.Add(transform.position.z);
        osc.Send(message);
        */


        // ---------------------------------------------------------------------------------------------------// 
        // rotate the sources so the sound production face of the cube faces the listener in the center

        float angleRotationZ = -((360 / numberOfSources) * (sourceNumber - 1) + 90);
        this.gameObject.transform.Rotate(0, angleRotationZ, 0);

    }

    // Update is called once per frame
    void Update()
    {

        //---------------------------------------------------------------------------------------------------//
        // Calculate 3 dimensional vector of distance between source and listener position
        distanceVector = this.transform.position - listener.transform.position;

        // Calculate XZ dimensional vector of distance between source and listerner position
        //Change to distanceListenrSourceXZ
        distanceListenrSource = Mathf.Sqrt(Mathf.Pow(distanceVector.z, 2) + Mathf.Pow(distanceVector.x, 2));

        // Calculates absolute value of theta angle and addresses the correct signal according to the position on of the source
        // If the source is placed in the left side
        if (this.transform.position.x - listener.transform.position.x < 0)
        {  
            theta = -(Mathf.Acos(distanceVector.z / distanceListenrSource)) * Mathf.Rad2Deg;
        }
        // If the source is placed in the left side
        else
        {
            theta = (Mathf.Acos(distanceVector.z / distanceListenrSource)) * Mathf.Rad2Deg;
        }

        // ---------------------------------------------------------------------------------------------------// 
        // Tracking rotation of the headset
        headRotation = head.transform.eulerAngles.y;

        if (headRotation >= 180)
        {
            headRotation = (head.transform.eulerAngles.y - 360);
        }

        // ---------------------------------------------------------------------------------------------------//
        // Calculate the relative angle of rotation between source and listener
        azimuth = (theta - headRotation);

        // ---------------------------------------------------------------------------------------------------//
        // Calculate the elevation angle
        //distanceListenrSourceXY = Mathf.Sqrt(Mathf.Pow(distanceVector.y, 2) + Mathf.Pow(distanceVector.x, 2));






        // ---------------------------------------------------------------------------------------------------//
  
        maxLimit = (360 / numberOfSources) / 2;         // approx. 24
        minLimit = -((360 / numberOfSources) / 2) - 1;   // approx. -24


        if (azimuth > minLimit && azimuth < maxLimit)
        {
            
            inTheRange = true;
        }
        else
        {
            inTheRange = false;
        }

            







        /*

        // ---------------------------------------------------------------------------------------------------//
        // OSC MESSAGES

        // sending rotation angle between sound source and listener (theta)
        OscMessage message = new OscMessage();
        message.address = "/" + gameObject.name + "azimuth";
        message.values.Add(azimuth);
        osc.Send(message);


        // sending distance XZ between sound source and listener (distanceListenerSource) 
        message = new OscMessage();
        message.address = "/" + gameObject.name + "distanceXZ";
        message.values.Add(distanceListenrSource);
        osc.Send(message);
        */

        // ---------------------------------------------------------------------------------------------------//
        // DEBUG 
        //Debug.Log("headRotation = " + headRotation);
        //Debug.Log(gameObject.name + " azimuth = " + azimuth);

    }
}
