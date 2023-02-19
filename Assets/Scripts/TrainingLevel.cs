using Entities;
using MainMenu;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingLevel : MonoBehaviour
{
    public GameObject hand;
    public Target apple;
    public TextMeshProUGUI helpLabel;
    
    private Animator _handAnimator;

    private void Awake()
    {
        _handAnimator = hand.GetComponent<Animator>();
        
        GlobalEventManager.OnTargetGetDamage.AddListener(ShowTrainingHint);
        GlobalEventManager.OnTargetHitCart.AddListener(ShowTrainingEnding);

        helpLabel.text = "Перетяни камень в рогатку, и отпусти, когда прицелишься в яблоко";
    }

    private void Update()
    {
        _handAnimator.enabled = apple.health == apple.maxHealth;
        hand.SetActive(apple.health == apple.maxHealth);
    }

    private void ShowTrainingHint(Target target)
    {
        if (target != apple)
        {
            return;
        }
        helpLabel.text = apple.health switch
        {
            1 => "Остался последний раз! Выстрели, когда тележка будет под яблоком",
            > 1 => $"Попади в яблоко ещё {apple.health} раза",
            _ => "Попробуй снова! Перезагрузи уровень кнопкой сверху"
        };
    }
    
    private void ShowTrainingEnding(Target target)
    {
        if (target != apple)
        {
            return;
        }
        helpLabel.text = "Молодец, нажимай на стрелку вправо для продолжения";
    }
}
