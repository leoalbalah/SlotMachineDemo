using System.Collections;
using UnityEngine;

public class NewReelsManager : MonoBehaviour
{
    public NewReel[] reels;
    
    public void Spin()
    {
        for (var i = 0; i < reels.Length; i++)
        {
            reels[i].isSpinning = true;
        }
    }

    public void Stop(ArrayList results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            reels[i].stopOn = (int)results[i];
        }
    }
}
