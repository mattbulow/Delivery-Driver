using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{

    private SpriteRenderer sprite;
    [SerializeReference] private float flashHalfFreq = 2f;
    private Color color;
    private bool increase = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        color = new Color(1, 0, 1, 1);
    }

    private void Update()
    {
        if (color.a <= 0) { increase = true; }
        else if (color.a >= 1) { increase = false; }

        if (increase) { color.a += flashHalfFreq * Time.deltaTime; }
        else { color.a -= flashHalfFreq * Time.deltaTime; }

        sprite.color = color;
    }
}
