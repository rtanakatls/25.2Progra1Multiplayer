using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen instance;

    public static LoadingScreen Instance
    {
        get
        {
            if(instance==null)
            {
                GameObject obj=Instantiate(Resources.Load<GameObject>("Prefabs/LoadingScreen"));
                instance=obj.GetComponent<LoadingScreen>();
            }
            return instance;
        }
    }


    public void Show()
    {

    }

    public void Hide()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
