using System.Collections;
using UnityEngine;

public class NewReelsManager : MonoBehaviour
{
    [SerializeField] private NewGameManager gameManager;

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

        gameManager.ShowResults();
    }
}