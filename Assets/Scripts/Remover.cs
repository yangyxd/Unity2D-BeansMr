using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏场景重置
/// </summary>
public class Remover : MonoBehaviour {

	/// <summary>
	/// 飞溅动画
	/// </summary>
	public GameObject splash;

	// 碰撞检测
	void OnTriggerEnter2D (Collider2D col) {
		bool isPlayer = col.gameObject.tag == "Player";

		// 实例化一个飞溅动画
		Instantiate(splash,  col.transform.position, transform.rotation);
		// 删除这个游戏对象
		Destroy(col.gameObject);

		// 如果是玩家掉在了河面上
		if (isPlayer) {
			
			// 停止主摄像机跟踪
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;

			// 如果玩家生命条已经显示， 则隐藏玩家生命条
			if(GameObject.FindGameObjectWithTag("HealthBar").activeSelf) {
				GameObject.FindGameObjectWithTag("HealthBar").SetActive(false);
			}

			// 开启协程，执行 ReloadGame 方法
			StartCoroutine("ReloadGame");
		}
	}

	// 重新加载游戏
	IEnumerator ReloadGame() {			
		// ... 短暂停留
		yield return new WaitForSeconds(2);
        // ... 然后重新加载关卡
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		// Application.LoadLevel(Application.loadedLevel);
	}
}
