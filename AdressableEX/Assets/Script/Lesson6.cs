using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class Lesson6 : MonoBehaviour
{
    
    void Start()
    {
        ///ͬʱ���ض��ͬ����Դ
        Addressables.LoadAssetsAsync<Object>("HD", (obj) => { 
        //{
        //    print(obj.name);

        });
        ////����ʹ���õڶ��ַ��� �����ͷ�
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

        // ͬʱ�ñ�ǩ������ͬʱԼ�� 
        Addressables.LoadAssetsAsync<Object>(new List<string>(2) { "Cube","SD"} , (obj) => {
            print(obj.name);
        },Addressables.MergeMode.Intersection);




    }


}
