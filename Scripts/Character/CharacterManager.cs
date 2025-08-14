using Halabang.Story;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace Halabang.Character {
  public class CharacterManager : MonoBehaviour {
    public List<CharacterBase> CharactersInScene { get; private set; }

    public void RegisterCharacter(CharacterBase character) {
      if (CharactersInScene == null) CharactersInScene = new List<CharacterBase>();

      CharactersInScene.Add(character);
    }

    public CharacterBase GetCharacter(ActorPreset actorPreset) {
      if (actorPreset == null) return null;

      return CharactersInScene.Where(r => r.CurrentActor.Guid.Equals(actorPreset.ActorGuid, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
    public CharacterBase GetCharacter(string actorGuid) {
      return CharactersInScene.Where(r => r.CurrentActor.Guid.Equals(actorGuid, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
  }
}
