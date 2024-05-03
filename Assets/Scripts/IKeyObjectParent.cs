using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyObjectParent {

    public Transform GetKeyObjectFollowTransform();
    public void SetKeyObject(KeyObject keyObject);
    public KeyObject GetKeyObject();
    public bool HasKeyObject();

    public void ClearKeyObject();

}
