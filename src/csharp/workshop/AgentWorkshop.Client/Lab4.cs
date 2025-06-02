using Azure.AI.Agents.Persistent;
using Azure.AI.Projects;

namespace AgentWorkshop.Client;

public class Lab4(AIProjectClient client, string modelName, string bingConnectionName = "groundingwithbingsearch") : Lab(client, modelName)
{
    protected override string InstructionsFileName => "bing_grounding.txt";

    public override IEnumerable<ToolDefinition> IntialiseLabTools()
    {
        var connection = client.GetConnectionsClient().Get(bingConnectionName);
        var configuration = new BingGroundingSearchConfiguration(connection.Id);
        return [new BingGroundingToolDefinition(new BingGroundingSearchToolParameters([configuration]))];
    }
}
