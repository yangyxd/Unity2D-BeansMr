#if UNITY_STANDALONE // || UNITY_EDITOR
#else
#define USE_INPUT_EX  // 定义使用InputEx
#endif

using UnityEngine;
#if USE_INPUT_EX
using UnityEngine.UI;
#endif
using System.Collections;

/// <summary>
/// 扩展输入工具。 方便跨平台使用
/// </summary>
public class InputEx : MonoBehaviour {	

	#if USE_INPUT_EX
	private static bool isLeft = false;
	private static bool isRight = false;
	private static bool isUp = false;
	private static bool isDown = false;
	private static bool isJump = false;

	private static bool isFire1 = false;
	private static bool isFire2 = false;
	private static bool isFire3 = false;
	private static bool isFire4 = false;
	private static bool isFire5 = false;
	#endif

	void Awake () {
		#if USE_INPUT_EX
		#else
		gameObject.SetActive(false);
		#endif
	}

    void Start() {
        #if USE_INPUT_EX
        SetListener("btnLeft", "0");
        SetListener("btnRight", "1");
        SetListener("btnAttack", "2");
        SetListener("btnBomb", "3");
        SetListener("btnJump", "4");
        #endif
    }

    void SetListener(string name, string tag) { 
        Button btn = transform.root.Find(name).GetComponent<Button>();
        EventTriggerListener.Get(btn.gameObject).onDown = OnDown;
        EventTriggerListener.Get(btn.gameObject).onUp = OnUp;
    }

    void OnDown(GameObject obj) {
        SetValue(obj, true);
    }

    void OnUp(GameObject obj) {
        SetValue(obj, false);
    }

    void SetValue(GameObject obj, bool v) {
        switch (obj.name) {
            case "btnLeft": SetLeft(v); break;
            case "btnRight": SetRight(v); break;
            case "btnAttack": SetFire1(v); break;
            case "btnBomb": SetFire2(v); break;
            case "btnJump": SetJump(v); break;
        }
    }

    public void SetLeft(bool v) {
		#if USE_INPUT_EX
		isLeft = v;
        if (v) isRight = false;
        #endif
    }

	public void SetRight(bool v) {
        #if USE_INPUT_EX
        if (v) isLeft = false;
		isRight = v;
        Debug.Log("SetRight: " + v);
		#endif
	}

	public void SetUp(bool v) {
		#if USE_INPUT_EX
		isUp = v;
        if (v) isDown = false;
		#endif
	}

	public void SetDown(bool v) {
		#if USE_INPUT_EX
		isDown = v;
        if (v) isUp = false;
		#endif
	}

	public void SetJump(bool v) {
		#if USE_INPUT_EX
		isJump = v;
		#endif
	}

	public void SetFire1(bool v) {
		SetFire (1, v);
	}
	public void SetFire2(bool v) {
		SetFire (2, v);
	}
	public void SetFire3(bool v) {
		SetFire (3, v);
	}
	public void SetFire4(bool v) {
		SetFire (4, v);
	}
	public void SetFire5(bool v) {
		SetFire (5, v);
	}

	public void SetFire(int index, bool v) {
		#if USE_INPUT_EX
		switch (index) {
		case 1:
		isFire1 = v; break;
		case 2:
		isFire2 = v; break;
		case 3:
		isFire3 = v; break;
		case 4:
		isFire4 = v; break;
		case 5:
		isFire5 = v; break;
		}
		#endif
	}

	public static float GetAxis(string axisName) {
		#if USE_INPUT_EX
		switch (axisName.ToLower()) {
		case "horizontal":
		case "mouse x":
			if (isLeft) return -1;
			else if (isRight) return 1;
			else return 0;
		case "vertical":
		case "mouse y":
			if (isUp) return -1;
			else if (isDown) return 1;
			else return 0;
		case "jump":
		case "space":
		if (isJump) return 1; else return 0; 
		case "fire1":
		if (isFire1) return 1; else return 0;
		case "fire2":
		if (isFire2) return 1; else return 0;
		case "fire3":
		if (isFire3) return 1; else return 0;
		case "fire4":
		if (isFire4) return 1; else return 0;
		case "fire5":
		if (isFire5) return 1; else return 0;
		default:
		return Input.GetAxis (axisName);
		}
		#else
		return Input.GetAxis (axisName);
		#endif
	}

	public static bool GetButtonDown(string buttonName) {
		#if USE_INPUT_EX
		switch (buttonName.ToLower()) {
		case "jump":
                if (isJump) {
                    isJump = false;
                    return true;
                } else return false;
		case "space":
                if (isJump) {
                    isJump = false;
                    return true;
                } else return false;
		case "fire1":
                if (isFire1) {
                    isFire1 = false;
                    return true;
                } else return false;
		case "fire2":
                if (isFire2) {
                    isFire2 = false;
                    return true;
                } else return false;
		case "fire3":
                if (isFire3) { 
                    isFire3 = false;
                    return true;
                } else return false;
		case "fire4":
                if (isFire4) {
                    isFire4 = false;
                    return true;
                } else return false;
		case "fire5":
                if (isFire5) {
                    isFire5 = false;
                    return true;
                } else return false;
		default:
			return Input.GetButtonDown (buttonName);
		}
		#else
		return Input.GetButtonDown (buttonName);
		#endif
	}
}
