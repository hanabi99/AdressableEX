using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Lesson14 : MonoBehaviour
{
    public AssetReference reference;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 回顾学过的加载可寻址资源的方式
        //1.通过标识类进行加载(指定资源加载) 
        //2.通过资源名或标签名加载单个资源(动态加载)
        //Addressables.LoadAssetAsync<GameObject>("Cube")
        //3.通过资源名或标签名或两者组合加载多个资源(动态加载)
        Addressables.LoadAssetsAsync<GameObject>(new List<string>() { "Cube", "SD" }, (obj) => { }, Addressables.MergeMode.Intersection);
        #endregion

        #region 知识点二 加载资源时Addressables帮助我们做了哪些事情？
        //1.查找指定键的资源位置
        //2.收集依赖项列表
        //3.下载所需的所有远程AB包
        //4.将AB包加载到内存中
        //5.设置Result资源对象的值
        //6.更新Status状态变量参数并且调用完成事件Completed

        //如果加载成功Status状态为成功，并且可以从Result中得到内容

        //如果加载失败除了Status状态为失败外
        //如果我们启用了 Log Runtime Exceptions选项 会在Console窗口打印信息
        #endregion

        #region 知识点三 根据名字或者标签获取 资源定位信息 加载资源
        //参数一：资源名或者标签名
        //参数二：资源类型
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync("Cube", typeof(GameObject));
        handle.Completed += (obj) =>
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in obj.Result)
                {
                    //我们可以利用定位信息 再去加载资源
                    //print(item.PrimaryKey);
                    Addressables.LoadAssetAsync<GameObject>(item).Completed += (obj) =>
                    {
                        Instantiate(obj.Result);
                    };
                }
            }
            else
            {
                Addressables.Release(handle);
            }
        };
        #endregion

        #region 知识点四 根据名字标签组合信息获取 资源定位信息 加载资源
        //参数一：资源名和标签名的组合
        //参数二：合并模式
        //参数三：资源类型
        AsyncOperationHandle<IList<IResourceLocation>> handle2 = Addressables.LoadResourceLocationsAsync(new List<string>() { "Cube", "Sphere", "SD" }, Addressables.MergeMode.Union, typeof(Object));
        handle2.Completed += (obj) => { 
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                //资源定位信息加载成功
                foreach (var item in obj.Result)
                {
                    //使用定位信息来加载资源
                    //我们可以利用定位信息 再去加载资源
                    print("******");
                    print(item.PrimaryKey);
                    print(item.InternalId);
                    print(item.ResourceType.Name);

                    Addressables.LoadAssetAsync<Object>(item).Completed += (obj) =>
                    {
                        //Instantiate(obj.Result);
                    };
                }
            }
            else
            {
                Addressables.Release(handle);
            }
        };
        #endregion

        #region 知识点五 根据资源定位信息加载资源的注意事项
        //1.资源信息当中提供了一些额外信息
        //  PrimaryKey：资源主键（资源名）
        //  InternalId：资源内部ID（资源路径）
        //  ResourceType：资源类型（Type可以获取资源类型名）
        //  我们可以利用这些信息处理一些特殊需求
        //  比如加载多个不同类型资源时 可以通过他们进行判断再分别进行处理

        //2.根据资源定位信息加载资源并不会加大我们加载开销
        //  只是分部完成加载了而已
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
