using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Lesson12 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ �༭������Դ���صļ��ַ�ʽ
        //Use Asset Database��fastest����
        //ʹ����Դ���ݿ⣨���ģ�
        //���ô�AB����ֱ�ӱ��ؼ�����Դ����Ҫ���ڿ�������ʱ

        //Simulate Groups��advanced����
        //ģ���飨���ڣ�
        //���ô�AB��
        //ͨ��ResourceManager���ʲ����ݿ�����ʲ�������ͨ��AB������һ��
        //ͨ������ʱ���ӳ٣�ģ��Զ��AB���������ٶȺͱ���AB�������ٶ�
        //�ڿ����׶ο���ʹ�����ģʽ��������Դ����

        //Use Existing Build��requires built groups����
        //�����˾��ļ���AB����Դ
        //�����AB����ʹ��
        //���AB���м�����Դ
        #endregion

        #region ֪ʶ��� ������Դ����
        Addressables.LoadAssetsAsync<GameObject>(new List<string>() { "Cube", "SD" }, (obj) =>
        {
            Instantiate(obj);
        }, Addressables.MergeMode.Intersection);

        //���ط���
        //������ļ��غͷ�����ѡ�񱾵�·��
        //LocalBuildPath-���·��
        //LocalLoadPath-����·��

        //ע�⣺ʹ��Ĭ�����ã�������Ӧ�ó���ʱ�����Զ������ǽ�AB������StreamingAssets�ļ�����
        //����޸���·����������Ҫ�Լ������ݷ���StreamingAssets�ļ�����

        //���������޸� Ĭ�ϵı��ع����ͼ���·�� ����Ϊ������޸��� ����Ҫ�Լ��ֶ���ȥ��AB����������ƶ���StreamingAssets�ļ�����
        #endregion

        #region ֪ʶ���� ģ��Զ�˷�����Դ
        //��һ����������ģ��Ϊһ̨��Դ��������ͨ��Unity�Դ����߻��ߵ���������
        //�ڶ���������Զ��·��
        //�����������
        #endregion

        #region ֪ʶ���� ʵ���ϵ�Զ�˷�����Դ
        //��֪ʶ�����Ļ�����
        //1.��Զ�˵ĵ����ϴHttp������
        //2.�������������Դ�ϴ�����Ӧ��������
        #endregion

        #region �ܽ�
        //һ����Ŀ�е���Դ�����Ǳ��ػ���Զ�ˣ�����ʵ���������
        //1.������Ҫ�ȸ��µ�������Ϸ
        //  Ĭ�ϻ�����Դ��Ϊ������Դ���󲿷���Դ��ΪԶ����Դ
        //2.���ڲ���Ҫ�ȸ��µĵ�����Ϸ
        //  ���е���Դ���Ǳ�����Դ
        //������õĴ�����Ը���ʵ���������
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
