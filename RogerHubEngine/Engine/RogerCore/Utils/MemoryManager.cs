using System.Runtime;

namespace RogerHubEngine.Engine.RogerCore.Utils
{
    internal static class MemoryManager
    {
        /// <summary>
        /// Forces a full garbage collection, including LOH compaction.
        /// Call only when you know it's a one-time operation (e.g. closing the NN UI).
        /// </summary>
        public static void ForceGarbageCollection()
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
        }
    }
}
