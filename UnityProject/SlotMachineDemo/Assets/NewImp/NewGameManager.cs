using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    [SerializeField] private NewReelsManager reelsManager;

    public void Spin()
    {
        Debug.Log("First Spin");
        reelsManager.Spin();
    }
}