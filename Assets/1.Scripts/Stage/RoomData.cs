using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    None,
    StartRoom,
    PortalRoom,
    SotreRoom,
    EventRoom
}

[CreateAssetMenu]
public class RoomSO : ScriptableObject
{
    [Header("Info")]
    public RoomType roomType;

    [Header("Prefabs")]
    public GameObject Goblin;
    public GameObject Mushroom;    
}
