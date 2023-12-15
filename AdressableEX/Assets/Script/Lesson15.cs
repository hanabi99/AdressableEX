using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson15 : MonoBehaviour
{
    AsyncOperationHandle<GameObject> handle;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 回顾目前动态异步加载的使用方式
        //handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        //通过事件监听的方式 结束时使用资源
        //handle.Completed += (obj) =>
        //{
        //    if (handle.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        print("事件创建对象");
        //        Instantiate(obj.Result);
        //    }

        //};
        #endregion

        #region 知识点二 3种使用异步加载资源的方式
        //1.事件监听（目前学习过的）
        //2.协同程序
        //3.异步函数（async和await ）
        #endregion

        #region 知识点三 通过协程使用异步加载
        //StartCoroutine(LoadAsset());
        #endregion

        #region 知识点四 通过异步函数async和await加载
        //注意：WebGL平台不支持异步函数语法

        //单任务等待
        Load();
        //多任务等待
        #endregion
    }

    IEnumerator LoadAsset()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        //一定是没有加载成功 再去 yield return
        if(!handle.IsDone)
            yield return handle;

        //加载成功 那么久可以使用了
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            print("协同程序创建对象");
            Instantiate(handle.Result);
        }
        else
            Addressables.Release(handle);
    }

    async void Load()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("Cube");

        AsyncOperationHandle<GameObject> handle2 = Addressables.LoadAssetAsync<GameObject>("Sphere2");

        //单任务等待
        //await handle.Task;

        //多任务等待
        await Task.WhenAll(handle.Task, handle2.Task);

        print("异步函数的形式加载的资源");
        Instantiate(handle.Result);
        Instantiate(handle2.Result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
