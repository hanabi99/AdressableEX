using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Lesson12 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 编辑器中资源加载的几种方式
        //Use Asset Database（fastest）：
        //使用资源数据库（最快的）
        //不用打AB包，直接本地加载资源，主要用于开发功能时

        //Simulate Groups（advanced）：
        //模拟组（后期）
        //不用打AB包
        //通过ResourceManager从资产数据库加载资产，就像通过AB包加载一样
        //通过引入时间延迟，模拟远程AB包的下载速度和本地AB包加载速度
        //在开发阶段可以使用这个模式来进行资源加载

        //Use Existing Build（requires built groups）：
        //正儿八经的加载AB包资源
        //必须打AB包后使用
        //会从AB包中加载资源
        #endregion

        #region 知识点二 本地资源发布
        Addressables.LoadAssetsAsync<GameObject>(new List<string>() { "Cube", "SD" }, (obj) =>
        {
            Instantiate(obj);
        }, Addressables.MergeMode.Intersection);

        //本地发布
        //所有组的加载和发布都选择本地路径
        //LocalBuildPath-打包路径
        //LocalLoadPath-加载路径

        //注意：使用默认设置，当发布应用程序时，会自动帮我们将AB包放入StreamingAssets文件夹中
        //如果修改了路径，我们需要自己将内容放入StreamingAssets文件夹中

        //不建议大家修改 默认的本地构建和加载路径 ，因为如果你修改了 就需要自己手动的去把AB包相关内容移动到StreamingAssets文件夹中
        #endregion

        #region 知识点三 模拟远端发布资源
        //第一步：将本机模拟为一台资源服务器，通过Unity自带工具或者第三方工具
        //第二步：设置远端路径
        //第三部：打包
        #endregion

        #region 知识点四 实际上的远端发布资源
        //在知识点三的基础上
        //1.在远端的电脑上搭建Http服务器
        //2.将打包出来的资源上传到对应服务器上
        #endregion

        #region 总结
        //一个项目中的资源到底是本地还是远端，根据实际情况而定
        //1.对于需要热更新的网络游戏
        //  默认基础资源作为本地资源，大部分资源作为远端资源
        //2.对于不需要热更新的单机游戏
        //  所有的资源都是本地资源
        //具体采用的打包策略根据实际情况来定
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
