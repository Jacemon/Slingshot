using System.Collections.Generic;
using Tools.Interactive;
using Tools.Interfaces;
using UnityEngine;

namespace Managers.Controllers
{
    public class InteractiveController : InteractiveGameObject
    {
        [SerializeReference]
        public List<InteractiveGameObject> interactiveObjects;

        public override void SetInteractive(bool canInteract)
        {
            foreach (var interactiveObject in interactiveObjects)
            {
                interactiveObject.SetInteractive(canInteract);
            }
        }
    }
}