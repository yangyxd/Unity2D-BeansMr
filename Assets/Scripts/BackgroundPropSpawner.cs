using UnityEngine;
using System.Collections;

/// <summary>
/// 背景层道具产生器
/// </summary>
public class BackgroundPropSpawner : MonoBehaviour {
    /// <summary>
    /// 要产生的道具刚体
    /// </summary>
    public Rigidbody2D backgroundProp;

    public float leftSpawnPosX;             // 实例化后，道具从左边的出发x轴坐标
    public float rightSpawnPosX;            // 实例化后，道具将要到达的右边的x轴坐标
    public float minSpawnPosY;              // 实例化时允许在y轴最小值
    public float maxSpawnPosY;              // 实例化时允许在y轴最大值
    public float minTimeBetweenSpawns;      // 产生道具的最小时间间隔
    public float maxTimeBetweenSpawns;      // 产生道具的最大时间间隔
    public float minSpeed;                  // 道具的最小速度
    public float maxSpeed;                  // 道具的最大速度

    // Use this for initialization
    void Start () {
        // 初始化随机种子为当前时间的毫秒数， 这样就可以有不同的结果
        Random.seed = System.DateTime.Today.Millisecond;
        // 在协程中执行Spawn方法
        if (backgroundProp != null)
            StartCoroutine("Spawn");
    }

    // 生成道具
    IEnumerator Spawn() {
        // 道具实例化之前，随机一个等待时间
        float waitTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

        // 等待指定的时间
        yield return new WaitForSeconds(waitTime);

        // 随机决定是向左或向右
        bool facingLeft = Random.Range(0, 2) == 0;

        // 如果向左，它应该被初始化在右边，反之则应初始化在左边
        float posX = facingLeft ? rightSpawnPosX : leftSpawnPosX;

        // 在由minSpawnPosY和maxSpawnPosY限定的范围内生成一个随机y轴坐标.
        float posY = Random.Range(minSpawnPosY, maxSpawnPosY);

        // 设置该道具的位置矢量
        Vector3 spawnPos = new Vector3(posX, posY, transform.position.z);

        // 在指定的位置实例化道具
        Rigidbody2D propInstance = Instantiate(backgroundProp, spawnPos, Quaternion.identity) as Rigidbody2D;

        // 精灵道具默认面向左边。如果当前需要面向右边，则需要进行水平翻转，使其向右
        if (!facingLeft) {
            // 将当前缩放的x轴乘以-1就可以达到目的
            Vector3 scale = propInstance.transform.localScale;
            scale.x *= -1;
            propInstance.transform.localScale = scale;
        }

        // 随机产生一个速度
        float speed = Random.Range(minSpeed, maxSpeed);

        // 如果是向左，速度应该是负值，向右则是正值。通过乘1或负1来改变速度的方向
        speed *= facingLeft ? -1f : 1f;

        // 将速度应用到道具上
        propInstance.velocity = new Vector2(speed, 0);

        // 开始下一个协程，继续产生道具
        StartCoroutine(Spawn());

        // 如果道具还存在
        while (propInstance != null) {
            // 向左
            if (facingLeft) {
                // 如果它超出了限定的区域，则删除这个道具
                if (propInstance.transform.position.x < leftSpawnPosX - 0.5f)
                    Destroy(propInstance.gameObject);
            } else {
                // 向右时一样，超出区域，删除道具
                if (propInstance.transform.position.x > rightSpawnPosX + 0.5f)
                    Destroy(propInstance.gameObject);
            }

            // 在下一次Update后继续执行
            yield return null;
        }
    }

}
