using FluentAssertions;
using Itmo.Csharp.Microservices.Lab1.Zip;
using Xunit;

namespace Itmo.Csharp.Microservices.Lab1.Tests;

public class ZipTests
{
    [Theory]
    [MemberData(nameof(TestsData.ZipTestCases_NoArguments), MemberType = typeof(TestsData))]
    public void Zip_WithOneCollection_MustReturnCollectionsOfOneElement(
        IEnumerable<int> first,
        IEnumerable<IEnumerable<int>> expected)
    {
        IEnumerable<IEnumerable<int>> actual = first.ZipSync();

        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [MemberData(nameof(TestsData.ZipTestCases_CollectionsSameSize), MemberType = typeof(TestsData))]
    public void Zip_WithMultipleCollectionsWithSameSize_MustReturnValidAnswer(
        IEnumerable<int> first,
        IEnumerable<int> second,
        IEnumerable<int> third,
        IEnumerable<IEnumerable<int>> expected)
    {
        IEnumerable<IEnumerable<int>> actual = first.ZipSync(second, third);

        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [MemberData(nameof(TestsData.ZipTestCases_CollectionsDifferentSize), MemberType = typeof(TestsData))]
    public void Zip_WithMultipleCollectionsWithDifferentSize_MustReturnValidAnswer(
        IEnumerable<int> first,
        IEnumerable<int> second,
        IEnumerable<int> third,
        IEnumerable<IEnumerable<int>> expected)
    {
        IEnumerable<IEnumerable<int>> actual = first.ZipSync(second, third);

        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }
}