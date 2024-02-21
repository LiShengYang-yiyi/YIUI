namespace ET
{
	/// <summary>
	/// 每个Config单例的基类
	/// 用于生命周期管理
	/// </summary>
	public interface IConfig
	{
		void ResolveRef();
	}
}