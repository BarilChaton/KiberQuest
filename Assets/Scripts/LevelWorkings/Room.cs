using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] traps;
    private Vector3[] initalPosition;

    private void Awake()
    {
        initalPosition = new Vector3[traps.Length];
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i] != null)
                initalPosition[i] = traps[i].transform.position;
        }
    }

    public void ActivateRoom(bool _status)
    {
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i] != null)
            {
                traps[i].SetActive(_status);
                traps[i].transform.position = initalPosition[i];
            }
        }
    }
}
