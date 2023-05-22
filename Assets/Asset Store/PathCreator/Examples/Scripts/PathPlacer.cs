using PathCreation;
using UnityEngine;

namespace PathCreation.Examples
{

    [ExecuteInEditMode]
    public class PathPlacer : PathSceneTool
    {

        public GameObject[] prefab;
        public GameObject holder;
        public float spacing = 3;

        const float minSpacing = .1f;

        public void Generate()
        {
            if (pathCreator != null && prefab != null && holder != null)
            {
                DestroyObjects();

                VertexPath path = pathCreator.path;

                spacing = Mathf.Max(minSpacing, spacing);
                float dst = spacing;

                while (dst < path.length)
                {
                    Vector3 point = path.GetPointAtDistance(dst);
                    Quaternion rot = path.GetRotationAtDistance(dst);
                    Vector3 rotation = new Vector3(0, rot.eulerAngles.y, 0);

                    GameObject obj = Instantiate(prefab[0]);
                    obj.transform.position = point;
                    obj.transform.Rotate(rotation);
                    obj.transform.SetParent(holder.transform);

                    dst += spacing;
                }
            }
        }

        void DestroyObjects()
        {
            int numChildren = holder.transform.childCount;
            for (int i = numChildren - 1; i >= 0; i--)
            {
                DestroyImmediate(holder.transform.GetChild(i).gameObject, false);
            }
        }

        protected override void PathUpdated()
        {
            if (pathCreator != null)
            {
                // Generate ();
            }
        }
    }
}