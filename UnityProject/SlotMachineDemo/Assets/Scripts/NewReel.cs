using UnityEngine;

/// <summary>  
/// Implementation of the slot machine reel.
/// </summary>
public class NewReel : MonoBehaviour
{
    #region Properties

    [SerializeField] private Transform reseter;
    [SerializeField] public GameObject reelObject;

    public bool isSpinning = false;

    private GameObject _supportReelObject;
    public int stopOn = -1;
    public float speed = 10;

    #endregion

    private void Start()
    {
        // Instantiate a second reel for infinite loop.
        _supportReelObject = Instantiate(reelObject, transform);
        var separation = reelObject.GetComponent<NewReelObject>().figureId.Length * 270;
        _supportReelObject.transform.position = reelObject.transform.position + new Vector3(0, separation, 0);
    }

    private void Update()
    {
        var mustStop = stopOn != -1;
        if (isSpinning & !mustStop)
        {
            Spin();
        }

        if (isSpinning & mustStop)
        {
            Stop();
        }
    }

    /// <summary>  
    /// Handles the reel stopping.
    /// </summary>
    private void Stop()
    {
        var stopPos = reelObject.GetComponent<NewReelObject>().yPos[stopOn] + 380;

        if (reelObject.transform.position.y != stopPos)
        {
            _supportReelObject.transform.SetParent(reelObject.transform);

            var position = reelObject.transform.position;

            if (position.y > stopPos)
                position = Vector3.MoveTowards(
                    position,
                    new Vector3(position.x, stopPos, position.z),
                    speed * 2);
            else
                position = Vector3.MoveTowards(
                    position,
                    new Vector3(position.x, position.y - 160, position.z),
                    speed * 2);

            reelObject.transform.position = position;
        }
        else
        {
            isSpinning = false;
            stopOn = -1;
            _supportReelObject.transform.SetParent(reelObject.transform.parent);
        }

        Reset();
    }

    /// <summary>  
    /// Handles the reel spinning.
    /// </summary>
    private void Spin()
    {
        MoveDown();
        Reset();
    }

    /// <summary>  
    /// Repositions the reels when below a certain point to ensure a smooth infinite loop.
    /// </summary>
    private void Reset()
    {
        _supportReelObject.transform.SetParent(reelObject.transform.parent);

        if (reelObject.transform.position.y <= reseter.position.y)
            reelObject.transform.position = _supportReelObject.transform.position + new Vector3(0,
                _supportReelObject.GetComponent<NewReelObject>().figureId.Length * 270, 0);
        if (_supportReelObject.transform.position.y <= reseter.position.y)
            _supportReelObject.transform.position = reelObject.transform.position + new Vector3(0,
                reelObject.GetComponent<NewReelObject>().figureId.Length * 270, 0);
    }

    private void MoveDown()
    {
        reelObject.transform.Translate(new Vector3(0, -160, 0) * (speed * Time.deltaTime));
        _supportReelObject.transform.Translate(new Vector3(0, -160, 0) * (speed * Time.deltaTime));
    }
}