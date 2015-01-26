using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public struct RoomInfo
{
    public string name;
    public GameObject gameObjectReference;
    public int numOfDoors;
}

public static class CustomRooms
{

    public static IEnumerable<RoomInfo> Rooms { get; private set; }

    static string[] GetLineFiles(string s, params char[] separators)
    {
        List<string> results = new List<string>();

        int startLineIndex = -1;
        int currentIndex = 0;

        for (currentIndex = 0; currentIndex < s.Length; currentIndex++)
        {
            bool substantial = s[currentIndex] != ' ';
            bool lineCut = false;
            for (int i = 0; i < separators.Length; i++)
            {
                substantial = substantial && s[currentIndex] != separators[i];
                lineCut = lineCut || s[currentIndex] == separators[i];
            }

            if (substantial && startLineIndex == -1)
            {
                startLineIndex = currentIndex;
            }

            if (lineCut && startLineIndex != -1)
            {
                results.Add(s.Substring(startLineIndex, currentIndex - startLineIndex));
                startLineIndex = -1;
            }
        }

        if (startLineIndex != -1)
        {
            results.Add(s.Substring(startLineIndex));
        }

        return results.ToArray();
    }

    static CustomRooms()
    {
        TextAsset mydata = Resources.Load("CustomRooms/RoomDatas") as TextAsset;

        string s = Encoding.ASCII.GetString(mydata.bytes);

        string[] lines = GetLineFiles(s,';','\r','\n');

        List<RoomInfo> roomStuff = new List<RoomInfo>();

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');

            string name = parts[0];
            int numDoors = 1;
            int.TryParse(parts[1], out numDoors);

            roomStuff.Add(new RoomInfo() { name = name, numOfDoors = numDoors, gameObjectReference = Resources.Load("CustomRooms/" + name) as GameObject });
        }

        Rooms = roomStuff;
    }
}