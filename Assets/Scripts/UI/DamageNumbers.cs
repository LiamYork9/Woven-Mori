using UnityEngine;
using TMPro;
using UnityEditor.AI;

public class DamageNumbers : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text text;
    public int value;
    public float normalFontSize;
    public float critFontSize;
    public Color normalColor = Color.white;
    public Color critColor = Color.red;
    public bool isCrit;


    [Header("Animation")] 
    public AnimationCurve curve;
    public float duration;

    [Header("Curve Settings")]
    public Vector2 highPointOffset = new Vector2(-350,300);
    public Vector2 lowPointOffset = new Vector2(-100,-500);
    public float heightVarMax = 150;
    public float heightVarMin = 50;

    public Vector3 highPointOffsetByDirection = Vector3.zero;
    
    public Vector3 dropPointOffsetByDirection = Vector3.zero;


    public bool direction;


    [Header("Visualization")]
    public bool showGizmos;
    public int gizmoResolution;
    public Vector3 gizmoStartingPoint;
    public PopUpManager pupManager;
    public Coroutine moveCo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = value.ToString();

        if (!isCrit)
        {
            text.fontSize = normalFontSize;
            text.color = normalColor;
        }
        else
        {
            text.fontSize = critFontSize;
            text.color = critColor;
        }
    }

    void OnDrawGizmos()
    {
        if (!showGizmos)
        {
            return;
        }
        
        

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OrientCurveByDir()
    {
        highPointOffsetByDirection = highPointOffset;
        dropPointOffsetByDirection = lowPointOffset;

        if(direction)
        {
            return;
        }
    }
}
