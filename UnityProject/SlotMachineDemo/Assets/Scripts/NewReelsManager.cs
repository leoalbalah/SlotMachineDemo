using System.Collections;
using UnityEngine;

public class NewReelsManager : MonoBehaviour
{
    #region Properties

    [SerializeField] private NewGameManager gameManager;

    public NewReel[] reels;
    public int minSpeed;
    public int maxSpeed;

    [HideInInspector] public bool mustStop;
    private ArrayList _results = new ArrayList();

    #endregion

    private void Update()
    {
        if (!mustStop) return;

        if (!reels[reels.Length - 1].isSpinning)
        {
            mustStop = false;
            gameManager.ShowResults();
            _results = new ArrayList();
        }
        else
        {
            for (var i = 0; i < _results.Count; i++)
            {
                if (i == 0)
                {
                    reels[i].stopOn = (int)_results[i];
                }
                else if (!reels[i - 1].isSpinning)
                {
                    reels[i].stopOn = (int)_results[i];
                    _results[i - 1] = -1;
                }
            }
        }
    }

    /// <summary>  
    /// Invokes the Spinning Coroutine.
    /// </summary>
    public void Spin()
    {
        StartCoroutine(StartSpinEnum());
    }

    /// <summary>  
    /// Cycles throw the reels and starts the spinning with a small delay between iterations.
    /// </summary>
    private IEnumerator StartSpinEnum()
    {
        var wait = new WaitForSeconds(.3f);

        foreach (var reel in reels)
        {
            reel.isSpinning = true;
            reel.speed = Random.Range(minSpeed, maxSpeed);
            yield return wait;
        }
    }

    public void Stop(ArrayList results)
    {
        _results = results;
        mustStop = true;
    }
}