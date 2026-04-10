using TenMaker.Core.Visualizer;
using UnityEngine;
using UnityEngine.Serialization;

namespace TenMaker.RankMode
{
    public class Debugger : MonoBehaviour
    {
        [FormerlySerializedAs("regionVisualizer")] [SerializeField] private RegionView regionView;


        [ContextMenu("Toggle Region Visualizer")]
        public void ToggleRegionVisualizer()
        {
            if (regionView.IsEnabled)
            {
                regionView.Disable();
            }
            else
            {
                regionView.Enable();
            }
        }
    }
}