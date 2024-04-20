using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;


public class Lesson17 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ Ŀ¼������

        //Ŀ¼�ļ��ı�����Json�ļ���һ��Hash�ļ�
        //���м�¼����Ҫ������
        //Json�ļ��м�¼���ǣ�
        //1.����AB����ͼ������Դ��������ʵ�����������õĽű�����ͨ������ȥ����������ʹ�ã�
        //2.AB����������Դ���Ͷ�Ӧ���ࣨ��ͨ������ȥ����������ʹ�ã�
        //3.AB����Ӧ·��
        //4.��Դ��path��
        //�ȵ�
        //Hash�ļ��м�¼���ǣ�
        //Ŀ¼�ļ���Ӧhash�루ÿһ���ļ�����һ��Ψһ�룬�����ж��ļ��Ƿ�仯��
        //����ʱ���ص��ļ�hash����Զ��Ŀ¼��hash����жԱ�
        //������ֲ�һ���ͻ����Ŀ¼�ļ�

        //������ʹ��Զ�˷�������ʱ������Դ������Ҳ����һ��Ŀ¼�ļ�
        //Addressables��������ʱ�Զ�����Ŀ¼
        //���Զ��Ŀ¼�����仯��(����ͨ��hash�ļ�����洢�������ж��Ƿ�����Ŀ¼)
        //�����Զ������°汾��������ص��ڴ���

        #endregion

        #region ֪ʶ��� �ֶ�����Ŀ¼

        //1.���Ҫ�ֶ�����Ŀ¼ �����������йر��Զ�����

        //2.�Զ��������Ŀ¼�Ƿ��и��£�������Ŀ¼API
        Addressables.UpdateCatalogs().Completed += (obj) => { Addressables.Release(obj); };

        //3.��ȡĿ¼�б��ٸ���Ŀ¼
        //���� bool ���Ǽ��ؽ����� �᲻���Զ��ͷ��첽���صľ��
        Addressables.CheckForCatalogUpdates(true).Completed += (obj) =>
        {
            //����б���������ݴ���0 ֤���п��Ը��µ�Ŀ¼
            if (obj.Result.Count > 0)
            {
                //����Ŀ¼�б����Ŀ¼
                Addressables.UpdateCatalogs(obj.Result, true).Completed += (handle) =>
                {
                    //��Bool�Ƿ��Զ��ͷ���Դ�������
                    //���������� �ǵ��ͷ���Դ
                    //Addressables.Release(handle);
                    //Addressables.Release(obj);
                };
            }
        };


        #endregion

        #region ֪ʶ���� Ԥ���ذ�

        //����ͨ��Э��������
        StartCoroutine(LoadAsset());
        //1.���Ȼ�ȡ���ذ��Ĵ�С

        //2.Ԥ����

        //3.���ؽ���

        #endregion

        #region �ܽ�

        //һ�����ǻ���
        //�ս�����Ϸʱ ���� �л�����ʱ ��ʾһ��Loading����
        //���ǿ����ڴ�ʱ��ǰ���ذ�������֮����ʹ����Դ�Ͳ���������Ե��첽�����ӳٸ�
        //Ŀ¼���� ����һ�㶼����ڽ�����Ϸ��ʼ��Ϸ֮ǰ

        #endregion
    }

    IEnumerator CheckUpdate()
    {
        AsyncOperationHandle<List<string>> handle = Addressables.CheckForCatalogUpdates(true);
        while (handle.IsDone)
        {
            DownloadStatus info = handle.GetDownloadStatus();
            yield return null;
        }

        if (handle.Result.Count > 0)
        {
            AsyncOperationHandle<List<IResourceLocator>>
                updatehandle = Addressables.UpdateCatalogs(handle.Result, true);
            while (updatehandle.IsDone)
            {
                DownloadStatus info = updatehandle.GetDownloadStatus();
            }
        }
    }


    IEnumerator LoadAsset()
    {
        //1.���Ȼ�ȡ���ذ��Ĵ�С
        //���Դ���Դ������ǩ�����������ߵ����
        AsyncOperationHandle<long> handleSize =
            Addressables.GetDownloadSizeAsync(new List<string>() { "Cube", "Sphere", "SD" });
        yield return handleSize;
        //2.Ԥ����
        if (handleSize.Result > 0)
        {
            //�����Ϳ����첽���� ����������AB�����������
            AsyncOperationHandle handle =
                Addressables.DownloadDependenciesAsync(new List<string>() { "Cube", "Sphere", "SD" },
                    Addressables.MergeMode.Union);
            while (!handle.IsDone)
            {
                //3.���ؽ���
                DownloadStatus info = handle.GetDownloadStatus();
                print(info.Percent);
                print(info.DownloadedBytes + "/" + info.TotalBytes);
                yield return 0;
            }

            Addressables.Release(handle);
        }

    }

    IEnumerator DoUpdateAddressadble()
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // ������
        var checkHandle = Addressables.CheckForCatalogUpdates(true);
        yield return checkHandle;
        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("CheckForCatalogUpdates Error\n" + checkHandle.OperationException.ToString());
            yield break;
        }

        if (checkHandle.Result.Count > 0)
        {
            var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, true);
            yield return updateHandle;

            if (updateHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString());
                yield break;
            }

            // �����б������
            List<IResourceLocator> locators = updateHandle.Result;
            foreach (var locator in locators)
            {
                List<object> keys = new List<object>();
                keys.AddRange(locator.Keys);
                // ��ȡ�����ص��ļ��ܴ�С
                var sizeHandle = Addressables.GetDownloadSizeAsync(keys.GetEnumerator());
                yield return sizeHandle;
                if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString());
                    yield break;
                }

                long totalDownloadSize = sizeHandle.Result;
                //updateText.text = updateText.text + "\ndownload size : " + totalDownloadSize;
                Debug.Log("download size : " + totalDownloadSize);
                if (totalDownloadSize > 0)
                {
                    // ����
                    var downloadHandle = Addressables.DownloadDependenciesAsync(keys, true);
                    while (!downloadHandle.IsDone)
                    {
                        if (downloadHandle.Status == AsyncOperationStatus.Failed)
                        {
                            Debug.LogError("DownloadDependenciesAsync Error\n" +
                                           downloadHandle.OperationException.ToString());
                            yield break;
                        }

                        // ���ؽ���
                        float percentage = downloadHandle.PercentComplete;
                        Debug.Log($"������: {percentage}");
                        //updateText.text = updateText.text + $"\n������: {percentage}";
                        yield return null;
                    }

                    if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("�������!");
                        //updateText.text = updateText.text + "\n�������";
                    }
                }
            }
        }
        else
        {
            //updateText.text = updateText.text + "\nû�м�⵽����";
        }
    }
}

