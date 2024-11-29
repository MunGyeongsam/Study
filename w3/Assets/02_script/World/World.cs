using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    [SerializeField]
    private Texture[] _texture;

    Material[] _mat;
    ZoneManager zoneMgr;

    // Start is called before the first frame update
    void Start()
    {
        int nLayer = _texture.Length;
        _mat = new Material[nLayer];
        for(int i=0; i<nLayer; ++i)
            _mat[i] = W3Util.CreateMaterial(_texture[i], i);

        zoneMgr = new ZoneManager(nLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (zoneMgr == null)
            return;

        if (Input.GetKeyUp(KeyCode.E))
            WorldPosition.ENABLED = !WorldPosition.ENABLED;

        UI.AppendDbgInfo("@{ World");
        UI.AppendDbgInfo(string.Format("  - WorldPosition.ENABLED : {0}", WorldPosition.ENABLED.ToString()));

        zoneMgr.Update();
        zoneMgr.Draw(_mat);

        UI.AppendDbgInfo("@} World");
    }

    public bool Pick()
    {
        return zoneMgr.Pick();
    }
    public Vector3 PickedPoint()
    {
        return zoneMgr.Picked;
    }
}