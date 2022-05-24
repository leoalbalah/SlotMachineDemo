using System.Collections;
using UnityEngine;

public class NewReelsManager : MonoBehaviour
{
    [SerializeField] private NewGameManager gameManager;

    public NewReel[] reels;
    public int minSpeed;
    public int maxSpeed;

    public bool mustStop;
    private ArrayList _results = new ArrayList();

    private void Update()
    {
        if (mustStop)
        {
            if (!reels[reels.Length - 1].isSpinning)
            {
                mustStop = false;
                gameManager.ShowResults();
                _results = new ArrayList();
            }
            else
            {
                for (int i = 0; i < _results.Count; i++)
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
    }

    public void Spin()
    {
        StartCoroutine(StartSpinEnum());
    }

    private IEnumerator StartSpinEnum()
    {
        for (var i = 0; i < reels.Length; i++)
        {
            reels[i].isSpinning = true;
            reels[i].speed = Random.Range(minSpeed, maxSpeed);
            yield return new WaitForSeconds(.5f);
        }
    }

    public void Stop(ArrayList results)
    {
        _results = results;
        mustStop = true;
    }
}