using Photon.Pun;
using UnityEngine;

public class GameManagerNew : MonoBehaviour
{
    void Awake()
    {
        CreatePlayer();
    }
    
    void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
}
