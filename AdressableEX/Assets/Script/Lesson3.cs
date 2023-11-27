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
               GameObject cube =  Instantiate(handle.Result); //因为是实例化出来的 用的是配置文件复制出去的 不是原生的预制体

                assetReference.ReleaseAsset();//释放资源 不会影响实例化的出来的Obj，但是会影响使用及引用

                assetReferenceTMaterial.LoadAssetAsync().Completed += (AsyncOperationStatus) =>
                {
                    cube.GetComponent<MeshRenderer>().material =  AsyncOperationStatus.Result;

                    assetReferenceTMaterial.ReleaseAsset(); //会丢失材质 因为不是实例化 会实实在在用的引用类型 会使用原生的资源
                    //必须使用AB包加载的模式 才能看出

                    print(assetReferenceTMaterial.Asset);//null
                    print(AsyncOperationStatus.Result);//不为空
                };
            }
        }
    }
    /// <summary>
    /// 异步场景加载
    /// </summary>
    public void LoadSecen()
    {
        SceneassetReference.LoadSceneAsync().Completed += (AsyncOperationHandle) => { print("?"); };
    }
    /// <summary>
    /// 直接实例化对象 
    /// </summary>
    public void Init()
    {
        objReference.InstantiateAsync();
    }
    void Update()
    {
        
    }
}
