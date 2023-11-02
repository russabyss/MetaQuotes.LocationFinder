using BenchmarkDotNet.Attributes;
using MetaQuotes.LocationFinder.Core.Helpers;

namespace MetaQuotes.LocationFinder.Benchmarks
{
    /*
    | Method                             | Mean       | Error     | StdDev    | Median     |
    |----------------------------------- |-----------:|----------:|----------:|-----------:|
    | FileReadSimple                     |   5.171 ms | 0.1152 ms | 0.3249 ms |   5.064 ms |
    | FileReadWithParsing                | 106.603 ms | 2.1311 ms | 4.5416 ms | 106.124 ms |
    | FileReadWithParsingIntoSearchIndex |   5.912 ms | 0.1172 ms | 0.2202 ms |   5.842 ms |
    */

    public class DbReaderHelperBenchmark
    {
        private const string FilePath = "Data/geobase.dat";

        /// <summary>
        /// Чтение файла с диска.
        /// </summary>
        [Benchmark]
        public void FileReadSimple()
        {
            _ = DbReaderHelper.ReadFile(FilePath);
        }

        /// <summary>
        /// Чтение файла с диска с парсингом данных.
        /// (медленно).
        /// </summary>
        [Benchmark]
        public void FileReadWithParsing()
        {
            _ = DbReaderHelper.LoadFromFile(FilePath);
        }

        /// <summary>
        /// Чтение файла с диска с парсингом данных в поисковый индекс.
        /// (медленно).
        /// </summary>
        [Benchmark]
        public void FileReadWithParsingIntoSearchIndex()
        {
            _ = DbReaderHelper.CreateSearchIndex(FilePath);
        }
    }
}
