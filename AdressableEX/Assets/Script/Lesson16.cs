using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson16 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 获取加载进度
        StartCoroutine(LoadAsset());
        #endregion

        #region 知识点二 无类型句柄转换
        AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>("Cube");
        //AsyncOperationHandle temp = handle;
        //把无类型句柄 转换为 有类型的泛型对象
        //handle = temp.Convert<Texture2D>();
        #endregion

        #region 知识点三 强制同步加载资源
        //如果执行了WaitForCompletion 那么会卡主主线程 一定要当资源加载结束后
        //才会继续往下执行
        print("1");
        handle.WaitForCompletion();
        print("2");
        print(handle.Result.name);
        //注意：
        //Unity2020.1版本或者之前，执行该句代码不仅会等待该资源
        //他会等待所有没有加载完成的异步加载加载完后才会继续往下执行
        //Unity2020.2版本或以上版本，在加载已经下载的资源时性能影响会好一些
        //所以，总体来说不建议大家使用这种方式加载资源
        #endregion
    }
    IEnumerator LoadAsset()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Sphere2");

        //if (!handle.IsDone)
        //    yield return handle;

        //注意：如果该资源相关的AB包 已经加载过了 那么 只会打印0
        while (!handle.IsDone)
        {
            DownloadStatus info = handle.GetDownloadStatus();
            //进度
            print(info.Percent);
            //字节加载进度 代表 AB包 加载了多少
            //当前下载了多少内容 /  总体有多少内容 单位是字节数
            print(info.DownloadedBytes + "/" + info.TotalBytes);
            yield return 0;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
        }
        else
            Addressables.Release(handle);
    }
}
