using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Luban.Runtime.dll",
		"Newtonsoft.Json.dll",
		"System.Core.dll",
		"UniTask.dll",
		"UnityEngine.CoreModule.dll",
		"YooAsset.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<EntityRender.<>c__DisplayClass6_0.<<SetData>b__0>d<object,object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<UIView.UICore.<CountDownDestroy>d__7>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<EntityRender.<>c__DisplayClass6_0.<<SetData>b__0>d<object,object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<UIView.UICore.<CountDownDestroy>d__7>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// System.Action<LogData>
	// System.Action<UIView.UIBindTool.BindingVO>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<LogData>
	// System.Collections.Generic.ArraySortHelper<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<LogData>
	// System.Collections.Generic.Comparer<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<LogData>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<LogData>
	// System.Collections.Generic.IComparer<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<LogData>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<LogData>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<LogData>
	// System.Collections.Generic.IList<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<LogData>
	// System.Collections.Generic.List.Enumerator<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<LogData>
	// System.Collections.Generic.List<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<LogData>
	// System.Collections.Generic.ObjectComparer<UIView.UIBindTool.BindingVO>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<LogData>
	// System.Collections.ObjectModel.ReadOnlyCollection<UIView.UIBindTool.BindingVO>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<LogData>
	// System.Comparison<UIView.UIBindTool.BindingVO>
	// System.Comparison<object>
	// System.Func<Cysharp.Threading.Tasks.UniTaskVoid>
	// System.Func<int,byte>
	// System.Func<int>
	// System.Func<object,byte>
	// System.Func<object,int>
	// System.Func<object,object>
	// System.Linq.Enumerable.Iterator<int>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<int>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,int>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,int>
	// System.Linq.Enumerable.WhereSelectListIterator<object,int>
	// System.Nullable<long>
	// System.Predicate<LogData>
	// System.Predicate<UIView.UIBindTool.BindingVO>
	// System.Predicate<object>
	// }}

	public void RefMethods()
	{
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UIView.UICore.<CountDownDestroy>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UIView.UICore.<CountDownDestroy>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,EntityRender.<>c__DisplayClass6_0.<<SetData>b__0>d<object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,EntityRender.<>c__DisplayClass6_0.<<SetData>b__0>d<object,object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<EntityRender.<>c__DisplayClass6_0.<<SetData>b__0>d<object,object>>(EntityRender.<>c__DisplayClass6_0.<<SetData>b__0>d<object,object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UIView.UICore.<CountDownDestroy>d__7>(UIView.UICore.<CountDownDestroy>d__7&)
		// string Luban.StringUtil.CollectionToString<object>(System.Collections.Generic.IEnumerable<object>)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string,Newtonsoft.Json.JsonSerializerSettings)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// int System.Linq.Enumerable.Max<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,int>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Select<object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,int>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Iterator<object>.Select<int>(System.Func<object,int>)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// object UnityEngine.Component.GetComponent<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// YooAsset.AllAssetsHandle YooAsset.ResourcePackage.LoadAllAssetsSync<object>(string)
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetAsync<object>(string,uint)
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetSync<object>(string)
		// YooAsset.AssetHandle YooAsset.YooAssets.LoadAssetAsync<object>(string,uint)
		// YooAsset.AssetHandle YooAsset.YooAssets.LoadAssetSync<object>(string)
		// string string.Join<object>(string,System.Collections.Generic.IEnumerable<object>)
		// string string.JoinCore<object>(System.Char*,int,System.Collections.Generic.IEnumerable<object>)
	}
}