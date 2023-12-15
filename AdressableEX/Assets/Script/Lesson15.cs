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
        #region ֪ʶ��һ �ع�Ŀǰ��̬�첽���ص�ʹ�÷�ʽ
        //handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        //ͨ���¼������ķ�ʽ ����ʱʹ����Դ
        //handle.Completed += (obj) =>
        //{
        //    if (handle.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        print("�¼���������");
        //        Instantiate(obj.Result);
        //    }

        //};
        #endregion

        #region ֪ʶ��� 3��ʹ���첽������Դ�ķ�ʽ
        //1.�¼�������Ŀǰѧϰ���ģ�
        //2.Эͬ����
        //3.�첽������async��await ��
        #endregion

        #region ֪ʶ���� ͨ��Э��ʹ���첽����
        //StartCoroutine(LoadAsset());
        #endregion

        #region ֪ʶ���� ͨ���첽����async��await����
        //ע�⣺WebGLƽ̨��֧���첽�����﷨

        //������ȴ�
        Load();
        //������ȴ�
        #endregion
    }

    IEnumerator LoadAsset()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        //һ����û�м��سɹ� ��ȥ yield return
        if(!handle.IsDone)
            yield return handle;

        //���سɹ� ��ô�ÿ���ʹ����
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            print("Эͬ���򴴽�����");
            Instantiate(handle.Result);
        }
        else
            Addressables.Release(handle);
    }

    async void Load()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("Cube");

        AsyncOperationHandle<GameObject> handle2 = Addressables.LoadAssetAsync<GameObject>("Sphere2");

        //������ȴ�
        //await handle.Task;

        //������ȴ�
        await Task.WhenAll(handle.Task, handle2.Task);

        print("�첽��������ʽ���ص���Դ");
        Instantiate(handle.Result);
        Instantiate(handle2.Result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
