using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"MongoDB.Bson.dll",
		"System.Core.dll",
		"System.dll",
		"Unity.Core.dll",
		"Unity.Loader.dll",
		"Unity.ThirdParty.dll",
		"UnityEngine.CoreModule.dll",
		"YIUIFramework.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// ET.AEvent<ET.Client.NetClientComponentOnRead>
	// ET.AEvent<ET.EventType.AfterCreateClientScene>
	// ET.AEvent<ET.EventType.AfterCreateCurrentScene>
	// ET.AEvent<ET.EventType.AfterUnitCreate>
	// ET.AEvent<ET.EventType.AppStartInitFinish>
	// ET.AEvent<ET.EventType.ChangePosition>
	// ET.AEvent<ET.EventType.ChangeRotation>
	// ET.AEvent<ET.EventType.EnterMapFinish>
	// ET.AEvent<ET.EventType.EntryEvent1>
	// ET.AEvent<ET.EventType.EntryEvent3>
	// ET.AEvent<ET.EventType.LoginFinish>
	// ET.AEvent<ET.EventType.MoveStart>
	// ET.AEvent<ET.EventType.MoveStop>
	// ET.AEvent<ET.EventType.NumbericChange>
	// ET.AEvent<ET.EventType.SceneChangeFinish>
	// ET.AEvent<ET.EventType.SceneChangeStart>
	// ET.AInvokeHandler<ET.ConfigComponent.GetAllConfigBytes,object>
	// ET.AInvokeHandler<ET.ConfigComponent.GetOneConfigBytes,object>
	// ET.AInvokeHandler<ET.NavmeshComponent.RecastFileLoader,object>
	// ET.AInvokeHandler<ET.TimerCallback>
	// ET.ATimer<object>
	// ET.AwakeSystem<object,int>
	// ET.AwakeSystem<object,object,int>
	// ET.AwakeSystem<object,object,object>
	// ET.AwakeSystem<object,object>
	// ET.AwakeSystem<object>
	// ET.Client.IYIUIEvent<ET.Client.EventPutTipsView>
	// ET.Client.IYIUIEvent<ET.Client.OnClickChildListEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnClickItemEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnClickParentListEvent>
	// ET.Client.IYIUIEvent<ET.Client.OnGMEventClose>
	// ET.Client.IYIUIOpen<object,object,object>
	// ET.Client.IYIUIOpen<object>
	// ET.Client.YIUIBindSystem<object>
	// ET.Client.YIUICloseTweenSystem<object>
	// ET.Client.YIUIEventSystem<object,ET.Client.EventPutTipsView>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnClickChildListEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnClickItemEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnClickParentListEvent>
	// ET.Client.YIUIEventSystem<object,ET.Client.OnGMEventClose>
	// ET.Client.YIUIInitializeSystem<object>
	// ET.Client.YIUIOpenSystem<object,object,object,object>
	// ET.Client.YIUIOpenSystem<object,object>
	// ET.Client.YIUIOpenSystem<object>
	// ET.Client.YIUIOpenTweenSystem<object>
	// ET.ConfigSingleton<object>
	// ET.DestroySystem<object>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>
	// ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>
	// ET.ETAsyncTaskMethodBuilder<byte>
	// ET.ETAsyncTaskMethodBuilder<int>
	// ET.ETAsyncTaskMethodBuilder<object>
	// ET.ETAsyncTaskMethodBuilder<uint>
	// ET.ETTask<ET.Client.Wait_CreateMyUnit>
	// ET.ETTask<ET.Client.Wait_SceneChangeFinish>
	// ET.ETTask<ET.Client.Wait_UnitStop>
	// ET.ETTask<System.ValueTuple<uint,object>>
	// ET.ETTask<byte>
	// ET.ETTask<int>
	// ET.ETTask<object>
	// ET.ETTask<uint>
	// ET.EntityRef<object>
	// ET.IAwake<int>
	// ET.IAwake<object,int>
	// ET.IAwake<object,object>
	// ET.IAwake<object>
	// ET.IAwakeSystem<int>
	// ET.IAwakeSystem<object,int>
	// ET.IAwakeSystem<object,object>
	// ET.LateUpdateSystem<object>
	// ET.ListComponent<Unity.Mathematics.float3>
	// ET.ListComponent<object>
	// ET.LoadSystem<object>
	// ET.Singleton<object>
	// ET.UpdateSystem<object>
	// MongoDB.Bson.Serialization.IBsonSerializer<object>
	// System.Action<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Action<Unity.Mathematics.float3>
	// System.Action<byte,byte>
	// System.Action<int,int>
	// System.Action<int>
	// System.Action<long,int>
	// System.Action<long,long,object>
	// System.Action<long>
	// System.Action<object,object>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ArraySortHelper<Unity.Mathematics.float3>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.Comparer<Unity.Mathematics.float3>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<uint>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ComparisonComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ComparisonComparer<int>
	// System.Collections.Generic.ComparisonComparer<long>
	// System.Collections.Generic.ComparisonComparer<object>
	// System.Collections.Generic.ComparisonComparer<uint>
	// System.Collections.Generic.Dictionary.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ushort,object>
	// System.Collections.Generic.Dictionary<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,long>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,long>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ushort,object>
	// System.Collections.Generic.EqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<uint>
	// System.Collections.Generic.EqualityComparer<ushort>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet.Enumerator<ushort>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSet<ushort>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.HashSetEqualityComparer<ushort>
	// System.Collections.Generic.ICollection<ET.RpcInfo>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.ICollection<Unity.Mathematics.float3>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<ET.RpcInfo>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerable<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<ET.RpcInfo>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ushort>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IList<Unity.Mathematics.float3>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,long>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,long>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<ushort,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.List.Enumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.List<Unity.Mathematics.float3>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ObjectComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<uint>
	// System.Collections.Generic.ObjectEqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<uint>
	// System.Collections.Generic.ObjectEqualityComparer<ushort>
	// System.Collections.Generic.Queue.Enumerator<int>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<int>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<int,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<int,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<int,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<int,object>
	// System.Collections.Generic.SortedDictionary<int,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.ObjectModel.ReadOnlyCollection<Unity.Mathematics.float3>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Comparison<Unity.Mathematics.float3>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<uint>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,byte>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,int>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Func<System.ValueTuple<uint,uint>>
	// System.Func<object,System.ValueTuple<uint,uint>>
	// System.Func<object,byte>
	// System.Func<object,object,System.ValueTuple<uint,uint>>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Linq.Buffer<ET.RpcInfo>
	// System.Linq.Buffer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.Iterator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.Enumerable.WhereSelectListIterator<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.EnumerableSorter<System.Collections.Generic.KeyValuePair<object,int>,int>
	// System.Linq.EnumerableSorter<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.OrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>,int>
	// System.Linq.OrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Nullable<YIUIFramework.YIUIBindVo>
	// System.Predicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Predicate<Unity.Mathematics.float3>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ushort>
	// System.Runtime.CompilerServices.ConditionalWeakTable.<>c<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<System.ValueTuple<uint,uint>>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<System.ValueTuple<uint,uint>>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<System.ValueTuple<uint,uint>>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory.<>c<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory.<>c<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass32_0<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass32_0<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory<object>
	// System.ValueTuple<uint,object>
	// System.ValueTuple<uint,uint>
	// UnityEngine.Events.UnityAction<object>
	// YIUIFramework.LinkedListPool.<>c<object>
	// YIUIFramework.LinkedListPool<object>
	// YIUIFramework.ObjAsyncCache<object>
	// YIUIFramework.ObjCache<object>
	// YIUIFramework.ObjectPool<object>
	// YIUIFramework.Singleton<object>
	// YIUIFramework.UIDataValueBase<byte>
	// YIUIFramework.UIDataValueBase<int>
	// YIUIFramework.UIDataValueBase<object>
	// YIUIFramework.UIEventDelegate<byte>
	// YIUIFramework.UIEventDelegate<int>
	// YIUIFramework.UIEventDelegate<object>
	// YIUIFramework.UIEventHandleP1<byte>
	// YIUIFramework.UIEventHandleP1<int>
	// YIUIFramework.UIEventHandleP1<object>
	// YIUIFramework.UIEventP1<byte>
	// YIUIFramework.UIEventP1<int>
	// YIUIFramework.UIEventP1<object>
	// YIUIFramework.YIUILoopScroll.<>c__DisplayClass83_0<int,object>
	// YIUIFramework.YIUILoopScroll.<>c__DisplayClass83_0<object,object>
	// YIUIFramework.YIUILoopScroll.ListItemRenderer<int,object>
	// YIUIFramework.YIUILoopScroll.ListItemRenderer<object,object>
	// YIUIFramework.YIUILoopScroll.OnClickItemEvent<int,object>
	// YIUIFramework.YIUILoopScroll.OnClickItemEvent<object,object>
	// YIUIFramework.YIUILoopScroll<int,object>
	// YIUIFramework.YIUILoopScroll<object,object>
	// }}

	public void RefMethods()
	{
		// ET.ETTask ET.Client.YIUIEventSystem.Event<ET.Client.EventPutTipsView>(ET.Client.EventPutTipsView)
		// ET.ETTask ET.Client.YIUIEventSystem.Event<ET.Client.OnClickChildListEvent>(ET.Client.OnClickChildListEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.Event<ET.Client.OnClickItemEvent>(ET.Client.OnClickItemEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.Event<ET.Client.OnClickParentListEvent>(ET.Client.OnClickParentListEvent)
		// ET.ETTask ET.Client.YIUIEventSystem.Event<ET.Client.OnGMEventClose>(ET.Client.OnGMEventClose)
		// ET.ETTask<bool> ET.Client.YIUIMgrComponentSystem.ClosePanelAsync<object>(ET.Client.YIUIMgrComponent,bool,bool)
		// ET.ETTask<object> ET.Client.YIUIPanelComponentSystem.OpenViewAsync<object>(ET.Client.YIUIPanelComponent)
		// ET.ETTask<object> ET.Client.YIUIRootComponentSystem.OpenPanelAsync<object,object,object,object>(ET.Client.YIUIRootComponent,object,object,object)
		// ET.ETTask<object> ET.Client.YIUIRootComponentSystem.OpenPanelAsync<object>(ET.Client.YIUIRootComponent)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,object>(ET.ETTaskCompleted&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<System.ValueTuple<uint,uint>>,object>(System.Runtime.CompilerServices.TaskAwaiter<System.ValueTuple<uint,uint>>&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,object>(System.Runtime.CompilerServices.TaskAwaiter<object>&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,object>(ET.ETTaskCompleted&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,object>(ET.ETTaskCompleted&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,object>(System.Runtime.CompilerServices.TaskAwaiter<object>&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.EventPutTipsView>>(ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.EventPutTipsView>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnClickChildListEvent>>(ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnClickChildListEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnClickItemEvent>>(ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnClickItemEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnClickParentListEvent>>(ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnClickParentListEvent>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnGMEventClose>>(ET.Client.YIUIEventSystem.<Event>d__16<ET.Client.OnGMEventClose>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__27<ET.EventType.AppStartInitFinish>>(ET.EventSystem.<PublishAsync>d__27<ET.EventType.AppStartInitFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__27<ET.EventType.EntryEvent1>>(ET.EventSystem.<PublishAsync>d__27<ET.EventType.EntryEvent1>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__27<ET.EventType.EntryEvent2>>(ET.EventSystem.<PublishAsync>d__27<ET.EventType.EntryEvent2>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__27<ET.EventType.EntryEvent3>>(ET.EventSystem.<PublishAsync>d__27<ET.EventType.EntryEvent3>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__27<ET.EventType.LoginFinish>>(ET.EventSystem.<PublishAsync>d__27<ET.EventType.LoginFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.YIUIMgrComponentSystem.<ClosePanelAsync>d__12<object>>(ET.Client.YIUIMgrComponentSystem.<ClosePanelAsync>d__12<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIPanelComponentSystem.<OpenViewAsync>d__11<object>>(ET.Client.YIUIPanelComponentSystem.<OpenViewAsync>d__11<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__1<object>>(ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__1<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__5<object,object,object,object>>(ET.Client.YIUIRootComponentSystem.<OpenPanelAsync>d__5<object,object,object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.Start<object>(object&)
		// object ET.Entity.AddChild<object,object,object>(object,object,bool)
		// object ET.Entity.AddChildWithId<object,int>(long,int,bool)
		// object ET.Entity.AddComponent<object,int>(int,bool)
		// object ET.Entity.AddComponent<object,object,int>(object,int,bool)
		// object ET.Entity.AddComponent<object>(bool)
		// object ET.Entity.GetChild<object>(long)
		// object ET.Entity.GetComponent<object>()
		// object ET.Entity.GetParent<object>()
		// System.Void ET.Entity.RemoveComponent<object>()
		// System.Void ET.EventSystem.Awake<int>(ET.Entity,int)
		// System.Void ET.EventSystem.Awake<object,int>(ET.Entity,object,int)
		// System.Void ET.EventSystem.Awake<object,object>(ET.Entity,object,object)
		// object ET.EventSystem.Invoke<ET.NavmeshComponent.RecastFileLoader,object>(ET.NavmeshComponent.RecastFileLoader)
		// object ET.EventSystem.Invoke<ET.NavmeshComponent.RecastFileLoader,object>(int,ET.NavmeshComponent.RecastFileLoader)
		// System.Void ET.EventSystem.Publish<ET.Client.NetClientComponentOnRead>(ET.Scene,ET.Client.NetClientComponentOnRead)
		// System.Void ET.EventSystem.Publish<ET.EventType.AfterCreateClientScene>(ET.Scene,ET.EventType.AfterCreateClientScene)
		// System.Void ET.EventSystem.Publish<ET.EventType.AfterCreateCurrentScene>(ET.Scene,ET.EventType.AfterCreateCurrentScene)
		// System.Void ET.EventSystem.Publish<ET.EventType.AfterUnitCreate>(ET.Scene,ET.EventType.AfterUnitCreate)
		// System.Void ET.EventSystem.Publish<ET.EventType.ChangePosition>(ET.Scene,ET.EventType.ChangePosition)
		// System.Void ET.EventSystem.Publish<ET.EventType.ChangeRotation>(ET.Scene,ET.EventType.ChangeRotation)
		// System.Void ET.EventSystem.Publish<ET.EventType.EnterMapFinish>(ET.Scene,ET.EventType.EnterMapFinish)
		// System.Void ET.EventSystem.Publish<ET.EventType.MoveStart>(ET.Scene,ET.EventType.MoveStart)
		// System.Void ET.EventSystem.Publish<ET.EventType.MoveStop>(ET.Scene,ET.EventType.MoveStop)
		// System.Void ET.EventSystem.Publish<ET.EventType.NumbericChange>(ET.Scene,ET.EventType.NumbericChange)
		// System.Void ET.EventSystem.Publish<ET.EventType.SceneChangeFinish>(ET.Scene,ET.EventType.SceneChangeFinish)
		// System.Void ET.EventSystem.Publish<ET.EventType.SceneChangeStart>(ET.Scene,ET.EventType.SceneChangeStart)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.AppStartInitFinish>(ET.Scene,ET.EventType.AppStartInitFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.EntryEvent1>(ET.Scene,ET.EventType.EntryEvent1)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.EntryEvent2>(ET.Scene,ET.EventType.EntryEvent2)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.EntryEvent3>(ET.Scene,ET.EventType.EntryEvent3)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.LoginFinish>(ET.Scene,ET.EventType.LoginFinish)
		// object ET.Game.AddSingleton<object>()
		// object ET.JsonHelper.FromJson<object>(string)
		// object ET.MongoHelper.FromJson<object>(string)
		// System.Void ET.RandomGenerator.BreakRank<object>(System.Collections.Generic.List<object>)
		// string ET.StringHelper.ArrayToString<float>(float[])
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(MongoDB.Bson.IO.IBsonReader,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(string,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// MongoDB.Bson.Serialization.IBsonSerializer<object> MongoDB.Bson.Serialization.BsonSerializer.LookupSerializer<object>()
		// object MongoDB.Bson.Serialization.IBsonSerializerExtensions.Deserialize<object>(MongoDB.Bson.Serialization.IBsonSerializer<object>,MongoDB.Bson.Serialization.BsonDeserializationContext)
		// object ReferenceCollector.Get<object>(string)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// bool System.Enum.TryParse<int>(string,bool,int&)
		// bool System.Enum.TryParse<int>(string,int&)
		// System.Linq.IOrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>> System.Linq.Enumerable.OrderBy<System.Collections.Generic.KeyValuePair<object,int>,int>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Func<System.Collections.Generic.KeyValuePair<object,int>,int>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<System.Collections.Generic.KeyValuePair<object,int>,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>)
		// ET.RpcInfo[] System.Linq.Enumerable.ToArray<ET.RpcInfo>(System.Collections.Generic.IEnumerable<ET.RpcInfo>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<System.Collections.Generic.KeyValuePair<object,int>>.Select<object>(System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>)
		// object System.Reflection.CustomAttributeExtensions.GetCustomAttribute<object>(System.Reflection.MemberInfo)
		// System.Threading.Tasks.Task<object> System.Threading.Tasks.TaskFactory.StartNew<object>(System.Func<object>,System.Threading.CancellationToken)
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Resources.Load<object>(string)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<byte>(byte&,int,byte)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<float>(float&,int,float)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<int>(int&,int,int)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<long>(long&,int,long)
		// YIUIFramework.ParamGetResult YIUIFramework.ParamVo.Get<object>(object&,int,object)
		// byte YIUIFramework.ParamVo.Get<byte>(int,byte)
		// float YIUIFramework.ParamVo.Get<float>(int,float)
		// int YIUIFramework.ParamVo.Get<int>(int,int)
		// long YIUIFramework.ParamVo.Get<long>(int,long)
		// object YIUIFramework.ParamVo.Get<object>(int,object)
		// bool YIUIFramework.StrConv.ToNumber<byte>(string,byte&)
		// bool YIUIFramework.StrConv.ToNumber<float>(string,float&)
		// bool YIUIFramework.StrConv.ToNumber<int>(string,int&)
		// bool YIUIFramework.StrConv.ToNumber<long>(string,long&)
		// bool YIUIFramework.StrConv.ToNumber<object>(string,object&)
		// object YIUIFramework.UIBindComponentTable.FindComponent<object>(string)
		// object YIUIFramework.UIBindDataTable.FindDataValue<object>(string)
		// object YIUIFramework.UIBindEventTable.FindEvent<object>(string)
	}
}