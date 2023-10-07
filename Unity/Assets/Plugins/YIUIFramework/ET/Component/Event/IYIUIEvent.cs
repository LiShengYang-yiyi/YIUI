namespace ET.Client
{
    public interface IYIUICommonEvent
    {
        /// <summary>
        /// 触发消息
        /// </summary>
        /// <param name="uiScene">当前UI所在的场景</param>
        /// <param name="message">你所监听的消息的数据</param>
        void Run(Scene uiScene, object message);
    }
}