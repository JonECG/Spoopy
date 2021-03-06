﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public enum DoorFrequency
{
    Never = 0, Normal = 1, Always = 2
}

public class RoomInfo
{
    public string name;
    public GameObject gameObjectReference;
    public int numOfDoors;
    public DoorFrequency physicalDoorFrequency;
    internal int count = 0;
}

public static class CustomRooms
{

    public static IEnumerable<RoomInfo> Rooms { get; private set; }

    public static void ResetCounts()
    {
        foreach(RoomInfo room in Rooms)
        {
            room.count = 0;
        }
    }

    public static RoomInfo GetRandomOf(IEnumerable<RoomInfo> selection)
    {
        int low = selection.Min( n=>n.count );
        List<RoomInfo> lowest = selection.Where(n => n.count == low).ToList();
        RoomInfo result = lowest[Random.Range(0, lowest.Count)];
        result.count++;
        return result;
    }

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

            int df = 0;
            int.TryParse(parts[2], out df );

            roomStuff.Add(new RoomInfo() { name = name, numOfDoors = numDoors, gameObjectReference = Resources.Load("CustomRooms/" + name) as GameObject, physicalDoorFrequency = (DoorFrequency) df });
        }

        Rooms = roomStuff;
    }
}