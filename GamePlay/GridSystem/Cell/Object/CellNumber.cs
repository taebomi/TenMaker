using TenMaker.Utility.SO;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public abstract class CellNumber : MonoBehaviour
    {
        public abstract void SetValue(int newValue);

        public void SetVisible(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}