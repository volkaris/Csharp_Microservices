namespace Itmo.Csharp.Microservices.Lab1.Zip;

public static class ZipExtensions
{
    public static IEnumerable<IEnumerable<T>> ZipSync<T>(this IEnumerable<T> first, params IEnumerable<T>[] others)
    {
        var enumerators = new List<IEnumerator<T>> { first.GetEnumerator() };
        enumerators.AddRange(others.Select(iEnumerable => iEnumerable.GetEnumerator()));

        try
        {
            while (enumerators.All(enumerator => enumerator.MoveNext()))
                yield return enumerators.Select(enumerator => enumerator.Current).ToArray();
        }
        finally
        {
            foreach (IEnumerator<T> enumerator in enumerators) enumerator.Dispose();
        }
    }

    public static async IAsyncEnumerable<IEnumerable<T>> ZipAsync<T>(
        this IAsyncEnumerable<T> first,
        params IAsyncEnumerable<T>[] others)
    {
        var enumerators = new List<IAsyncEnumerator<T>> { first.GetAsyncEnumerator() };

        enumerators.AddRange(others.Select(iEnumerable => iEnumerable.GetAsyncEnumerator()));

        try
        {
            while (true)
            {
                Task<bool>[] moveTasks =
                    enumerators.Select(enumerator => enumerator.MoveNextAsync().AsTask()).ToArray();

                bool[] moveResult = await Task.WhenAll(moveTasks).ConfigureAwait(false);

                if (moveResult.Any(successfullyMoved => !successfullyMoved)) yield break;
                yield return enumerators.Select(enumerator => enumerator.Current).ToArray();
            }
        }
        finally
        {
            foreach (IAsyncEnumerator<T> enumerator in enumerators)
                await enumerator.DisposeAsync().ConfigureAwait(false);
        }
    }
}