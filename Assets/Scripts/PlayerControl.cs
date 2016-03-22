using UnityEngine;
using System.Collections;

/// <summary>
/// 玩家控制脚本
/// </summary>
public class PlayerControl : MonoBehaviour {
    /// <summary>
    /// 控制角色朝向
    /// </summary>
    [HideInInspector]public bool facingRight = true;

    /// <summary>
    /// 判断角色是否跳起
    /// </summary>
    [HideInInspector]public bool jump = false;

    /// <summary>
    /// 每次为角色添加的刚体力大小
    /// </summary>
    public float moveForce = 365f;
	/// <summary>
	/// 每次为角色起跳时添加的刚体力大小
	/// </summary>
	public float jumpFore = 1000f;
    /// <summary>
    /// 角色移动的最大速度
    /// </summary>
    public float maxSpeed = 5f;
	/// <summary>
	/// 角色跳跃声效
	/// </summary>
	public AudioClip[] jumpClips;

	// 角色是否在地面上
	private bool grounded = false;
	// 角色跳跃射线检测辅助检测的物体
	private Transform groundCheck;

    private Animator anim;


    public float tauntProbability = 50f;    // 嘲弄产生机率
    public float tauntDelay = 1f;			// 当发生嘲笑时的延时
    public AudioClip[] taunts;				// 记录嘲笑玩家的剪辑
    private int tauntIndex;					// 记录最近嘲笑剪辑的数组索引


    void Awake() {
        anim = GetComponent<Animator>();
		groundCheck = transform.FindChild ("groundCheck");
    }

	// Update is called once per frame
	void Update () {
        // 通过2D射线检测角色与辅助检测对象之间是否存在Ground层中的物体
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		// Debug.DrawLine (transform.position, groundCheck.position, Color.red, 1f);
        // 如果玩家按下了跳跃键，并且检测到地面，则进入跳跃状态
		if (InputEx.GetButtonDown("Jump") && grounded)
            jump = true;
    }

    void FixedUpdate() {
        // 获取水平输入
		float h = InputEx.GetAxis("Horizontal");
        // 设置动画的速度为水平方向的输入值
        anim.SetFloat("Speed", Mathf.Abs(h));
        // 如果对象在x轴上的当前方向刚体力小于最大速度，则为对象施加刚体力
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            // 给玩家添加刚体力，力的大小为水平方向乘以moveForce, 这里通过h的使用，不需要判断力的方向了
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
        // 如果对象在x轴上的当前方向刚体力大于最大速度
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // 使用新的向量来设置速度，通过Mathf.Sign来获取角色的方向乘以最大速度来确定x轴上的速度，保持原方向
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        // 判断角色是否需要转身
        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

		// 角色跳跃处理
		if (jump) {
			// 播放跳跃动画
			anim.SetTrigger ("Jump");
			// 随机播放一个音效
			int i = Random.Range (0, jumpClips.Length);
			AudioSource.PlayClipAtPoint (jumpClips [i], transform.position);
			// 为角色跳跃增加向上的刚体力
			GetComponent<Rigidbody2D> ().AddForce (new Vector2(0f, jumpFore));
			// 重置jump为false，以免为角色不断的增加向上刚体力
			jump = false;
		}
    }

    // 角色转身
    void Flip() {
        // 设置角色的局部缩放属性 * -1，等于是当前值取反，就相当于转身了。
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// 嘲笑玩家
    /// </summary>
    /// <returns></returns>
    public IEnumerator Taunt() {
        // 随机生成一个0~100的数
        float tauntChance = Random.Range(0f, 100f);
        // 如果大于产生嘲笑的机率
        if (tauntChance > tauntProbability) {
            // 等待指定的秒数
            yield return new WaitForSeconds(tauntDelay);

            // 如果目标音源已经束速播放
            if (!GetComponent<AudioSource>().isPlaying) {
                // 随机生成一个用于嘲笑玩家的音效索引
                tauntIndex = TauntRandom();

                // 播放这个音效
                GetComponent<AudioSource>().clip = taunts[tauntIndex];
                GetComponent<AudioSource>().Play();
            }
        }
    }

    // 随机生成一个用于嘲笑玩家的音效数组索引
    int TauntRandom() {
        int i = Random.Range(0, taunts.Length);
        // 如果与上一次的相同，重新生成
        if (i == tauntIndex)
            return TauntRandom();
        else
            return i;
    }

}
