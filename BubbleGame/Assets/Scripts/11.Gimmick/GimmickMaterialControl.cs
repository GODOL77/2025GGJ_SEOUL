using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gimmick
{
    public class GimmickMaterialControl : MonoBehaviour
    {
        public GameObject[] targetObjects;
        public Material material;
        
        public void AddMaterial()
        {
            if (ReferenceEquals(targetObjects, null)) return;
            foreach (var obj in targetObjects)
            {
                if (obj.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    var mats = meshRenderer.sharedMaterials.ToList();
                    mats.Add(material);
                    meshRenderer.SetMaterials(mats);
                }
            }
        }

        public void RemoveMaterial()
        {
            if (ReferenceEquals(targetObjects, null)) return;
            foreach (var obj in targetObjects)
            {
                if (obj.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    var mats = meshRenderer.sharedMaterials.ToList();
                    mats.Remove(material);
                    meshRenderer.SetMaterials(mats);
                }
            }
        }
    }
}