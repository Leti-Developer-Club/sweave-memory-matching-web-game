using UnityEngine;

public class PanelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisablePanel();
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
