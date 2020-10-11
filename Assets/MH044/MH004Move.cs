using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MH004Move : MonoBehaviour
{
    [SerializeField] float WalkSpeed = 3;
    [SerializeField] float RunSpeed = 6;
    [SerializeField] LayerMask GroundLayer;
    float ForwardDirection;
    float allowPlayerRotation = .1f;

    Camera cam;
    CharacterController Controller;
    MH004Anim anim;

    Vector3 DesiredMoveDirection;
    float DesiredRotationSpeed = .1f;

    private void Start()
    {
        cam = Camera.main;
        Controller = GetComponent<CharacterController>();
        anim = GetComponent<MH004Anim>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            anim.RotateBehavior(true);
        else if (Input.GetMouseButtonUp(0))
            anim.RotateBehavior(false);

        MoveBehavior();
    }

    void MoveBehavior()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");
        // 計算 InputX + InputZ 的合力
        ForwardDirection = new Vector2(InputX, InputZ).sqrMagnitude;

        anim.SetSpeed(ForwardDirection);

        if (ForwardDirection > allowPlayerRotation)
        {
            Vector3 camForward = cam.transform.forward;
            Vector3 camRight = cam.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            DesiredMoveDirection = camForward * InputZ + camRight * InputX;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(DesiredMoveDirection), DesiredRotationSpeed);

            float Speed = SpeedJudgment();

            if (Physics.Raycast(transform.position, Vector3.down, .1f, GroundLayer))
                DesiredMoveDirection.y = 0;
            else
                DesiredMoveDirection.y = -2;
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);

            Controller.Move(DesiredMoveDirection * Time.deltaTime * Speed);
        }
    }

    // 速度判斷
    float SpeedJudgment()
    {
        float m_Speed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            m_Speed = RunSpeed;
        else
            m_Speed = WalkSpeed;

        return m_Speed;
    }
}
