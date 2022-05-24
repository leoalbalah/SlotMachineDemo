using UnityEngine;

public class NewReel : MonoBehaviour
{
    public bool isSpinning = false;

    [SerializeField] private Transform reseter;
    [SerializeField] private GameObject reelObject;
    private GameObject _supportReelObject;

    private void Start()
    {
        _supportReelObject = Instantiate(reelObject, transform);

        var separation = reelObject.GetComponent<NewReelObject>().figureId.Length * 270;

        _supportReelObject.transform.position = reelObject.transform.position + new Vector3(0, separation, 0);
    }

    private void Update()
    {
        if (isSpinning)
        {
            Spin();
        }
    }

    public void Spin()
    {
        MoveDown();

        Reset();
    }

    private void Reset()
    {
        if (reelObject.transform.position.y <= reseter.position.y)
            reelObject.transform.position = _supportReelObject.transform.position + new Vector3(0,
                _supportReelObject.GetComponent<NewReelObject>().figureId.Length * 270, 0);
        if (_supportReelObject.transform.position.y <= reseter.position.y)
            _supportReelObject.transform.position = reelObject.transform.position + new Vector3(0,
                reelObject.GetComponent<NewReelObject>().figureId.Length * 270, 0);
    }

    private void MoveDown()
    {
        var position = reelObject.transform.position;

        position = Vector3.MoveTowards(
            position,
            new Vector3(position.x, position.y - 160,
                position.z),
            5);

        reelObject.transform.position = position;

        var supportPosition = _supportReelObject.transform.position;

        supportPosition = Vector3.MoveTowards(
            supportPosition,
            new Vector3(supportPosition.x, supportPosition.y - 160,
                supportPosition.z),
            5);

        _supportReelObject.transform.position = supportPosition;
    }
}