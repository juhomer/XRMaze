using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralNumberGenerator : MonoBehaviour
{
    public static int currentPosition = 0;
    public static string key = "123123241124312412412412412431241241312123444322142341";

    private void Awake()
    {
        //if (MazeValues.isArPlayer)
        //    Generate();
    }

    public static string Generate(int length = 69)
    {
        for (int i = 0; i < length; i++)
        {
            key += (Random.Range(1, 5)).ToString();

        }
        print($"INNA GENERATE KEY IS {key}");
        return key;
        //return "";
    }



    public static int GetNextNumber()
    {
        Debug.Log($"KEY IS {key}");
        string currentNum = key.Substring(currentPosition++ % key.Length, 1);
        return int.Parse(currentNum);
        //return Random.Range(1, 5);
    }
}