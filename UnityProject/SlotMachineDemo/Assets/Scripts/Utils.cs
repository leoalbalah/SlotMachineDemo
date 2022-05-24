using UnityEngine;

/// <summary>  
/// Struct for storing a set of coordinates mapping a win pattern.
/// </summary>
[System.Serializable]
public struct WinPattern
{
    public Coordinate[] coords;
    public GameObject highLights;
}

/// <summary>  
/// Struct used for storing matrix positions.
/// </summary>
[System.Serializable]
public struct Coordinate
{
    public int row;
    public int col;
}

/// <summary>  
/// Used for storing a set of values that defines a win combination.
/// </summary>
[System.Serializable]
public struct WinCombination
{
    public int slot;
    public int amount;
    public int reward;
}

/// <summary>  
/// Auxiliary struct for storing the combination of pattern a combinations present in the roll.
/// </summary>
public struct WinPatternCombination
{
    public WinPattern WinPattern;
    public WinCombination WinCombination;
}

/// <summary>  
/// Auxiliary struct for a set of slot sprite and id.
/// </summary>
[System.Serializable]
public struct SlotMatch
{
    public int id;
    public Sprite graphics;
}