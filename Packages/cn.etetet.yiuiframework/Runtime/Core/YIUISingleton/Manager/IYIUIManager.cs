using ET;

namespace YIUIFramework
{
    /*public interface IYIUIEntity
    {
        EntityRef<Entity> EntityRef { get; set; }
    }*/

    public interface IYIUIManager : IYIUISingleton
    {
        /// <summary>
        /// 已经初始化结束 且成功了
        /// 失败了可以重复初始化
        /// </summary>
        bool InitedSucceed { get; }

        //激活状态
        //激活被关闭时 Update,LateUpdate,FixedUpdate 都会被停止
        //其他不影响
        bool Enabled { get; }
    }

    public interface IYIUIManagerAsyncInit : IYIUIManager
    {
        ETTask<bool> ManagerAsyncInit();
    }

    public interface IYIUIManagerUpdate : IYIUIManager
    {
        void ManagerUpdate();
    }

    public interface IYIUIManagerLateUpdate : IYIUIManager
    {
        void ManagerLateUpdate();
    }

    public interface IYIUIManagerFixedUpdate : IYIUIManager
    {
        void ManagerFixedUpdate();
    }
}