using UnityEngine;

namespace Sample_Test
{
    //������ �̵��� �����ϴ� Ŭ����
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //����
        public Rigidbody volleyball;            //�豸�� ��ü
        public GroundCheck groundCheck;         //�׶��� üũ Ʈ����
        private Transform cameraTransform;

        //������ġ��
        [SerializeField] private float defaultMoveForce = 5f;   //�̵� �� �⺻��
        [SerializeField] private float runForceFactor = 2f;     //�޸��� �μ�(���)
        [SerializeField] private float maxSpeed = 10f;          //�ִ� �ӵ�
        private float moveForce;                                //������ ���������� �������� �� (move)

        [SerializeField] private float jumpForce = 5f;          //���� ��
        private bool isJump = false;

        private Vector3 moveInput;      //�̵� �Է�

        #endregion

        #region Unity Event Method
        private void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            transform.position = volleyball.transform.position;
            RotateWithAspect();     //ī�޶� �ٶ󺸰� �ִ� ��������
            HandleInput();          //WASD �Է¹ޱ�

            //�޸��� - ���߿��� ���ڱ� �������� ����ϴϱ� �������� �޸� �� �ְ�
            if (Input.GetKey(KeyCode.LeftShift) && groundCheck.IsGrounded)    //������ Shift �Է��� ��
            {
                Run();
            }
            else
            {
                //�̵��ӵ� ����ȭ
                moveForce = defaultMoveForce;
            }


        }
        private void FixedUpdate()
        {
            ClampLinearVelocity();
            Move();
            if (isJump)
            {
                Jump();
            }
            
        }
        #endregion

        #region Custom Method
        //��ǲ �޾ƿ���
        private void HandleInput()
        {
            //WASD �Է�
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            moveInput = new Vector3(horizontal, 0f, vertical).normalized;

            //���� ��ǲ
            if (Input.GetKeyDown(KeyCode.Space) && groundCheck.IsGrounded)  //������ Space�� �Է��� ��
            {
                isJump = true;
                //Debug.Log(isJump);
            }
        }

        //�޾ƿ� ��ǲ ���� ���� ���� ������
        private void Move()
        {
            if (volleyball.linearVelocity.z > maxSpeed || volleyball.linearVelocity.x > maxSpeed)
                return;

            //���� �������� ���� ��ȯ
            Vector3 localMove = transform.TransformDirection(moveInput);
            volleyball.AddForce(localMove * moveForce, ForceMode.Force);
        }
        
        //ī�޶� �ٶ󺸴� �������� ȸ�� (y�� ����)
        private void RotateWithAspect()
        {
            Vector3 dir = transform.position - cameraTransform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        private void Run()
        {
            //�������� �� ����
            moveForce = runForceFactor * defaultMoveForce;
            //���־� ����Ʈ (����� etc...)
            //...
        }

        //�ִ�ӵ� ����
        void ClampLinearVelocity()
        {
            Vector3 velocity = volleyball.linearVelocity;

            // XZ ��� �ӵ� ����
            Vector3 flatVelocity = new Vector3(velocity.x, 0f, velocity.z);

            // ��� �ӵ��� max�� �Ѿ����� �ڸ���
            if (flatVelocity.magnitude > maxSpeed)
            {
                flatVelocity = flatVelocity.normalized * maxSpeed;
                volleyball.linearVelocity = new Vector3(flatVelocity.x, velocity.y, flatVelocity.z);
            }

            //Debug.Log(flatVelocity.magnitude);
        }

        private void Jump()
        {
            volleyball.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //���־� ����Ʈ
            //...

            //isJump �ʱ�ȭ
            isJump = false;
            //Debug.Log(isJump);
        }
        #endregion
    }

}

/*
25-06-20 : �ִ�ӵ� ����
�̷л� �����ϸ� ������ �ö� �� ������ ��� ���� �ӵ��� �ȿö󰡴� �� ���� (�ƴϸ� �ſ� ����� �ö󰡰ų�)
�׷��� �ӵ������� ���� �ʿ���� ����??
*/