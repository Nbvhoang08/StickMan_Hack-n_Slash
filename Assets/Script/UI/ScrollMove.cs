using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMove : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public float speed;
    [SerializeField] private Renderer bgRender;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bgRender.material.mainTextureOffset += new Vector2(speed * Time.deltaTime,0);
    }

}
