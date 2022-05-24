using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>  
/// Handles the slot machine data and functionalities.
/// </summary>
public class ReelsManager : MonoBehaviour
{
    public SlotMatch[] slotMatches;
    public GameObject slotPrefab;
    public Reel[] reels;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private int[] _stopIds = { -1, -1, -1, -1, -1 };

    public void Spin()
    {
        foreach (var reel in reels)
        {
            reel.speedRange = Random.Range(minSpeed, maxSpeed);
        }

        StartCoroutine(StartSpin());
    }

    public void StopSpin(ArrayList roll)
    {
        StartCoroutine(StopSpinEnum(roll));
    }

    private IEnumerator StopSpinEnum(ArrayList roll)
    {
        for (int i = 0; i < reels.Length; i++)
        {
            var slotId = (int)roll[i];

            _stopIds[i] = slotId;
            reels[i].stopOn = slotId;

            yield return new WaitForSeconds(.5f);
        }
    }

    private IEnumerator StartSpin()
    {
        _stopIds = new[] { -1, -1, -1, -1, -1 };
        foreach (var reel in reels)
        {
            reel.isSpinning = true;
            yield return new WaitForSeconds(.5f);
        }
    }
}