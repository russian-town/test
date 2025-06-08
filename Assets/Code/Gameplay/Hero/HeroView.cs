using Code.Gameplay.Shadow.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Hero
{
    public class HeroView : MonoBehaviour, IShadowReceiver
    {
        public void BeginReceive() { }

        public void Receive()
        {
            Debug.Log("Receive");
        }

        public void EndReceive() { }
    }
}
