using UnityEngine;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour
{
    [SerializeField]
    private StateMachine stateMachine;

    private Image blackImage;

    void Start()
    {
        blackImage = GetComponent<Image>();
    }

    public void Update()
    {
        blackImage.color = new Color32(0, 0, 0, (byte)stateMachine.FadeAmount);
    }
}
