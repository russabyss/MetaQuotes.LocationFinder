using BenchmarkDotNet.Running;
using MetaQuotes.LocationFinder.Benchmarks;

/*
| Method                             | Mean       | Error     | StdDev    | Median     |
|----------------------------------- |-----------:|----------:|----------:|-----------:|
| FileReadSimple                     |   5.171 ms | 0.1152 ms | 0.3249 ms |   5.064 ms |
| FileReadWithParsing                | 106.603 ms | 2.1311 ms | 4.5416 ms | 106.124 ms |
| FileReadWithParsingIntoSearchIndex |   5.912 ms | 0.1172 ms | 0.2202 ms |   5.842 ms |
*/

var summary = BenchmarkRunner.Run<DbReaderHelperBenchmark>();
