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
}
