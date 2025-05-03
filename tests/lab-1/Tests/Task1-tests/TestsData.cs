namespace Itmo.Csharp.Microservices.Lab1.Tests.Task1_tests;

#pragma warning disable SA1413
public static class TestsData
{
    private static readonly int[] FirstDataSetFirstTestFirstRow = new[] { 1 };
    private static readonly int[] FirstDataSetFirstTestSecondRow = new[] { 2 };
    private static readonly int[] FirstDataSetFirstTestThirdRow = new[] { 3 };

    public static IEnumerable<object[]> ZipTestCases_NoArguments()
    {
        yield return new object[]
        {
            new[] { 1, 2, 3 },
            new List<IEnumerable<int>>
            {
                FirstDataSetFirstTestFirstRow,
                FirstDataSetFirstTestSecondRow,
                FirstDataSetFirstTestThirdRow,
            },
        };
    }

    private static readonly int[] SecondDataSetFirstTestFirstRow = new[] { 1, 3, 6 };
    private static readonly int[] SecondDataSetFirstTestSecondRow = new[] { 2, 4, 7 };

    private static readonly int[] SecondDataSetSecondTestFirstRow = new[] { 1, 4, 7 };
    private static readonly int[] SecondDataSetThirdTestSecondRow = new[] { 2, 5, 8 };
    private static readonly int[] SecondDataSetThirdTestThirdRow = new[] { 3, 6, 9 };

    public static IEnumerable<object[]> ZipTestCases_CollectionsSameSize()
    {
        yield return new object[]
        {
            new[] { 1, 2 },
            new[] { 3, 4 },
            new[] { 6, 7 },
            new List<IEnumerable<int>>
            {
                SecondDataSetFirstTestFirstRow,
                SecondDataSetFirstTestSecondRow,
            },
        };

        yield return new object[]
        {
            new[] { 1, 2, 3 },
            new[] { 4, 5, 6 },
            new[] { 7, 8, 9 },

            new List<IEnumerable<int>>
            {
                SecondDataSetSecondTestFirstRow,
                SecondDataSetThirdTestSecondRow,
                SecondDataSetThirdTestThirdRow
            }
        };
    }

    private static readonly int[] ThirdDataSetFirstTestFirstRow = new[] { 1, 3, 6 };
    private static readonly int[] ThirdDataSetFirstTestSecondRow = new[] { 2, 4, 7 };

    private static readonly int[] ThirdDataSetSecondTestFirstRow = new[] { 1, 4, 7 };
    private static readonly int[] ThirdDataSetThirdTestSecondRow = new[] { 2, 5, 8 };

    public static IEnumerable<object[]> ZipTestCases_CollectionsDifferentSize()
    {
        yield return new object[]
        {
            new[] { 1, 2 },
            new[] { 3, 4, 5 },
            new[] { 6, 7, 8, 9 },
            new List<IEnumerable<int>>
            {
                ThirdDataSetFirstTestFirstRow,
                ThirdDataSetFirstTestSecondRow
            }
        };

        yield return new object[]
        {
            new[] { 1, 2, 3 },
            new[] { 4, 5, 6 },
            new[] { 7, 8 },
            new List<IEnumerable<int>>
            {
                ThirdDataSetSecondTestFirstRow,
                ThirdDataSetThirdTestSecondRow
            }
        };
    }
}