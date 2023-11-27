using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class AdressableMrg
{
    private AdressableMrg()
    {
    }
    private static AdressableMrg instance = null;//保证内存可见性，防止编译器过度优化(指令重排序)
    public static AdressableMrg getInstance()
    {
        if (instance == null)
        {
            instance = new AdressableMrg();
        }
        return instance;

    }

    /// <summary>
    /// 存储异步加载返回值
    /// </summary>
    public Dictionary<string, IEnumerator> resDic = new Dictionary<string, IEnumerator>();

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void LoadAssetAsync<T>(string name, Action<AsyncOperationHandle<T>> callback)
    {
        //由于存在同名 不同类型资源的区分加载
        //通过名字加类型名拼接
        string keyname = name + "_" + typeof(T).Name;
        AsyncOperationHandle<T> handle;

        //如果加载过资源
        if (resDic.ContainsKey(keyname))
        {
            handle = (AsyncOperationHandle<T>)resDic[keyname];

            //如果连续whlie循环加载 他加载结束了
            if (handle.IsDone)//如果异步加载结束 那肯定是成功了的
            {
                callback(handle);
            }
            else//加载没结束
            {
                handle.Completed += (obj) =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        callback(obj);
                    }
                };
            }
            return;
        }
        //直到进行异步加载 并且记录
        handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                callback(obj);
            }
            else
            {
                Debug.LogWarning(keyname + "资源加载失败了");
                if (resDic.ContainsKey(keyname))
                    resDic.Remove(keyname);
            }

        };
        resDic.Add(keyname, handle);
    }

    /// <summary>
    /// 加载多个满足条件资源 或者 指定条件资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mergeMode"></param>
    /// <param name="callback"></param>
    /// <param name="keys"></param>
    public void LoadAssetsAsync<T>(Addressables.MergeMode mergeMode, Action<T> callback, params string[] keys)
    {
        List<string> list = new List<string>(keys);
        string Keyname = "";
        foreach (var key in keys)
        {
            Keyname += key + "_";
        }
        Keyname += typeof(T).Name;

        AsyncOperationHandle<IList<T>> handle;

        if (resDic.ContainsKey(Keyname))
        {
            handle = (AsyncOperationHandle<IList<T>>)resDic[Keyname];
            if (handle.IsDone)
            {
                foreach (var item in handle.Result)
                {
                    callback(item);
                }
            }
            else
            {
                handle.Completed += (obj) =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        foreach (var item in handle.Result)
                        {
                            callback(item);
                        }
                    }
                };
            }
            return;
        }


        handle = Addressables.LoadAssetsAsync<T>(list, callback, mergeMode);
        handle.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.Log("资源加载失败" + Keyname);
                if (resDic.ContainsKey(Keyname))
                {
                    resDic.Remove(Keyname);
                }
            }
        };
        resDic.Add(Keyname, handle);

    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public void Release<T>(string name)
    {
        string keyName = name + "_" + typeof(T).Name;
        if (resDic.ContainsKey(keyName))
        {
            AsyncOperationHandle<T> handle = (AsyncOperationHandle<T>)resDic[keyName];
            Addressables.Release(handle);
            resDic.Remove(keyName);
        }
    }

    /// <summary>
    /// 释放指定条件资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    public void Release<T>(params string[] keys)
    {
        List<string> list = new List<string>(keys);
        string Keyname = "";
        foreach (var key in keys)
        {
            Keyname += key + "_";
        }
        Keyname += typeof(T).Name;

        if (resDic.ContainsKey(Keyname))
        {
            AsyncOperationHandle<IList<T>> handel = (AsyncOperationHandle<IList<T>>)resDic[Keyname];
            Addressables.Release(handel);
            resDic.Remove(Keyname);
        }
    }
    /// <summary>
    /// 清空资源
    /// </summary>
    public void ClearRes()
    {
        resDic.Clear();
        AssetBundle.UnloadAllAssetBundles(true);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }


}
