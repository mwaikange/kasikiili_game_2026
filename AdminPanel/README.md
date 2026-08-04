# Kasikili admin panel recovery

This directory revives the latest admin source found in the shared Google Drive
folder: `Dhruv Source Code-20230730T160946Z-001.zip`, modified 30 July 2023.

## Projects

- `web`: the recovered React 18 admin UI, migrated from Create React App to Vite
  so it builds reliably on a current Node.js runtime. Screens, styles, routes and
  business calls remain the recovered implementation.
- `backend`: the matching recovered Express, Knex and MySQL API.

The API base path intentionally remains `/kaslkili` because that spelling is part
of the recovered client/server contract.

## Run the admin UI

```powershell
cd AdminPanel\web
npm install
npm run dev
```

The frontend defaults to `http://localhost:3008/kaslkili`. Override it when needed:

```powershell
$env:VITE_API_URL='https://example.com/kaslkili'
npm run build
```

## Run the API

```powershell
cd AdminPanel\backend
Copy-Item .env.example .env
npm install
npm start
```

Fill the MySQL, JWT and rotated SMS credentials in `.env` before using protected
or database-backed endpoints. Never commit `.env`.

## Current verification

- Frontend production build: passed with Vite 5.4.14 (528 modules transformed).
- Frontend preview: HTTP 200 and app root present.
- Backend syntax/startup: passed.
- Backend root health response: HTTP 200, `Welcome to the KASLKILI`.
- Database-backed routes: pending because no MySQL schema/dump was included in
  the recovered admin folder.

## Security recovery notes

- Hardcoded historical SMS Portal credentials were removed from source and
  replaced with environment variables. The exposed credentials must be rotated.
- The recovered backend uses SHA-256 password hashes and a reversible Base64
  payload wrapper. These are historical behaviors to replace during the database
  and authentication rebuild, not production security targets.
- CORS is currently open (`*`) as in the recovered backend. Restrict it before
  deployment.
