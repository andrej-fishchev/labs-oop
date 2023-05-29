using labs.builders;
using labs.entities;
using labs.shared.data.abstracts.graph;
using labs.shared.data.algorithms.Graph.linq;
using labs.shared.data.algorithms.Graph.searches;
using labs.shared.data.algorithms.Graph.walks;
using labs.shared.data.algorithms.Graph.walks.linq;
using labs.shared.data.requests.graph.requesters;

namespace labs.lab17;

public class Task1 : LabTask
{
    private static Task1? instance;

    private GraphMatrix? adjacencyMatrix;

    public static Task1 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task1(name, description);

        return instance;
    }

    private Task1(string name, string description) :
        base(name, description)
    {
        adjacencyMatrix = null;
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Ввод матрицы смежности")
                .ExecuteAction(GetAdjacencyMatrix)
                .Build(),
            
            new LabTaskActionBuilder().Name("Ввод матрицы смежности в строку")
                .ExecuteAction(GetAdjacencyMatrixInline)
                .Build(),
            
            new LabTaskActionBuilder().Name("Исключить петли из матрицы")
                .ExecuteAction(ExcludeLoopsFromMatrix)
                .Build(),
            
            new LabTaskActionBuilder().Name("Обход графа в ширину")
                .ExecuteAction(BreadthWalk)
                .Build(),

            new LabTaskActionBuilder().Name("Обход графа в глубину")
                .ExecuteAction(DepthWalk)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод компонент графа")
                .ExecuteAction(OutputGraphComponents)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод клик графа")
                .ExecuteAction(OutputGraphCliques)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод матрицы достижимости")
                .ExecuteAction(GetReachabilityMatrix)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод матрицы смежности")
                .ExecuteAction(OutputAdjacencyMatrix)
                .Build()

        };
    }

    public void OutputGraphComponents()
    {
        if (adjacencyMatrix == null)
        {
            Console.WriteLine("Ожидается ввод матрицы смежностей");
            return;
        }
        
        foreach (var component in adjacencyMatrix.ToGraph().Components())
        {
            for(var i = 0; i < component.Count; i++)
                Console.Write("{0}{1}{2}", 
                    ((i == 0) ? "" : ", "), 
                    component[i].ShortName, 
                    (i + 1 == component.Count) ? "\n" : ""
                );
        }
    }
    
    public void OutputGraphCliques()
    {
        if (adjacencyMatrix == null)
        {
            Console.WriteLine("Ожидается ввод матрицы смежностей");
            return;
        }

        
    }

    public void BreadthWalk() =>
        GraphWalk(adjacencyMatrix, new BreadthFirstWalk());
    
    public void DepthWalk() =>
        GraphWalk(adjacencyMatrix, new DepthFirstWalk());

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

    private void GetAdjacencyMatrixInline()
    {
        if (GraphRequester.GetGraphAdjacencyMatrixInline("...") is not { } buffer)
            return;

        adjacencyMatrix = buffer;
        
        Console.WriteLine("Матрица создана");
    }
    
    private void GetAdjacencyMatrix()
    {
        if (GraphRequester.GetGraphAdjacencyMatrix("...") is not { } buffer)
            return;

        adjacencyMatrix = buffer;
        
        Console.WriteLine("Матрица создана");
    }

    public void GetReachabilityMatrix() =>
        Console.WriteLine("Матрица достижимости: \n{0}", 
            adjacencyMatrix != null
                ? adjacencyMatrix.GetReachabilityMatrix()
                : "Недоступна, т.к. матрица межности не поступала на ввод"
            );

    public void OutputAdjacencyMatrix() => 
        Console.WriteLine("{0}", adjacencyMatrix != null ? adjacencyMatrix : "Ожидается ввод матрицы смежности");
}