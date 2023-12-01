using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EX : MonoBehaviour
{
    void Start()
    {
        //AdressableMrg.getInstance().LoadAssetAsync<GameObject>("Cube", (obj) =>
        //{
        //    GameObject cube = Instantiate(obj.Result);
        //    cube.transform.position = new Vector3(1, 1, 1);

        //});
        //AdressableMrg.getInstance().LoadAssetAsync<GameObject>("Cube", (obj) =>
        //{
        //    GameObject cube = Instantiate(obj.Result);
        //    cube.transform.position = new Vector3(2, 2, 2);

        //    AdressableMrg.getInstance().Release<GameObject>("Cube");
        //});
        //AdressableMrg.getInstance().LoadAssetsAsync<Object>(Addressables.MergeMode.Intersection, (obj) =>
        //{
        //    Instantiate(obj);
        //    Debug.Log(obj.name);
        //}, "Cube", "SD");
        AdressableMrg.getInstance().LoadAssetsAsync<GameObject>(Addressables.MergeMode.Intersection, (obj) =>
        {
            Instantiate(obj);
            Debug.Log(obj.name);
        }, "Cube", "SD");
        //Addressables.LoadAssetsAsync<GameObject>(new List<string>(2) { "Sphere", "HD" }, (obj) =>
        //{
        //    print(obj.name);
        //    Instantiate(obj);
        //}, Addressables.MergeMode.Intersection);
    }


}
