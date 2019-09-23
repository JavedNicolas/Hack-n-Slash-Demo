using UnityEngine;
using System.Collections;

public class RadiusDisplayer : MonoBehaviour
{
    [SerializeField] [Range(10, 360)] int precision;
    [SerializeField] float _radius;
    [SerializeField] float lineWidth;
    [SerializeField] Color lineColor;
    [SerializeField] LineRenderer line;
    public float radius { get => _radius; } 

    private void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();
    }

    private void Update()
    {
        drawRadius();
    }

    void drawRadius()
    {
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.startColor = lineColor;
        line.endColor = lineColor;
        line.positionCount = precision + 1;

        var points = new Vector3[precision + 1];

        for(int i = 0; i < precision +1; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / precision);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.1f, Mathf.Cos(rad) * radius); 
        }

        line.SetPositions(points);
    }

    public void updateRadius(float radius)
    {
        this._radius = radius;
    }

}
