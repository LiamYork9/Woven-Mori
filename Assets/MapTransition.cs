using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    enum Direction { Up, Down, Left, Right }
    public ZoneCheck zoneCheck;



    public PlayerController pController;

    private void Awake()
    {
        confiner = FindObjectsByType<CinemachineConfiner2D>(FindObjectsSortMode.None)[0];
    }

    public void Start()
    {
        pController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
         if(zoneCheck.activeZone == true)
        {
            confiner.BoundingShape2D = mapBoundry;
        }
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundry;
            UpdatePlayerPos(collison.gameObject);
            pController.SetPosition();
        }
    }

    public void Update()
    {
        if(zoneCheck.activeZone == true)
        {
            confiner.BoundingShape2D = mapBoundry;
        }
    }


    private void UpdatePlayerPos(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += pController.movePoint.position.y + 2;
                break;
            case Direction.Down:
                newPos.y -= 2;
                break;
            case Direction.Left:
                newPos.x += 2;
                break;
            case Direction.Right:
                newPos.x -= 2;
                break;

        }
        player.transform.position = newPos;
    }
}
