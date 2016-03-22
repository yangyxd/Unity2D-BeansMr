using UnityEngine;
using System.Collections;

/// <summary>
/// 角色发射子弹
/// </summary>
public class Gun : MonoBehaviour {
	/// <summary>
	/// 要发身的火箭刚体
	/// </summary>
	public Rigidbody2D rocket;
	/// <summary>
	/// 子弹的速度
	/// </summary>
	public float speed;

	private PlayerControl playerCtrl;
	private Animator anim;

	void Awake() {
		// 当前脚本是附着在角色属下的一个空物体Gun中，所以要获取角色身上的脚本，需要用transform.root
		anim = transform.root.gameObject.GetComponent<Animator> ();
		playerCtrl = transform.root.GetComponent<PlayerControl> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// 如果按下了攻击键（默认是鼠标左键）
		if (InputEx.GetButtonDown ("Fire1")) {
			// 播放动画，播放音效
			anim.SetTrigger("Shoot");
			GetComponent<AudioSource>().Play();
			if (playerCtrl.facingRight) {
				// 角色向右时，向右边发射子弹
				// 实例化一个子弹
				Rigidbody2D bulletInstance = Instantiate (rocket, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
				// 直接为刚体设置速度
				bulletInstance.velocity = new Vector2 (speed, 0);
			} else {
				// 角色向左时，向右边发射子弹
				// 实例化一个子弹
				Rigidbody2D bulletInstance = Instantiate (rocket, transform.position, Quaternion.Euler (new Vector3 (0, 0, 180f))) as Rigidbody2D;
				// 直接为刚体设置速度
				bulletInstance.velocity = new Vector2 (-speed, 0);
			}
		}
	}
}
