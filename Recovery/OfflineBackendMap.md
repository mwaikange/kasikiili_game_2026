# Kasikili retired-backend dependency map

This map separates gameplay state from services that were previously supplied by
the retired Kasikili backend. The offline recovery must replace each service
explicitly while preserving the roulette state guards.

| Area | Original dependency | Why it existed | Temporary offline behavior | Production replacement |
|---|---|---|---|---|
| Startup | Connection test | Detect backend availability | Immediate success | Health/readiness endpoint |
| Authentication | Login, logout, signup, OTP, password reset | Identity and account lifecycle | Direct local user and local success responses | New auth service and token storage |
| Opening wallet | Player balance | Confirm spendable credit before play | Initialize N$500; enable play only when credit is greater than zero | Transactional wallet API |
| Round start | Connection test and total-user balance | Prevent starting while offline and supply pool/tub data | Immediate connection success and local total value | Round authorization endpoint |
| Probability | Probability/auto-jackpot API | Supply the round win multiplier and jackpot limit | Local `OFFLINE` response with multiplier 10 and high test limit | Signed/server-authoritative round result |
| Settlement | Update-user-balance API | Persist win/loss result | Apply `post-bet credit + win` directly to local wallet | Idempotent settlement transaction |
| Round reset | Fetch-player-balance API | Reconcile authoritative wallet after settlement | Preserve local settled wallet; clear selections and restore guarded ready state | Settlement receipt plus reconciled wallet |
| Leaderboard | Leaderboard and prize APIs | Rankings, player position and prize table | Local Top 10 and prize JSON | Gamification/ranking service |
| Cashout | Cashout request, awaiting amount and history | Submit and track real payouts | Validate and deduct local test wallet; awaiting/history are local placeholders | Audited payout workflow |
| Distributors | Distributor-list API | Show cash-in/out contacts | One clearly labelled offline distributor row | Distributor directory service |
| Notifications | FCM token registration | Push-notification delivery | Disabled | New push-token endpoint |
| Encryption bridge | Encrypt/decrypt endpoints | Wrap old API payloads | Not used by patched local paths | Replace with normal TLS and server-side authentication |

## Preserved gameplay rules

- Selectors require a ready table and positive credit.
- Starting requires at least one selected bet and prevents duplicate starts.
- The table locks while the wheel, probability, payout and reset sequences run.
- CANCEL is allowed only while the guarded cancel command considers the table ready.
- Win, probability, jackpot, audio and reset events remain the original Unity event chain.

## Not production-safe

The offline probability, wallet, cashout, leaderboard and identity values are test
fixtures. They must not be used for real-money play. A production rebuild needs
server-authoritative round creation and idempotent wallet settlement so a client
cannot choose outcomes or change balances.
