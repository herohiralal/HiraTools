using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.HiraWorlds
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("HiraTools/HiraWorlds/HiraWorld Volume")]
    public class HiraWorldVolume : MonoBehaviour, IEnumerable<HiraWorldLoader>
    {
        [SerializeField] private HiraWorldLoader[] correspondingWorlds = null;
        [SerializeField] private StringReference playerTag = null;

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(playerTag.Value) && !other.CompareTag(playerTag.Value)) return;
            
            foreach (var loadProperty in correspondingWorlds) 
                loadProperty.SubmitRequestForWorldToLoad();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!string.IsNullOrEmpty(playerTag.Value) && !other.CompareTag(playerTag.Value)) return;
            
            foreach (var loadProperty in correspondingWorlds) 
                loadProperty.WithdrawRequestForWorldToLoad();
        }
        public IEnumerator<HiraWorldLoader> GetEnumerator() => 
            ((IEnumerable<HiraWorldLoader>) correspondingWorlds).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}