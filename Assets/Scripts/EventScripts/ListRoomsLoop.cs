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
            if (NetManager.Connected && !NetManager.JoinedRoom)
            {
                NetManager.SendData("ListRooms", 20);
            }
        }
    }

    public void StartLoop()
    {
        StartCoroutine(listRoomLoop());
    }

    void Start()
    {
        StartLoop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
