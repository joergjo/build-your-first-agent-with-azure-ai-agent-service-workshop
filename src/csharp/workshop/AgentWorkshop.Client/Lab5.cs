using Azure.AI.Agents.Persistent;
using Azure.AI.Projects;

namespace AgentWorkshop.Client;

public class Lab5(AIProjectClient client, string modelName) : Lab(client, modelName)
{
    protected override string InstructionsFileName => "code_interpreter_multilingual.txt";

    private PersistentAgentFileInfo? fontFile;

    protected override async Task InitialiseLabAsync(PersistentAgentsClient agentClient)
    {
        fontFile = await agentClient.Files.UploadFileAsync(
            File.OpenRead(Path.Combine(SharedPath, "fonts", "fonts.zip")),
            PersistentAgentFilePurpose.Agents,
            "fonts.zip"
        );
    }

    public override IEnumerable<ToolDefinition> IntialiseLabTools() =>
        [new CodeInterpreterToolDefinition()];

    protected override ToolResources? InitialiseToolResources()
    {
        if (fontFile is null)
        {
            throw new InvalidOperationException("Fonts must be uploaded before creating the tool resources.");
        }

        var codeInterpreterToolResource = new CodeInterpreterToolResource();
        codeInterpreterToolResource.FileIds.Add(fontFile.Id);

        return new ToolResources
        {
            CodeInterpreter = codeInterpreterToolResource
        };
    }

    protected override async Task<string> CreateInstructionsAsync()
    {
        var instructions = await base.CreateInstructionsAsync();

        if (fontFile is not null)
        {
            instructions = instructions.Replace("{font_file_id}", fontFile.Id);
        }

        return instructions;
    }
}
