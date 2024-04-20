using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson18 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 什么是引用计数规则？
        //当我们通过加载使用可寻址资源时
        //Addressables会在内部帮助我们进行引用计数
        //使用资源时，引用计数+1
        //释放资源时，引用计数-1
        //当可寻址资源的引用为0时，就可以卸载它了

        //为了避免内存泄露（不需要使用的内容残留在内存中）
        //我们要保证加载资源和卸载资源是配对使用的

        //注意：释放的资源不一定立即从内存中卸载
        //在卸载资源所属的AB包之前，不会释放资源使用的内存
        //(比如自己所在的AB包 被别人使用时，这时AB包不会被卸载，所以自己还在内存中)
        //我们可以使用Resources.UnloadUnusedAssets卸载资源（建议在切换场景时调用）

        //AB包也有自己的引用计数（Addressables把它也视为可寻址资源）
        //从AB包中加载资源时，引用计数+1
        //从AB包中卸载资源时，引用计数-1
        //当AB包引用计数为0时，意味着不再使用了，这时会从内存中卸载

        //总结：Addressables内部会通过引用计数帮助我们管理内存
        //我们只需要保证 加载和卸载资源配对使用即可
        #endregion

        #region 知识点二 举例说明引用计数
        //我们创建两个一样的资源
        //然后一个一个的释放他们的资源句柄
        //观察他们创建出来的对象变化

        //注意：使用第三种模式加载资源（从AB包中加载）
        #endregion

        #region 知识点三 回顾之前写的资源管理器
        //我们之前写的资源管理器
        //通过自己管理异步加载的返回句柄会让系统自带的引用计数功能不起作用
        //因为我们不停的在复用一个句柄
        #endregion
    }

    private List<AsyncOperationHandle<GameObject>> list = new List<AsyncOperationHandle<GameObject>>();
    private void Update()
    {
        //创建对象 记录异步操作句柄
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Cube");
            //handle.Completed += (obj) =>
            //{
            //    Instantiate(obj.Result);
            //};
            //list.Add(handle);

            AdressableMrg.getInstance().LoadAssetAsync<GameObject>("Cube", (obj) =>
            {
                Instantiate(obj.Result);
            });
        }

        //从创建对象中 释放异步操作句柄资源
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //if(list.Count > 0)
            //{
            //    Addressables.Release(list[0]);
            //    list.RemoveAt(0);
            //}

            AdressableMrg.getInstance().Release<GameObject>("Cube");
        }
    }
}
