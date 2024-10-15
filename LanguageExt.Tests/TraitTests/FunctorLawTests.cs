using LanguageExt.Common;
using Xunit;
namespace LanguageExt.Tests.TraitTests;

public class FunctorLawTests
{
    [Fact]
    public void EffRT()
    {
        var eq = (K<Eff<Unit>, int> vx, K<Eff<Unit>, int> vy) => vx.Run(unit).Equals(vy.Run(unit));
        var ma = Eff<Unit, int>.Pure(1);
        var mx = Eff<Unit, int>.Fail(Errors.Closed);
        var r1 = FunctorLaw<Eff<Unit>>.lawsHold(ma, eq);
        var r2 = FunctorLaw<Eff<Unit>>.lawsHold(mx, eq);
        Assert.True(r1);
        Assert.True(r2);
    }
    
    [Fact]
    public void Eff()
    {
        var eq = (K<Eff, int> vx, K<Eff, int> vy) => vx.Run().Equals(vy.Run());
        var ma = Eff<int>.Pure(1);
        var mx = Eff<int>.Fail(Errors.Closed);
        var r1 = FunctorLaw<Eff>.lawsHold(ma, eq);
        var r2 = FunctorLaw<Eff>.lawsHold(mx, eq);
        Assert.True(r1);
        Assert.True(r2);
    }
    
    [Fact]
    public void IO()
    {
        var eq = (K<IO, int> vx, K<IO, int> vy) => vx.RunSafe().Equals(vy.RunSafe());
        var ma = IO<int>.Pure(1);
        var mx = IO<int>.Fail(Errors.Closed);
        var r1 = FunctorLaw<IO>.lawsHold(ma, eq);
        var r2 = FunctorLaw<IO>.lawsHold(mx, eq);
        Assert.True(r1);
        Assert.True(r2);
    }
    
    [Fact]
    public void Option()
    {
        var fa = Option<int>.Some(1);
        var r  = FunctorLaw<Option>.lawsHold(fa);
        Assert.True(r);
    }
    
}
