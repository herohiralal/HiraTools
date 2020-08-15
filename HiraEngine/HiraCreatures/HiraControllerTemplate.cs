namespace UnityEngine
{
    public class HiraControllerTemplate : ScriptableObject
    {
        [SerializeField] private HiraController controllerPrefab = null;

        public void Possess(HiraCreature creature)
        {
            creature.Controller.Unpossess();
            creature.OnUnpossess();
            var controller = Instantiate(controllerPrefab, Vector3.zero, Quaternion.identity);
            controller.Possess(creature);
            creature.OnPossess(controller);
        }
    }
}