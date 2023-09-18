### EF migration commands

**Note**: run commands in Solution  **Nebim.Era.Plt.Comm.Customer** directory

### Add Migration

> dotnet ef migrations add initial --output-dir Data/Migrations -p src/Infrastructure -s src/Service -c CustomerDbContext

### Update Migration

> dotnet ef database update -p src/Infrastructure -s src/Service -c CustomerDbContext

### Create Sql Script

> dotnet ef migrations script --idempotent -o ci-cd/Database/initial.sql -p src/Infrastructure -s src/Service -c CustomerDbContext
