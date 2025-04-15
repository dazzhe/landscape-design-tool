using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Landscape2.Runtime.BuildingEditor
{
    public class BuildingCopyManager
    {
        private static readonly List<GameObject> copiedBuildings = new List<GameObject>();
        private static GameObject originalBuilding;
        private static GameObject copyingBuilding;
        private static bool isFollowingCursor = false;

        public static void AddCopiedBuilding(GameObject building)
        {
            copiedBuildings.Add(building);
        }

        public static List<GameObject> GetCopiedBuildings()
        {
            return copiedBuildings;
        }

        public static bool IsCopying()
        {
            return isFollowingCursor && copyingBuilding != null;
        }

        public static void StartCopying(GameObject building)
        {
            originalBuilding = building;
            if (building == null) return;

            copyingBuilding = GameObject.Instantiate(building);
            copyingBuilding.name = building.name;
            
            isFollowingCursor = true;
            
            MakeTransparent(copyingBuilding, 0.5f);
        }

        public static void PlaceCopy(Vector3 position)
        {
            if (!isFollowingCursor || copyingBuilding == null) return;

            copyingBuilding.transform.position = position;
            
            MakeTransparent(copyingBuilding, 1.0f);
            
            AddCopiedBuilding(copyingBuilding);
            
            isFollowingCursor = false;
            copyingBuilding = null;
        }

        public static void CancelCopying()
        {
            if (copyingBuilding != null)
            {
                GameObject.Destroy(copyingBuilding);
                copyingBuilding = null;
            }
            isFollowingCursor = false;
        }

        public static void UpdateCopyPosition(Vector3 position)
        {
            if (!isFollowingCursor || copyingBuilding == null) return;
            copyingBuilding.transform.position = position;
        }

        private static void MakeTransparent(GameObject obj, float alpha)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                foreach (Material material in materials)
                {
                    Color color = material.color;
                    color.a = alpha;
                    material.color = color;
                }
            }
        }
    }
}
