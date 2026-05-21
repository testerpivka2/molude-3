using UnityEngine;

public class SamuraiEnemy : MonoBehaviour
{

    public float speed = 3f;
    public float attackRange = 1.5f;
    public Transform player;

    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    private IEnemyState currentState;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        ChangeState(new SamuraiIdleState(this));
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }
}
