using UnityEngine;
using System.Collections;

/// <summary>
/// 炸弹脚本
/// </summary>
public class Bomb : MonoBehaviour {
	/// <summary>
	/// 爆炸效果预制体
	/// </summary>
	public GameObject explosion;
	/// <summary>
	/// 爆炸粒子特效
	/// </summary>
	public ParticleSystem explosionFX;	
	/// <summary>
	/// 爆炸音效
	/// </summary>
	public AudioClip boom;
	/// <summary>
	/// 燃烧引信的音效
	/// </summary>
	public AudioClip fuse;
	/// <summary>
	/// 引信燃烧时间
	/// </summary>
	public float fuseTime = 1.5f;
	/// <summary>
	/// 炸弹的杀伤半径
	/// </summary>
	public float bombRadius = 10f;
	/// <summary>
	/// 敌人被抛起的力量
	/// </summary>
	public float bombForce = 100f;


	// 引用相关脚本
	private LayBombs layBombs;
	private PickupSpawner pickupSpawner;
	
	void Awake () {
		pickupSpawner = GameObject.Find ("PickupManager").GetComponent<PickupSpawner> ();
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		if (obj != null)
			layBombs = obj.GetComponent<LayBombs> ();
	}

	// Use this for initialization
	void Start () {
		// 开始协程 BombDetonation
		// 如果炸弹没有父亲， 则它已经被玩家引爆了
		if (transform.root == transform)
			StartCoroutine (BombDetonation ());
	}

	/// <summary>
	/// 爆炸
	/// </summary>
	public void Explode() {
		// 设置玩家已经安装炸弹的状态为flase
		layBombs.bombLaid = false;
		// 继续出现空降包
		//pickupSpawner.StartCoroutine (pickupSpawner.DeliverPickup);

		// 碰撞检测， 在指定半径内的敌人
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemies"));

		foreach (Collider2D en in enemies) {
			// 获取检测到的碰撞器的刚体对象
			Rigidbody2D rb = en.GetComponent<Rigidbody2D> ();
			// 如果是敌人
			if (rb != null && rb.tag == "Enemy") {
				// 将hp设为0
				rb.gameObject.GetComponent<Enemy> ().HP = 0;
				// 计算出从炸弹到敌人的矢量
				Vector3 deltaPos = rb.transform.position - transform.position;
				// 根据矢量计算出刚体力的大小（越近力量越大）
				Vector3 force = deltaPos.normalized * bombForce;
				// 为敌人附加刚体力，使其被炸飞
				rb.AddForce (force);
			}
		}

		// 播放特效
		explosionFX.transform.position = transform.position;
		explosionFX.Play ();

		// 实例化爆炸预制体
		Instantiate(explosion,transform.position, Quaternion.identity);

		// 播放音效
		AudioSource.PlayClipAtPoint(boom, transform.position);

		// 删除炸弹
		Destroy (gameObject);
	}

	// 炸弹爆炸
	IEnumerator BombDetonation() {
		// 播放音效
		if (fuse != null)
			AudioSource.PlayClipAtPoint (fuse, transform.position);
		// 等待指定的时间
		yield return new WaitForSeconds (fuseTime);
		// 爆炸
		Explode ();
	}
}
