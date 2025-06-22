using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public Transform target;         // 따라갈 대상 (플레이어)
    public float zoomInSize = 3f;    // 줌인 시 카메라 사이즈 (Orthographic)
    public float zoomDuration = 1f;  // 줌 인 시간

    private Camera cam;
    private float originalSize;

    void Start()
    {
        cam = Camera.main;
        originalSize = cam.orthographicSize;
    }

    public void ZoomInOnPlayer()
    {
        StartCoroutine(ZoomCoroutine());
    }

    private IEnumerator ZoomCoroutine()
    {
        float elapsed = 0f;
        float startSize = cam.orthographicSize;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(startSize, zoomInSize, elapsed / zoomDuration);

            // 카메라 위치도 플레이어 중심으로 이동
            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                new Vector3(target.position.x, target.position.y, cam.transform.position.z),
                elapsed / zoomDuration
            );

            yield return null;
        }
    }
}
