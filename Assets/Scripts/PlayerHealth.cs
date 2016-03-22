using UnityEngine;
using System.Collections;

/// <summary>
/// 玩家状态脚本
/// </summary>
public class PlayerHealth : MonoBehaviour {
    /// <summary>
    /// 玩家生命值
    /// </summary>
	public float HP = 100f;
    /// <summary>
    /// 损失生命值需要间隔多长时间
    /// </summary>
	public float repeatDamagePeriod = 2f;
    /// <summary>
    /// 玩家损失HP时的音效数组
    /// </summary>
	public AudioClip[] ouchClips;
    /// <summary>
    /// 受到伤害时，玩家身体被向上挤压的力度
    /// </summary>
	public float hurtForce = 10f;
    /// <summary>
    /// 当敌人攻击到玩家时，每次损失的HP量
    /// </summary>
	public float damageAmount = 10f;

	private SpriteRenderer healthBar;			// 显示生命条渲染器
	private float lastHitTime;					// 玩家最后受攻击时间
	private Vector3 healthScale;				// 最初的生命条缩放尺寸（满血时）
	private PlayerControl playerControl;		// 玩家控制器
	private Animator anim;						// 玩家动画控制器


	void Awake () {
		// 引用相关设置
		playerControl = GetComponent<PlayerControl>();
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		// 记录生命条初始缩放尺寸
		healthScale = healthBar.transform.localScale;
	}


	void OnCollisionEnter2D (Collision2D col) {
		// 如果碰撞到敌人
		if(col.gameObject.tag == "Enemy") {
			// 并且已经大于损失生命值的间隔时间
			if (Time.time > lastHitTime + repeatDamagePeriod) {
				// 如果HP大于0（角色还存活）.
				if(HP > 0f) {
					// 受到攻击，并重新记录最后受攻击时间
					TakeDamage(col.transform); 
					lastHitTime = Time.time; 
				} else {
                    // 如果玩家生命值已经为空，让他落入河中，并重新加载关卡

					// 触发所有碰撞器
					Collider2D[] cols = GetComponents<Collider2D>();
					foreach(Collider2D c in cols) {
						c.isTrigger = true;
					}

					// 移动所有精灵到玩家排序层的前面
					SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
					foreach(SpriteRenderer s in spr) {
						s.sortingLayerName = "UI";
					}

					// 禁用玩家控制脚本
					GetComponent<PlayerControl>().enabled = false;

					// 禁用枪支脚本，不能让一个死人继续攻击
					GetComponentInChildren<Gun>().enabled = false;

					// 触发死亡动画
					anim.SetTrigger("Die");
				}
			}
		}
	}

    // 玩家受到伤害
	void TakeDamage (Transform enemy){
		// 让玩家无法跳跃
		playerControl.jump = false;

		// 创建一个敌人把玩家向上推的向量
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// 为玩家添加刚体力，使用上一步的向量
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

		// 减少玩家的生命值
		HP -= damageAmount;

		// 更新玩家生命条的显示
		UpdateHealthBar();

		// 播放音效
		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
	}

    // 更新玩家生命条的显示
    public void UpdateHealthBar ()
	{
		// 根据玩家的HP值，设置生命条绿色到红色之间的比例。
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - HP * 0.01f);

		// 根据玩家的HP值，设置生命条x轴的缩放比例
		healthBar.transform.localScale = new Vector3(healthScale.x * HP * 0.01f, 1, 1);
	}
}
