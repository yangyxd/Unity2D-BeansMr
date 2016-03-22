using UnityEngine;
using System.Collections;

/// <summary>
/// 摄像机背景运动补偿
/// </summary>
public class BackgroundParallax : MonoBehaviour {
	/// <summary>
	/// 背景中需要进行运动补偿的元素
	/// </summary>
	public Transform[] backgrounds; 
	/// <summary>
	/// 关联缩放变量。决定摄像机的运动与背景运动的比例关系
	/// </summary>
	public float parallaxScale;	
	/// <summary>
	/// 关联缩放衰减因子变量。决定条个层中远近不同时运动的衰减系数
	/// </summary>
	public float parallaxReductionFactor;
	/// <summary>
	/// 运动补偿平滑度, 即使用多少秒来完成移动
	/// </summary>
	public float smoothing;

	// 当前主相机的位置
	private Transform cam;
	// 上一帧中的摄像机位置
	private Vector3 prevCamPos;

	void Awake() {
		cam = Camera.main.transform;
	}

	void Start () {
		// 记录摄像机位置为当前的位置
		prevCamPos = cam.position;
	}

	void Update () {
		// 计算出当前帧中x轴上需要补偿运动补偿量
		float parallax = (prevCamPos.x - cam.position.x) * parallaxScale;
		// 循环来处理每个背景元素
		for (int i = 0; i < backgrounds.Length; i++) {
			// 计算出目标位置。以元素的当前x轴上的位置加上视差移动的大小。
			// 计算时，运动补偿量*(i*关联缩放衰减因子+1)，是保证至少移动一个运动补偿量
			float bgTargetPosX = backgrounds[i].position.x + parallax * (i * parallaxReductionFactor + 1);
			// 生成新的向量
			Vector3 newTargetPos = new Vector3 (bgTargetPosX, backgrounds[i].position.y, backgrounds [i].position.z);
			// 平滑的移动元素到新的位置
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, newTargetPos, smoothing * Time.deltaTime);
		}
		// 重新记录摄像机的位置
		prevCamPos = cam.position;
	}

}
