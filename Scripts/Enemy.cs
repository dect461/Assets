using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.4f;
    [SerializeField] private float attackInterval = 1.0f;
    [SerializeField] private int attackDamage = 1;
    public Animator animator;

    [SerializeField] private UnityEngine.SpriteRenderer boardSprite;

    private Game game;
    private int xBoard = -1;
    private int yBoard = -1;
    private bool isMoving;
    public bool isAttacking;
    private bool isDamageCivilians;
    private Coroutine attackRoutine;
    private Health currentTarget;
    private GameObject currentTargetObject;
    private Rigidbody2D rb;
    private Vector2 movementDirection = Vector2.left;

    public void SetBoardSprite(UnityEngine.SpriteRenderer sr) { boardSprite = sr; }

    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        if (boardSprite == null)
            Debug.LogError("Enemy: boardSprite (SpriteRenderer доски) не назначен");

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = true;
        }
    }

    public void Activate()
    {
        if (game == null)
            game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();

        transform.position = GetCellCenterFromPng(xBoard, yBoard);
        StartMoving();
    }

    public void SetXBoard(int x) => xBoard = x;
    public void SetYBoard(int y) => yBoard = y;
    public int GetXBoard() => xBoard;
    public int GetYBoard() => yBoard;

    void Update()
    {
        // Проверяем, если цель убежала из зоны атаки
        if (isAttacking && currentTargetObject != null)
        {
            // Проверяем расстояние до цели
            float distance = Vector2.Distance(transform.position, currentTargetObject.transform.position);
            if (distance > 1.2f) // Увеличиваем дистанцию проверки
            {
                Debug.Log("Цель сбежала из зоны атаки!");
                ResumeMovement();
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving && !isAttacking)
        {
            // Двигаемся влево с постоянной скоростью
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            animator.SetBool("IsWalk", true);
            animator.SetBool("IsAtack", false);
        }
        else if (!isAttacking)
        {
            // Останавливаемся если не двигаемся и не атакуем
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void StartMoving()
    {
        isMoving = true;
        isAttacking = false;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        animator.SetBool("IsWalk", true);
        animator.SetBool("IsAtack", false);
    }

    public void StartAttacking(Health target, GameObject targetObject)
    {
        isMoving = false;
        isAttacking = true;
        rb.linearVelocity = Vector2.zero; // Останавливаем движение

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsAtack", true);

        currentTarget = target;
        currentTargetObject = targetObject;

        if (target != null)
        {
            target.OnDeath += OnTargetDied;
        }

        // Запускаем корутину атаки
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
        attackRoutine = StartCoroutine(AttackRoutine(target, targetObject));
    }

    IEnumerator AttackRoutine(Health target, GameObject targetObject)
    {
        while (target != null && targetObject != null && isAttacking)
        {
            // Наносим урон
            if (target != null)
            {
                target.ApplyDamage(attackDamage);
            }

            yield return new WaitForSeconds(attackInterval);
        }

        // Если вышли из цикла атаки, возобновляем движение
        if (isAttacking)
        {
            ResumeMovement();
        }
    }

    private void OnTargetDied()
    {
        if (currentTargetObject.name == "white_king")
        {
            game.Lose();
        }

        ResumeMovement();
    }

    public void ResumeMovement()
    {
        isAttacking = false;

        if (currentTarget != null)
        {
            currentTarget.OnDeath -= OnTargetDied;
            currentTarget = null;
        }

        currentTargetObject = null;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        StartMoving();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking) return;

        // Обработка столкновения с фигурами (ChessMan)
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            currentTarget = health;
            currentTargetObject = collision.gameObject;
            StartAttacking(health, collision.gameObject);
            return;
        }

        // Обработка столкновения с мирными жителями (Civilians)
        if (collision.gameObject.tag == "Civilians")
        {
            isDamageCivilians = true;
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(150);
                Destroy(gameObject);
            }
            return;
        }

        // Обработка столкновения с другими врагами (для предотвращения наложения)
        Enemy otherEnemy = collision.gameObject.GetComponent<Enemy>();
        if (otherEnemy != null)
        {
            // Можно добавить логику обхода других врагов
            rb.linearVelocity = Vector2.zero;
            // Или временно изменить направление
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Если цель, которую мы атаковали, ушла из зоны триггера - прекращаем атаку
        if (currentTargetObject == collision.gameObject && isAttacking)
        {
            ResumeMovement();
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от событий при уничтожении
        // Подумать убрать эти строчки кода или нет
        if (currentTarget != null)
        {
            currentTarget.OnDeath -= OnTargetDied;
        }

        // Награждаем игрока за убийство врага
        Player pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (pl != null && !isDamageCivilians)
        {
            pl.AddCoins(10);
        }
    }

    Vector3 GetCellCenterFromPng(float cx, int cy)
    {
        if (boardSprite == null) return transform.position;

        var b = boardSprite.bounds;
        float cellX = b.size.x / 8f;
        float cellY = b.size.y / 8f;

        Vector3 origin = b.min;

        float wx = origin.x + (cx + 0.5f) * cellX;
        float wy = origin.y + (cy + 0.5f) * cellY;
        return new Vector3(wx, wy, 0f);
    }
}