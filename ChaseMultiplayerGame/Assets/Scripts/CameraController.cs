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
        transform.position = Target.position + (offset + Target.forward);
    }

    void Update()
    {
        if (Target == null) return;

        //camera logic here
        //float mouseX = Input.GetAxis("Mouse X");
        //if (mouseX > 0)
        //{
        //    transform.RotateAround(Target.transform.position + offset, Vector3.up, 10);
        //    //  transform.rot
        //}
        //else if (mouseX < 0)
        //{
        //    transform.RotateAround(Target.transform.position + offset, Vector3.up, -10);
        //}


        transform.position = Target.position + (offset + Target.forward);
        transform.LookAt(Target.position);


       

    }
}
