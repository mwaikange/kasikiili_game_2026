# Kasikili Game Centre architecture

## Product navigation

Authentication remains the entry boundary. After a successful sign-in, the app
opens the **Game Centre reception**. A user must select a game before any gameplay
screen is exposed.

```text
Login / Signup / Reset Password
              |
              v
      Game Centre reception
       |                 |
       |                 +--> Profile button --> partial left drawer
       |
       +--> Kasikili Roulette --> existing Game scene
       +--> My Lucky 6 Deluxe --> catalog placeholder
       +--> Kasikili Predictions --> catalog placeholder
```

The existing roulette scene remains intact. In the recovered APK, the reception
is a high-priority Unity canvas over that scene. Selecting Kasikili Roulette
removes the canvas and reveals the original gameplay. The former roulette menu
button returns to the reception instead of exposing roulette-owned account pages.

## Reception drawer

The circular profile button above the balance opens a 76%-width drawer from the
left. It intentionally does not cover the full screen. Its destinations are:

1. Token Transfers
2. Cash-out
3. Cash-in
4. Leaderboard
5. Rewards
6. Settings

These are platform-level features and must not be owned by the roulette game.

## Wallet rules

The platform uses one Main Balance plus one wallet per game.

```text
Kasikili Roulette wallet ----+
Lucky 6 wallet --------------+--> Main Balance --> Cash-out
Predictions wallet ----------+

Main Balance --> any enabled game wallet
```

- Game-to-game transfers are not allowed directly.
- Funds must first move from the source game wallet to Main Balance.
- Funds can then move from Main Balance into another game wallet.
- Cash-out may debit Main Balance only.
- Every production transfer must be an authenticated, idempotent ledger entry.

The current recovery build implements these rules with a local test fixture. It
does not transfer or pay real money.

## API boundaries

### Game catalog

The reception consumes a catalog-shaped model with `id`, `displayName`,
`sceneName`/launch target and `available`. The production endpoint should also
return icon URLs, minimum supported app version, maintenance state and wallet ID.

### Wallet summary

One response should return Main Balance and every game-wallet balance, all with a
ledger version. The client must display server values and must never calculate an
authoritative balance itself.

### Transfer

The server validates that either source or destination is Main Balance, reserves
funds, records both ledger legs atomically and returns the resulting wallet
summary. Client-generated idempotency keys prevent duplicate taps from creating
duplicate transfers.

### Cash-out

Cash-out accepts Main Balance only. The server reserves funds and returns an
auditable payout request. PaySme cash-in will be added separately when its code
and contract are supplied.

### Referral rewards

The existing registration UI already contains a referral-code input. It is
optional in the recovered client, but the old signup command did not send it to
the backend. The rebuilt registration API must accept an optional referral code,
validate it server-side and bind attribution once. Rewards must be calculated
from verified qualifying events, never from a client-reported count.

## Temporary recovery behavior

- Main Balance starts from the local N$500 recovery fixture.
- Additional games are catalog entries marked `COMING SOON`.
- Transfers and cash-out update local test state only.
- Leaderboard rows are local demonstration data.
- Rewards expose a local demonstration referral code.
- Notification settings persist locally through Unity `PlayerPrefs`.
- Cash-in displays a PaySme integration placeholder.

All temporary behavior must be replaced with authenticated backend services
before production or any real-money test.
