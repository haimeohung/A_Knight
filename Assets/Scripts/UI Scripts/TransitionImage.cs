using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionImage : MonoBehaviour
{
    public float timeTransition = 1f;
    public bool isOpenTransition = true;

    private Image image;
    private float deltaChanged;
    private int changing = 0;
    private float counter;
    // Start is called before the first frame update
    void Start()
    {
        deltaChanged = timeTransition == 0 ? float.PositiveInfinity : 1f / timeTransition;
        if (isOpenTransition)
        {
            gameObject.SetActive(true);
            changing = -1;
        }
        else
        {
            gameObject.SetActive(false);
            changing = 0;
        }
        image = gameObject.GetComponent<Image>();
        if (image != null)
            image.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.fixedDeltaTime;
        if (image != null && changing != 0 && counter >= 0) 
            DoingTransiton();
    }

    private void DoingTransiton()
    {
        float change = deltaChanged * Time.fixedDeltaTime * changing;
        change += image.color.a;
        if (change < 0f)
        {
            change = 0f;
            changing = 0;
            gameObject.SetActive(false);
        }
        if (change>=1f)
        {
            change = 1f;
            changing = 0;
            gameObject.SetActive(true);
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, change);
    }

    public void CloseTransitionTrigger(float delayTrigger = 0f)
    {
        changing = 1;
        counter = -delayTrigger;
        gameObject.SetActive(true);
    }

    public void OpenTransitionTrigger(float delayTrigger = 0f)
    {
        changing = -1;
        counter = -delayTrigger;
        gameObject.SetActive(true);
    }
}
