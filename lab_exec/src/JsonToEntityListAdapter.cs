using System.Reflection;
using System.Text.Json.Nodes;
using labs.entities;

namespace lab_exec;

// TODO: refactor this
public static class JsonToEntityListAdapter
{
    public static IList<ILabEntity<string>> Adapt(JsonArray data, Type[] sigs)
    {
        string labSig, taskSig;
        JsonArray tasks;
        
        List<ILabEntity<string>> list = new List<ILabEntity<string>>();

        for (int i = 0; i < data.Count; i++)
        {
            JsonNode? node = data[i];

            if (node == null)
                continue;

            Lab lab = new Lab(
                node["name"]!.GetValue<string>(),
                node["description"]!.GetValue<string>());

            labSig = node["signature"]!.GetValue<string>();
            tasks = node["tasks"]!.AsArray();

            for (int j = 0; j < tasks.Count; j++)
            {
                node = tasks[j];

                if(node == null)
                    continue;

                taskSig = node["signature"]!.GetValue<string>();

                Type? sig = Array.Find(sigs, x => x.FullName!.Equals(labSig + "." + taskSig));
                
                if(sig == null)
                    continue;

                MethodInfo? singletonCallSig = sig.GetMethod(
                    "GetInstance", BindingFlags.Public | BindingFlags.Static);
                
                if(singletonCallSig == null)
                    continue;

                object? instance = singletonCallSig.Invoke(
                    null, new object?[]
                    {
                        node["name"]!.GetValue<string>(),
                        node["description"]!.GetValue<string>()
                    });
                
                if(instance == null)
                    continue;
                
                lab.Tasks.Add((LabTask) instance);
            }
            
            list.Add(lab);
        }

        return list;
    }
}