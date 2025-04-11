using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;  // Assign the player in the Inspector
    public NavMeshAgent agent;
    public float chaseRange = 10f; // Distance at which enemy starts chasing
    public float chaseSpeed = 4f;  // Speed when chasing the player
    public float normalSpeed = 2f; // Speed when returning to the original position
    public GameObject gameOverPanel; // Assign Game Over UI panel in Inspector

    private Vector3 startPosition; // Enemy's starting position
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position; // Store initial position
        gameOverPanel.SetActive(false); // Hide Game Over panel initially
        animator = GetComponent<Animator>();

        agent.speed = normalSpeed; // Set default speed
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= chaseRange)
        {
            // Start chasing the player
            agent.SetDestination(player.position);
            agent.speed = chaseSpeed; // Increase speed when chasing
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Return to original position if player is out of range
            agent.SetDestination(startPosition);
            agent.speed = normalSpeed; // Reduce speed when returning
        }

        // Stop walking animation if enemy is not moving
        if (agent.velocity.magnitude < 0.1f)
        {
            animator.SetBool("isWalking", false);
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            // Game Over when enemy touches the player
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }
}
