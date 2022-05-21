using System.Collections;
using UnityEngine;

public class ReelsManager : MonoBehaviour
{
    public SlotMatch[] SlotMatches;
    public GameObject slotPrefab;

    public Reel[] reels;

    public void Spin()
    {
        StartCoroutine(NewMethod(reels));
    }

    private static IEnumerator NewMethod(Reel[] reelsx)
    {
        WaitForSeconds wfs = new WaitForSeconds(1);
        foreach (var reel in reelsx)
        {
            reel.isSpinning = true;
            yield return wfs;
        }
    }

    public void StopSpin()
    {
        foreach (var reel in reels)
        {
            reel.isSpinning = false;
        }
    }
}