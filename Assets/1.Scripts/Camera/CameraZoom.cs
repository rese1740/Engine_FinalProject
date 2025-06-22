using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public Transform target;         // ���� ��� (�÷��̾�)
    public float zoomInSize = 3f;    // ���� �� ī�޶� ������ (Orthographic)
    public float zoomDuration = 1f;  // �� �� �ð�

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

            // ī�޶� ��ġ�� �÷��̾� �߽����� �̵�
            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                new Vector3(target.position.x, target.position.y, cam.transform.position.z),
                elapsed / zoomDuration
            );

            yield return null;
        }
    }
}
