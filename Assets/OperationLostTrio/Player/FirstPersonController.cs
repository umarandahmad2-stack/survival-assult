using UnityEngine;
namespace OperationLostTrio {
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour {
        public Transform cameraRoot;
        public float walkSpeed = 4.5f, sprintSpeed = 7f, jumpHeight = 1.2f, gravity = -20f, mouseSensitivity = 2f, maxLookAngle = 85f;
        CharacterController controller; float verticalVelocity, pitch;
        void Awake() { controller = GetComponent<CharacterController>(); Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButtonDown(0)) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
            if (cameraRoot != null && Cursor.lockState == CursorLockMode.Locked) {
                transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
                pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * mouseSensitivity, -maxLookAngle, maxLookAngle);
                cameraRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            }
            Vector3 move = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized;
            controller.Move(move * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed) * Time.deltaTime);
            if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
            if (controller.isGrounded && Input.GetButtonDown("Jump")) verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            verticalVelocity += gravity * Time.deltaTime; controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }
    }
}