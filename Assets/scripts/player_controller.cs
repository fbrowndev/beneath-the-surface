using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    // speeds
    public float movement_speed = 7.0f;
    public float jump_force = 14.0f;

    // state
    public bool isGrounded = true;

    // components
    private Rigidbody2D m_rigidbody;
    private BoxCollider2D m_collider;

    // children
    private GameObject foot_object;
        private BoxCollider2D foot_collider;
            private float foot_height = 0.1f;

    // other objects
    public Camera follower_camera;
    public bool camera_follow = true;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
        follower_camera = Camera.main;

        // generating "foot"
        foot_object = new GameObject("player_feet", typeof(BoxCollider2D));
        foot_object.transform.parent = transform;
        foot_object.transform.localPosition = new Vector2(0, 0);
        
        // generating foot collider
        foot_collider = foot_object.GetComponent<BoxCollider2D>();
        foot_collider.offset = m_collider.offset - new Vector2(0, m_collider.size.y/2 + foot_height/2 + 0.1f);
        foot_collider.size = new Vector2(0.7f, foot_height);
        foot_collider.isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        grounded_check();
        move();
        camera_fixing();
    }

    void grounded_check() 
    {
        List<Collider2D> colliders = new List<Collider2D>();
        foot_collider.OverlapCollider((new ContactFilter2D()).NoFilter(), colliders);
        bool caught = false;
        try
        {
            Collider2D c = colliders[0];
        }
        catch
        {
            caught = true;
            isGrounded = false;
        }
        finally
        {
            if (!caught)
            isGrounded = true;
        }
    }

    void move()
    {
        float h_move = Input.GetAxis("Horizontal");
        transform.Translate(new Vector2(h_move, 0) * movement_speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            m_rigidbody.AddForce(new Vector2(0, jump_force), ForceMode2D.Impulse);
        }
    }

    void camera_fixing()
    {
        follower_camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
