using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Skill")]
public class SkillType : ScriptableObject
{
    public AnimatorOverrideController animatorOverrideController;
    public SkillEffect skillEffect;
    public float skillEffectScale;
    public float cooldown;
    public bool useSkill;
    public float mana;
    
    public enum SkillEffect{
        Damage=1,Force,Shield
    }
}
