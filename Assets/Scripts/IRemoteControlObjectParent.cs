using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRemoteControlObjectParent {

    public Transform GetRemoteControlObjectFollowTransform();
    public void SetRemoteControlObject(RemoteControl remoteControlObject);
    public RemoteControl GetRemoteControlObject();
    public bool HasRemoteControlObject();

    public void ClearRemoteControlObject();

}
