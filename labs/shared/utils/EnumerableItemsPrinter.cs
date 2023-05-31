using System.Text;
using labs.shared.data.abstracts.graph;

namespace labs.shared.utils;

public static class EnumerableItemsPrinter
{
    public static void VerticallyWithNumbers<T>(TextWriter writer, IEnumerator<T> items, string messageOnEmpty)
    {
        int pos = 0;
        while (items.MoveNext())
        {
            writer.WriteLine($"{++pos}. {items.Current} \n");
        }

        if(pos == 0)
            writer.WriteLine(messageOnEmpty);
    }

    public static void GraphBitMatrix(TextWriter buffer, GraphMatrix? graphMatrix)
    {
        if(graphMatrix == null)
            return;
        
        var vertexCount = graphMatrix.VertexCount;
        
        if (vertexCount == 0)
            return;

        var bitContainer = graphMatrix.GetBitMatrixContainer();
        
        var builder = new StringBuilder();
        for (int i = -1; i < vertexCount; i++)
        {
            if (i == -1)
                builder.Append('\t');

            else builder.Append(graphMatrix.GetVertex(i).ShortName).Append('\t');
            
            for (int j = 0; j < vertexCount; j++)
            {
                if (i == -1)
                    builder.Append("|\t").Append(graphMatrix.GetVertex(j).ShortName).Append('\t');

                else
                    builder.Append("|\t").Append(Convert.ToInt32(bitContainer[i][j])).Append('\t');
            }

            builder.Append("|\n");
        }

        buffer.WriteLine(builder);
    }
    
    public static void GraphMatrix(TextWriter buffer, GraphMatrix? graphMatrix)
    {
        if(graphMatrix == null)
            return;
        
        var vertexCount = graphMatrix.VertexCount;
        
        if (vertexCount == 0)
            return;
        
        var builder = new StringBuilder();
        for (int i = -1; i < vertexCount; i++)
        {
            if (i == -1)
                builder.Append('\t');

            else builder.Append(graphMatrix.GetVertex(i).ShortName).Append('\t');
            
            for (int j = 0; j < vertexCount; j++)
            {
                if (i == -1)
                    builder.Append("|\t").Append(graphMatrix.GetVertex(j).ShortName).Append('\t');

                else
                    builder.Append("|\t").Append(graphMatrix.GetRelation(i, j)).Append('\t');
            }

            builder.Append("|\n");
        }

        buffer.WriteLine(builder);
    }
}