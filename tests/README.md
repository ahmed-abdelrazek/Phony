Run the regression suite on Windows with the .NET 10 SDK:

```powershell
dotnet test tests/Phony.Tests/Phony.Tests.csproj
```

The tests create isolated temporary databases. They cover transactional sale/return rollback,
stock checks, debt and cash refunds, report totals, encrypted backups, inactive accounts,
invoice selection state, and dashboard polling lifecycle.

Full returns restore the quantities and service balances consumed by the invoice, cancel its
original unpaid amount, and refund only the cash retained after giving change. An invoice
can be returned once. Restoring a backup validates it first and preserves the previous
database beside the working file with a `.before-restore-...bak` suffix.
