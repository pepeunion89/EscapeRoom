using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour {

    [SerializeField] private ItemSO keySO;

    private IKeyObjectParent keyObjectParent;

    public void SetKeyObjectParent(IKeyObjectParent keyObjectParent) {

        this.keyObjectParent = keyObjectParent;

        keyObjectParent.SetKeyObject(this);

        transform.parent = keyObjectParent.GetKeyObjectFollowTransform();

    }

    public IKeyObjectParent GetKeyObjectParent() {
        return keyObjectParent;
    }

}
