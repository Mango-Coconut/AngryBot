using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class Movement : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterController controller;
    new Transform transform;
    Animator animator;
    new Camera camera;
    Plane plane;
    Ray ray;
    Vector3 hitpoint;
    PhotonView pv;
    CinemachineVirtualCamera virtualCamera;
    public float moveSpeed = 10f;

    Vector3 receivePos;
    Quaternion receiveRot;
    public float damping = 10;

    float gravity = -30f;       // 취향에 따라 -20~-40
    float verticalVelocity = 0; // 프레임 간 유지
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }

        plane = new Plane(transform.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            Move();
            Turn();
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position, receivePos, Time.deltaTime * damping
                );
            transform.rotation = Quaternion.Slerp(
                transform.rotation, receiveRot, Time.deltaTime * damping
            );
        }
    }

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");
    void Move()
    {
        // 카메라 기준 방향
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // 입력 기반 평면 이동 방향
        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);

        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir.Normalize();
        }

        // 지면 체크 & 중력 처리
        if (controller.isGrounded)
        {
            // 땅에 있을 때는 살짝 아래로 눌러서 "착 달라붙게"
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
        }
        else
        {
            // 공중이면 중력 누적
            verticalVelocity += gravity * Time.deltaTime;
        }

        // 최종 속도 (수평 + 수직)
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        // 실제 이동
        controller.Move(velocity * Time.deltaTime);

        // 애니메이션 파라미터 (수평 이동 기준)
        float forward = 0f;
        float strafe = 0f;

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            // 카메라 기준 moveDir을 그대로 쓰면 어색할 수 있으니
            // 현재 캐릭터 forward/right 기준 투영
            forward = Vector3.Dot(moveDir, transform.forward);
            strafe = Vector3.Dot(moveDir, transform.right);
        }

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    void Turn()
    {
        // 마우스 위치 → 레이 쏘기
        ray = camera.ScreenPointToRay(Input.mousePosition);

        float enter;
        if (!plane.Raycast(ray, out enter))
        {
            return;
        }

        // 레이가 Plane과 만나는 지점
        hitpoint = ray.GetPoint(enter);

        // 캐릭터 위치 → 히트 포인트 방향
        Vector3 lookDir = hitpoint - transform.position;

        // 상하 기울기 제거 (수평 방향만 사용)
        lookDir.y = 0f;

        // 너무 가까우면 LookRotation이 이상해질 수 있으니 체크
        if (lookDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        // 수평만 보고 회전
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //자신의 로컬 캐릭터인 경우 자신의 데이터를 다른 네트워크 유저에게 송신
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
