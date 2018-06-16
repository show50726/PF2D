using UnityEngine;

public class MouseLineDrawer : MonoBehaviour
{
    //LineRenderer  
    private LineRenderer lineRenderer;
    //定義一個Vector3,用來存儲鼠標點擊的位置  
    private Vector3 position;
    //用來索引端點  
    private int index = 0;
    //端點數  
    private int LengthOfLineRenderer = 0;

    void Start()
    {
        //添加LineRenderer組件  
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        //設置材質  
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //設置顏色  
        //lineRenderer.SetColors(Color.red, Color.yellow);
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        //設置寬度  
        //lineRenderer.SetWidth(0.02f, 0.02f);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

    }

    void Update()
    {
        //獲取LineRenderer組件  
        lineRenderer = GetComponent<LineRenderer>();
        //鼠標左擊  
        if (Input.GetMouseButton(0))
        {
            //將鼠標點擊的屏幕坐標轉換為世界坐標，然後存儲到position中  
            position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
            //端點數+1  
            LengthOfLineRenderer++;
            //設置線段的端點數  
            //lineRenderer.SetVertexCount(LengthOfLineRenderer);
            lineRenderer.positionCount = LengthOfLineRenderer;

        }
        //連續繪製線段  
        while (index < LengthOfLineRenderer)
        {
            //兩點確定一條直線，所以我們依次繪製點就可以形成線段了  
            lineRenderer.SetPosition(index, position);
            index++;
        }


    }

    void OnGUI()
    {
        GUILayout.Label("當前鼠標X軸位置：" + Input.mousePosition.x);
        GUILayout.Label("當前鼠標Y軸位置：" + Input.mousePosition.y);
    }


}
