using UnityEngine;
using System.Collections;

/// <summary>
/// 怪物生成器脚本
/// </summary>
public class Spawner : MonoBehaviour {
    public float spawnTime = 5f;        // 生成怪物需要的时间
    public float spawnDelay = 3f;       // 生成怪物前需要等待的时间
    public GameObject[] enemies;        // 敌人预制体数组

    // Use this for initialization
    void Start () {
        // 每隔一段时间，反复调用 Spawn
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }
	
	void Spawn() {
		if (enemies.Length == 0)
			return;
		
        // 实例化一个敌人
        int enemyIndex = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyIndex], transform.position, transform.rotation);

        // 播放生成敌人的粒子特效
        foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>()) 
            p.Play();
    }
}
