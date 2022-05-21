using System;
using System.Collections;
using UnityEngine;

public class Reel : MonoBehaviour
{
    public int[] slots;
    public Transform reseter;
    private ReelsManager _manager;
    public bool isSpinning;

    private ArrayList _instances = new ArrayList();

    private void Awake()
    {
        _manager = GetComponentInParent<ReelsManager>();
    }

    private void Start()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            var temp = Instantiate(_manager.slotPrefab, transform);

            temp.GetComponent<Slot>().slotDef = slots[i] - 1;
            temp.GetComponent<Slot>().graphics.sprite = _manager.SlotMatches[slots[i] - 1].graphics;

            temp.transform.position += new Vector3(0, (i * 140), 0);
            _instances.Add(temp);
        }
    }

    private void Update()
    {
        if (isSpinning)
            Spin(1);
    }

    public void Spin(int endPos)
    {
        for (int i = _instances.Count - 1; i > -1; i--)
        {
            if (((GameObject)_instances[i]).transform.position.y <= reseter.position.y)
            {
                var upperPos = 0f;
                foreach (var instance in _instances)
                {
                    if (((GameObject)instance).transform.position.y > upperPos)
                        upperPos = ((GameObject)instance).transform.position.y;
                }

                ((GameObject)_instances[i]).transform.position += new Vector3(0, upperPos + 100, 0);
            }

            ((GameObject)_instances[i]).transform.position = Vector3.MoveTowards(
                ((GameObject)_instances[i]).transform.position,
                ((GameObject)_instances[i]).transform.position + Vector3.down * 100,
                3);
        }
    }
}