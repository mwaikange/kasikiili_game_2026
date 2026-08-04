# Kasikili bypass register and backend restoration plan

**Document status:** Living recovery document

**Last updated:** 2026-08-04

**Current recovery branch:** `recovery/kasikili-v2`

**Current audited APK:** `Kasikili-V2.0.1-WIN-FIX.apk`
**APK SHA-256:** `B81B8C7D172C9285918BEEC708E1C56ED2A9434CEFFECD187A46AD10AC72E66B`

This is the canonical register of every server dependency bypassed while reviving
the original Kasikili Unity APK. Update it whenever a bypass is added, changed or
replaced by the new backend.

## Status meanings

- **Bypassed:** the original backend behavior is not currently used.
- **Local fixture:** hard-coded or in-memory test data replaces the server.
- **Preserved:** original Unity behavior remains active.
- **Restore before production:** required before real users, credits or payouts.

## Explicit bypass register

### BYP-001 — Login authentication

- **Original behavior:** Mobile number and password were posted to the backend.
  The returned user ID and access token identified the player and authorized later
  wallet, history, distributor and cashout calls.
- **Why it existed:** To prove who the player was and prevent one player from
  accessing another player's wallet or account.
- **Current bypass:** SIGN IN ignores both fields, creates a local test user and
  loads the configured Unity Game scene directly.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Validate credentials, return a short-lived access token
  plus refresh token, store them securely and load the game only after success.
- **Production risk if retained:** Anyone can enter the app as the same user.

### BYP-002 — Logout request and token invalidation

- **Original behavior:** Logout informed the server to invalidate the session,
  then emitted `OnLogoutSuccess` and returned to the login scene.
- **Why it existed:** To terminate stolen or reused sessions and cleanly end an
  authenticated account session.
- **Current bypass:** SIGN OUT loads the Menu/login scene directly.
- **Current status:** Bypassed.
- **Restore with backend:** Revoke the refresh token, clear local credentials and
  return to login even if the network logout request fails.
- **Production risk if retained:** A real token would remain valid after sign-out.

### BYP-003 — Signup and mobile-number uniqueness

- **Original behavior:** The server checked whether a mobile number already
  existed and created the account after the registration flow.
- **Why it existed:** To prevent duplicate accounts and bind a wallet to a unique
  verified mobile number.
- **Current bypass:** Check-mobile and signup calls return local success.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Enforce normalized E.164 mobile uniqueness, region
  validation, adult consent and server-side account creation.
- **Production risk if retained:** Duplicate/fake identities and wallets.

### BYP-004 — OTP send, registration OTP and OTP verification

- **Original behavior:** The backend generated an OTP, sent it by SMS and verified
  it before signup or password recovery could continue.
- **Why it existed:** To prove control of the registered mobile number.
- **Current bypass:** Send, register and verify OTP calls return local success; no
  SMS is sent and no code is genuinely verified.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Use expiring, rate-limited, one-time OTP challenges with
  attempt limits and an SMS provider.
- **Production risk if retained:** Account takeover and fake registrations.

### BYP-005 — Password change/reset

- **Original behavior:** A verified recovery flow changed the password on the
  server and returned the player to login.
- **Why it existed:** To securely recover account access without exposing or
  locally storing the password.
- **Current bypass:** Change-password returns local success and does not persist a
  real password.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Require a valid OTP challenge, hash the new password
  server-side and revoke existing sessions.
- **Production risk if retained:** No meaningful password security or persistence.

### BYP-006 — Startup/server health check

- **Original behavior:** `TestConnectionCmd` called the backend before allowing
  network-dependent flows to continue.
- **Why it existed:** To avoid entering operations that could not complete and to
  show a controlled network error.
- **Current bypass:** Startup connection reports immediate local success.
- **Current status:** Bypassed.
- **Restore with backend:** Use a short-timeout readiness endpoint, retry policy and
  an explicit offline/unavailable state.
- **Production risk if retained:** The client cannot distinguish a healthy backend
  from an outage.

### BYP-007 — Round-start connection authorization

- **Original behavior:** START performed another connection check before loading
  balance/pool data and executing the round.
- **Why it existed:** To prevent a client from beginning a monetary round that the
  server could not authorize or settle.
- **Current bypass:** `TestGameConnectionCmd` emits success immediately.
- **Current status:** Bypassed.
- **Restore with backend:** Replace it with a transactional `create round` request
  that validates account, balance, limits and idempotency before spinning.
- **Production risk if retained:** Rounds can start without server authorization.

### BYP-008 — Opening player balance

- **Original behavior:** `GetPlayerBalance` fetched the server's authoritative
  spendable credit for the logged-in user.
- **Why it existed:** The server, not the device, must own real-money balances.
- **Current bypass:** The local wallet starts at N$500; the game enables
  `gameActive` and `tableActive` only when local credit is greater than zero.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Fetch a versioned wallet snapshot from a transactional
  ledger and never trust a client-supplied balance.
- **Production risk if retained:** The APK can create or edit money locally.

### BYP-009 — Total-user balance/tub lookup

- **Original behavior:** Round start requested total available user balance and
  cached it in `currentTub`.
- **Why it existed:** It appears to have supplied a pool/exposure value used by the
  original game economics and probability controls.
- **Current bypass:** Returns a local `total_available` value of 500.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Define the exact pool/exposure rule, calculate it on the
  server and return it as part of round authorization.
- **Production risk if retained:** Jackpot and exposure limits are not meaningful.

### BYP-010 — Probability multiplier and auto-jackpot API

- **Original behavior:** The API returned a label, `rwm` multiplier and
  `autoJackpot` limit. On a winning number, `rwm` controlled the payout and drove
  probability/jackpot lights and sounds.
- **Why it existed:** To control payout exposure and jackpot behavior centrally.
- **Current bypass:** The winning branch assigns label `OFFLINE` and multiplier 10
  directly, without entering the retired API manager/callback boundary. It then
  runs the original probability, jackpot, win and reset event sequence. A high
  N$1,000,000 test limit remains configured.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** The server must create and sign/record the complete
  round result and multiplier before settlement. The client should only animate it.
- **Production risk if retained:** A modified APK can choose its own payout.

### BYP-011 — Balance update after win/loss

- **Original behavior:** `PostBalance` sent the calculated new amount to the old
  `/updateuserbalance` endpoint.
- **Why it existed:** To persist a round's financial result beyond the device.
- **Current bypass:** Settlement applies `post-bet credit + winnings` directly to
  the in-memory wallet. Losing stakes were already deducted during selection.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Submit only a round ID; the server must calculate and
  atomically post debit/credit ledger entries exactly once.
- **Production risk if retained:** Client-controlled wins, replay and balance fraud.

### BYP-012 — Authoritative balance reconciliation during reset

- **Original behavior:** After win/loss animation, reset waited three seconds,
  fetched the latest server balance, cleared selections and re-enabled play.
- **Why it existed:** To reconcile the display with the authoritative settled
  wallet and prevent another round before settlement completed.
- **Current bypass:** Reset preserves the locally settled balance and immediately
  runs the original reset sequence. It clears bet values/lights and restores the
  guarded ready state according to `RouletteState`.
- **Current status:** Backend fetch bypassed; Unity reset lifecycle preserved.
- **Restore with backend:** Wait for an idempotent settlement receipt containing
  the new balance, then clear the table and unlock the next round.
- **Production risk if retained:** No authoritative reconciliation or dispute trail.

### BYP-013 — Leaderboard data

- **Original behavior:** The backend returned monthly candidates, player rank,
  score and referral totals.
- **Why it existed:** Rankings require a trusted shared dataset across all users.
- **Current bypass:** The existing leaderboard opens synchronously with a local Top
  10 fixture and local player position.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Calculate rankings server-side from authenticated,
  auditable activity and return privacy-masked mobile numbers.
- **Production risk if retained:** Rankings and prizes are fictional.

### BYP-014 — Prize-distribution data

- **Original behavior:** The server supplied prize amounts for leaderboard places.
- **Why it existed:** Prize values can change without releasing a new APK and must
  match the operator's funded promotion.
- **Current bypass:** Local prize JSON supplies the displayed values.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Version and publish prize rules with effective dates and
  retain an audit record for each competition period.
- **Production risk if retained:** Displayed prizes may not match real obligations.

### BYP-015 — Cashout submission

- **Original behavior:** Cashout posted an authenticated payout request and awaited
  processing by the operator/distributor workflow.
- **Why it existed:** Real payouts need validation, limits, approval, fraud controls
  and an auditable status lifecycle.
- **Current bypass:** Amounts from N$100 up to the local balance are deducted from
  the test wallet and the existing success UI is shown. No money is transferred.
- **Current status:** Bypassed; local simulation.
- **Restore with backend:** Create an idempotent payout request, reserve funds,
  process it through an audited provider/admin workflow and expose final status.
- **Production risk if retained:** The app can claim success without making payment.

### BYP-016 — Awaiting cashout and cashout history

- **Original behavior:** The backend returned pending payout total and transaction
  history for the authenticated user.
- **Why it existed:** Players need transparent payout tracking and support evidence.
- **Current bypass:** Awaiting amount is zero and history is an empty local list.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Return immutable, paginated payout transactions and
  pending totals from the wallet/payout ledger.
- **Production risk if retained:** No payout traceability.

### BYP-017 — Distributor directory

- **Original behavior:** The server returned distributor name, region, mobile
  number and balance/availability status.
- **Why it existed:** Distributor availability and contact information change and
  must be managed centrally.
- **Current bypass:** One clearly labelled offline distributor fixture is returned.
- **Current status:** Bypassed; local fixture.
- **Restore with backend:** Provide an admin-managed, region-filtered directory with
  availability derived from real distributor status.
- **Production risk if retained:** Incorrect contact and availability information.

### BYP-018 — Firebase/FCM token registration

- **Original behavior:** The app registered the device's push token with the
  backend so notifications could be targeted to the user/device.
- **Why it existed:** Account, payout, referral and operator messages need a device
  delivery address.
- **Current bypass:** `CallFcmAPI` is disabled.
- **Current status:** Bypassed.
- **Restore with backend:** Register/rotate/delete tokens per authenticated device,
  respect notification consent and secure the messaging project.
- **Production risk if retained:** Push notifications cannot be delivered.

### BYP-019 — Old encryption/decryption bridge

- **Original behavior:** Several gateways first posted payloads to old encrypt or
  decrypt endpoints before calling the business endpoint.
- **Why it existed:** It attempted to wrap application payloads in an additional
  proprietary transport layer.
- **Current bypass:** Local paths do not invoke the bridge because no network
  payload is sent.
- **Current status:** Indirectly bypassed.
- **Restore with backend:** Use TLS, normal authenticated JSON APIs, server-side
  authorization and platform keystore protection. Do not depend on client-held
  secrets as the primary security boundary.
- **Production risk if retained:** Reintroducing the old bridge would restore dead
  dependencies without providing trustworthy client security.

## Gameplay compatibility changes that are not backend bypasses

These changes support the offline recovery but must still be reviewed during the
proper source rebuild.

### GAME-001 — Selector readiness derivation

- **Original rule:** Selection required both `tableActive` and `gameActive`.
- **Recovery behavior:** Selection requires `tableActive`, positive local credit
  and a 0.15-second debounce. A valid ready-table tap derives `gameActive = true`
  from the already-confirmed positive wallet.
- **Why:** The retired balance callback sometimes left `gameActive` false even
  though the local wallet was valid. `tableActive` still protects spinning,
  settlement and paused states.
- **Production target:** A single explicit `READY` state entered only after the
  server authorizes the wallet/round.

### GAME-002 — Immediate CANCEL input

- **Original rule:** The input component imposed a one-second debounce, then
  `CancelRoundCmd` checked the state, refunded selections and emitted refreshes.
- **Recovery behavior:** The input debounce is removed; the original command-level
  state guard, refund and selection-light clearing remain.
- **Why:** The input debounce made CANCEL appear broken during testing.
- **Production target:** Keep the command guard and use a short UI debounce only if
  telemetry proves it is needed.

### GAME-003 — Original state and event chain preserved

- `StartRoundCmd` still requires an active game/table and at least one selection.
- START still prevents duplicate presses.
- `tableActive` becomes false during the wheel/payment sequence.
- `OnRound`, `OnPayment`, `OnProbability`, `OnJackpot`, `OnWin`, `OnReset` and
  `OnResetFinished` remain the original Unity event chain for animations and audio.
- The original winning number is generated locally with
  `RNGCryptoServiceProvider`; this was original APK behavior, not a recovery bypass.
  For production real-money play, the outcome must move to the server as part of
  round authorization and settlement.

## Backend rebuild checklist

### Phase 1 — Identity and platform security

- [ ] User registration and unique mobile-number model
- [ ] OTP provider, expiry, rate limits and attempt limits
- [ ] Password hashing and reset flow
- [ ] Access/refresh token issuance, rotation and revocation
- [ ] Device/session management
- [ ] Replace retired encryption bridge with TLS-authenticated APIs

### Phase 2 — Wallet and round engine

- [ ] Double-entry or equivalent immutable wallet ledger
- [ ] Authoritative balance snapshot with version
- [ ] Idempotent create-round endpoint
- [ ] Server-generated outcome and multiplier
- [ ] Server-side bet validation and limit/exposure checks
- [ ] Atomic, idempotent settlement
- [ ] Settlement receipt containing resulting balance
- [ ] Recovery for interrupted/retried rounds

### Phase 3 — Cashout and administration

- [ ] Cashout request with balance reservation
- [ ] Admin/distributor approval workflow
- [ ] Payout provider integration
- [ ] Awaiting amount and immutable payout history
- [ ] Distributor directory and availability management
- [ ] Audit log and role-based admin permissions

### Phase 4 — Engagement services

- [ ] Leaderboard calculation and privacy masking
- [ ] Versioned prize-distribution rules
- [ ] Referral accounting
- [ ] FCM token lifecycle and notification consent
- [ ] Monitoring, fraud detection and operator alerts

## Production acceptance gates

Do not remove a bypass from this register until all applicable checks pass:

- [ ] API contract documented and versioned
- [ ] Authentication and authorization tested
- [ ] Retry and idempotency behavior tested
- [ ] Timeout, offline and partial-failure UI tested
- [ ] Wallet conservation/reconciliation tests pass
- [ ] Tampered APK cannot choose outcomes or balances
- [ ] Audit events exist for rounds, settlements and payouts
- [ ] Staging test evidence recorded
- [ ] Bypass removed from patcher and marked restored below

## Restoration progress

| Bypass | Status | Backend endpoint/service | Verification evidence | Restored in commit |
|---|---|---|---|---|
| BYP-001 through BYP-019 | Offline bypass active | Not yet rebuilt | Offline recovery testing only | — |

## Change log

| Date | Revision | Change |
|---|---|---|
| 2026-08-04 | 1.0 | Created initial backend dependency map. |
| 2026-08-04 | 2.0 | Expanded into explicit BYP-001–BYP-019 register, documented original intent, risks, restoration requirements, gameplay compatibility changes and production gates. |
| 2026-08-04 | 2.1 | Removed the final probability callback boundary from the winning branch so local multiplier, payout, audio/visual events and reset execute as one sequence. |
