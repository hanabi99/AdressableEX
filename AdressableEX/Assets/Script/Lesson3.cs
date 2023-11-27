using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson3 : MonoBehaviour
{
    public AssetReference assetReference;
    public AssetReferenceAtlasedSprite atlasReference;
    public AssetReferenceGameObject objReference;
    public AssetReferenceTexture textureReference;
    public AssetReferenceSprite spriteReference;
    public AssetReferenceT<AudioClip> audioReference;
    public AssetReferenceT<RuntimeAnimatorController> animReference;
    public AssetReferenceT<TextAsset> textReference;
    public AssetReference SceneassetReference;
    public AssetReferenceT<Material> assetReferenceTMaterial;
    void Start()
    {
        AsyncOperationHandle<GameObject> handle =  assetReference.LoadAssetAsync<GameObject>();
        handle.Completed += Handle_Completed;
        Init();
       // LoadSecen();
    }

    private void Handle_Completed(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (handle.Result != null)
            {
               GameObject cube =  Instantiate(handle.Result); //��Ϊ��ʵ���������� �õ��������ļ����Ƴ�ȥ�� ����ԭ����Ԥ����

                assetReference.ReleaseAsset();//�ͷ���Դ ����Ӱ��ʵ�����ĳ�����Obj�����ǻ�Ӱ��ʹ�ü�����

                assetReferenceTMaterial.LoadAssetAsync().Completed += (AsyncOperationStatus) =>
                {
                    cube.GetComponent<MeshRenderer>().material =  AsyncOperationStatus.Result;

                    assetReferenceTMaterial.ReleaseAsset(); //�ᶪʧ���� ��Ϊ����ʵ���� ��ʵʵ�����õ��������� ��ʹ��ԭ������Դ
                    //����ʹ��AB�����ص�ģʽ ���ܿ���

                    print(assetReferenceTMaterial.Asset);//null
                    print(AsyncOperationStatus.Result);//��Ϊ��
                };
            }
        }
    }
    /// <summary>
    /// �첽��������
    /// </summary>
    public void LoadSecen()
    {
        SceneassetReference.LoadSceneAsync().Completed += (AsyncOperationHandle) => { print("?"); };
    }
    /// <summary>
    /// ֱ��ʵ�������� 
    /// </summary>
    public void Init()
    {
        objReference.InstantiateAsync();
    }
    void Update()
    {
        
    }
}
