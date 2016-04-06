using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ILockable {

    bool Locked { get; set; }

    void Unlock();
}
