using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VCG_Library;

public class ListRoomsLoop : MonoBehaviour
{
    IEnumerator listRoomLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (NetManager.Connected)
            {
                NetManager.SendData("ListRooms", 20);
            }
        }
    }

    void Start()
    {
        StartCoroutine(listRoomLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
