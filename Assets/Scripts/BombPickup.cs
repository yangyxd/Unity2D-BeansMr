using UnityEngine;
using System.Collections;

// 炸弹包
public class BombPickup : MonoBehaviour {

	public AudioClip pickupClip;		// 音效

	private PickupSpawner pickupSpawner;// 引用拾取脚本
	private Animator anim;				// 引用动画控制器
	private bool landed = false;		// 是否已经落地

	void Awake() {
		pickupSpawner = GameObject.Find("PickupManager").GetComponent<PickupSpawner>();
		anim = transform.root.GetComponent<Animator>();
	}


	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log (other.tag);
		// 如果碰到玩家
		if(other.tag == "Player") {
			// 播放音效
			AudioSource.PlayClipAtPoint(pickupClip, transform.position);

			// 增加玩家的炸弹数量
			other.GetComponent<LayBombs>().bombCount++;

			// 在协程中继续制造空降包
			pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

			// 释放游戏对象
			Destroy(transform.root.gameObject);

		} else if(other.tag == "Ground" && !landed) {
			// 如果碰到建筑或UFO
			// 触发落地动画
			anim.SetTrigger("Land");
			// 解除与父物体的连接
			GameObject parent = transform.parent.gameObject;
			transform.parent = null;
			// 添加刚体属性
			gameObject.AddComponent<Rigidbody2D>();
			// 标识为已经落地
			landed = true;	
			// 删除父级对象
			Destroy (parent);
		}
	}


}
