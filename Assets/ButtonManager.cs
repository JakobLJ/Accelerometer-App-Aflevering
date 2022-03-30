using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ButtonManager : MonoBehaviour
{
    public GameObject button;
    public GameObject result;
    public float highestAcceleration;
    public string fileName;
    public int fileNumber;
    public void ButtonDebug()
    {
        Debug.Log("Clicked!");
    }
    public void ButtonRemover()
    {
        button = GameObject.Find("Button");
        //I think it's easier to just move the button far away than actually deactivate it.
        button.transform.Translate(1000, 1000, 1000);
        StartCoroutine(ButtonReturner());
    }
    public void AccelerometerReader()
    {
        //This stuff all gets the text beneath the button ready to display the result.
        result = GameObject.Find("Result");
        result.GetComponent<Text>().text = "";
        //Resets the highest acceleration value so the upcoming if statement isn't affected by results from earlier tests.
        highestAcceleration = 0;
        StartCoroutine(AccelerometerReaderCoroutine());
    }
    IEnumerator ButtonReturner()
    {
        yield return new WaitForSecondsRealtime(5f);
        button.transform.Translate(-1000, -1000, -1000);
        Debug.Log("Button Reappears");
    }

    public IEnumerator AccelerometerReaderCoroutine()
    {
    //I chose to have it test the accelerometer twice a second for 5 seconds. This was a mistake, as I didn't take into account that a phone can easily complete a fall in less than half a second.
    for (int count = 0; count < 10; count++)
    {
    //I thought converting the acceleration data to a float via the distance function would be simpler than having to process a bunch of 3d vector data.
        if (Vector3.Distance(Input.acceleration, Vector3.zero) > highestAcceleration)
        {
            highestAcceleration = Vector3.Distance(Input.acceleration, Vector3.zero);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log("Highest Acceleration:" + highestAcceleration);
    }
    result.GetComponent<Text>().text = "Highest Acceleration:" + highestAcceleration;
    //All CSV stuff is taken from the video on moodle
    fileName = Application.dataPath + "/AccelerationFile" + fileNumber + ".csv";
    fileNumber++;
    TextWriter tw = new StreamWriter(fileName, true);
    tw.WriteLine(highestAcceleration);
    tw.Close();
    }


}
