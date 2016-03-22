using UnityEngine;
using System.Collections;

/// <summary>
/// 玩家得分显示
/// </summary>
public class Score : MonoBehaviour {
    public int score = 0;                   // 玩家当前分数

    private PlayerControl playerCtrl;       // 玩家控制脚本
    private int previousScore = 0;          // 前一帧中的得分

    // Use this for initialization
    void Start () {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
        // 设置分数文本
        GetComponent<GUIText>().text = "Score: " + score;

        if (previousScore != score) {
            // 执行角色控制脚本中的嘲笑协程， 随机播放音效
            playerCtrl.StartCoroutine(playerCtrl.Taunt());
            // 记住当前分数值
            previousScore = score;
        }

    }
}
