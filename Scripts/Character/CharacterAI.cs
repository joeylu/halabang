using Halabang.Plugin;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Halabang.Character {
  [RequireComponent(typeof(CharacterUI))]
  public class CharacterAI : MonoBehaviour {

    private CharacterBase character;

    private void Awake() {
      character = GetComponent<CharacterBase>();
    }
  }
}
