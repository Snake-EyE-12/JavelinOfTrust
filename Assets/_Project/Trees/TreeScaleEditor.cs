using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class TreeScaleEditor : MonoBehaviour
{
    [Button]
    public void ScaleAllTreesInChildren()
    {
        GetComponentsInChildren<TreeScale>().ToList().ForEach(x => x.Preset());
    }
}