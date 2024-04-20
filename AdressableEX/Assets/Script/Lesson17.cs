using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;


public class Lesson17 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 目录的作用

        //目录文件的本质是Json文件和一个Hash文件
        //其中记录的主要内容有
        //Json文件中记录的是：
        //1.加载AB包、图集、资源、场景、实例化对象所用的脚本（会通过反射去加载他们来使用）
        //2.AB包中所有资源类型对应的类（会通过反射去加载他们来使用）
        //3.AB包对应路径
        //4.资源的path名
        //等等
        //Hash文件中记录的是：
        //目录文件对应hash码（每一个文件都有一个唯一码，用来判断文件是否变化）
        //更新时本地的文件hash码会和远端目录的hash码进行对比
        //如果发现不一样就会更新目录文件

        //当我们使用远端发布内容时，在资源服务器也会有一个目录文件
        //Addressables会在运行时自动管理目录
        //如果远端目录发生变化了(他会通过hash文件里面存储的数据判断是否是新目录)
        //它会自动下载新版本并将其加载到内存中

        #endregion

        #region 知识点二 手动更新目录

        //1.如果要手动更新目录 建议在设置中关闭自动更新

        //2.自动检查所有目录是否有更新，并更新目录API
        Addressables.UpdateCatalogs().Completed += (obj) => { Addressables.Release(obj); };

        //3.获取目录列表，再更新目录
        //参数 bool 就是加载结束后 会不会自动释放异步加载的句柄
        Addressables.CheckForCatalogUpdates(true).Completed += (obj) =>
        {
            //如果列表里面的内容大于0 证明有可以更新的目录
            if (obj.Result.Count > 0)
            {
                //根据目录列表更新目录
                Addressables.UpdateCatalogs(obj.Result, true).Completed += (handle) =>
                {
                    //该Bool是否自动释放资源（句柄）
                    //如果更新完毕 记得释放资源
                    //Addressables.Release(handle);
                    //Addressables.Release(obj);
                };
            }
        };


        #endregion

        #region 知识点三 预加载包

        //建议通过协程来加载
        StartCoroutine(LoadAsset());
        //1.首先获取下载包的大小

        //2.预加载

        //3.加载进度

        #endregion

        #region 总结

        //一般我们会在
        //刚进入游戏时 或者 切换场景时 显示一个Loading界面
        //我们可以在此时提前加载包，这样之后在使用资源就不会出现明显的异步加载延迟感
        //目录更新 我们一般都会放在进入游戏开始游戏之前

        #endregion
    }

    IEnumerator CheckUpdate()
    {
        AsyncOperationHandle<List<string>> handle = Addressables.CheckForCatalogUpdates(true);
        while (handle.IsDone)
        {
            DownloadStatus info = handle.GetDownloadStatus();
            yield return null;
        }

        if (handle.Result.Count > 0)
        {
            AsyncOperationHandle<List<IResourceLocator>>
                updatehandle = Addressables.UpdateCatalogs(handle.Result, true);
            while (updatehandle.IsDone)
            {
                DownloadStatus info = updatehandle.GetDownloadStatus();
            }
        }
    }


    IEnumerator LoadAsset()
    {
        //1.首先获取下载包的大小
        //可以传资源名、标签名、或者两者的组合
        AsyncOperationHandle<long> handleSize =
            Addressables.GetDownloadSizeAsync(new List<string>() { "Cube", "Sphere", "SD" });
        yield return handleSize;
        //2.预加载
        if (handleSize.Result > 0)
        {
            //这样就可以异步加载 所有依赖的AB包相关内容了
            AsyncOperationHandle handle =
                Addressables.DownloadDependenciesAsync(new List<string>() { "Cube", "Sphere", "SD" },
                    Addressables.MergeMode.Union);
            while (!handle.IsDone)
            {
                //3.加载进度
                DownloadStatus info = handle.GetDownloadStatus();
                print(info.Percent);
                print(info.DownloadedBytes + "/" + info.TotalBytes);
                yield return 0;
            }

            Addressables.Release(handle);
        }

    }

    IEnumerator DoUpdateAddressadble()
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // 检测更新
        var checkHandle = Addressables.CheckForCatalogUpdates(true);
        yield return checkHandle;
        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("CheckForCatalogUpdates Error\n" + checkHandle.OperationException.ToString());
            yield break;
        }

        if (checkHandle.Result.Count > 0)
        {
            var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, true);
            yield return updateHandle;

            if (updateHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString());
                yield break;
            }

            // 更新列表迭代器
            List<IResourceLocator> locators = updateHandle.Result;
            foreach (var locator in locators)
            {
                List<object> keys = new List<object>();
                keys.AddRange(locator.Keys);
                // 获取待下载的文件总大小
                var sizeHandle = Addressables.GetDownloadSizeAsync(keys.GetEnumerator());
                yield return sizeHandle;
                if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString());
                    yield break;
                }

                long totalDownloadSize = sizeHandle.Result;
                //updateText.text = updateText.text + "\ndownload size : " + totalDownloadSize;
                Debug.Log("download size : " + totalDownloadSize);
                if (totalDownloadSize > 0)
                {
                    // 下载
                    var downloadHandle = Addressables.DownloadDependenciesAsync(keys, true);
                    while (!downloadHandle.IsDone)
                    {
                        if (downloadHandle.Status == AsyncOperationStatus.Failed)
                        {
                            Debug.LogError("DownloadDependenciesAsync Error\n" +
                                           downloadHandle.OperationException.ToString());
                            yield break;
                        }

                        // 下载进度
                        float percentage = downloadHandle.PercentComplete;
                        Debug.Log($"已下载: {percentage}");
                        //updateText.text = updateText.text + $"\n已下载: {percentage}";
                        yield return null;
                    }

                    if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("下载完毕!");
                        //updateText.text = updateText.text + "\n下载完毕";
                    }
                }
            }
        }
        else
        {
            //updateText.text = updateText.text + "\n没有检测到更新";
        }
    }
}

