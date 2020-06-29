using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text description;
    [SerializeField]
    private Image image;

    public static PopUpController Instance;
    
    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string titleText, string descriptionText, Sprite sprite)
    {
        title.text = titleText;
        description.text = descriptionText;
        image.sprite = sprite;
        
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
