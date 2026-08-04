# Layout Specification

## Canvas

- Full device reference: **375 × 812 px**
- Real system status-bar reference: **42 px** (not drawn by Unity)
- Generated app-content canvas: **375 × 770 px**
- Orientation: portrait
- Main content background: `#033521`
- Top status background: `#0F393B`

All coordinates below are measured from the top-left of the generated content canvas. Add 42 px to Y when comparing with the full approved screenshot.

## Header and controls

| Element | X | Y | Width | Height |
|---|---:|---:|---:|---:|
| Exact game header artwork | 0 | 0 | 375 | 177 |
| Title | 57 | 177 | 262 | 28 |
| Menu button | 7 | 203 | 34 | 34 |
| Chip 10 | 58 | 204 | 40 | 36 |
| Chip 50 | 97 | 204 | 40 | 36 |
| Chip 1000 | 136 | 204 | 53 | 36 |
| Chip 2000 | 188 | 204 | 53 | 36 |
| Chip 25 | 240 | 204 | 40 | 36 |
| Chip 100 | 279 | 204 | 40 | 36 |
| CANCEL | 88 | 247 | 57 | 30 |
| START | 231 | 247 | 57 | 30 |

## Board

- Outer board: `x=10, y=283, width=355, height=481`
- Vertical lines: global `x=130` and `x=252`
- Horizontal lines: content `y=382`, `477`, `572`, `668`
- Number ellipse: **53 × 44**
- Bet button: **40 × 32**, radius approximately 3 px

## Colours

| Usage | Hex |
|---|---|
| Content background | `#033521` |
| Chip/action red | `#8B0F0F` |
| Bet yellow | `#C5C916` |
| Number black | `#000000` |
| Number red | `#8B0F0F` |
| Number zero teal | `#0F393B` |
| Grid line | `#6E877D` |
| Text | `#FFFFFF` or `#000000` depending on control |

## Number colour sequence

- 0: teal
- 1: red
- 2: black
- 3: red
- 4: black
- 5: red
- 6: black
- 7: red
- 8: black
- 9: red
- 10: black
- 11: red
- 12: black

## Button semantics

Every roulette number has four yellow button positions:

1. upper-left
2. upper-right
3. lower-left
4. lower-right

Unity object names follow `Bet_<number>_<position>`, for example `Bet_7_3`.
