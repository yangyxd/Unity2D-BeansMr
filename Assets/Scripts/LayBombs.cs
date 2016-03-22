using UnityEngine;
using System.Collections;

/// <summary>
/// 炸弹层
/// </summary>
public class LayBombs : MonoBehaviour {
	/// <summary>
	/// 现在是否有炸弹被放置
	/// </summary>
	[HideInInspector]public bool bombLaid = false;
	/// <summary>
	/// 拥有的炸弹数量
	/// </summary>
	public int bombCount = 0;
	/// <summary>
	/// 玩家丢炸弹时的音效
	/// </summary>
	public AudioClip bombsAway;	
	/// <summary>
	/// 炸弹预制体
	/// </summary>
	public GameObject bomb;

	/// <summary>
	/// 在屏幕上显示当前玩家的炸弹数量
	/// </summary>
	private GUITexture bombHUD;
	private GUIText bombNum;


	void Awake () {
		GameObject obj = GameObject.Find ("ui_bombHUD");
		bombHUD = obj.GetComponent<GUITexture>();
		bombNum = obj.GetComponent<GUIText> ();
	}


	void Update () {
		// 如果按下炸弹的按钮，且没有已经安置的炸弹，且还有未被安置的炸弹
		if(InputEx.GetButtonDown("Fire2") && !bombLaid && bombCount > 0) {
			// 减少炸弹数量
			bombCount--;

			// 设置已经安放炸弹
			bombLaid = true;

			// 播放音效
			AudioSource.PlayClipAtPoint(bombsAway, transform.position);

			// 实例化一个炸弹到玩家所在位置
			Instantiate(bomb, transform.position, transform.rotation);
		}

		// 如果炸弹数量大于0，则显示炸弹标志
		if (bombCount > 0) {
			bombHUD.enabled = true;
			bombNum.enabled = true;
			bombNum.text = "x" + bombCount;
		} else {
			bombHUD.enabled = false;
			bombNum.enabled = false;
		}			
	}

}
