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
    private static AdressableMrg instance = null;//��֤�ڴ�ɼ��ԣ���ֹ�����������Ż�(ָ��������)
    public static AdressableMrg getInstance()
    {
        if (instance == null)
        {
            instance = new AdressableMrg();
        }
        return instance;

    }

    /// <summary>
    /// �洢�첽���ط���ֵ
    /// </summary>
    public Dictionary<string, IEnumerator> resDic = new Dictionary<string, IEnumerator>();

    /// <summary>
    /// �첽����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void LoadAssetAsync<T>(string name, Action<AsyncOperationHandle<T>> callback)
    {
        //���ڴ���ͬ�� ��ͬ������Դ�����ּ���
        //ͨ�����ּ�������ƴ��
        string keyname = name + "_" + typeof(T).Name;
        AsyncOperationHandle<T> handle;

        //������ع���Դ
        if (resDic.ContainsKey(keyname))
        {
            handle = (AsyncOperationHandle<T>)resDic[keyname];

            //�������whlieѭ������ �����ؽ�����
            if (handle.IsDone)//����첽���ؽ��� �ǿ϶��ǳɹ��˵�
            {
                callback(handle);
            }
            else//����û����
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
        //ֱ�������첽���� ���Ҽ�¼
        handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                callback(obj);
            }
            else
            {
                Debug.LogWarning(keyname + "��Դ����ʧ����");
                if (resDic.ContainsKey(keyname))
                    resDic.Remove(keyname);
            }

        };
        resDic.Add(keyname, handle);
    }

    /// <summary>
    /// ���ض������������Դ ���� ָ��������Դ
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
                Debug.Log("��Դ����ʧ��" + Keyname);
                if (resDic.ContainsKey(Keyname))
                {
                    resDic.Remove(Keyname);
                }
            }
        };
        resDic.Add(Keyname, handle);

    }

    /// <summary>
    /// �ͷ���Դ
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
    /// �ͷ�ָ��������Դ
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
    /// �����Դ
    /// </summary>
    public void ClearRes()
    {
        resDic.Clear();
        AssetBundle.UnloadAllAssetBundles(true);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }


}
