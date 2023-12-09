using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarUI : MonoBehaviour
{
    [SerializeField] private CooldownSkillUI singleSkill;
    [SerializeField] private CooldownSkillUI combineSkill;

    public void startUseSingleSkillCooldown()
    {
        singleSkill.setCooldown(true);
    }

    public void stopUseSingleSkillCooldown()
    {
        singleSkill.setCooldown(false);
    }

    public void applySingleSkillCooldown(float remainingTime, float cooldownTime, bool isCooldown)
    {
        singleSkill.applyCooldown(remainingTime, cooldownTime, isCooldown);
    }

    public void startUseCombineSkillCooldown()
    {
        combineSkill.setCooldown(true);
    }

    public void stopUseCombineSkillCooldown()
    {
        combineSkill.setCooldown(false);
    }

    public void applyCombineSkillCooldown(float remainingTime, float cooldownTime, bool isCooldown)
    {
        combineSkill.applyCooldown(remainingTime, cooldownTime, isCooldown);
    }
}
