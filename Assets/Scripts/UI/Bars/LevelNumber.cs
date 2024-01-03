using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelNumber : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI levelNumber;
    [SerializeField] TextMeshProUGUI levelNumber_2;
    void Start()
    {
        HandleLevelChanged(GameManager.Instance.getCurrentLevel());
        GameManager.CurrentLevelChangedEvent += HandleLevelChanged;
    }

    // Update is called once per frame
    void HandleLevelChanged(int lvNumber)
    {
        if (levelNumber != null && levelNumber_2 != null)
        {
            levelNumber.text = lvNumber.ToString();
            levelNumber_2.text = lvNumber.ToString();
        }
    }
}
