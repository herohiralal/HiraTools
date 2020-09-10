﻿namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New HiraController", menuName = "Hiralal/HiraEngine/HiraCreatures/Controller Template")]
    public class HiraControllerTemplate : ScriptableObject
    {
        [SerializeField] private HiraController controllerPrefab = null;

        public void Possess(HiraCreature creature)
        {
            // TODO: Replace this with a GameMode based solution.
            if (creature.Controller != null)
            {
                creature.Controller.Unpossess();
                creature.OnUnpossess();
            }

            if (controllerPrefab != null)
            {
                var controller = Instantiate(controllerPrefab, Vector3.zero, Quaternion.identity);
                controller.Possess(creature);
                creature.OnPossess(controller);
            }
        }
    }
}