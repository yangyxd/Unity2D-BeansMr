using UnityEngine;
using System.Collections;

/// <summary>
/// 医疗包脚本
/// </summary>
public class HealthPickup : MonoBehaviour {

	/// <summary>
	/// 这只箱子给玩家多少HP
	/// </summary>
	public float healthBonus;
	/// <summary>
	/// 音效
	/// </summary>
	public AudioClip collect;


	private PickupSpawner pickupSpawner;	// 引用拾取脚本
	private Animator anim;					// 引用动画控制器
	private bool landed;					// 箱子是否已经降落


	void Awake () {
		pickupSpawner = GameObject.Find("PickupManager").GetComponent<PickupSpawner>();
		anim = transform.root.GetComponent<Animator>();
	}


	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log (other.tag);
		// 如果碰到玩家
   		if(other.tag == "Player") {
			// 引用玩家键康状态脚本
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

			//给玩家增加生命值
			playerHealth.HP += healthBonus;
			playerHealth.HP = Mathf.Clamp(playerHealth.HP, 0f, 100f);

			// 更新玩家生命条
			playerHealth.UpdateHealthBar();

			// 在协程中继续制造空降包
			pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

			// 播放音效
			AudioSource.PlayClipAtPoint(collect, transform.position);

			// 释放游戏对象
			Destroy(transform.root.gameObject);

		} else if(other.tag == "Ground" && !landed) {
			// 如果碰到建筑、UFO等
			// 触发落地动画
			anim.SetTrigger("Land");

			// 支掉降落伞和父对象
			transform.parent = null;
			// 添加刚体属性，让玩家可以碰撞
			gameObject.AddComponent<Rigidbody2D>();
			// 标志为落地状态
			landed = true;	
		}
	}

}
