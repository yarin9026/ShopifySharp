<div dir="rtl">

# Short Stack: a pancake-stacking game

A one-tap game: pancakes slide across the screen, and you tap to drop them onto the pile. Whatever hangs over gets cut off. Get it exactly on top and you get a bonus and the pancake grows back.

## Files

| File | What it is |
|---|---|
| `index.html` + `fonts/` | The game itself (one file, no outside dependencies) |
| `store/short-stack-crazygames.zip` | **The file you upload to CrazyGames** |
| `store/cover-landscape-1920x1080.png` | Landscape cover (required) |
| `store/cover-portrait-800x1200.png` | Portrait cover |
| `store/cover-square-800x800.png` | Square cover |
| `store/short-stack-preview-landscape.mp4` | Landscape preview video, 18 seconds, 1920×1080 |
| `store/short-stack-preview-portrait.mp4` | Portrait preview video, 18 seconds, 1080×1620 |

## Uploading to CrazyGames

1. Sign up for free at **developer.crazygames.com**.
2. Click **Submit a game** and upload `short-stack-crazygames.zip`.
3. Upload the 3 covers and 2 videos from the `store` folder.
4. Fill in the form with the answers below.
5. Check the game in their **Preview** screen, then send it for review.

### Form answers

| Question | Answer |
|---|---|
| Progress save | **Yes, using the Data Module from the CrazyGames SDK** |
| Game supports muting audio through SDK | **Yes** |
| Mobile / touch | **Yes** (phones and tablets) |
| Orientation | **Both** (landscape and portrait) |
| In-game purchases | **No** |

**Description (in English, paste as is):**

<div dir="ltr">

> Short Stack is a one-tap stacking game set in a late-night diner. Pancakes slide across the counter. Tap to drop each one on the stack. Whatever hangs over the edge gets sliced off, so every miss makes the next drop harder. Land three perfect drops in a row and the pancake grows back. How tall can you stack?

**Controls:** Tap, click, Space or Enter to drop a pancake.

</div>

## What happens after upload

1. **Review**: CrazyGames checks the game (usually a few days).
2. **Basic Launch**: the game goes live to a small audience for about 1–3 weeks. **There are no ads and no money at this stage.** They measure how long people play and whether they come back.
3. **Full Launch**: if the numbers are good, the ads turn on and you get a share of the revenue. Payment is monthly once you reach €100 (PayPal or bank transfer).

The game is already set up for the ads (between rounds, plus "Continue" by watching an ad), so nothing needs to change when you reach Full Launch.

## Updating the covers and videos after changes to the game

The covers and videos are generated from the game itself: `index.html?media=cover` shows the cover scene, and `index.html?media=demo` plays automatically for recording.

</div>
