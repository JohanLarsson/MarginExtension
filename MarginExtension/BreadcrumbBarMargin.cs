namespace MarginExtension
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Microsoft.VisualStudio.Text.Editor;

    /// <summary>Margin's canvas and visual definition including both size and content.</summary>
    internal class BreadcrumbBarMargin : BreadcrumbBar, IWpfTextViewMargin
    {
        public const string MarginName = "BreadcrumbBarMargin";

        public static readonly DependencyProperty LeftPaddingProperty = DependencyProperty.RegisterAttached(
            "LeftPadding",
            typeof(Thickness),
            typeof(BreadcrumbBarMargin), 
            new PropertyMetadata(default(Thickness)));

        private bool isDisposed;

        /// <summary>Initializes a new instance of the <see cref="BreadcrumbBarMargin"/> class for a given <paramref name="textView"/>.</summary>
        /// <param name="textView">The <see cref="IWpfTextView"/> to attach the margin to.</param>
        public BreadcrumbBarMargin(IWpfTextView textView, IWpfTextViewMargin container)
        {
            var leftMargin = textView.VisualElement
                .VisualAncestors()
                .OfType<Grid>()
                .FirstOrDefault()
                ?.Children
                .OfType<IWpfTextViewMargin>()
                .FirstOrDefault(m => m.GetType().Name == "LeftMargin");

            if (leftMargin != null)
            {
                var binding = new Binding(nameof(FrameworkElement.ActualWidth))
                {
                    Source = leftMargin.VisualElement,
                    Mode = BindingMode.OneWay,
                    Converter = new ActualWidthToLeftPaddingConverter()
                };

                BindingOperations.SetBinding(this, LeftPaddingProperty, binding);
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.FrameworkElement"/> that implements the visual representation of the margin.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">The margin is disposed.</exception>
        public FrameworkElement VisualElement
        {
            // Since this margin implements Canvas, this is the object which renders
            // the margin.
            get
            {
                this.ThrowIfDisposed();
                return this;
            }
        }

        /// <summary>
        /// Gets the size of the margin.
        /// </summary>
        /// <remarks>
        /// For a horizontal margin this is the height of the margin,
        /// since the width will be determined by the <see cref="ITextView"/>.
        /// For a vertical margin this is the width of the margin,
        /// since the height will be determined by the <see cref="ITextView"/>.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public double MarginSize
        {
            get
            {
                this.ThrowIfDisposed();

                // Since this is a horizontal margin, its width will be bound to the width of the text view.
                // Therefore, its size is its height.
                return this.ActualHeight;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the margin is enabled.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public bool Enabled
        {
            get
            {
                this.ThrowIfDisposed();

                // The margin should always be enabled
                return true;
            }
        }

        public static void SetLeftPadding(DependencyObject element, Thickness value)
        {
            element.SetValue(LeftPaddingProperty, value);
        }

        public static Thickness GetLeftPadding(DependencyObject element)
        {
            return (Thickness)element.GetValue(LeftPaddingProperty);
        }

        /// <summary>
        /// Gets the <see cref="ITextViewMargin"/> with the given <paramref name="marginName"/> or null if no match is found
        /// </summary>
        /// <param name="marginName">The name of the <see cref="ITextViewMargin"/></param>
        /// <returns>The <see cref="ITextViewMargin"/> named <paramref name="marginName"/>, or null if no match is found.</returns>
        /// <remarks>
        /// A margin returns itself if it is passed its own name. If the name does not match and it is a container margin, it
        /// forwards the call to its children. Margin name comparisons are case-insensitive.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="marginName"/> is null.</exception>
        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return string.Equals(marginName, BreadcrumbBarMargin.MarginName, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        /// <summary>
        /// Disposes an instance of <see cref="BreadcrumbBarMargin"/> class.
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                GC.SuppressFinalize(this);
                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Checks and throws <see cref="ObjectDisposedException"/> if the object is disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(MarginName);
            }
        }

        private class ActualWidthToLeftPaddingConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return new Thickness((double)value, 0, 0, 0);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
