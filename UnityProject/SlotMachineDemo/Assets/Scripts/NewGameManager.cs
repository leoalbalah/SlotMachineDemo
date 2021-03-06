using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NewGameManager : MonoBehaviour
{
    #region Properties

    [Header("Settings")] [SerializeField] private int roundCount;
    [SerializeField] private int credits = 100;
    [SerializeField] private int betAmount = 20;
    [Range(1, 3)] [SerializeField] private int multiplier;

    [Header("Data & References")] [SerializeField]
    private SlotMatch[] slotMatches;

    [SerializeField] private NewReelsManager reelsManager;
    [SerializeField] private GameObject canvas;
    [SerializeField] private WinCombination[] winCombinations;
    [SerializeField] private WinPattern[] winPatterns;
    [SerializeField] private Button spinBtn;
    [SerializeField] private RewardPanel rewardPanel;
    [SerializeField] private TextMeshProUGUI creditsUI;
    [SerializeField] private TextMeshProUGUI betUI;
    private object _topComb;
    private int[,] _resMatrix;
    private bool _debugMode = true;

    #endregion

    #region Bets & Credits

    private void Start()
    {
        // Sets UI values
        creditsUI.SetText(credits.ToString());
        multiplier = 1;
        betAmount = 20;
        betUI.SetText(betAmount.ToString());
    }

    public void IncreaseBet()
    {
        if (multiplier >= 3) return;
        multiplier++;
        betAmount = multiplier * 20;
        betUI.SetText(betAmount.ToString());
    }

    public void DecreaseBet()
    {
        if (multiplier <= 1) return;
        multiplier--;
        betAmount = multiplier * 20;
        betUI.SetText(betAmount.ToString());
    }

    private void PayFee()
    {
        credits -= betAmount;
        creditsUI.SetText(credits.ToString());
    }

    private void Earn()
    {
        credits += multiplier * ((WinPatternCombination)_topComb).WinCombination.reward;
        creditsUI.SetText(credits.ToString());
    }

    #endregion

    /// <summary>  
    /// Starts the Spinning Functionality.
    /// </summary>
    public void Spin()
    {
        // Ensure That the credits are enough for the spin
        if (betAmount > credits)
            return;

        spinBtn.interactable = false;

        PayFee();

        var roll = new ArrayList();
        Roll(roll);

        var winCombs = CheckWinCombinations(roll);

        // Debug
        roundCount++;
        var tRoll = roll.Cast<object>().Aggregate("", (current, r) => current + (r + " "));
        Debug.Log("Roll " + tRoll);
        Debug.Log("Winning Combinations: " + winCombs.Count);

        // Gets the more profitable combination.
        _topComb = winCombs[0];
        foreach (var winComb in winCombs)
        {
            if (((WinPatternCombination)winComb).WinCombination.reward >
                ((WinPatternCombination)_topComb).WinCombination.reward)
                _topComb = winComb;
        }

        // Starts the spinning and after a small delay the stop reels functions.
        reelsManager.Spin();
        LeanTween.delayedCall(Random.Range(2, 4), () => { reelsManager.Stop(roll); });
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
            var slots = reelsManager.reels[r].reelObject.GetComponent<NewReelObject>().figureId;

            // 1st Row
            if ((int)roll[r] + 1 == slots.Length)
                resMatrix[r, 2] = slots[0];
            else
                resMatrix[r, 0] = slots[(int)(roll[r]) + 1];
            t += resMatrix[r, 0] + " ";

            // 2nd Row
            resMatrix[r, 1] = slots[(int)(roll[r])];
            m += resMatrix[r, 1] + " ";


            // 3rd Row
            if ((int)roll[r] == 0)
                resMatrix[r, 0] = slots[(int)(slots.Length - 1)];

            else
                resMatrix[r, 2] = slots[(int)(roll[r]) - 1];
            b += resMatrix[r, 2] + " ";
        }

        if (_debugMode)
        {
            Debug.Log(t);
            Debug.Log(m);
            Debug.Log(b);
        }

        return resMatrix;
    }

    /// <summary>  
    /// Generates one random value per reel.
    /// <param name="roll">reference ArrayList(int) to store the random roll result.</param>.
    /// </summary>
    private void Roll(ArrayList roll)
    {
        foreach (var roller in reelsManager.reels)
        {
            var rollerRoll = Random.Range(0, roller.reelObject.GetComponent<NewReelObject>().figureId.Length);
            roll.Add(rollerRoll);
        }
    }

    public void ShowResults()
    {
        Earn();

        var highLights = Instantiate(((WinPatternCombination)_topComb).WinPattern.highLights, canvas.transform);

        rewardPanel.multiplier.SetText(multiplier + "X");
        rewardPanel.amount.SetText(((WinPatternCombination)_topComb).WinCombination.amount.ToString());
        rewardPanel.credits.SetText((((WinPatternCombination)_topComb).WinCombination.reward * multiplier).ToString());
        rewardPanel.figure.sprite = slotMatches[((WinPatternCombination)_topComb).WinCombination.slot - 1].graphics;

        rewardPanel.transform.gameObject.SetActive(true);

        LeanTween.delayedCall(2f, () =>
        {
            Destroy(highLights);
            rewardPanel.transform.gameObject.SetActive(false);
            spinBtn.interactable = true;
        });
    }
}