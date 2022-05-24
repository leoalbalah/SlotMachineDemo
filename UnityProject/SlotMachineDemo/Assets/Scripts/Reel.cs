using System;
using System.Collections;
using UnityEngine;

/// <summary>  
/// Handles Reel properties and methods.
/// </summary>
public class Reel : MonoBehaviour
{
    public int[] slots;
    public bool isSpinning;
    public float speedRange;
    public readonly ArrayList Instances = new ArrayList();

    [SerializeField] private Transform resetTransform;

    public int stopOn = -1;
    private ReelsManager _manager;
    private bool mustStop = false;

    private void Awake()
    {
        _manager = GetComponentInParent<ReelsManager>();
    }

    private void Start()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            var temp = Instantiate(_manager.slotPrefab, transform);

            temp.GetComponent<Slot>().graphics.sprite = _manager.slotMatches[slots[i] - 1].graphics;

            temp.transform.position += new Vector3(0, (i * 260), 0);
            Instances.Add(temp);
        }
    }

    private void Update()
    {
        Spin();
    }

    private void Spin()
    {
        const int endPos = 540;

        mustStop = stopOn != -1;

        if (isSpinning)
        {
            if (mustStop)
            {
                for (var i = Instances.Count - 1; i > -1; i--)
                {
                    if (i == stopOn)
                    {
                        if (Math.Abs(endPos - ((GameObject)Instances[i]).transform.position.y) < 1)
                        {
                            isSpinning = false;
                            stopOn = -1;
                        }

                        if (((GameObject)Instances[i]).transform.position.y < stopOn)
                        {
                            if (((GameObject)Instances[i]).transform.position.y <= resetTransform.position.y)
                            {
                                var upperPos = 0f;
                                foreach (var instance in Instances)
                                {
                                    if (((GameObject)instance).transform.position.y > upperPos)
                                        upperPos = ((GameObject)instance).transform.position.y;
                                }

                                ((GameObject)Instances[i]).transform.position += new Vector3(0, upperPos + 320, 0);
                            }

                            ((GameObject)Instances[i]).transform.position = Vector3.MoveTowards(
                                ((GameObject)Instances[i]).transform.position,
                                resetTransform.position,
                                speedRange);
                        }
                        else if (((GameObject)Instances[i]).transform.position.y > stopOn)
                        {
                            ((GameObject)Instances[i]).transform.position = Vector3.MoveTowards(
                                ((GameObject)Instances[i]).transform.position,
                                new Vector3(((GameObject)Instances[i]).transform.position.x, endPos,
                                    ((GameObject)Instances[i]).transform.position.z),
                                speedRange);
                        }
                    }
                    else
                    {
                        if (((GameObject)Instances[i]).transform.position.y <= resetTransform.position.y)
                        {
                            var upperPos = 0f;
                            foreach (var instance in Instances)
                            {
                                if (((GameObject)instance).transform.position.y > upperPos)
                                    upperPos = ((GameObject)instance).transform.position.y;
                            }

                            ((GameObject)Instances[i]).transform.position += new Vector3(0, upperPos + 320, 0);
                        }

                        ((GameObject)Instances[i]).transform.position = Vector3.MoveTowards(
                            ((GameObject)Instances[i]).transform.position,
                            resetTransform.position,
                            speedRange);
                    }
                }
            }
            else
            {
                for (var i = Instances.Count - 1; i > -1; i--)
                {
                    if (((GameObject)Instances[i]).transform.position.y <= resetTransform.position.y)
                    {
                        var upperPos = 0f;
                        foreach (var instance in Instances)
                        {
                            if (((GameObject)instance).transform.position.y > upperPos)
                                upperPos = ((GameObject)instance).transform.position.y;
                        }

                        ((GameObject)Instances[i]).transform.position += new Vector3(0, upperPos + 320, 0);
                    }

                    ((GameObject)Instances[i]).transform.position = Vector3.MoveTowards(
                        ((GameObject)Instances[i]).transform.position,
                        resetTransform.position,
                        speedRange);
                }
            }
        }
    }
}