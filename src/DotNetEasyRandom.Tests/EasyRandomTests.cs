// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EasyRandomTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="EasyRandom" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetEasyRandom.Tests;

/// <summary>
/// A class to test the <see cref="EasyRandom"/> class.
/// </summary>
[TestClass]
public class EasyRandomTests
{
    /// <summary>
    /// The seed of the class under test. A seed is used everywhere, so that a test that draws many values
    /// always draws the same ones and can never fail on one machine only.
    /// </summary>
    private const int TestSeed = 12;

    /// <summary>
    /// The number of values drawn by the tests that check which values a method can return.
    /// </summary>
    private const int Iterations = 1000;

    /// <summary>
    /// The class under test.
    /// </summary>
    private readonly IEasyRandom easyRandom = new EasyRandom(TestSeed);

    /// <summary>
    /// Checks whether the parameterless <see cref="EasyRandom.Next()"/> only returns values the wrapped
    /// <see cref="Random"/> can return.
    /// </summary>
    [TestMethod]
    public void NextReturnsAValueOfTheWrappedRandom()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var value = this.easyRandom.Next();

            Assert.IsGreaterThanOrEqualTo(0, value);
            Assert.IsLessThan(int.MaxValue, value);
        }
    }

    /// <summary>
    /// Checks whether the maximum value handed to <see cref="EasyRandom.Next(int)"/> is excluded, as the
    /// wrapped <see cref="Random"/> does it.
    /// </summary>
    [TestMethod]
    public void NextWithAMaximumExcludesTheMaximum()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var value = this.easyRandom.Next(5);

            Assert.IsGreaterThanOrEqualTo(0, value);
            Assert.IsLessThan(5, value);
        }
    }

    /// <summary>
    /// Checks whether <see cref="IncludeType.None"/> returns neither of the two borders.
    /// </summary>
    [TestMethod]
    public void NextWithNoneReturnsNeitherBorder()
    {
        var values = new HashSet<int>();

        for (var i = 0; i < Iterations; i++)
        {
            values.Add(this.easyRandom.Next(1, 5, IncludeType.None));
        }

        CollectionAssert.AreEquivalent(new[] { 2, 3, 4 }, values.ToList());
    }

    /// <summary>
    /// Checks whether <see cref="IncludeType.StartOnly"/> returns the minimum value, but not the maximum one.
    /// </summary>
    [TestMethod]
    public void NextWithStartOnlyReturnsTheMinimumOnly()
    {
        var values = new HashSet<int>();

        for (var i = 0; i < Iterations; i++)
        {
            values.Add(this.easyRandom.Next(1, 5, IncludeType.StartOnly));
        }

        CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4 }, values.ToList());
    }

    /// <summary>
    /// Checks whether <see cref="IncludeType.EndOnly"/> returns the maximum value, but not the minimum one.
    /// </summary>
    [TestMethod]
    public void NextWithEndOnlyReturnsTheMaximumOnly()
    {
        var values = new HashSet<int>();

        for (var i = 0; i < Iterations; i++)
        {
            values.Add(this.easyRandom.Next(1, 5, IncludeType.EndOnly));
        }

        CollectionAssert.AreEquivalent(new[] { 2, 3, 4, 5 }, values.ToList());
    }

    /// <summary>
    /// Checks whether <see cref="IncludeType.Both"/> returns both borders.
    /// </summary>
    [TestMethod]
    public void NextWithBothReturnsBothBorders()
    {
        var values = new HashSet<int>();

        for (var i = 0; i < Iterations; i++)
        {
            values.Add(this.easyRandom.Next(1, 5, IncludeType.Both));
        }

        CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4, 5 }, values.ToList());
    }

    /// <summary>
    /// Checks whether neighbouring parameters are rejected for <see cref="IncludeType.None"/>. Both candidates
    /// are excluded borders there, so no value is left. Up to version 1.0.13.0 the maximum value came back.
    /// </summary>
    [TestMethod]
    public void NextWithNoneForNeighbouringParametersThrowsANoValueBetweenParametersException()
    {
        var exception = Assert.ThrowsExactly<NoValueBetweenParametersException>(() => this.easyRandom.Next(1, 2, IncludeType.None));

        StringAssert.Contains(exception.Message, "There is no value between the parameter 1 and the parameter 2");
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether neighbouring parameters are fine for every other include type.
    /// </summary>
    [TestMethod]
    public void NextWithNeighbouringParametersWorksForTheOtherIncludeTypes()
    {
        Assert.AreEqual(1, this.easyRandom.Next(1, 2, IncludeType.StartOnly));
        Assert.AreEqual(2, this.easyRandom.Next(1, 2, IncludeType.EndOnly));
        Assert.IsGreaterThanOrEqualTo(1, this.easyRandom.Next(1, 2, IncludeType.Both));
    }

    /// <summary>
    /// Checks whether an include type that does not exist is rejected. Up to version 1.0.13.0 the
    /// <c>switch</c> had no default arm and returned 0.
    /// </summary>
    [TestMethod]
    public void NextWithAnUndefinedIncludeTypeThrowsAnArgumentOutOfRangeException()
    {
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => this.easyRandom.Next(1, 5, (IncludeType)42));

        Assert.AreEqual("type", exception.ParamName);
    }

    /// <summary>
    /// Checks whether two equal integer parameters are rejected.
    /// </summary>
    [TestMethod]
    public void NextWithEqualParametersThrowsAParametersMustNotBeEqualException()
    {
        var exception = Assert.ThrowsExactly<ParametersMustNotBeEqualException>(() => this.easyRandom.Next(5, 5, IncludeType.Both));

        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether a minimum value greater than the maximum value is rejected, and whether the message says
    /// what the caller has to change. Up to version 1.0.13.0 it stated the reason for the throw as the
    /// requirement.
    /// </summary>
    [TestMethod]
    public void NextWithTheMinimumGreaterThanTheMaximumThrowsAMinMustBeGreaterThanMaxException()
    {
        var exception = Assert.ThrowsExactly<MinMustBeGreaterThanMaxException>(() => this.easyRandom.Next(5, 1, IncludeType.Both));

        Assert.AreEqual("The parameter 5 must be less than the parameter 1", exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether <see cref="int.MaxValue"/> is rejected as a border. The reason is the
    /// <c>maxValue + 1</c> inside the class under test.
    /// </summary>
    [TestMethod]
    public void NextWithTheIntegerMaximumThrowsAnEqualToIntMaxValueException()
    {
        var exception = Assert.ThrowsExactly<EqualToIntMaxValueException>(() => this.easyRandom.Next(1, int.MaxValue, IncludeType.Both));

        StringAssert.Contains(exception.Message, "must not be equal to the int max value");
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether <see cref="int.MinValue"/> is accepted as a border. Only the maximum is checked, the
    /// minimum needs no check.
    /// </summary>
    [TestMethod]
    public void NextWithTheIntegerMinimumReturnsAValue()
    {
        var value = this.easyRandom.Next(int.MinValue, 0, IncludeType.Both);

        Assert.IsGreaterThanOrEqualTo(int.MinValue, value);
        Assert.IsLessThanOrEqualTo(0, value);
    }

    /// <summary>
    /// Checks whether <see cref="EasyRandom.NextDoubleExcludedBoth"/> really excludes both borders. The zero of
    /// the wrapped <see cref="Random.NextDouble"/> would return the minimum value, up to version 1.0.13.0 it
    /// was not drawn again.
    /// </summary>
    [TestMethod]
    public void NextDoubleExcludedBothStaysBetweenTheBorders()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var value = this.easyRandom.NextDoubleExcludedBoth(1.0, 2.0);

            Assert.IsGreaterThan(1.0, value);
            Assert.IsLessThan(2.0, value);
        }
    }

    /// <summary>
    /// Checks whether two equal double parameters are rejected.
    /// </summary>
    [TestMethod]
    public void NextDoubleExcludedBothWithEqualParametersThrowsAParametersMustNotBeEqualException()
    {
        var exception = Assert.ThrowsExactly<ParametersMustNotBeEqualException>(() => this.easyRandom.NextDoubleExcludedBoth(1.0, 1.0));

        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether a minimum value greater than the maximum value is rejected for doubles as well.
    /// </summary>
    [TestMethod]
    public void NextDoubleExcludedBothWithTheMinimumGreaterThanTheMaximumThrowsAMinMustBeGreaterThanMaxException()
    {
        var exception = Assert.ThrowsExactly<MinMustBeGreaterThanMaxException>(() => this.easyRandom.NextDoubleExcludedBoth(2.0, 1.0));

        Assert.AreEqual("The parameter 2 must be less than the parameter 1", exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether <see cref="double.MaxValue"/> is rejected as a border. The exception type says int
    /// although a double was checked, only the message tells the two apart.
    /// </summary>
    [TestMethod]
    public void NextDoubleExcludedBothWithTheDoubleMaximumThrowsAnEqualToIntMaxValueException()
    {
        var exception = Assert.ThrowsExactly<EqualToIntMaxValueException>(() => this.easyRandom.NextDoubleExcludedBoth(1.0, double.MaxValue));

        StringAssert.Contains(exception.Message, "must not be equal to the double max value");
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether <see cref="double.MinValue"/> is rejected as a border, and that this throws the max value
    /// exception too.
    /// </summary>
    [TestMethod]
    public void NextDoubleExcludedBothWithTheDoubleMinimumThrowsAnEqualToIntMaxValueException()
    {
        var exception = Assert.ThrowsExactly<EqualToIntMaxValueException>(() => this.easyRandom.NextDoubleExcludedBoth(double.MinValue, 1.0));

        StringAssert.Contains(exception.Message, "must not be equal to the double min value");
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Checks whether <see cref="EasyRandom.NextDouble"/> forwards to the wrapped <see cref="Random"/>, which
    /// includes the zero and excludes the one.
    /// </summary>
    [TestMethod]
    public void NextDoubleReturnsAValueBetweenZeroAndOne()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var value = this.easyRandom.NextDouble();

            Assert.IsGreaterThanOrEqualTo(0.0, value);
            Assert.IsLessThan(1.0, value);
        }
    }

    /// <summary>
    /// Checks whether <see cref="EasyRandom.NextBytes"/> writes into the buffer it was given.
    /// </summary>
    [TestMethod]
    public void NextBytesFillsTheGivenBuffer()
    {
        var buffer = new byte[16];

        this.easyRandom.NextBytes(buffer);

        Assert.IsTrue(buffer.Any(singleByte => singleByte != 0));
    }

    /// <summary>
    /// Checks whether the constructor taking a seed hands it on, so that two instances with the same seed
    /// return the same values.
    /// </summary>
    [TestMethod]
    public void TheSameSeedReturnsTheSameValues()
    {
        var first = new EasyRandom(TestSeed);
        var second = new EasyRandom(TestSeed);

        for (var i = 0; i < Iterations; i++)
        {
            Assert.AreEqual(first.Next(), second.Next());
            Assert.AreEqual(first.NextDouble(), second.NextDouble());
            Assert.AreEqual(first.Next(1, 5, IncludeType.Both), second.Next(1, 5, IncludeType.Both));
        }
    }
}
