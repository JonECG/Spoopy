using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CustomRooms
{
    public struct RoomInfo
    {
        public string name;
        public GameObject gameObjectReference;
        public int numOfDoors;
    }

    public static IEnumerable<RoomInfo> Rooms { get; private set; }

    static string[] GetLineFiles(string s, params char[] separators)
    {
        List<String> results = new List<String>();

        int startLineIndex = -1;
        int currentIndex = 0;

        for (currentIndex = 0; currentIndex < s.Length; currentLength++)
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
        TextAsset mydata = Resources.Load("CustomRooms/RoomDatas.txt") as TextAsset;

        string s = Encoding.ASCII.GetString(mydata.bytes);


    }
}