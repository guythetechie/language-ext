using System.Threading;
using LanguageExt.Attributes;

namespace LanguageExt.Interfaces
{
    /// <summary>
    /// Type-class giving a struct the trait of supporting task cancellation IO
    /// </summary>
    /// <typeparam name="RT">Runtime</typeparam>
    [Typeclass("*")]
    public interface HasCancel<RT>
        where RT : HasCancel<RT> 
    {
        /// <summary>
        /// Directly access the cancellation token
        /// </summary>
        /// <returns>CancellationToken</returns>
        CancellationToken CancellationToken { get; }
        
        /// <summary>
        /// Directly access the cancellation token source
        /// </summary>
        /// <returns>CancellationTokenSource</returns>
        Eff<RT, CancellationTokenSource> CancellationTokenSource { get; }

#if NETSTANDARD21
        /// <summary>
        /// Get the cancellation token
        /// </summary>
        /// <returns>CancellationToken</returns>
        public Eff<RT, CancellationToken> Token =>
            Eff<RT, CancellationToken>.Effect(env => env.CancellationToken);
#else
        /// <summary>
        /// Get the cancellation token
        /// </summary>
        /// <returns></returns>
        Eff<RT, CancellationToken> Token { get; }
#endif
    }
}
