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
        #region ֪ʶ��һ �ع�ѧ���ļ��ؿ�Ѱַ��Դ�ķ�ʽ
        //1.ͨ����ʶ����м���(ָ����Դ����) 
        //2.ͨ����Դ�����ǩ�����ص�����Դ(��̬����)
        //Addressables.LoadAssetAsync<GameObject>("Cube")
        //3.ͨ����Դ�����ǩ����������ϼ��ض����Դ(��̬����)
        Addressables.LoadAssetsAsync<GameObject>(new List<string>() { "Cube", "SD" }, (obj) => { }, Addressables.MergeMode.Intersection);
        #endregion

        #region ֪ʶ��� ������ԴʱAddressables��������������Щ���飿
        //1.����ָ��������Դλ��
        //2.�ռ��������б�
        //3.�������������Զ��AB��
        //4.��AB�����ص��ڴ���
        //5.����Result��Դ�����ֵ
        //6.����Status״̬�����������ҵ�������¼�Completed

        //������سɹ�Status״̬Ϊ�ɹ������ҿ��Դ�Result�еõ�����

        //�������ʧ�ܳ���Status״̬Ϊʧ����
        //������������� Log Runtime Exceptionsѡ�� ����Console���ڴ�ӡ��Ϣ
        #endregion

        #region ֪ʶ���� �������ֻ��߱�ǩ��ȡ ��Դ��λ��Ϣ ������Դ
        //����һ����Դ�����߱�ǩ��
        //����������Դ����
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync("Cube", typeof(GameObject));
        handle.Completed += (obj) =>
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in obj.Result)
                {
                    //���ǿ������ö�λ��Ϣ ��ȥ������Դ
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

        #region ֪ʶ���� �������ֱ�ǩ�����Ϣ��ȡ ��Դ��λ��Ϣ ������Դ
        //����һ����Դ���ͱ�ǩ�������
        //���������ϲ�ģʽ
        //����������Դ����
        AsyncOperationHandle<IList<IResourceLocation>> handle2 = Addressables.LoadResourceLocationsAsync(new List<string>() { "Cube", "Sphere", "SD" }, Addressables.MergeMode.Union, typeof(Object));
        handle2.Completed += (obj) => { 
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                //��Դ��λ��Ϣ���سɹ�
                foreach (var item in obj.Result)
                {
                    //ʹ�ö�λ��Ϣ��������Դ
                    //���ǿ������ö�λ��Ϣ ��ȥ������Դ
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

        #region ֪ʶ���� ������Դ��λ��Ϣ������Դ��ע������
        //1.��Դ��Ϣ�����ṩ��һЩ������Ϣ
        //  PrimaryKey����Դ��������Դ����
        //  InternalId����Դ�ڲ�ID����Դ·����
        //  ResourceType����Դ���ͣ�Type���Ի�ȡ��Դ��������
        //  ���ǿ���������Щ��Ϣ����һЩ��������
        //  ������ض����ͬ������Դʱ ����ͨ�����ǽ����ж��ٷֱ���д���

        //2.������Դ��λ��Ϣ������Դ������Ӵ����Ǽ��ؿ���
        //  ֻ�Ƿֲ���ɼ����˶���
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
