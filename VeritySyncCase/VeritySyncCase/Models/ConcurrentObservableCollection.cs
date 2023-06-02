using System.Collections.ObjectModel;

namespace VeritySyncCase.Models
{
    public partial class ConcurrentObservableCollection<T> : ObservableCollection<T>
    {
        public ConcurrentObservableCollection(IEnumerable<T> collection) : base(collection)
        {
        }
        public ConcurrentObservableCollection() : base()
        {
            CollectionSynchronizationCallback callback = CollectionSyncCallBack;

            CheckForUIThread(callback);
        }
        public ConcurrentObservableCollection(CollectionSynchronizationCallback callback)
        {
            CheckForUIThread(callback);
        }

        public new void Clear()
        {
            while (Count > 0)
            {
                RemoveAt(Count - 1);
            }
        }

        protected void CheckForUIThread(CollectionSynchronizationCallback callback)
        {
            if (callback != null)
            {
                BindingBase.EnableCollectionSynchronization(this, new object(), callback);
            }
        }
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static void CollectionSyncCallBack(System.Collections.IEnumerable collection, object context, Action accessMethod, bool writeAccess)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            lock (collection)
            {
                accessMethod?.Invoke();
            }
        }
    }
}
