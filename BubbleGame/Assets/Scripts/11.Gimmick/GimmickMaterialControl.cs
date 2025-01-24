using System.Linq;
using UnityEngine;

namespace Gimmick
{
    public class GimmickMaterialControl : MonoBehaviour
    {
        public GameObject targetObject;
        public Material material;
        
        public void AddMaterial()
        {
            if (ReferenceEquals(targetObject, null)) return;
            if (targetObject.TryGetComponent(out MeshRenderer meshRenderer))
            {
                var mats = meshRenderer.sharedMaterials.ToList();
                mats.Add(material);
                meshRenderer.SetMaterials(mats);
            }
        }

        public void RemoveMaterial()
        {
            if (ReferenceEquals(targetObject, null)) return;
            if (targetObject.TryGetComponent(out MeshRenderer meshRenderer))
            {
                var mats = meshRenderer.sharedMaterials.ToList();
                mats.Remove(material);
                meshRenderer.SetMaterials(mats);
            }
        }
    }
}