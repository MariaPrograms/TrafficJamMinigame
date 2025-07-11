using UnityEngine;
using System.Collections.Generic;

//Scriptable object of selectable colors and their corresponding button sprites for easy reference
[CreateAssetMenu(fileName = "Colors", menuName = "ScriptableObject/Colors")]
public class Colors : ScriptableObject
{
    public List<ColorInfo> colorsToUse => colorInfos;

    [SerializeField] private List<ColorInfo> colorInfos;

    public void ResetColors()
    {
        foreach (var item in colorInfos)
        {
            item.Selected = false;
        }
    }

    public ColorInfo GetRandomColor(bool getUnused)
    {
        int index;

        //Getting a color that isn't selected yet, for cars to be different colors
        if (getUnused)
        {
            List<ColorInfo> unusedColors = colorInfos.FindAll(a => !a.Selected);
            index = Random.Range(0, unusedColors.Count);
            unusedColors[index == -1 ? 0 : index].Selected = true;
            return unusedColors[index];
        }

        index = Random.Range(0, colorInfos.Count);
        colorInfos[index].Selected = true;
        return colorInfos[index];
    }

    public List<ColorInfo> GetUnusedColors()
    {
        return colorInfos.FindAll(a => !a.Selected);
    }

    public void SelectColor(string name)
    {
        GetColor(name).Selected = true;
    }

    public ColorInfo GetColor(string name)
    {
        return colorInfos.Find(a => a.Name == name);
    }

    public ColorInfo GetColor(int index)
    {
        return colorInfos[index];
    }
}

[System.Serializable]
public class ColorInfo
{
    public string Name;
    public Color Color;
    public Sprite Button;
    public bool Selected;
}
