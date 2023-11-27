using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class Lesson6 : MonoBehaviour
{
    
    void Start()
    {
        ///同时加载多个同名资源
        Addressables.LoadAssetsAsync<Object>("HD", (obj) => { 
        //{
        //    print(obj.name);

        });
        ////建议使用用第二种方法 便于释放
        AsyncOperationHandle<IList<Object>> asyncOperationHandle =  Addressables.LoadAssetsAsync<Object>("HD", (obj) =>
        {
            print(obj.name);
        });
        asyncOperationHandle.Completed += (obj) =>
        {
           foreach (var item in obj.Result)
            {
                //print(item.name);
            }
        };

        // 同时用标签和名字同时约束 
        Addressables.LoadAssetsAsync<Object>(new List<string>(2) { "Cube","SD"} , (obj) => {
            print(obj.name);
        },Addressables.MergeMode.Intersection);




    }


}
