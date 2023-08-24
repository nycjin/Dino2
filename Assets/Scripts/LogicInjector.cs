using forDino;
using Interface;
using Mirror;
using UnityEngine;

// !! 플레이 도중 생성되는 오브젝트는 대응 불가
// 멀티모드에서는 네트워크 연결 후 injection 하기 위해 NetworkBehaviour 상속
public class LogicInjector : NetworkBehaviour
{
    [SerializeField] GameObject logicManager;

    void Awake()
    {
        var currentSceneLogic = logicManager.GetComponent<ILogic>();
        var logicUsers = GameObject.FindGameObjectsWithTag("LogicUser");

        foreach ( var userObj in logicUsers )
        {
            var attachedScript = userObj.GetComponent<IInjectable>();
            if ( attachedScript == null )
            {
                Debug.LogError($"DI error : no injectable script on [{userObj.name}].");
                return;
            }

            attachedScript.SetLogic(currentSceneLogic);

            Debug.Log($"DI completed : \"{userObj.name}\" is injected by Injector !!");
        }
    }
}