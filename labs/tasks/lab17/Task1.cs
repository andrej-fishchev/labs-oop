using labs.builders;
using labs.entities;
using labs.shared.data.abstracts.graph;
using labs.shared.data.algorithms.Graph.linq;
using labs.shared.data.algorithms.Graph.searches;
using labs.shared.data.algorithms.Graph.sorts;
using labs.shared.data.algorithms.Graph.utils;
using labs.shared.data.algorithms.Graph.walks;
using labs.shared.data.algorithms.Graph.walks.linq;
using labs.shared.data.requests.graph.requesters;
using labs.shared.utils;

namespace labs.tasks.lab17;

public class Task1 : LabTask
{
    private static Task1? instance;

    private GraphMatrix? adjacencyMatrix;

    public static Task1 GetInstance(string name, string description)
    {
        return instance ??= new Task1(name, description);
    }

    private Task1(string name, string description) :
        base(name, description)
    {
        adjacencyMatrix = null;
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Ввод матрицы графа")
                .ExecuteAction(() => GetGraphMatrix(GraphRequester.GetGraphMatrix))
                .Build(),
            
            new LabTaskActionBuilder().Name("Ввод матрицы смежностей")
                .ExecuteAction(() => GetGraphMatrix(GraphRequester.GetGraphBitMatrix))
                .Build(),
            
            new LabTaskActionBuilder().Name("Исключить петли из матрицы")
                .ExecuteAction(ExcludeLoopsFromMatrix)
                .Build(),
            
            new LabTaskActionBuilder().Name("Обход графа в ширину")
                .ExecuteAction(() => GraphWalk(adjacencyMatrix, new BreadthFirstWalk()))
                .Build(),

            new LabTaskActionBuilder().Name("Обход графа в глубину")
                .ExecuteAction(() => GraphWalk(adjacencyMatrix, new DepthFirstWalk()))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод ярусно-параллельной формы")
                .ExecuteAction(TopologicalSort)
                .Build(),
            
            new LabTaskActionBuilder().Name("Алгоритм Дейкстры")
                .ExecuteAction(DijkstraAlgo)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод компонент графа")
                .ExecuteAction(OutputGraphComponents)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод остова наименьшего веса (Краскал)")
                .ExecuteAction(OutputKruskalAlgo)
                .Build(),

            new LabTaskActionBuilder().Name("Вывод матрицы графа")
                .ExecuteAction(() => OutputGraphMatrix(
                    EnumerableItemsPrinter.GraphMatrix, adjacencyMatrix
                ))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод матрицы достижимости")
                .ExecuteAction(() => OutputGraphMatrix(
                    EnumerableItemsPrinter.GraphBitMatrix, adjacencyMatrix?.GetReachabilityMatrix()
                ))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод матрицы смежностей")
                .ExecuteAction(() => OutputGraphMatrix(EnumerableItemsPrinter.GraphBitMatrix, adjacencyMatrix))
                .Build(),
            
            new LabTaskActionBuilder().Name("Линейный вывод матрицы смежностей")
                .ExecuteAction(() => Console.WriteLine(adjacencyMatrix))
                .Build()

        };
    }

    private void GetGraphMatrix(Func<string, GraphMatrix?> func, string terminate = "...") =>
        adjacencyMatrix = RequestGraphMatrix(func, terminate) ?? adjacencyMatrix;

    private void OutputGraphComponents()
    {
        if (adjacencyMatrix == null)
        {
            Console.WriteLine("Ожидается ввод матрицы смежностей");
            return;
        }
        
        foreach (var component in adjacencyMatrix
                     .Search<GraphComponentSearcher, List<GraphVertex>>(new GraphComponentSearcher()))
        {
            for(var i = 0; i < component.Count; i++)
                Console.Write("{0}{1}{2}", 
                    ((i == 0) ? "" : ", "), 
                    component[i].ShortName, 
                    (i + 1 == component.Count) ? "\n" : ""
                );
        }
    }

    private void TopologicalSort()
    {
        if (GraphUtils.IsUnOriented(adjacencyMatrix))
        {
            Console.WriteLine("Ожидался ориентированный граф");
            return;
        }

        List<(int level, GraphVertex vertex)> vertexes =
            adjacencyMatrix!.TopologicalSort().ToList();
 
        if (vertexes.Count != adjacencyMatrix!.VertexCount)
        {
            Console.WriteLine($"Невозможно привести к ярусно-параллельной форме");
            return;
        }
        
        for(int i = 0; i < vertexes.Count; i++)
            Console.Write("\t{0}{1}",
                $"{vertexes[i].level} : {vertexes[i].vertex.ShortName}", 
                (i + 1 == vertexes.Count || (vertexes[i].level < vertexes[i+1].level)) ? "\n" : "");
    }

    private void OutputKruskalAlgo()
    {
        if (GraphUtils.IsUnWeighted(adjacencyMatrix) || !GraphUtils.IsUnOriented(adjacencyMatrix))
        {
            Console.WriteLine("Ожидался неориентированный взвешенный граф");
            return;
        }

        var weight = 0;
        foreach (var edge in adjacencyMatrix!
                     .Search<KruskalАlgorithm, (GraphVertex, GraphVertex, int)>(new KruskalАlgorithm()))
        {
            Console.Write("{0}{1}", weight == 0 ? "" : ", ", 
                (edge.Item1.ShortName, edge.Item2.ShortName, edge.Item3)
            );
            
            weight += edge.Item3;
        }
        
        Console.WriteLine("\nВес: {0}", weight);
    }

    private void DijkstraAlgo()
    {
        if (GraphUtils.IsUnWeighted(adjacencyMatrix) || GraphUtils.IsUnOriented(adjacencyMatrix))
        {
            Console.WriteLine("Ожидался ориентированный взвешенный граф");
            return;
        }
        
        foreach (var edge in adjacencyMatrix!
                     .Search<DijkstraAlgorithm, (GraphVertex, int)>(new DijkstraAlgorithm()))
        {
            Console.Write(" -> {0}", (edge.Item1.ShortName, edge.Item2));
        }
    }

    private static void GraphWalk(GraphMatrix? matrix, IGraphWalkAlgorithm algorithm)
    {
        if (matrix == null)
        {
            Console.WriteLine("Ожидается ввод матрицы смежностей");
            return;
        }
        
        var buffer = matrix.ToGraph();

        GraphVertex? vertexStart;
        if ((vertexStart = GraphRequester.GetVertex(buffer)) == null)
            return;

        var path = buffer.Walk(algorithm, vertexStart.ShortName).ToList();
        
        for(int i = 0; i < path.Count; i++)
            Console.Write("{0}{1}{2}", ((i == 0) ? "" : ", "), path[i].ShortName, (i + 1 == path.Count) ? "\n" : "");
    }
    
    private void ExcludeLoopsFromMatrix()
    {
        adjacencyMatrix = adjacencyMatrix?.RemoveLoops();

        Console.WriteLine("Операция выполнена");
    }

    private static void OutputGraphMatrix(Action<TextWriter, GraphMatrix> func, GraphMatrix? matrix)
    {
        if(matrix == null)
            Console.WriteLine("Ожидается предварительный ввод матрицы графа");
        
        func.Invoke(Console.Out, matrix!);
    }
    
    private static GraphMatrix? RequestGraphMatrix(Func<string, GraphMatrix?> func, string terminate = "...")
    {
        GraphMatrix? result;
        if((result = func.Invoke(terminate)) != null)
            Console.WriteLine("Матрица создана");

        return result;
    }
}