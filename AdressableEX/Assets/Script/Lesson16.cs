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
        #region ֪ʶ��һ ��ȡ���ؽ���
        StartCoroutine(LoadAsset());
        #endregion

        #region ֪ʶ��� �����;��ת��
        AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>("Cube");
        //AsyncOperationHandle temp = handle;
        //�������;�� ת��Ϊ �����͵ķ��Ͷ���
        //handle = temp.Convert<Texture2D>();
        #endregion

        #region ֪ʶ���� ǿ��ͬ��������Դ
        //���ִ����WaitForCompletion ��ô�Ῠ�����߳� һ��Ҫ����Դ���ؽ�����
        //�Ż��������ִ��
        print("1");
        handle.WaitForCompletion();
        print("2");
        print(handle.Result.name);
        //ע�⣺
        //Unity2020.1�汾����֮ǰ��ִ�иþ���벻����ȴ�����Դ
        //����ȴ�����û�м�����ɵ��첽���ؼ������Ż��������ִ��
        //Unity2020.2�汾�����ϰ汾���ڼ����Ѿ����ص���Դʱ����Ӱ����һЩ
        //���ԣ�������˵��������ʹ�����ַ�ʽ������Դ
        #endregion
    }
    IEnumerator LoadAsset()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Sphere2");

        //if (!handle.IsDone)
        //    yield return handle;

        //ע�⣺�������Դ��ص�AB�� �Ѿ����ع��� ��ô ֻ���ӡ0
        while (!handle.IsDone)
        {
            DownloadStatus info = handle.GetDownloadStatus();
            //����
            print(info.Percent);
            //�ֽڼ��ؽ��� ���� AB�� �����˶���
            //��ǰ�����˶������� /  �����ж������� ��λ���ֽ���
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
