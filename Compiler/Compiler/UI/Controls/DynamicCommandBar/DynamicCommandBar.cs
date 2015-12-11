using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Compiler.UI.Controls
{
    /// <summary>
    /// We extend the CommandBar by adding logic to reflow commands as space is reduced
    /// </summary>
    public class DynamicCommandBar : CommandBar
    {
        public double ContentMinWidth
        {
            get { return (double)GetValue(ContentMinWidthProperty); }
            set { SetValue(ContentMinWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentMinWidthProperty = DependencyProperty.Register("ContentMinWidth", typeof(double), typeof(DynamicCommandBar), new PropertyMetadata(0.0d));

        private Button _moreButton;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _moreButton = GetTemplateChild("MoreButton") as Button;
        }

        // Store the item and the width before moving
        private readonly Stack<Tuple<ICommandBarElement, double>> _overflow = new Stack<Tuple<ICommandBarElement, double>>();
        private readonly Queue<Tuple<ICommandBarElement, double>> _separatorQueue = new Queue<Tuple<ICommandBarElement, double>>();
        private double _separatorQueueWidth;

        protected override Size MeasureOverride(Size availableSize)
        {
            var sizeToReport = base.MeasureOverride(availableSize);

            // Account for the size of the Content area
            var contentWidth = ContentMinWidth;

            // Account for the size of the More button 
            var expandButtonWidth = 0.0;
            if (_moreButton != null && _moreButton.Visibility == Visibility.Visible)
            {
                expandButtonWidth = _moreButton.DesiredSize.Width;
            }

            // Include the size of all the PrimaryCommands
            double requestedWidth = expandButtonWidth + contentWidth + PrimaryCommands.Cast<UIElement>().Sum(uie => uie.DesiredSize.Width);

            // First, move items to the _overflow until the remaining PrimaryCommands fit
            for (int i = PrimaryCommands.Count - 1; i >= 0 && requestedWidth > availableSize.Width; i--)
            {
                var item = PrimaryCommands[i];
                var element = item as UIElement;

                if (element == null)
                {
                    continue;
                }

                requestedWidth -= element.DesiredSize.Width;

                if (_overflow.Count == 0 && SecondaryCommands.Count > 0 && !(element is AppBarSeparator))
                {
                    // Insert a separator to differentiate between the items that were already in the _overflow versus
                    // those we moved
                    var abs = new AppBarSeparator();
                    abs.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    _separatorQueue.Enqueue(new Tuple<ICommandBarElement, double>(abs, abs.DesiredSize.Width));
                    _separatorQueueWidth += abs.DesiredSize.Width;
                }

                PrimaryCommands.RemoveAt(i);

                // move separators we've queued up 
                while (_separatorQueue.Count > 0)
                {
                    var next = _separatorQueue.Dequeue();
                    _separatorQueueWidth -= next.Item2;
                    _overflow.Push(next);
                    SecondaryCommands.Insert(0, next.Item1);
                }

                // We store the measured size before it moves to _overflow and will rely on that value
                // when determining how many items to move back out the _overflow.
                _overflow.Push(new Tuple<ICommandBarElement, double>(item, element.DesiredSize.Width));

                SecondaryCommands.Insert(0, (ICommandBarElement)element);

                // if a separator was adjacent to the one we removed then move the separator to our holding queue so that it doesn't appear without actually separating the content
                var last = PrimaryCommands.LastOrDefault() as AppBarSeparator;
                if (last != null)
                {
                    PrimaryCommands.RemoveAt(PrimaryCommands.Count - 1);
                    _separatorQueue.Enqueue(new Tuple<ICommandBarElement, double>(last, last.DesiredSize.Width));
                    _separatorQueueWidth += last.DesiredSize.Width;
                    requestedWidth -= last.DesiredSize.Width;
                }
            }

            // Next move items out of the _overflow if room is available
            while (_overflow.Count > 0 && requestedWidth + _separatorQueueWidth + _overflow.Peek().Item2 <= availableSize.Width)
            {
                var t = _overflow.Pop();

                SecondaryCommands.RemoveAt(0);

                if (_overflow.Count == 1 && SecondaryCommands.Count > 1)
                {
                    // must be the separator injected earlier
                    _overflow.Pop();
                    SecondaryCommands.RemoveAt(0);
                }

                if (t.Item1 is AppBarSeparator)
                {
                    _separatorQueue.Enqueue(t);
                    _separatorQueueWidth += t.Item2;
                    continue;
                }
                else
                {
                    while (_separatorQueue.Count > 0)
                    {
                        var next = _separatorQueue.Dequeue();
                        PrimaryCommands.Add(next.Item1);
                        _separatorQueueWidth -= next.Item2;
                        requestedWidth += next.Item2;
                    }
                }

                // Sometimes this property is being set to disabled
                ((Control)t.Item1).IsEnabled = true;

                PrimaryCommands.Add(t.Item1);
                requestedWidth += t.Item2;

                // check to see if after moving this item we leave a separator at the top of the _overflow
                if (_overflow.Count > 0)
                {
                    var test = _overflow.Peek();
                    if (test.Item1 is AppBarSeparator)
                    {
                        // we won't leave an orphaned separator
                        SecondaryCommands.RemoveAt(0);
                        var top = _overflow.Pop();

                        var element = top.Item1 as UIElement;
                        if (element != null)
                        {
                            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            _separatorQueue.Enqueue(new Tuple<ICommandBarElement, double>(top.Item1, element.DesiredSize.Width));
                            _separatorQueueWidth += element.DesiredSize.Width;
                        }
                    }
                }
            }

            return sizeToReport;
        }
    }
}
