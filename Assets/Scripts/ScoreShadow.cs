using UnityEngine;
using System.Collections;

/// <summary>
/// 评分阴影
/// </summary>
public class ScoreShadow : MonoBehaviour {

	public GameObject guiCopy;	// 复制一个分数对象

	void Awake() {
		// 将阴影层的位置设置在目标的下方一点点。                            
		Vector3 behindPos = transform.position;
		behindPos = new Vector3(guiCopy.transform.position.x, guiCopy.transform.position.y-0.005f, guiCopy.transform.position.z-1);
		transform.position = behindPos;
	}


	void Update () {
		// 同步更新文字内容  
		GetComponent<GUIText>().text = guiCopy.GetComponent<GUIText>().text;
	}
}
