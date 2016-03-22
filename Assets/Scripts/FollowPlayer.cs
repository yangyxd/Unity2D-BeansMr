using UnityEngine;
using System.Collections;

/// <summary>
/// 让玩家的状态条跟着玩家移动
/// </summary>
public class FollowPlayer : MonoBehaviour {
    /// <summary>
    /// 相对于玩家的偏移量
    /// </summary>
    public Vector3 offset;  

    private Transform player; 


    // Use this for initialization
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = player.position + offset;
    }
}
