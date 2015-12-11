using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Compiler.UI.AwaitableUI
{
    /// <summary>
    /// Contains an extension method for waiting for Storyboard to complete.
    /// </summary>
    public static class StoryboardExtensions
    {
        /// <summary>
        /// Begins a storyboard and waits for it to complete.
        /// </summary>
        public static async Task BeginAsync(this Storyboard storyboard)
        {
            await EventAsync.FromEvent<object>(
                eh => storyboard.Completed += eh,
                eh => storyboard.Completed -= eh,
                storyboard.Begin);
        }
    }
}
