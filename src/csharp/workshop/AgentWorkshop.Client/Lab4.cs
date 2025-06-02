using Azure.AI.Agents.Persistent;
using Azure.AI.Projects;

namespace AgentWorkshop.Client;

public class Lab4 : Lab
{
    private readonly AIProjectClient _client;
    private readonly string _bingConnectionName;

    public Lab4(AIProjectClient client, string modelName, string bingConnectionName = "groundingwithbingsearch") : base(client, modelName)
    {
        _client = client;
        _bingConnectionName = bingConnectionName;
    }

    protected override string InstructionsFileName => "bing_grounding.txt";

    public override IEnumerable<ToolDefinition> IntialiseLabTools()
    {
        var connection = _client.GetConnectionsClient().Get(_bingConnectionName);
        var configuration = new BingGroundingSearchConfiguration(connection.Id);
        return [new BingGroundingToolDefinition(new BingGroundingSearchToolParameters([configuration]))];
    }
}
