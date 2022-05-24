using UnityEngine;

public class NewReel : MonoBehaviour
{
    public bool isSpinning = false;

    [SerializeField] private Transform reseter;
    public GameObject reelObject;
    private GameObject _supportReelObject;
    public int stopOn = -1;
    public float speed = 10;


    private void Start()
    {
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

    private void Spin()
    {
        MoveDown();

        Reset();
    }

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