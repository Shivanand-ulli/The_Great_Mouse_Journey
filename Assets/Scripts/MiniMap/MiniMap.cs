using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] Transform player;

    void Update()
    {
        Vector3 newPostion = player.transform.position;
        newPostion.y = transform.position.y;
        transform.position = newPostion;
    }
}
