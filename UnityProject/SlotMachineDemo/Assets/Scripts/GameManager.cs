using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Properties

    [Header("Settings")] [SerializeField] private int roundCount;
    [SerializeField] private int coins = 100;
    [SerializeField] private int betAmount = 20;

    [Header("Data")] [SerializeField] private Roller[] rollers;
    [SerializeField]private ReelsManager _reelsManager;
    [SerializeField] private WinCombination[] winCombinations;
    [SerializeField] private WinPattern[] winPatterns;

    [Header("Debug")] [SerializeField] private bool debugMode;


    #endregion

    public void Spin()
    {
        if (betAmount > coins)
            return;

        roundCount++;
        PayFee();

        // Generate Values

        var roll = new ArrayList();
        Roll(roll);

        if (debugMode)
        {
            var tRoll = "";
            foreach (var r in roll)
            {
                tRoll += r + " ";
            }

            Debug.Log("Roll " + tRoll);
        }

        var winCombinations = CheckWinCombinations(roll);

        Debug.Log(winCombinations.Count);

        // Display Spin
        
        _reelsManager.Spin();
        
        // Display Result Phase
    }

    private ArrayList CheckWinCombinations(ArrayList roll)
    {
        var victoryCombs = new ArrayList();

        var resMatrix = GetResultMatrix(roll);

        for (int i = 0; i < winPatterns.Length; i++)
        {
            var correctSlotCount = 0;
            foreach (var combination in winCombinations)
            {
                foreach (var coord in winPatterns[i].coords)
                {
                    if (resMatrix[coord.col, coord.row] == combination.slot)
                        correctSlotCount++;
                }

                if (correctSlotCount == combination.amount)
                {
                    var winner = new WinPatternCombination
                    {
                        WinPattern = winPatterns[i],
                        WinCombination = combination
                    };
                    victoryCombs.Add(winner);
                }

                correctSlotCount = 0;
            }
        }

        return victoryCombs;
    }

    private int[,] GetResultMatrix(ArrayList roll)
    {
        var resMatrix = new int[rollers.Length, 3];

        for (int r = 0; r < roll.Count; r++)
        {
            // 1st Row
            if ((int)roll[r] == 0)
                resMatrix[r, 0] = rollers[r].slots[(int)(rollers[r].slots.Length - 1)];
            else
                resMatrix[r, 0] = rollers[r].slots[(int)(roll[r]) - 1];

            // 2nd Row
            resMatrix[r, 1] = rollers[r].slots[(int)(roll[r])];

            // 3rd Row
            if ((int)roll[r] + 1 == rollers[r].slots.Length)
                resMatrix[r, 2] = rollers[r].slots[0];
            else
                resMatrix[r, 2] = rollers[r].slots[(int)(roll[r]) + 1];
        }

        return resMatrix;
    }

    private void Roll(ArrayList roll)
    {
        foreach (var roller in rollers)
        {
            var rollerRoll = Random.Range(0, roller.slots.Length);
            roll.Add(rollerRoll);
        }
    }

    private void PayFee()
    {
        coins -= betAmount;
    }
}

public struct WinPatternCombination
{
    public WinPattern WinPattern;
    public WinCombination WinCombination;
}