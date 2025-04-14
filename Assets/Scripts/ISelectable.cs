using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    // interact with system control
    void OnPress();

    // draw highlight/outline
    void OnSelect(Material highlightMaterial);

    // unhighlight control
    void OnDeSelect();
}