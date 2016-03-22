using UnityEngine;
using System.Collections;

/// <summary>
/// 空降包拾取
/// </summary>
public class PickupSpawner : MonoBehaviour {
	/// <summary>
	/// 存放预制可拾器的游戏对象，第一个是炸弹，第二个放医疗箱
	/// </summary>
	public GameObject[] pickups;
	/// <summary>
	/// 产生空降包的延时
	/// </summary>
	public float pickupDeliveryTime = 5f;
	/// <summary>
	/// 出现在场景世界中时可能的最左边位置
	/// </summary>
	public float dropRangeLeft;	
	/// <summary>
	/// 出现在场景世界中时可能的最右边位置
	/// </summary>
	public float dropRangeRight;

	/// <summary>
	/// 只有玩家的生命值低于此值时才有可能出现医疗箱
	/// </summary>
	public float highHealthThreshold = 75f;
	/// <summary>
	/// 玩家生命值低于此值时，将只出现医疗箱
	/// </summary>
	public float lowHealthThreshold = 25f;	

	// 引用玩家健康脚本
	private PlayerHealth playerHealth;	


	void Awake () {
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}


	void Start () {
		// 在协程中执行 DeliverPickup  
		StartCoroutine(DeliverPickup());
	}

	/// <summary>
	/// 控制空降包的出现
	/// </summary>
	public IEnumerator DeliverPickup() {
		// 等待指定的间隔时间
		yield return new WaitForSeconds(pickupDeliveryTime);

		// 在指定的范围内随机产生一个x轴坐标
		float dropPosX = Random.Range(dropRangeLeft, dropRangeRight);

		// 根据生成的x轴坐标，生成一个包裹的出现矢量
		Vector3 dropPos = new Vector3(dropPosX, 15f, 1f);

		// 如果玩家的生命值大于设定的出现医疗包的上限，则生成炸弹包
		if(playerHealth.HP >= highHealthThreshold)
			Instantiate(pickups[0], dropPos, Quaternion.identity);
		// 如果玩家的生命值小于设定的下限值时，则生成医疗包
		else if(playerHealth.HP <= lowHealthThreshold)
			Instantiate(pickups[1], dropPos, Quaternion.identity);
		// 否则， 随机生成医疗包或炸弹包
		else {
			int pickupIndex = Random.Range(0, pickups.Length);
			Instantiate(pickups[pickupIndex], dropPos, Quaternion.identity);
		}
	}
}
