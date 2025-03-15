# NextSharp.EC

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19044.5371/21H2/November2021Update)

AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores

.NET SDK 9.0.102

  [Host]     : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

  DefaultJob : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

  Insert

| Method           | Mean     | Error     | StdDev   |
|----------------- |---------:|----------:|---------:|
| Insert_MySQL     | 5.834 ms | 0.4920 ms | 1.451 ms |
| Insert_PogreSQL  | 8.469 ms | 0.9632 ms | 2.840 ms |
| Insert_SQLServer | 8.592 ms |  1.041 ms | 3.070 ms |

  

Update (ChangeTracking)
| Method           | Mean     | Error   | StdDev   |
|----------------- |---------:|--------:|---------:|
| Update_MySQL     | 415.5 ns | 8.15 ns |  9.38 ns |
| Update_PosgreSQL | 466.8 ns | 7.40 ns |  6.92 ns |
| Update_SQLServer | 522.3 ns | 9.88 ns | 22.09 ns |

  

| Method                | Mean     | Error    | StdDev   | Median   |
|---------------------- |---------:|---------:|---------:|---------:|
| GetProduct_MySQL      | 43.92 ns | 1.510 ns | 4.453 ns | 41.77 ns |
| GetProduct_PostgreSQL | 37.29 ns | 0.551 ns | 0.461 ns | 37.28 ns |

------------------------------------------------------------------------

Update_Method

| Method           | Mean     | Error   | StdDev  |
|----------------- |---------:|--------:|--------:|
| Update_MySQL     | 295.1 μs | 5.76 μs | 7.49 μs |
| Update_PosgreSQL | 221.6 μs | 1.91 μs | 1.79 μs |
| Update_SQLServer | 218.6 μs | 2.34 μs | 1.83 μs |


| Method                           | Mean     | Error   | StdDev  |
|--------------------------------- |---------:|--------:|--------:|
| Update_MySQL_ChangeTracking      | 415.5 ns | 8.15 ns | 9.38 ns |
| Update_PostgreSQL_ChangeTracking | 466.8 ns | 7.40 ns | 6.92 ns |
  
