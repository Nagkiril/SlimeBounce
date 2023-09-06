using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilitySlime
{
    public void SetEnhanced(bool isEnhanced);

    public void ApplyParameters(List<float> parameters);
}
