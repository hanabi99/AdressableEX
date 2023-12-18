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
        #region ֪ʶ��һ ʲô�����ü�������
        //������ͨ������ʹ�ÿ�Ѱַ��Դʱ
        //Addressables�����ڲ��������ǽ������ü���
        //ʹ����Դʱ�����ü���+1
        //�ͷ���Դʱ�����ü���-1
        //����Ѱַ��Դ������Ϊ0ʱ���Ϳ���ж������

        //Ϊ�˱����ڴ�й¶������Ҫʹ�õ����ݲ������ڴ��У�
        //����Ҫ��֤������Դ��ж����Դ�����ʹ�õ�

        //ע�⣺�ͷŵ���Դ��һ���������ڴ���ж��
        //��ж����Դ������AB��֮ǰ�������ͷ���Դʹ�õ��ڴ�
        //(�����Լ����ڵ�AB�� ������ʹ��ʱ����ʱAB�����ᱻж�أ������Լ������ڴ���)
        //���ǿ���ʹ��Resources.UnloadUnusedAssetsж����Դ���������л�����ʱ���ã�

        //AB��Ҳ���Լ������ü�����Addressables����Ҳ��Ϊ��Ѱַ��Դ��
        //��AB���м�����Դʱ�����ü���+1
        //��AB����ж����Դʱ�����ü���-1
        //��AB�����ü���Ϊ0ʱ����ζ�Ų���ʹ���ˣ���ʱ����ڴ���ж��

        //�ܽ᣺Addressables�ڲ���ͨ�����ü����������ǹ����ڴ�
        //����ֻ��Ҫ��֤ ���غ�ж����Դ���ʹ�ü���
        #endregion

        #region ֪ʶ��� ����˵�����ü���
        //���Ǵ�������һ������Դ
        //Ȼ��һ��һ�����ͷ����ǵ���Դ���
        //�۲����Ǵ��������Ķ���仯

        //ע�⣺ʹ�õ�����ģʽ������Դ����AB���м��أ�
        #endregion

        #region ֪ʶ���� �ع�֮ǰд����Դ������
        //����֮ǰд����Դ������
        //ͨ���Լ������첽���صķ��ؾ������ϵͳ�Դ������ü������ܲ�������
        //��Ϊ���ǲ�ͣ���ڸ���һ�����
        #endregion
    }

    private List<AsyncOperationHandle<GameObject>> list = new List<AsyncOperationHandle<GameObject>>();
    private void Update()
    {
        //�������� ��¼�첽�������
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

        //�Ӵ��������� �ͷ��첽���������Դ
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
