using Unity.Cinemachine;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    private CinemachineCamera cinemachine;

    [SerializeField] private float targetLensSize;

    private void Awake()
    {
        cinemachine = GetComponentInChildren<CinemachineCamera>(true);
        //EnableCamera(false);
    }

    private void Update()
    {
        UpdateLensSizeIfNeeded();
    }

    private void UpdateLensSizeIfNeeded()
    {
        float currentLensSize = cinemachine.Lens.OrthographicSize;

        if (Mathf.Abs(currentLensSize - targetLensSize) > .05f)
        {
            float newLensSize = Mathf.Lerp(currentLensSize, targetLensSize, .5f * Time.deltaTime);
            cinemachine.Lens.OrthographicSize = newLensSize;
        }
        else
            cinemachine.Lens.OrthographicSize = targetLensSize;
    }

    public void EnableCamera(bool enable)
    {
        cinemachine.gameObject.SetActive(enable);
    }

    public void SetNewTarget(Transform newTarget)
    {
        cinemachine.Follow = newTarget;
    }
}
