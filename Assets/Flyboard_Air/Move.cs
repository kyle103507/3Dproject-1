using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	public bool Run;
    public float Speed;
    public float RotateSpeed = 30;
    Animator PlayAnim;

	void Start ()
    {
        // 得到角色的動畫控制器
        PlayAnim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        // 播放角色動畫(New Bool 判斷是否要 Fly )
        PlayAnim.SetBool("New Bool", Run);
        // 如果有按下方向鍵或sd執行
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") == 1)
        {
            // 滑板飛起來
            Run = true;
            // 向前飛
            if (Input.GetAxisRaw("Vertical") != 0)
                transform.Translate(0, 0, Input.GetAxisRaw("Vertical") * Time.deltaTime * Speed);

            // 往右按會往右飛
            if (Input.GetAxisRaw("Horizontal") >= 1)
                transform.Rotate(new Vector3(0, RotateSpeed * Time.deltaTime, 0));
            // 往左按會往左飛
            else if (Input.GetAxisRaw("Horizontal") <= -1)
                transform.Rotate(new Vector3(0, -RotateSpeed * Time.deltaTime, 0));
        }
        else
        {
            // 滑板不要飛起來
            Run = false;
        }
	}
}
