using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float moveSpeed = 2f;		// 怪物移动速度
	public int HP = 2;					// 怪物HP值（生命）
	public Sprite deadEnemy;			// 敌人死掉后的样子
	public Sprite damagedEnemy;			// 敌人被攻击后的样子（可选）
	public AudioClip[] deathClips;		// 供敌人死亡时的播放音效数组
	public GameObject hundredPointsUI;	// 预制的一个在敌人死后出现的UI
	public float deathSpinMin = -100f;	// 旋转敌人的最小扭矩
	public float deathSpinMax = 100f;	// 旋转敌人的最大扭矩


	private SpriteRenderer ren;			// 引用精灵渲染器
	private Transform frontCheck;		// 辅助检测前方是否有物体
	private bool dead = false;			// 是否已经死亡
	private Score score;				// 引用评分脚本


	void Awake() {
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;
		score = GameObject.Find("Score").GetComponent<Score>();
	}

	void FixedUpdate () {
		// 枚举在敌人前方的碰撞器
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);
		//Debug.DrawLine (transform.position, frontCheck.position, Color.red, 1f);

		// 检测每一个对撞机
		foreach(Collider2D c in frontHits) {
			// 如果是障碍物
			if(c.tag == "Obstacle")	{
				// 停止检测并转身
				Flip ();
				break;
			}
		}

		// 在x方向上按指定的速度移动敌人
		GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, 
			GetComponent<Rigidbody2D>().velocity.y);

		// 如果敌人的HP不是初始值，并且设置了受伤后的显示效果，则显示出来
		if(HP == 1 && damagedEnemy != null)
			ren.sprite = damagedEnemy;

		// 如果敌人的HP为0或更小，则让敌人死掉
		if(HP <= 0 && !dead)
			Death ();
	}

	/// <summary>
	/// 受到伤害
	/// </summary>
	public void Hurt() {
		HP--;
	}

	/// <summary>
	/// 死亡
	/// </summary>
	void Death() {
		// 找到自己及所有子渲染器
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		// 禁用所有渲染器
		foreach(SpriteRenderer s in otherRenderers) {
			s.enabled = false;
		}

		// 重新启用主要的渲染器，并设置成受伤后的效果
		ren.enabled = true;
		ren.sprite = deadEnemy;

		// 增加分数
		score.score += 100;

		// 设置死亡状态为真
		dead = true;

		// 允许敌人通过指定的扭矩来旋转自己
		GetComponent<Rigidbody2D>().fixedAngle = false;
		GetComponent<Rigidbody2D>().AddTorque(Random.Range(deathSpinMin,deathSpinMax));

		// 找到所有碰撞机，并触发他们
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols) {
			c.isTrigger = true;
		}

		// 播放一个随机音效
		int i = Random.Range(0, deathClips.Length);
		AudioSource.PlayClipAtPoint(deathClips[i], transform.position);

		// 生成一个坐标，实例化敌人死后出来的UI
		if (hundredPointsUI != null) {
			Vector3 scorePos;
			scorePos = transform.position;
			scorePos.y += 1.5f;

			Instantiate (hundredPointsUI, scorePos, Quaternion.identity);
		}
	}

	/// <summary>
	/// 转身
	/// </summary>
	public void Flip() {
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
