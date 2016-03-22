using UnityEngine;
using System.Collections;

/// <summary>
/// 主摄像机跟踪
/// </summary>
public class CameraFollow : MonoBehaviour {
	/// <summary>
	/// 摄像机与跟踪目标x轴距离, 摄像机与目标距离超过此值时，才进行跟踪
	/// </summary>
	public float xMargin = 1f;	
	/// <summary>
	/// 摄像机与跟踪目标y轴距离, 摄像机与目标距离超过此值时，才进行跟踪
	/// </summary>
	public float yMargin = 1f;

	/// <summary>
	/// 摄像机跟随主角时在x轴的平滑程度
	/// </summary>
	public float xSmooth = 8f;
	/// <summary>
	/// 摄像机跟随主角时在y轴的平滑程度
	/// </summary>
	public float ySmooth = 8f;

	/// <summary>
	/// 摄像机能运动的最大xy轴坐标
	/// </summary>
	public Vector2 maxXY;
	/// <summary>
	/// 摄像机能运动的最小xy轴坐标
	/// </summary>
	public Vector2 minXY;

	// 要跟踪的主角
	private Transform player;

	void Awake() {
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		if (obj != null) player = obj.transform;
	}

	// 在固定更新函数中来跟踪，一般意味着在主角跟新之后执行
	void FixedUpdate () {
		if (player != null)
			TrackPlayer ();
	}

	// 如果摄像机当前的x轴位置与主角的距离大于xMargin返回true
	bool CheckXMargin() {		
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}

	// 如果摄像机当前的y轴位置与主角的距离大于yMargin返回true
	bool CheckYMargin() {
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}

	// 跟踪主角
	void TrackPlayer() {
		// 设置摄像机的目标位置为当前位置
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		// 检查摄像机与主角的距离，如果超出，则开始跟踪，让摄像机向角色平滑靠近
		if (CheckXMargin ())
			targetX = Mathf.Lerp (transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		if (CheckYMargin ())
			targetY = Mathf.Lerp (transform.position.y, player.position.y, ySmooth * Time.deltaTime);

		// 将摄像机的位置限定在minXY, maxXY中
		targetX = Mathf.Clamp (targetX, minXY.x, maxXY.x);
		targetY = Mathf.Clamp (targetY, minXY.y, maxXY.y);

		// 将摄像机移动到新的位置
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}

}
