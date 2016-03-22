using UnityEngine;
using System.Collections;

/// <summary>
/// 设置粒子排序层名称
/// </summary>
public class SetParticleSortingLayer : MonoBehaviour {
	/// <summary>
	/// 粒子排序层名称
	/// </summary>
	public string sortingLayerName;

	void Start () {
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
	}
}
