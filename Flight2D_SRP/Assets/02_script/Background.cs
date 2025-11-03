using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Scroll[] _ground = new Scroll[2];
    private Scroll[] _cloud1 = new Scroll[2];
    private Scroll[] _cloud2 = new Scroll[2];

    [SerializeField] private GameObject _prefabGround;
    [SerializeField] private GameObject _prefabCloud1;
    [SerializeField] private GameObject _prefabCloud2;

    [SerializeField] private float _speedGround = 0F;
    [SerializeField] private float _speedCloud1 = 0F;
    [SerializeField] private float _speedCloud2 = 0F;

    // Start is called before the first frame update
    void Start()
    {
        Transform t = transform;
        for(int i=0; i<2; ++i)
        {
            var ground = Instantiate(_prefabGround, t);
            var cloud1 = Instantiate(_prefabCloud1, t);
            var cloud2 = Instantiate(_prefabCloud2, t);

            var bsGround = ground.GetComponent<BgScale>();
            var bsCloud1 = cloud1.GetComponent<BgScale>();
            var bsCloud2 = cloud2.GetComponent<BgScale>();

            bsGround.Init();
            bsCloud1.Init();
            bsCloud2.Init();

            _ground[i] = ground.GetComponent<Scroll>();
            _cloud1[i] = cloud1.GetComponent<Scroll>();
            _cloud2[i] = cloud2.GetComponent<Scroll>();

            _ground[i].Init(i * bsGround.Height);
            _cloud1[i].Init(i * bsGround.Height);
            _cloud2[i].Init(i * bsGround.Height);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = -Time.deltaTime;
        Scroll(_ground, dt * _speedGround);
        Scroll(_cloud1, dt * _speedCloud1);
        Scroll(_cloud2, dt * _speedCloud2);
    }

    void Scroll(Scroll[] scroll, float delta)
    {
        foreach (var s in scroll)
            s.Move(delta);
    }
}
