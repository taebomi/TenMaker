using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Core.Visualizer
{
    public class RegionDisplay : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private SpriteRenderer outlineSr;


        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetRegion(Region region)
        {
            transform.position = region.Center;
            sr.size = region.Size;
            outlineSr.size = region.Size;
        }
    }
}