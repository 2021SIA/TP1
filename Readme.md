# Sokoban solver

## Build
Requires .Net 5.0 SDK

```console
dotnet build -c Release
```

The project can also be imported in Visual Studio 2019

## Run

```console
cd TP1\bin\Debug\net5.0\
.\TP1.exe [options]
```
Information about valid arguments can be found with the --help command

```console
.\TP1.exe --help

TP1:
  Sokoban map solver

Usage:
  TP1 [options]

Options:
  --map <map>                                     Map to analize
  --show                                          Print the intermediate states in the solution found
  --strategy <AStar|BFS|DFS|GGS|IDAStar|IDDFS>    Search method to use
  --heuristic <heuristic>                         Heuristic to use in informed search methods. Can be 1, 2 or 3 [default: 1]
  --depth <depth>                                 Initial depth for IDDFS [default: 30]
  --version                                       Show version information
  -?, -h, --help                                  Show help and usage information
```

## Examples

```console
.\TP1.exe --map Maps\soko1.txt --strategy BFS
```
```console
.\TP1.exe --map Maps\8x8.txt --strategy IDAStar --heuristic 2 --depth 100 --show
```

## Map format

This program accepts levels in the standard format presented [here](http://www.sokobano.de/wiki/index.php?title=Level_format#Level)

