using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson5 : MonoBehaviour
{
    private void Start()
    {
        ///��̬����һ�����壨������������ ��ǩ��
        AsyncOperationHandle asyncOperationHandle =  Addressables.LoadAssetAsync<GameObject>("Sphere");
        asyncOperationHandle.Completed += (asyncOperationHandle) =>
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
              GameObject.Instantiate(asyncOperationHandle.Result as GameObject);
            }
            ///ע�⶯̬���ص��κ���Դ ֻҪ�ͷź� ����Ӱ��֮ǰ����ʹ����Դ����
            Addressables.Release(asyncOperationHandle); //���߷���ondestroy�������ں�����
        };
        //ע��  �������ͬ��ͬ���� ͬ��ǩ �����޷�ȷ��������һ�� ֻ���ҵ����һ������
        //�����ͬ��ͬ��ǩ ��ͬ���� ���Ը��ݷ��ͼ���


        ///�ͷ���Դ
        //Addressables.Release(asyncOperationHandle);


        ///��̬���س�����API
        Addressables.LoadSceneAsync("New Scene", UnityEngine.SceneManagement.LoadSceneMode.Single, false, 100).Completed += (asyncOperationHandle) =>
        {

            asyncOperationHandle.Result.ActivateAsync().completed += (obj) =>
            {

                Addressables.Release(asyncOperationHandle);//�ͷŲ�����ӰӰ���Ѽ��ص�����
            };
        };
        
    }
}
