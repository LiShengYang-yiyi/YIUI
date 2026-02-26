using System;
using ET;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace YIUIFramework
{
    public struct YIUIInvokeEntity_GetAssetInfo
    {
        public string Location;
        public Type AssetType;
    }

    public struct YIUIInvokeEntity_GetAssetInfoByGUID
    {
        public string AssetGUID;
        public Type AssetType;
    }

    // 加载任意实例化 to Vo
    public struct YIUIInvokeEntity_LoadInstantiateByVo
    {
        public YIUIBindVo BindVo;
        public EntityRef<Entity> ParentEntity;
        public Transform ParentTransform;
    }

    //实例化一个GameObject
    public struct YIUIInvokeEntity_InstantiateGameObject
    {
        public string PkgName;
        public string ResName;
    }

    //回收YIUI实例化资源
    public struct YIUIInvokeEntity_ReleaseInstantiate
    {
        public GameObject obj;
    }

    // 回收YIUI资源
    public struct YIUIInvokeEntity_Release
    {
        public UnityObject obj;
    }

    // 加载任意
    public struct YIUIInvokeEntity_Load
    {
        public Type LoadType;
        public string PkgName;
        public string ResName;
    }

    // 加载图片
    public struct YIUIInvokeEntity_LoadSprite
    {
        public string ResName;
    }

    // 回收YIUI 精灵/图片 资源
    public struct YIUIInvokeEntity_ReleaseSprite
    {
        public Sprite obj;
    }

    public struct YIUIInvokeEntity_LoadTexture2D
    {
        public string ResName;
    }

    //屏蔽所有YIUI操作
    public struct YIUIInvokeEntity_BanLayerOptionForever
    {
    }

    //恢复指定屏蔽的操作
    public struct YIUIInvokeEntity_RecoverLayerOptionForever
    {
        public long ForeverCode;
    }

    //添加倒计时
    public struct YIUIInvokeEntity_CountDownAdd
    {
        public CountDownTimerCallback TimerCallback;
        public double TotalTime;
        public double Interval;
        public bool StartCallback;
        public bool Forever;
    }

    //移除倒计时
    public struct YIUIInvokeEntity_CountDownRemove
    {
        public CountDownTimerCallback TimerCallback;
    }

    //等一帧 (1毫秒)
    public struct YIUIInvokeEntity_WaitFrameAsync
    {
    }

    //等指定毫秒
    public struct YIUIInvokeEntity_WaitAsync
    {
        public long Time;
        public ETCancellationToken CancellationToken;
    }

    //等指定秒
    public struct YIUIInvokeEntity_WaitSecondAsync
    {
        public float Time;
        public ETCancellationToken CancellationToken;
    }

    //协程锁
    public struct YIUIInvokeEntity_CoroutineLock
    {
        public long LockType;
        public long Lock;
    }
}