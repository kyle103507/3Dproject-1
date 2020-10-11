using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MH004Anim : MonoBehaviour
{
    [Header("撞暈時間")] [SerializeField] float StunnedTime = 1;
    [Header("旋轉碰撞半徑")] [SerializeField] float RotateRadius = 1.5f;
    [Header("旋轉碰撞圖層")] [SerializeField] LayerMask RotateLayer;
    [Header("旋轉的力道")] [SerializeField] float RotateForce = 500;
    Animator Anim;

    private void Start()
    {
        // Anim 得到動畫元件
        Anim = GetComponent<Animator>();
    }

    public void SetSpeed(float Speed)
    {
        // 如果沒有按下Shift鍵速度為走路速度
        Speed = Mathf.Clamp(Speed, 0, .5f);

        // 如果按下Shift鍵 速度改為 跑步速度
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            Speed += .5f;

        // 執行跑步或走路動畫
        Anim.SetFloat("Speed", Speed);
    }

    // 撞暈行為
    public void StunnedBehavior()
    {
        // 開啟暈眩動畫
        Anim.SetTrigger("Stun");
    }

    // 暈眩事件
    public void StunnedEvent()
    {
        StartCoroutine(Recoverydelay(StunnedTime));
    }

    IEnumerator Recoverydelay(float time)
    {
        // 得到 HM004Move腳本
        MH004Move MoveElement = GetComponent<MH004Move>();
        // 關閉 HM004Move腳本
        MoveElement.enabled = false;
        // 關閉 HM004Anim腳本
        enabled = false;

        // 等待 time秒 後執行下面
        yield return new WaitForSeconds(time);

        // 開啟 HM004Move腳本
        MoveElement.enabled = true;
        // 開啟 HM004Anim腳本
        enabled = true;
    }

    // 旋轉動畫
    public void RotateBehavior(bool IsRotate)
    {
        // 如果IsRotate是true 開啟旋轉  不是時候 關閉旋轉
        Anim.SetBool("Rotate", IsRotate);
    }
    // 旋轉功能
    public void RotateEvent()
    {
        // 得到角色附近是RotateLayer圖層的碰撞器
        Collider[] collision = Physics.OverlapSphere(transform.position, RotateRadius, RotateLayer);

        // 跑過每個附近 RotateLayer圖層的碰撞器
        foreach (Collider i in collision)
        {
            // 得到RotateLayer圖層的碰撞器 的鋼體
            Rigidbody rb = i.GetComponent<Rigidbody>();
            // 如果他有剛體就執行碰撞
            if(rb != null)
            {
                rb.AddExplosionForce(RotateForce, transform.position, RotateRadius);
            }
        }
    }

    // 畫線顯示自訂碰撞範圍黃色參考輔助線
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, RotateRadius);
    }
}
