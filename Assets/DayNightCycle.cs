using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Sta
    // rt is called before the first frame update

    public Light sun;
    public float speedOfDay; //time of 0.01 -> 3min, 20 sec day = 200 sec. 0.06666666 = 5 min day
    public bool running;
    public float t;
    public Color morningColor;
    public Color noonColor;
    public Color nightColor;
    private bool beforeNoon;
    public float startAngle = -30;
    public float endAngle = 210;
    public AnimationCurve intensityOverDay;
    public GameObject environment;
    public GameObject sunUI;
    
    void Start()
    {
        StartDay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDay()
    {
        Debug.Log("startTime = " + Time.time);
        StartCoroutine(PlayDay());
       
    }

    IEnumerator PlayDay()
    {
        running = true;
        Color sunColor = new Color();
        float sunAngle;
        t = 0;
        float angleT = 0;
        float rotUI = 0;
        float sunUIRot = 0;
        while (running)
        {
            if (beforeNoon)
                sunColor = SmoothStepColor(morningColor, noonColor, t/2);
            else
                sunColor = SmoothStepColor(noonColor, nightColor, t/2);
            sunAngle = Mathf.Lerp(startAngle, endAngle, angleT/2);
            sun.intensity = intensityOverDay.Evaluate(angleT/2);
            t += Time.deltaTime * speedOfDay;
            angleT += Time.deltaTime * speedOfDay;
            rotUI += Time.deltaTime * speedOfDay;
            sunUIRot = Mathf.Lerp(40, -216, rotUI/2);
            sunUI.transform.localEulerAngles = new Vector3(0, 0, sunUIRot);
            sun.color = sunColor;
            sun.shadowStrength = (intensityOverDay.Evaluate(angleT / 2) - 1f) * 0.9f + 0.1f;
            sun.transform.localEulerAngles = new Vector3(50, sunAngle, 0);
            if (2 - t < 0.0001f)
            {
                running = false;    

            }
            if (2-t > 1)
            {
                beforeNoon = true;
                
            } else
            {
                beforeNoon = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    Color SmoothStepColor(Color from, Color to, float t)
    {
        float r = Mathf.SmoothStep(from.r, to.r, t);
        float g = Mathf.SmoothStep(from.g, to.g, t);
        float b = Mathf.SmoothStep(from.b, to.b, t);
        float a = 1;
        Color c = new Color(r, g, b, a);
        return c;
    }
}
