using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public void UseSkill(ItemData item)
    {
        if (item == null) return;

        switch (item.SkillType)
        {
            case SkillType.Fireball:
                CastFireball();
                break;
            case SkillType.Heal:
                HealPlayer();
                break;
            case SkillType.Dash:
                DashForward();
                break;
        }

    }

    void CastFireball()
    {
      
    }

    void HealPlayer()
    {
        Debug.Log("�÷��̾� ü�� ȸ��!");
    }

    void DashForward()
    {
        Debug.Log("�÷��̾� ���!");
    }

    
}
