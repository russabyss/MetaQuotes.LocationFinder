using BenchmarkDotNet.Running;
using MetaQuotes.LocationFinder.Benchmarks;

//| Method               | Mean       | Error      | StdDev    |
//| -------------------- | ----------:| ----------:| ---------:|
//| FileReadSimple       | 5.057 ms   | 0.0727 ms  | 0.0645 ms |
//| FileReadWithParsing  | 99.374 ms  | 1.6648 ms  | 1.8504 ms |

var summary = BenchmarkRunner.Run<DbReaderHelperBenchmark>();
