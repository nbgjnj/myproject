using UnityEngine;

public class ObjectMovementWithMouseCameraControl : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 移动速度
    public float rotationSpeed = 3.0f; // 视角旋转速度
    public float maxFollowDistance = 10.0f; // 最大跟随距离
    public float minFollowDistance = 5.0f; // 最小跟随距离
    public float smoothSpeed = 0.125f; // 相机移动的平滑速度

    private Camera mainCamera;
    private Vector3 targetPosition;
    private Vector3 targetRotation;

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position;
        targetRotation = transform.eulerAngles;
    }

    void Update()
    {
        // 获取水平和垂直输入（方向键）
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 计算移动方向
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput) * moveSpeed * Time.deltaTime;

        // 移动物体
        transform.Translate(movement);

        // 更新目标位置
        if (movement != Vector3.zero)
        {
            targetPosition = transform.position;
        }

        // 视角旋转
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        targetRotation.y += mouseX;
        targetRotation.x -= mouseY;
        targetRotation.x = Mathf.Clamp(targetRotation.x, -90f, 90f);
        mainCamera.transform.eulerAngles = targetRotation;

        // 更新相机位置
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // 计算相机目标位置（保持一定距离）
        float distanceToTarget = Vector3.Distance(mainCamera.transform.position, targetPosition);
        if (distanceToTarget > maxFollowDistance)
        {
            Vector3 desiredPosition = targetPosition - mainCamera.transform.forward * maxFollowDistance;
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;
        }
        else if (distanceToTarget < minFollowDistance)
        {
            // 不做任何操作，保持在最小跟随距离
        }
        else
        {
            // 相机与物体之间的距离在最大和最小跟随距离之间，不需要调整相机位置
        }
    }
}