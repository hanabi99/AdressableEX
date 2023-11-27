using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson5 : MonoBehaviour
{
    private void Start()
    {
        ///动态加载一个物体（物体名）或者 标签名
        AsyncOperationHandle asyncOperationHandle =  Addressables.LoadAssetAsync<GameObject>("Sphere");
        asyncOperationHandle.Completed += (asyncOperationHandle) =>
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
              GameObject.Instantiate(asyncOperationHandle.Result as GameObject);
            }
            ///注意动态加载的任何资源 只要释放后 都会影响之前按在使该资源对象
            Addressables.Release(asyncOperationHandle); //或者放入ondestroy生命周期函数里
        };
        //注意  如果存在同名同类型 同标签 我们无法确定加载哪一个 只会找到最近一个加载
        //如果是同名同标签 不同类型 可以根据泛型加载


        ///释放资源
        //Addressables.Release(asyncOperationHandle);


        ///动态加载场景的API
        Addressables.LoadSceneAsync("New Scene", UnityEngine.SceneManagement.LoadSceneMode.Single, false, 100).Completed += (asyncOperationHandle) =>
        {

            asyncOperationHandle.Result.ActivateAsync().completed += (obj) =>
            {

                Addressables.Release(asyncOperationHandle);//释放并不会影影响已加载的物体
            };
        };
        
    }
}
