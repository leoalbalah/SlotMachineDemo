using System.Collections;
using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    [SerializeField] private NewReelsManager reelsManager;

    private ArrayList results;

    public void Spin()
    {
        results = new ArrayList() { 0, 7, 10, 1, 4};
        
        reelsManager.Spin();
        LeanTween.delayedCall(3, () => { reelsManager.Stop(results); });
    }
}