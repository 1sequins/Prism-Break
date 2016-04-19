using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ILockable {

    bool Locked { get; set; }
    List<GameObject> Locks { get; set; }

    void LinkLocks();
    void Unlock();
}
