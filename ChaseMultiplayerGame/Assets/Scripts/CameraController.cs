using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton
    public static CameraController Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    Transform Target = null;

    [SerializeField] Vector3 offset;

    public void InitializeCamera(Transform aTarget)
    {
        Target = aTarget;
    }

    void Update()
    {
        if (Target == null) return;

        //camera logic here

        transform.position = Target.position + offset;
        transform.LookAt(Target.position);

    }
}
