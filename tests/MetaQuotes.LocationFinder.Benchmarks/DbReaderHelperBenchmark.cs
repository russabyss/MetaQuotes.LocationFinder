using BenchmarkDotNet.Attributes;
using MetaQuotes.LocationFinder.Core.Helpers;

namespace MetaQuotes.LocationFinder.Benchmarks
{
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
