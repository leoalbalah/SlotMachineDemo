using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>  
/// Controls the Game Flow and Settings.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Properties

    [Header("Settings")] [SerializeField] private int roundCount;
    [SerializeField] private int credits = 100;
    [SerializeField] private int betAmount = 20;

    [SerializeField] private ReelsManager reelsManager;
    [SerializeField] private GameObject canvas;
    [SerializeField] private WinCombination[] winCombinations;
    [SerializeField] private WinPattern[] winPatterns;
    [SerializeField] private Button spinBtn;

    [Header("Audio")] [SerializeField] private AudioClip ambience;

    [Header("Debug")] [SerializeField] private bool debugMode;

    private object _topComb;
    private int[,] _resMatrix;

    #endregion

    public void Spin()
    {
        if (betAmount > credits)
            return;

        spinBtn.interactable = false;

        roundCount++;
        PayFee();

        // Spin the reels

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

        var winCombs = CheckWinCombinations(roll);

        Debug.Log("Winning Combinations: " + winCombs.Count);

        _topComb = winCombs[0];
        foreach (var winComb in winCombs)
        {
            if (((WinPatternCombination)winComb).WinCombination.reward >
                ((WinPatternCombination)_topComb).WinCombination.reward)
                _topComb = winComb;
        }

        StartCoroutine(SpinAndStop(roll));
    }

    private IEnumerator SpinAndStop(ArrayList roll)
    {
        yield return new WaitForSeconds(.5f);
        reelsManager.Spin();
        yield return new WaitForSeconds(3f);
        reelsManager.StopSpin(roll);
        yield return new WaitForSeconds(3f);
        var highLights = Instantiate(((WinPatternCombination)_topComb).WinPattern.highLights, canvas.transform);
        LeanTween.delayedCall(2f, () =>
        {
            Destroy(highLights);
            spinBtn.interactable = true;
        });

        spinBtn.interactable = true;
    }

    /// <summary>  
    /// Given the roll data analyzes and stores all the rolled winning combinations.
    /// <param name="roll">ArrayList(int) containing the randomly rolled position.</param>.
    /// </summary>
    private ArrayList CheckWinCombinations(ArrayList roll)
    {
        var victoryCombs = new ArrayList();

        _resMatrix = GetResultMatrix(roll);

        for (var i = 0; i < winPatterns.Length; i++)
        {
            var correctSlotCount = 0;
            foreach (var combination in winCombinations)
            {
                foreach (var coord in winPatterns[i].coords)
                {
                    if (_resMatrix[coord.col, coord.row] == combination.slot)
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

    /// <summary>  
    /// Given the roll data generates the final 5x3 slot result matrix.
    /// <param name="roll">ArrayList(int) containing the randomly rolled position.</param>.
    /// </summary>
    private int[,] GetResultMatrix(ArrayList roll)
    {
        var resMatrix = new int[reelsManager.reels.Length, 3];

        var t = "";
        var m = "";
        var b = "";

        for (var r = 0; r < roll.Count; r++)
        {
            // 1st Row
            if ((int)roll[r] == 0)
                resMatrix[r, 0] = reelsManager.reels[r].slots[(int)(reelsManager.reels[r].slots.Length - 1)];
            else
                resMatrix[r, 0] = reelsManager.reels[r].slots[(int)(roll[r]) - 1];
            t += resMatrix[r, 0] + " ";

            // 2nd Row
            resMatrix[r, 1] = reelsManager.reels[r].slots[(int)(roll[r])];
            m += resMatrix[r, 1] + " ";


            // 3rd Row
            if ((int)roll[r] + 1 == reelsManager.reels[r].slots.Length)
                resMatrix[r, 2] = reelsManager.reels[r].slots[0];
            else
                resMatrix[r, 2] = reelsManager.reels[r].slots[(int)(roll[r]) + 1];
            b += resMatrix[r, 2] + " ";
        }

        Debug.Log(t);
        Debug.Log(m);
        Debug.Log(b);

        return resMatrix;
    }

    private void Roll(ArrayList roll)
    {
        foreach (var roller in reelsManager.reels)
        {
            var rollerRoll = Random.Range(0, roller.slots.Length);
            roll.Add(rollerRoll);
        }
    }

    private void PayFee()
    {
        credits -= betAmount;
    }
}