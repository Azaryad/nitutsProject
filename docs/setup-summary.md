# nitutsProject — Setup Summary

## 1. GitHub Repository

**SSH remote:** `git@github.com:Azaryad/nitutsProject.git`

### Files created at project root
| File | Purpose |
|------|---------|
| `CLAUDE.md` | Project instructions for Claude Code |
| `README.md` | Public-facing overview |
| `.gitignore` | Ignores `cloned/`, `.mcp.json`, OS/editor/secret files |
| `docs/` | Course notes, PDFs, and write-ups |

### Initial push
```bash
git init
git remote add origin git@github.com:Azaryad/nitutsProject.git
git branch -M main
git push --force -u origin main   # force needed — GitHub had auto-created a README
```

---

## 2. Course Documents (docs/)

Two System Analysis & Design PDFs uploaded to `docs/`:

| File | Description |
|------|-------------|
| `docs/group_17_part_a.pdf` | Part A — org analysis, business processes, requirements (Wholestay / Transfers TLV) |
| `docs/PartB_Group17.pdf` | Part B — Use Cases, Class Diagram, BPMN flows |

---

## 3. Teacher's Sample Project

Cloned as a local reference (not tracked by git):

```bash
git clone https://github.com/dcodish/SAD-sample-project.git cloned
```

`cloned/` is listed in `.gitignore` so it is never committed.

---

## 4. MSSQL MCP Server Setup

### Prerequisites
- Install `uv` (provides `uvx`):
  ```
  winget install astral-sh.uv
  ```
  Restart VS Code / terminal after installing so the PATH updates.

- SQL Server instance on this machine: **`localhost\SQLEXPRESS`**  
  (service name: `MSSQL$SQLEXPRESS`, confirmed running via `Get-Service`)

### Package used
`mssql_mcp_server` by RichardHan — installed automatically by `uvx` on first run.

**Full path to uvx** (winget does not add it to PATH automatically):
```
C:\Users\Dan Azaryad\AppData\Local\Microsoft\WinGet\Packages\astral-sh.uv_Microsoft.Winget.Source_8wekyb3d8bbwe\uvx.exe
```

### `.mcp.json` (project root — gitignored)
```json
{
  "mcpServers": {
    "mssql": {
      "command": "C:\\Users\\Dan Azaryad\\AppData\\Local\\Microsoft\\WinGet\\Packages\\astral-sh.uv_Microsoft.Winget.Source_8wekyb3d8bbwe\\uvx.exe",
      "args": ["mssql_mcp_server"],
      "env": {
        "MSSQL_HOST": "localhost\\SQLEXPRESS",
        "MSSQL_DATABASE": "master",
        "MSSQL_USER": "windows_auth",
        "MSSQL_PASSWORD": "windows_auth",
        "Trusted_Connection": "yes",
        "TrustServerCertificate": "yes"
      }
    }
  }
}
```

> **Why placeholder credentials?**  
> The package always validates that `MSSQL_USER` and `MSSQL_PASSWORD` are non-empty before connecting.  
> With `Trusted_Connection=yes`, pyodbc uses Windows Authentication and ignores the UID/PWD values entirely —  
> so the placeholder strings just satisfy the validation check without being used.

### Important: open VS Code in the right folder
Claude Code reads `.mcp.json` only from the folder it is opened in.  
Always open VS Code at:
```
C:\Users\Dan Azaryad\OneDrive - post.bgu.ac.il\Documents\Claude\Projects\nitutsProject
```

### Verifying the MCP server works (after restart)
After reopening VS Code in nitutsProject, confirm the tools loaded by asking Claude Code:
- `mssql.execute_sql` should be available
- Run a test query: `SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES` against `master`

---

## 5. .gitignore entries added for this setup

```
cloned/       # teacher's sample project — local reference only
.mcp.json     # MCP config contains local paths — not for version control
```
