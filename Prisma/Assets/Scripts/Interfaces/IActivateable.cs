using UnityEngine;
using System.Collections;

public interface IActivateable {
    bool Active { get; set; }

    void Activate();
    void Deactivate();
}
