using UnityEngine;

public interface ITriggerCheckable
{
   bool isWithinAttackingDistance { get; set; }
   bool isWithinKickingDistance { get; set; }

    void setAttackingDistanceBool(bool canAttack);
    void setKickingDistanceBool(bool canKick);

}

