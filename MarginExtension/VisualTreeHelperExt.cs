namespace MarginExtension
{
    using System.Collections.Generic;
    using System.Windows.Media;

    internal static class VisualTreeHelperExt
    {
        internal static IEnumerable<Visual> VisualAncestors(this Visual child)
        {
            var parent = VisualTreeHelper.GetParent(child) as Visual;
            while (parent != null)
            {
                yield return parent;
                child = parent;
                parent = VisualTreeHelper.GetParent(child) as Visual;
            }
        }
    }
}