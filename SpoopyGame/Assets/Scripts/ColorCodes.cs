using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum ColorCodeValues
{
    Red, Green, Blue, Yellow, Pink, Cyan, White, Black
}

public class ColorCode
{
    public Color Color { get; private set; }
    public string Name { get; private set; }


    public static List<ColorCode> colors;

    static ColorCode()
    {
        colors = new List<ColorCode>();

        colors.Add(new ColorCode("red", new Color(1, 0, 0)));
        colors.Add(new ColorCode("green", new Color(0, 1, 0)));
        colors.Add(new ColorCode("blue", new Color(0, 0, 1)));
        colors.Add(new ColorCode("yellow", new Color(1, 1, 0)));
        colors.Add(new ColorCode("pink", new Color(1, 0, 1)));
        colors.Add(new ColorCode("cyan", new Color(0, 1, 1)));
        colors.Add(new ColorCode("white", new Color(1, 1, 1)));
        colors.Add(new ColorCode("black", new Color(0.15f, 0.15f, 0.15f)));
    }

    public ColorCode(string name, Color color)
    {
        this.Name = name;
        this.Color = color;
    }

    public static ColorCode RandomColor()
    {
        return colors[Random.Range(0, colors.Count)];
    }

    public static ColorCode FromValue(ColorCodeValues val)
    {
        return colors.Where(n => n.Name == val.ToString().ToLower()).FirstOrDefault();
    }
}
