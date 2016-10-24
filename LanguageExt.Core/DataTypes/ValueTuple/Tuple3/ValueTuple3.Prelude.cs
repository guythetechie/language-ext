using System;
using LanguageExt;
using static LanguageExt.Prelude;
using static LanguageExt.TypeClass;
using System.Diagnostics.Contracts;
using LanguageExt.TypeClasses;
using LanguageExt.ClassInstances;

namespace LanguageExt
{
    public static partial class Prelude
    {
        /// <summary>
        /// Append an extra item to the tuple
        /// </summary>
        [Pure]
        public static ValueTuple<T1, T2, T3, T4> append<T1, T2, T3, T4>(ValueTuple<T1, T2, T3> self, T4 fourth) =>
            ValueTuple.Create(self.Item1, self.Item2, self.Item3, fourth);

        /// <summary>
        /// Semigroup append
        /// </summary>
        [Pure]
        public static ValueTuple<A, B, C> append<SemiA, SemiB, SemiC, A, B, C>(ValueTuple<A, B, C> a, ValueTuple<A, B, C> b)
            where SemiA : struct, Semigroup<A>
            where SemiB : struct, Semigroup<B>
            where SemiC : struct, Semigroup<C> =>
            VTuple(
                default(SemiA).Append(a.Item1, b.Item1),
                default(SemiB).Append(a.Item2, b.Item2),
                default(SemiC).Append(a.Item3, b.Item3));

        /// <summary>
        /// Monoid concat
        /// </summary>
        [Pure]
        public static ValueTuple<A, B, C> concat<MonoidA, MonoidB, MonoidC, A, B, C>(ValueTuple<A, B, C> a, ValueTuple<A, B, C> b)
            where MonoidA : struct, Monoid<A>
            where MonoidB : struct, Monoid<B>
            where MonoidC : struct, Monoid<C> =>
            VTuple(
                mconcat<MonoidA, A>(a.Item1, b.Item1),
                mconcat<MonoidB, B>(a.Item2, b.Item2),
                mconcat<MonoidC, C>(a.Item3, b.Item3));

        /// <summary>
        /// Take the first item
        /// </summary>
        [Pure]
        public static T1 head<T1, T2, T3>(ValueTuple<T1, T2, T3> self) =>
            self.Item1;

        /// <summary>
        /// Take the last item
        /// </summary>
        [Pure]
        public static T3 last<T1, T2, T3>(ValueTuple<T1, T2, T3> self) =>
            self.Item3;

        /// <summary>
        /// Take the second item onwards and build a new tuple
        /// </summary>
        [Pure]
        public static ValueTuple<T2, T3> tail<T1, T2, T3>(ValueTuple<T1, T2, T3> self) =>
            VTuple(self.Item2, self.Item3);

        /// <summary>
        /// Sum of the items
        /// </summary>
        [Pure]
        public static A sum<NUM, A>(ValueTuple<A, A, A> self)
            where NUM : struct, Num<A> =>
            sum<NUM, FoldTuple<A>, ValueTuple<A, A, A>, A>(self);

        /// <summary>
        /// Product of the items
        /// </summary>
        [Pure]
        public static A product<NUM, A>(ValueTuple<A, A, A> self)
            where NUM : struct, Num<A> =>
            product<NUM, FoldTuple<A>, ValueTuple<A, A, A>, A>(self);

        /// <summary>
        /// One of the items matches the value passed
        /// </summary>
        [Pure]
        public static bool contains<EQ, A>(ValueTuple<A, A, A> self, A value)
            where EQ : struct, Eq<A> =>
            contains<EQ, FoldTuple<A>, ValueTuple<A, A, A>, A>(self, value);

        /// <summary>
        /// Map to R
        /// </summary>
        [Pure]
        public static R map<T1, T2, T3, R>(ValueTuple<T1, T2, T3> self, Func<T1, T2, T3, R> map) =>
            map(self.Item1, self.Item2, self.Item3);

        /// <summary>
        /// Map to tuple
        /// </summary>
        [Pure]
        public static ValueTuple<R1, R2, R3> map<T1, T2, T3, R1, R2, R3>(ValueTuple<T1, T2, T3> self, Func<ValueTuple<T1, T2, T3>, ValueTuple<R1, R2, R3>> map) =>
            map(self);

        /// <summary>
        /// Tri-map to tuple
        /// </summary>
        [Pure]
        public static ValueTuple<R1, R2, R3> trimap<T1, T2, T3, R1, R2, R3>(ValueTuple<T1, T2, T3> self, Func<T1, R1> firstMap, Func<T2, R2> secondMap, Func<T3, R3> thirdMap) =>
            ValueTuple.Create(firstMap(self.Item1), secondMap(self.Item2), thirdMap(self.Item3));

        /// <summary>
        /// First item-map to tuple
        /// </summary>
        [Pure]
        public static ValueTuple<R1, T2, T3> mapFirst<T1, T2, T3, R1>(ValueTuple<T1, T2, T3> self, Func<T1, R1> firstMap) =>
            ValueTuple.Create(firstMap(self.Item1), self.Item2, self.Item3);

        /// <summary>
        /// Second item-map to tuple
        /// </summary>
        [Pure]
        public static ValueTuple<T1, R2, T3> mapSecond<T1, T2, T3, R2>(ValueTuple<T1, T2, T3> self, Func<T2, R2> secondMap) =>
            ValueTuple.Create(self.Item1, secondMap(self.Item2), self.Item3);

        /// <summary>
        /// Second item-map to tuple
        /// </summary>
        [Pure]
        public static ValueTuple<T1, T2, R3> mapThird<T1, T2, T3, R3>(ValueTuple<T1, T2, T3> self, Func<T3, R3> thirdMap) =>
            ValueTuple.Create(self.Item1, self.Item2, thirdMap(self.Item3));

        /// <summary>
        /// Iterate
        /// </summary>
        public static Unit iter<T1, T2, T3>(ValueTuple<T1, T2, T3> self, Action<T1, T2, T3> func)
        {
            func(self.Item1, self.Item2, self.Item3);
            return Unit.Default;
        }

        /// <summary>
        /// Iterate
        /// </summary>
        public static Unit iter<T1, T2, T3>(ValueTuple<T1, T2, T3> self, Action<T1> first, Action<T2> second, Action<T3> third)
        {
            first(self.Item1);
            second(self.Item2);
            third(self.Item3);
            return Unit.Default;
        }

        /// <summary>
        /// Fold
        /// </summary>
        [Pure]
        public static S fold<T1, T2, T3, S>(ValueTuple<T1, T2, T3> self, S state, Func<S, T1, T2, T3, S> fold) =>
            fold(state, self.Item1, self.Item2, self.Item3);

        /// <summary>
        /// Tri-fold
        /// </summary>
        [Pure]
        public static S trifold<T1, T2, T3, S>(ValueTuple<T1, T2, T3> self, S state, Func<S, T1, S> firstFold, Func<S, T2, S> secondFold, Func<S, T3, S> thirdFold) =>
            thirdFold(secondFold(firstFold(state, self.Item1), self.Item2), self.Item3);

        /// <summary>
        /// Tri-fold
        /// </summary>
        [Pure]
        public static S trifoldBack<T1, T2, T3, S>(ValueTuple<T1, T2, T3> self, S state, Func<S, T3, S> firstFold, Func<S, T2, S> secondFold, Func<S, T1, S> thirdFold) =>
            thirdFold(secondFold(firstFold(state, self.Item3), self.Item2), self.Item1);
    }
}