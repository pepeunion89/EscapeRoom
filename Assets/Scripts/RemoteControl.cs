using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControl : MonoBehaviour {

    public static RemoteControl Instance { get; private set; }

    [SerializeField] private ItemSO remoteControlSO;

    private IRemoteControlObjectParent remoteControlObjectParent;

    public void SetRemoteControlObjectParent(IRemoteControlObjectParent remoteControlObjectParent) {

        this.remoteControlObjectParent = remoteControlObjectParent;

        remoteControlObjectParent.SetRemoteControlObject(this);

        transform.parent = remoteControlObjectParent.GetRemoteControlObjectFollowTransform();

    }

    public IRemoteControlObjectParent GetRemoteControlObjectParent() {
        return remoteControlObjectParent;
    }


}
