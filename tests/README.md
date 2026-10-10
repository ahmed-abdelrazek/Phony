Run the regression suite on Windows with the .NET 10 SDK:

```powershell
dotnet test tests/Phony.Tests/Phony.Tests.csproj
```

Database-backed tests create isolated temporary databases and restore the previous database
connection and report paper size after each test. Test parallelization is disabled because
the application shares static state and a WPF dispatcher.

Coverage includes transactional sale/return rollback, stock checks, debt and cash refunds,
report totals, encrypted backups, inactive accounts, invoice selection state, and dashboard
polling lifecycle. Each view model has a test class; record-management view models have
separate add, edit, and delete command validation tests. These check command availability
and selection state rather than executing every dialog-driven CRUD operation. Additional
tests cover data loading, navigation, settings, themes, and database fixture initialization.

Full returns restore the quantities and service balances consumed by the invoice, cancel its
original unpaid amount, and refund only the cash retained after giving change. An invoice
can be returned once. Restoring a backup validates it first and, if a working database
already exists, preserves that database beside the working file with a
`.before-restore-...bak` suffix.
