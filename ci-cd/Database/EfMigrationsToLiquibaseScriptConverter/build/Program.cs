using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Common.Tools.DotNet;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Path = System.IO.Path;
using Directory = System.IO.Directory;

namespace EfMigrationsToLiquibaseScriptConverter;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string MigrationFolder { get; set; } = "src/Infrastructure/Data/Migrations";
    public string LiquibaseScriptsFolder { get; set; } = "ci-cd/Database/scripts";
    public string DatabaseFolder { get; set; } = "ci-cd/Database";
    public string MigrationProject { get; set; } = "src/Infrastructure";
    public string StartProject { get; set; } = "src/Service";
    public string ContextName { get; set; } = "CustomerDbContext";
    public List<string> Migrations { get; set; } = new();
    public List<string> Scripts { get; set; } = new();
    public DirectoryPath SlnProjectDirectory { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        SlnProjectDirectory = context.Environment.WorkingDirectory.GetParent().GetParent().GetParent().GetParent();
        context.Environment.WorkingDirectory = SlnProjectDirectory;
    }
}

[TaskName("FindMigrations")]
public sealed class FindMigrations : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Migration Search starting ...");

        var pathName = context.SlnProjectDirectory.Combine(context.MigrationFolder).ToString();
        var migrations = Directory.GetFiles(pathName).Select(Path.GetFileName).Where(migration =>
            !migration.EndsWith("Snapshot.cs") && !migration.EndsWith("Designer.cs") && !migration.StartsWith(".editorconfig") &&
            migration[..14].All(chr => chr is <= '9' and >= '0')).ToList();

        context.Migrations = migrations;

        if (migrations.Any())
        {
            context.Log.Information($"{migrations.Count} migrations found");
            foreach (var migration in context.Migrations)
            {
                context.Log.Information(migration);
            }
        }
    }
}

[TaskName("FindLiquibaseSqlScripts")]
[IsDependentOn(typeof(FindMigrations))]
public sealed class FindLiquibaseSqlScripts : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var pathName = context.SlnProjectDirectory.Combine(context.LiquibaseScriptsFolder).ToString();
        var sqlScripts = Directory.GetFiles(pathName).Select(Path.GetFileName)
            .Where(fileName => fileName.StartsWith("changelog")).ToList();
        context.Scripts = sqlScripts;

        if (sqlScripts.Any())
        {
            context.Log.Information($"{sqlScripts.Count} liquibase scripts found");
            foreach (var script in sqlScripts)
            {
                context.Log.Information(script);
            }
        }
    }
}

[TaskName("CreateMigrationScripts")]
[IsDependentOn(typeof(FindLiquibaseSqlScripts))]
public sealed class CreateMigrationScripts : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var scriptUpCommandTemplate =
            $"dotnet ef migrations script FROM TO  -o ci-cd/Database/FOLDER/FILENAME.sql -p {context.MigrationProject} -s {context.StartProject} -c {context.ContextName}  --no-transactions";

        var scriptDownCommandTemplate =
            $"dotnet ef migrations script TO FROM  -o ci-cd/Database/FOLDER/FILENAME.sql -p {context.MigrationProject} -s {context.StartProject} -c {context.ContextName}  --no-transactions";

        var orderedMigrations = context.Migrations.OrderBy(t => t).ToList();
        var liquibaseSqlScripts = context.Scripts.ToList();

        foreach (var migration in orderedMigrations)
        {
            if (!liquibaseSqlScripts.Any(script => script.Contains(migration[..14])))
            {
                var fromMigration = "";
                var toMigration = "";

                //tek migration var veya ilk migration eksik
                if (migration == orderedMigrations.First() || orderedMigrations.Count == 1)
                {
                    fromMigration = "0";
                    toMigration = migration;
                }
                //sonuncu migration eksik - genelde bu case olur . sürekli yeni ekleneceği için
                else if (migration == orderedMigrations.Last())
                {
                    fromMigration = orderedMigrations[^2];
                    toMigration = orderedMigrations.Last();
                }
                //ortalarda bir migration eksik kendisinden bir öncekini bulup revert ve kendisini oluşturmak gerekli
                else
                {
                    toMigration = migration;
                    fromMigration = orderedMigrations[orderedMigrations.IndexOf(migration) - 1];
                }

                var fileName = $"changelog{migration[..14]}";
                var revertFileName = $"revert_changelog{migration[..14]}";
                var folder = $"changelog{migration[..14]}";
                fromMigration = fromMigration.Replace(".cs", "");
                toMigration = toMigration.Replace(".cs", "");

                var scriptUpCommand = scriptUpCommandTemplate
                    .Replace("FROM", fromMigration)
                    .Replace("TO", toMigration)
                    .Replace("FILENAME", fileName)
                    .Replace("FOLDER", folder);

                var scriptDownCommand = scriptDownCommandTemplate
                    .Replace("FROM", fromMigration)
                    .Replace("TO", toMigration)
                    .Replace("FILENAME", revertFileName)
                    .Replace("FOLDER", folder);


                context.Log.Information($"{toMigration} script creating");
                context.DotNetTool(scriptUpCommand);

                context.Log.Information($"{toMigration} revert script creating");
                context.DotNetTool(scriptDownCommand);
            }
        }
    }
}

[TaskName("ConvertLiquibaseScripts")]
[IsDependentOn(typeof(CreateMigrationScripts))]
public sealed class ConvertLiquibaseScripts : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var orderedMigrations = context.Migrations.OrderBy(t => t).ToList();
        var liquibaseSqlScripts = context.Scripts.ToList();

        foreach (var migration in orderedMigrations)
        {
            var mergeFilePathName = context.SlnProjectDirectory.Combine(context.DatabaseFolder)
                .Combine($"changelog{migration[..14]}").ToString();

            var createdFileName = context.SlnProjectDirectory.Combine(context.LiquibaseScriptsFolder)
                .Combine($"changelog{migration[..14]}.sql").ToString();
            if (!liquibaseSqlScripts.Any(script => script.Contains(migration[..14])))
            {
                var stringBuilder = new StringBuilder()
                    .AppendLine("--liquibase formatted sql")
                    .AppendLine($"--changeset author:{migration[..14]} context:{migration[15..^3]}");

                //sıralayalım böylece revert herzaman changelog dan sonra gelecek
                var mergeFiles = Directory.GetFiles(mergeFilePathName).Order().ToList();

                foreach (var mergeFile in mergeFiles)
                {
                    if (Path.GetFileName(mergeFile).StartsWith("revert"))
                    {
                        foreach (var line in File.ReadAllLines(mergeFile))
                        {
                            var createdLine = line.Length < 1
                                ? line
                                : "--rollback " + line;
                            stringBuilder.AppendLine(createdLine);
                        }
                    }
                    else
                    {
                        foreach (var line in File.ReadAllLines(mergeFile))
                        {
                            stringBuilder.AppendLine(line);
                        }
                    }
                }

                stringBuilder.Replace("ALTER DATABASE CHARACTER SET utf8mb4;", "");
                stringBuilder.Replace("CHARACTER SET utf8mb4", "");
                stringBuilder.Replace("CHARACTER SET=utf8mb4", "");
                File.WriteAllText(createdFileName, stringBuilder.ToString());
            }
        }
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(ConvertLiquibaseScripts))]
public class DefaultTask : FrostingTask
{
}
