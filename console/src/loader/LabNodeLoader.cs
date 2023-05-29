using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using labs.entities;

namespace console.loader;

// TODO: temp
public class LabNodeLoader
{
    private JsonObject jsonObject;

    public LabNodeLoader(string path)
    {
        JsonNode? node = JsonNode.Parse(File.OpenRead(path));

        if (node == null)
            throw new ArgumentException("Invalid path value of Lab Node, expected JSON obj");

        jsonObject = node.AsObject();
    }

    public IList<ILabEntity<string>> TryLoadTasksFromMemory(Assembly lib)
    {
        string nodeSignature = jsonObject["signature"]!
            .GetValue<string>();
        
        IList<Type> types = lib.GetTypes().Where(x =>
        {
            var baseType = x.BaseType;
            return baseType is { Name: "LabTask" };
        }).ToList();

        if (types.Count == 0)
            return new List<ILabEntity<string>>();

        string labSignature, taskSignature;

        IList<ILabEntity<string>> labs = new List<ILabEntity<string>>();

        if (!jsonObject.ContainsKey("labs"))
            return labs;

        JsonArray jsonLabs = jsonObject["labs"]!.AsArray();
        
        Lab labBuffer;
        JsonNode? node;
        JsonArray tasks;
        for (int i = 0; i < jsonLabs.Count; i++)
        {
            node = jsonLabs[i];
            
            if(node == null)
                continue;

            labBuffer = new Lab(node["name"]!.GetValue<string>(), 
                node["description"]!.GetValue<string>());
            labSignature = node["signature"]!.GetValue<string>();
            tasks = node["tasks"]!.AsArray();

            for (int j = 0; j < tasks.Count; j++)
            {
                if((node = tasks[j]) == null)
                    continue;

                taskSignature = node["signature"]!.GetValue<string>();

                Type? type = Array.Find(types.ToArray(), x => x.FullName!.Equals(
                    new StringBuilder(nodeSignature)
                        .Append(".")
                        .Append(labSignature)
                        .Append(".")
                        .Append(taskSignature)
                        .ToString()));
                
                if(type == null)
                    continue;

                LabTask? instance = CreateSignatureFromType("GetInstance", type, 
                    node["name"]?.GetValue<string>(), node["description"]?.GetValue<string>());
                
                if(instance == null)
                    continue;
                
                labBuffer.Tasks.Add(instance);
            }

            labs.Add(labBuffer);
        }

        return labs;
    }

    private LabTask? CreateSignatureFromType(string methodName, Type type, params object?[] param)
    {
        MethodInfo? info = type.GetMethod(methodName, 
            BindingFlags.Public | BindingFlags.Static);

        if (info == null)
            return null;

        return (LabTask?) info.Invoke(null, param);
    }
}