using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialScript : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM4", 9600);
    string serialData;

    public static bool player1Data;
    

    // Start is called before the first frame update
    void Start()
    {
        stream.Open();

        Thread t = new Thread(new ThreadStart(ParseData));
        t.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ParseData() {
        while(true) {
            serialData = stream.ReadLine();
            string[] parsedData = serialData.Split(' ');

            // If the Serial Monitor is sending data for Player 1
            if (parsedData[0] == "p1:") {
                BladeScript.playerInput1 = int.Parse(parsedData[1]);
            }

            // If the Serial Monitor is sending data for Player 2
            else if (parsedData[0] == "p2:") {
                BladeScript.playerInput2 = int.Parse(parsedData[1]);
            }
        }
    }
}
