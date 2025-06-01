using Azure.AI.Agents.Persistent;
using Azure.AI.Projects;

namespace AgentWorkshop.Client;

public static class Utils
{
    public static void LogGreen(string message) => Console.WriteLine($"\u001b[32m{message}\u001b[0m");
    public static void LogPurple(string message) => Console.WriteLine($"\u001b[35m{message}\u001b[0m");
    public static void LogBlue(string message) => Console.WriteLine($"\u001b[34m{message}\u001b[0m");

    public static async Task<PersistentAgentsVectorStore> CreateVectorStore(AIProjectClient projectClient, List<string> files, string vectorStoreName)
    {
        List<string> fileIds = new();
        string env = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "local";
        string prefix = env == "container" ? "src/workshop/" : "";
        var agentsClient = projectClient.GetPersistentAgentsClient();

        foreach (var file in files)
        {
            string filePath = prefix + file;
            LogPurple($"Uploading file: {filePath}");
            // Adjust the upload API call if needed
            PersistentAgentFileInfo fileInfo = await agentsClient.Files.UploadFileAsync(filePath, PersistentAgentFilePurpose.Agents);
            fileIds.Add(fileInfo.Id);
        }

        LogPurple("Creating the vector store");
        PersistentAgentsVectorStore vectorStore = await agentsClient.VectorStores.CreateVectorStoreAsync(fileIds, vectorStoreName);
        LogPurple("Vector store created and files added.");
        return vectorStore;
    }
}
