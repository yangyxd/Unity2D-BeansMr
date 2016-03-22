using UnityEngine;
using System.Collections;

/// <summary>
/// 火箭弹
/// </summary>
public class Rocket : MonoBehaviour {
	/// <summary>
	/// 爆炸特效
	/// </summary>
	public GameObject explosion;

	// Use this for initialization
	void Start () {
		// 如果2秒后火箭还存在，则自动爆炸
		Destroy (gameObject, 2);
	}

	// 爆炸时执行
	void OnExplode() {
		// 生成一个被随机旋转过的爆炸特效
		Quaternion randomRotation = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f));
		Instantiate (explosion, transform.position, randomRotation);
	}

	// 碰撞检测
	void OnTriggerEnter2D(Collider2D col) {
		// 根据碰撞到的对象的tag进行判断
		if (col.tag == "Enemy") {
			// 击中了敌人

			// 对敌人造成伤害
			col.gameObject.GetComponent<Enemy>().Hurt();
			// 爆炸
			OnExplode();
			Destroy (gameObject);
		} else if (col.tag == "BombPickup") {
			// 击中了炸弹箱子等道具

			// 让道具爆炸，获得道具
			col.gameObject.GetComponent<Bomb>().Explode();
			// 释放被击中的道具对象和自己
			Destroy (col.transform.root.gameObject);
			Destroy (gameObject);
		} else if (col.tag != "Player") {
			// 击中了非玩家对象
			OnExplode();
			Destroy (gameObject);
		}
	}

}
