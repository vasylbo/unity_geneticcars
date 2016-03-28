using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarBuilder : MonoBehaviour {
    public delegate void CarUpdate(GameObject pCar);
    public static CarUpdate OnCarUpdated;

    void Start() {
        // register to take 
        OnCarUpdated += buildACar;
    }

    /// <summary>
    /// Builds a cars body using its chromosome
    /// </summary>
    /// <param name="pCar"> Cars game object </param>
    public void buildACar(GameObject pCar) {
        var carTracker = pCar.GetComponent<CarTracker>();
        List<Gene> genes = carTracker.carChromosome.genes;

        int vertices = 6; // num of body vertices

        GameObject[] children = new GameObject[vertices];
        float[] angles = new float[vertices];

        var lastWeight = genes[(vertices - 1) * 2].value;
        float lastAngle = 0;
        float angle = 0;
        for (var i = 0; i < vertices; i++) {
            var weight = genes[i * 2].value;
            if (i == vertices - 1) {
                angle = 360;
            } else {
                angle += genes[i * 2 + 1].value;
            }

            angles[i] = angle;

            var mesh = createMesh(
                lastAngle, angle, 
                lastWeight, weight);
            var child = pCar.transform.GetChild(i).gameObject;
            var meshFilter = child.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var renderer = child.GetComponent<MeshRenderer>();
            renderer.material.color =
                new Color(Random.value, Random.value, Random.value);

            PolygonCollider2D collider = child.GetComponent<PolygonCollider2D>();
            collider.points = createPhysMesh(mesh);

            children[i] = child;
            
            lastWeight = weight;
            lastAngle = angle;
        }

        connect(children[children.Length - 1], children[0]);
        for (int j = 0; j < children.Length - 1; j++) {
            connect(children[j], children[j + 1]);
        }

        int offset = vertices * 2;
        for (var k = 0; k < 2; k++) {
            int i = offset + k * 2;
            int vI = (int) genes[i].value;

            connectWheel(
                pCar.transform.GetChild(vertices + k).gameObject, 
                children[vI],
                angles[vI], // angle of car part
                genes[vI * 2].value, // radius of car part
                genes[i + 1].value); // radius of car wheel
        }
    }

    /// <summary>
    /// Connect car parts
    /// </summary>
    /// <param name="pFirst"></param>
    /// <param name="pSecond"></param>
    void connect(GameObject pFirst, GameObject pSecond) {
        FixedJoint2D joint = pFirst.GetComponent<FixedJoint2D>();
        joint.connectedBody = pSecond.GetComponent<Rigidbody2D>();
        joint.connectedAnchor = new Vector2(0, 0);
    }

    /// <summary>
    /// Connects a wheel to a car part 
    /// </summary>
    /// <param name="pWheel"></param>
    /// <param name="pPart"></param>
    /// <param name="pAngle"></param>
    /// <param name="pRadius"></param>
    /// <returns></returns>
    GameObject connectWheel(
            GameObject pWheel, GameObject pPart, 
            float pAngle, float pRadius, float pWheelRadius) {
        Vector3 pos = pPart.transform.position;
        float angle = pPart.transform.eulerAngles.z * Mathf.Deg2Rad;

        // calculate position relating cars center
        float x = Mathf.Cos(Mathf.Deg2Rad * pAngle) * pRadius;
        float y = Mathf.Sin(Mathf.Deg2Rad * pAngle) * pRadius;

        // set propper position
        pWheel.transform.position = new Vector3(
            x * Mathf.Cos(angle) - Mathf.Sin(angle) * y + pos.x, 
            x * Mathf.Sin(angle) + Mathf.Cos(angle) * y + pos.y, 
            pos.z);

        // set wheel size through scale
        pWheel.transform.localScale = new Vector3(pWheelRadius, pWheelRadius, 1f);

        // setap joing between car part and wheel
        HingeJoint2D joint = pWheel.GetComponent<HingeJoint2D>();
        joint.connectedBody = pPart.GetComponent<Rigidbody2D>();
        joint.connectedAnchor = new Vector2(x, y);

        SpriteRenderer renderer = pWheel.GetComponent<SpriteRenderer>();
        if (!renderer) {
            renderer = pWheel.AddComponent<SpriteRenderer>();
        }
        renderer.sprite = GraphicsFactory.createWheelSprite();

        return pWheel;
    }

    /// <summary>
    /// Creates a triangle mesh for car part
    /// </summary>
    /// <param name="pA1"> begin angle </param>
    /// <param name="pA2"> end angle </param>
    /// <param name="pR1"> begin radius </param>
    /// <param name="pR2"> end radius</param>
    /// <returns> A mesh of car part </returns>
    Mesh createMesh(
            float pA1, float pA2, 
            float pR1, float pR2) {
        Vector3[] vertices = new Vector3[3];

        float x;
        float y;

        // calculate vertices in positions 
        // relative to cars center

        // center point
        vertices[0] = new Vector3(0, 0, 0);

        // other points of triangle will lay on 
        // circles with radiuses pR1 and pR2 on angles pA1 and pA2 
        x = Mathf.Cos(Mathf.Deg2Rad * pA1) * pR1;
        y = Mathf.Sin(Mathf.Deg2Rad * pA1) * pR1;
        vertices[1] = new Vector3(x, y, 0);

        x = Mathf.Cos(Mathf.Deg2Rad * pA2) * pR2;
        y = Mathf.Sin(Mathf.Deg2Rad * pA2) * pR2;
        vertices[2] = new Vector3(x, y, 0);

        // create mesh
        Mesh mesh = new Mesh();

        // set mesh vertices
        mesh.vertices = vertices;

        // make one triangle
        // each int represents an index in vertices array
        // clockwize order means that mesh face will look on camera
        mesh.triangles = new int[]{0, 1, 2};

        return mesh;
    }

    /// <summary>
    /// Creates vertices needed for PolygonCollider2D
    /// </summary>
    /// <param name="pMesh"> Mesh of the car part </param>
    /// <returns></returns>
    Vector2[] createPhysMesh(Mesh pMesh) {
        Vector2[] physVertices = new Vector2[3];
        Vector3[] vertices = pMesh.vertices;
        Vector2 physVer;

        for (var i = 0; i < 3; i++) {
            // converting Vector3 points to Vector2 for 2D polygon collider
            physVer = new Vector2(vertices[i].x, vertices[i].y);
            physVertices[i] = physVer;
        }

        return physVertices;
    }


}
