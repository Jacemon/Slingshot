using Entities;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TrainingLevel : MonoBehaviour
{
    public GameObject hand;
    public Target apple;
    public TextMeshProUGUI helpLabel;
    
    private Animator _handAnimator;

    private void Awake()
    {
        _handAnimator = hand.GetComponent<Animator>();
    }

    private void Update()
    {
        _handAnimator.enabled = apple.health == apple.maxHealth;
        hand.SetActive(apple.health == apple.maxHealth);

        helpLabel.text = apple.health switch
        {
            0 => "Молодец, нажимай на стрелку вправо для продолжения",
            1 => "Осталось попасть ещё раз, выстрели, когда тележка будет под яблоком",
            _ => $"Попади в яблоко ещё {apple.health} раза"
        };
    }
}
