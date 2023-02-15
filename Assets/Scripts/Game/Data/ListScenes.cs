using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ListScenes", menuName = "DataObjects/ListScenes", order = 0)]
    public class ListScenes : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private List<DescriptionScene> _listScenes;

        public List<DescriptionScene> ListAllScenes => _listScenes;
    }
}