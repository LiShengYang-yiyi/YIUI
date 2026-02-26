namespace ET
{
    public enum EHashWaitError
    {
        Error = -1, //其他
        Success = 0, //成功
        Destroy = 1, //摧毁
        Cancel = 2, //取消
        Timeout = 3, //超时
        Reset = 4, //重置
    }
}