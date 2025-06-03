using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Sequence", fileName = "NewCutsceneSequence")]
public class CutsceneSequence : ScriptableObject
{
    public List<CutsceneAction> actions;
}
