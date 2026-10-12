# Doc Mo guides: notes and assessment

<div dir="rtl">

## הסיכום הקצר (בעברית)

- **מה זה:** 21 מדריכים חינמיים של Doc Mo (docmo.agency). המטרה שלהם היא למשוך לקוחות לסוכנות שלו, דרך הודעה בפרטי. הם מלמדים לבנות אתרי "קולנוע" לעסקים בדיוניים עם AI.
- **השיטה שחוזרת בכולם:** תמונות AI ← סרטוני AI שמחברים ביניהן ← פירוק לפריימים ← "סרט" שזז עם הגלילה.
- **מה טוב בהם:** הנוסחאות לפרומפטים של תמונות ווידאו (תאורה אחת קבועה, עדשה קבועה, אילוצים בתוך הפרומפט), חוקי וידאו AI, ומספרי אנימציה מדויקים.
- **מה פחות:** חלק גדול מהקוד והתיקונים מוסתר. האתרים כבדים מאוד לטלפון. אין עברית ואין נגישות. רוב הבניות תלויות בקרדיטים בתשלום ובמחשב.
- **סיכון משפטי:** חלק מהמדריכים משתמשים במותגים אמיתיים (Nike, CHANEL, Coca-Cola, מכוניות), ממציאים רופאים וביקורות, או משתמשים בשמות מוגנים של מאכלים. לא להעתיק את זה.
- **הכי שימושיים לך:** IVORY (בחירת ספר או מעצב), BRACE ו-explosion (אוכל), NOIR BREW (קפה ומאפה), Sultan Al-Oud (סרטון לשמן זקן במספרה). גם SMASH מקבל ציון גבוה כרעיון.
- **השורה התחתונה:** כדאי לקחת מהם טכניקות, לא את המודל העסקי. המחיר שלו (€2,500–5,000 לאתר) והכלים שלו לא מתאימים לאתר של 500–1,500 ₪ שבונים מטלפון.

</div>

> This public copy omits the guides' verbatim prompt text (their author's content); every prompt is in the source PDFs linked per guide below.


This file stands in for the 21 free Doc Mo PDF guides (guides.docmo.agency, 2026). Read it instead of re-reading the PDFs. It covers the shared method, a technique library, and per-guide details with the reusable prompts. The guides' sales copy has been left out on purpose.

**User context these notes were written for:** an Israeli freelancer selling small-business websites (barbers, cafés, bakeries, clinics, eateries) for about ₪500–1500. Works from a phone, has a $0 budget, and builds with Claude. Every guide shows a demo for a fictional brand and has an English LTR layout. None of them covers Hebrew, RTL, accessibility law or pricing in shekels.

---

## 1. The shared system (what every guide is really teaching)

1. **AI stills first.** Generate keyframe stills in **Nano Banana Pro (Gemini 3 Pro Image)**, usually on **Higgsfield**, at 2K. Keep one named light source, one palette and one void or surface across every frame. Lock consistency with reference images ("Use image_1 as style only… image_2 as exact base").
2. **AI video bridges the stills.** Use **Kling 3.0** (pro mode) or **Seedance 2.0** with locked start and end frames, one camera move per clip, and the last frame of clip N as the start of clip N+1 ("chaining"). Kling suits camera moves and interiors; Seedance suits physics, choreography and exteriors.
3. **ffmpeg** concatenates the clips and extracts a **WebP/JPG frame sequence**: a desktop set of about 200–300 frames and a mobile set of about 100–145.
4. **Canvas scroll-scrub hero ("ScrollStage").** A pinned or sticky section holds a `<canvas>`. Scroll progress 0–1 maps to a frame index, and `drawImage()` paints that frame. A camera HUD overlay is optional.
5. **Standard section kit around the hero.** Menu cards → configurator with a "fly-to-price" clone animation → editorial split → count-up stats → full-bleed "The Sear"-style moment → ingredient/craft grid → brand story timeline → reservation calendar (Web3Forms) → footer. Clinic variant: "character-select" staff roster → services with slide-in panels → journey → stats → before/after gallery → testimonials → booking with a WhatsApp deep link.
6. **Stack.** Two options:
   - React 19 + Vite + TypeScript + Tailwind 4 + GSAP/ScrollTrigger.
   - One vanilla HTML file + GSAP.
   Deployed on **Vercel**. **Claude Code** writes the code from plain-language descriptions.
7. **Every build starts from a clone of the previous one** (white-label templates: UMAMI → BIRRIA → SMASH → 6 BROTHERS; APEX → ATLAS → IVORY; NOIR BREW → Nike). Most pitfalls come from template leftovers (German strings, old videos, wrong labels).
8. **Guide format (lead magnet).** Prompts shown range from 0 (Cosmos, NEXUS) to about 17 (explosion); code, timing constants, reference-chain setup and pitfall fixes are mostly withheld "by design". The only Doc Mo client price stated is €2,500–€5,000 for a restaurant or food brand (explosion); €8k–15k figures are what a traditional agency would charge. Most guides (14 of 21) end with "DM WEBSITE"; others end with "Comment <KEYWORD>" or just a demo link. Expect gaps.

---

## 2. Technique library

### 2.1 Animation patterns

| Pattern | How it works | Guides |
|---|---|---|
| **Canvas scroll-scrub (frame sequence)** | Pinned/sticky section + canvas. `frame = round(progress × (TOTAL−1))` then `drawImage`. Preload frames. Separate mobile set. | 6 BROTHERS, BIRRIA, BRACE, BRULEE, CHRONOS, NEXUS, SMASH, UMAMI, explosion, villa |
| Scrub pacing | About **27px of scroll per frame**. Use a duration in % of viewport height (`+=600%` for 200 frames) and `scrub: 0.4`. Avoid fixed pixel durations and `scrub:true`. | NEXUS |
| rAF lerp instead of a scroll library | Native `position: sticky`. A rAF loop lerps the displayed frame toward the target. Cap DPR at 1.5. | BRULEE |
| Throttled frame loading | Load frame 0 first, then a bounded pool of parallel requests, prioritising frames near the current scrub position and fetching the landed frame on demand. Don't fire all requests at once. | BRULEE, UMAMI, BIRRIA (pitfall) |
| Reserved scroll ends | Hold frame 0 for about the first 10% (wordmark shatter) and hold the last frame at the end (product reveal). | BRULEE |
| Scroll-range storytelling | Each scene owns a progress window, and labels fade in and out per window. | villa, 6 BROTHERS (HUD chapters) |
| Camera HUD overlay | REC timecode (`setInterval(1000/24)`), corner brackets, crosshair, ISO/WB read-out, progress rail and frame counter, all updated in the same `onUpdate`. Run the timer only while the hero is visible. | BRACE, BIRRIA, UMAMI, 6 BROTHERS |
| **Fly-to-price / fly-to-configurator clone** | Clone the thumbnail as a fixed-position element (`top:0; left:0`) and tween it in an arc: `power2.out` to a midpoint 100px above the path, then `power2.in` to the target from `getBoundingClientRect`. The price "punches" (scale 1.4, glow) from `onComplete`, then the clone is removed. | BRACE (exact numbers), UMAMI, BIRRIA, SMASH, 6 BROTHERS |
| ORDER → configurator | Clone the card image and fly it (`power3.inOut`, 0.7s), smooth-scroll after 200ms, switch the preview. | BRACE, UMAMI, BIRRIA |
| One-shot reveals | Opacity 0→1 and y 40→0 with a 0.1s stagger, run once and then kill/unobserve. Use a shared `.reveal` class with `ScrollTrigger.batch`. | almost all |
| Count-up stats | Numbers count up when they enter the viewport. | almost all |
| **"Coffee Drift" 3-layer slider** | Product spins out 290° with a 29px anticipation dip (`back.in(0.8)`) and the incoming one unwinds with `power4.inOut`. The background word flips `rotationY` ±60°. Float cutouts leave as ghost DOM clones (`back.in(2)`) while new floats enter. Float exit duration ×1.25 so all layers reach the edge together. 2.2s input lock. Arrows, keyboard and swipe. | NOIR BREW, Nike |
| Character-select roster | Active person large and bright, neighbours smaller or shorter and dimmed. Prev/next, keyboard and swipe drive one shared index. "02 / 05" counter. Info panel with animated stat bars. Full-screen profile overlay with a Book CTA. | APEX, ATLAS, IVORY, ECHOES |
| Reusable slide-in panel | One slide-from-right panel + dim overlay, reused for staff, services and gallery modals. | APEX, ATLAS, IVORY |
| Screen-blend "hologram" video | Generate on pure black and use `mix-blend-mode: screen` over a dark page, so the black disappears. Optional 2× playbackRate. | APEX |
| Ping-pong loop from one clip | Reverse the AI clip with ffmpeg to get the "dissolve" half, then alternate the two `<video>` elements on `ended`. Simpler alternative: concat forward + reverse into one file and use `loop`. | ATLAS |
| Alpha (transparent) video | Shoot on white, use the Remove-BG mask, `alphamerge`, erode 2px, despill white, ProRes 4444 intermediate, then VP9 WebM (yuva420p) + HEVC MOV (hvc1) as two `<source>` tags. | ECHOES |
| Pinned gauge / thermometer | SVG `stroke-dashoffset`, or the same scroll proxy as the scrub, drives a temperature gauge and background crossfade. | UMAMI, explosion |
| Exploded-view / anatomy reveal | Layers fly in from below with labels; stack vertically on mobile. | explosion, 6 BROTHERS, SMASH, CHRONOS |
| Dual-direction marquee + watermark word | Two marquees running opposite ways with a giant word behind them. | BRACE |
| Canvas starfield engine | rAF-first loop with try/catch, positions as 0–1 fractions, data-driven content, one 0→1 progress per reveal style, an opaque base under overlays, `save`/`restore` pairs, a dismiss cooldown. | Cosmos |
| Z-index curtain | The hero gets a higher z-index so its scroll-off covers the next pinned section's gap. | NEXUS |
| Sound gate / START gate | Opt-in screen before heavy assets or audio. The later builds dropped sound entirely. | villa, NEXUS, 6 BROTHERS, SMASH (removed) |

### 2.2 Layout patterns

- **Restaurant/food brand kit:** navbar (transparent until scroll) → canvas hero → 3-item menu → build-your-own configurator → editorial split → 4 stats → full-viewport "The Sear/Stone/Simmer" → ingredients bento grid → craft/sourcing grid → story timeline → reserve (calendar, lunch/dinner slots, party-size stepper) → footer. Used by UMAMI, BIRRIA, BRACE, SMASH, 6 BROTHERS and explosion.
- **Clinic kit:** hero (hologram or object orbit) → team intro video → character-select → treatments with slide-in panels → methods/tech cards → 4-step journey → stats → before/after gallery → testimonial carousel → booking (dropdown, native-Date calendar with past days disabled, time slots, WhatsApp deep link) → Web3Forms. Used by APEX, ATLAS and IVORY.
- **Product slider:** full-screen colour-coded slides with a giant background word and corner ingredient floats. Used by NOIR BREW and Nike.
- **Theme tokens:** keep several palettes in code and ship one (IVORY has 7 themes, APEX has 8). Re-skin per client.
- **Two variants:** a heavy AI showpiece and a light "clean" version (IVORY: 2.2 MB vs 0.75 MB).
- **Material/texture tabs:** marble, oak and travertine macros as a cheap premium section (villa). For a bakery, use flour or crust textures.

### 2.3 Image-prompt formulas (Nano Banana Pro)

There is no negative-prompt field, so every constraint goes inside the prompt. The guides disagree about whether to phrase constraints positively or as "no X".

- **Food skeleton (BRACE/BIRRIA/UMAMI):** `[dish + key details], on [dark volcanic slate / matte black], [angle], [85mm | 50mm overhead flat lay | 100mm macro]. Single directional key light upper-left, warm amber 2700–2800K. Cinematic, premium food photography/editorial, dark and brooding mood, deep blacks. Shot on full-frame studio camera. Sharp throughout, no overexposed [highlight]. [4:3 menu | 1:1 ingredient]`
- **Universal food formula (explosion):** `[Subject + key detail], [surface: black slate / marble / cast iron], dramatic single overhead spotlight, [atmosphere: smoke / steam / condensation], dark cinematic black background, food photography editorial, photorealistic, 8k`
- **Product shot (NOIR BREW):** `[Product + key detail], [background: dark / deep colour matching brand], [lighting: single overhead spot / rim light / backlit], [atmosphere: steam / condensation / texture], cinematic studio product photography, photorealistic, 4K`. For cutouts add "clean edges for cutout".
- **Staff portrait (APEX/ATLAS/IVORY):** `[age/gender/role], [wardrobe in neutral brand colour], [pose], three-quarter angle, waist up / near-square bust crop. Single soft key light upper-left/front-left, subtle [brand colour] rim light, [neutral grey / dark graphite] backdrop. 85mm portrait lens, shallow DOF, sharp eyes. Natural skin texture, no plastic look. [style tag]. 2K, no text, no logos. [4:5]`. Repeat the same rig on every portrait and add "consistent lighting rig with companion portraits". Encode styling rules explicitly (IVORY: loupes on a neck strap, not over the eyes).
- **Action/documentary shot:** 35–50mm, candid, window daylight mixed with a warm practical lamp, focus "on the point of contact".
- **Architecture (villa):** subject + materials + view + time-of-day light + "No people" + lens/aperture (24mm f/2.8, 35mm f/1.8) + camera (Phase One) + publication style (Architectural Digest / Wallpaper) + "Ultra-detailed, photorealistic". For materials: "100mm macro… no context, just the stone… fills the entire frame edge to edge".
- **Isolated product set (Nike):** a "universal rule" sentence for orientation repeated in every prompt (e.g. "right shoe, left-facing 3/4 view, toe lower-left, heel upper-right"), plus "Entire X fully within frame, nothing cut off. Perfectly centered, equal padding. Pure white background, perfectly isolated." Use the same 2K and 1:1 settings for every asset. Shoot white/thin/looped objects on dark #1a1a1a.
- **Luxury liquid/glass (CHANEL/Coca-Cola):** subject + "Macro, 85mm" + "Single warm golden key light from upper left, soft rim" + "Infinite studio void — [gradient]" + "IOR 1.5 glass, IOR 1.33 liquid" (questionable, works mostly as flavour) + style + "Shot on full-frame cinema camera" + "Sharp label typography, correct spelling" + ratio.
- **Still-prompt order (BRULEE):** subject → lens/framing → mood → a named light *source* → guards ("no smoke/haze" where it would steal the next beat; "Exactly one X, no duplicate X"; "no text, no labels, no logos"; ratio). Keep motion vocabulary out of still prompts.
- **Reference roles:** "Use image_1 as style, lighting and colour-grade only, do not copy its composition. Use image_2 as the exact base for the subject." For late-chain frames, lock everything ("same glass, same position, same angle…") and name the single thing that changes.
- **Split-screen fix:** "One single continuous photographic image, NOT a split screen … this is the SAME photograph, only a moment later."
- **Keyframe framing:** "Full-bleed 16:9… no border, no black bars, no letterboxing."
- **Style reference at low strength (0.25–0.35)** so it steers light and mood only (BIRRIA/UMAMI, via a wrapper UI).
- **Character sheet:** a 4-panel sheet (front + 3/4, close-up + full body), generated once and referenced into every frame (Sultan Al-Oud).

### 2.4 Video-prompt rules (Kling 3.0 / Seedance 2.0)

- **6-step formula (BRACE):** Subject → Action → Environment → Camera → Style → Quality suffix.
- Quality suffix (placebo-ish; "4K" does not change output resolution): `4K, Ultra HD, rich details, sharp clarity, cinematic texture, natural colors, soft lighting, no blur, no ghosting, no flickering, stable picture.`
- One camera move per clip. End with **"then settles and holds"** (CHANEL/Coca-Cola).
- High energy: **"Camera locked off. Subject moves. Explosive velocity on the burst."** All three lines must be used together.
- Anti-freeze: "never stop moving, never hover, never freeze… Continuous motion throughout, no freeze, no pause". Write it as prose with **no timestamps** (BRULEE). Sultan Al-Oud contradicts this and uses timeline prompts `[0s]…[2s]…[3.5s] impact…[4-5s] holds` with one impact beat per clip. Both approaches work in different cases.
- **"— already there"** for persistent elements so they don't pop in.
- Name what must **NOT** move first, in capitals ("DO NOT ROTATE AT ALL"), or use a "Rule one… Rule four" preamble (6 BROTHERS).
- Don't rotate detailed textures (the model "invents the average"). Never create a new object mid-motion. Stagger events ("one after another, never all at once"). Use explicit occlusion ("passes BEHIND"). Use "IN PLACE… never slides sideways".
- Continuity: open with "Same X, same composition as previous clip". Use a match-cut where fragments reverse and reassemble to hide a model switch, and restate the lighting (BIRRIA).
- Kling: set `mode: "pro"` explicitly (standard silently gives 720p), sound off, decline Higgsfield/Kling **preset suggestions** (they overwrite your prompt).
- Seedance: leave numbers with units out (they render as text). Start from an exterior shot to avoid the NSFW filter. Switch to Kling if weapons or interiors trip the filter.
- Anti-duplication: "The griddle surface is now completely empty… Exactly one of each ingredient, no duplicates." Add a continuity-reveal keyframe that shows staged items before they are used (SMASH).
- Hand-as-anchor: show a gloved forearm, never a face (SMASH).
- QA: split clips into stills at 2fps. Measure the per-frame pixel difference to trim end-freeze/dead frames and detect fake motion.
- Reference-anchor: pin the product into a fixed environment plate (Obsidian). Lock screen direction (left-to-right) across clips.

### 2.5 Copy / content patterns (site copy, not the guides' marketing)

- Section names as craft verbs or nouns: "The Sear", "The Stone", "The Simmer", "The Layers", "The Craft", "La Línea", "Build Your [Brand]".
- A stats strip with real process numbers (120g patty, 8-second smash window, 230°C griddle; 485°C / 90s / 72h). Avoid invented ones.
- Badge/eyebrow lines: "SLOW-BRAISED — SINCE 2019", "SIMMERING NOW 92°C", "ARCHIVE // BRAND PHILOSOPHY".
- Heritage story as 4 chapter cards with years. Use the real history only.
- Short product taglines per slide ("Born 1985. Never retired."). Keep them short and about the product.
- Giant background or watermark word per section or slide (ESPRESSO, TOPPINGS, kanji).
- Clinic: specialist cards with specialty, quote and skill bars; patient journey in 4 steps. Real staff and consented reviews only.
- Label demo data as demo (NEXUS's honest "fictional" labels).

### 2.6 Engineering pitfalls worth remembering

- `gsap.from()` reads its start from the current inline style, so if the element already has the final value nothing animates (NEXUS).
- Never animate one property with both a CSS transition and GSAP (NEXUS).
- GSAP transforms override CSS `translate(-50%,-50%)` centering. Use `xPercent`/`yPercent` (NOIR BREW/Nike).
- Size the canvas from its wrapper element, not `window.innerHeight` (mobile address bar) (NEXUS).
- A pinned hero shifts the trigger positions of later sections. Create triggers in page order or call `ScrollTrigger.refresh()` (UMAMI).
- `TOTAL` must equal the real frame count. Check with `ls | wc -l` (BRACE, villa).
- Use `cover`, not `contain`, for a full-bleed hero. Keep the navbar out of the pinned hero (BIRRIA).
- Decoded memory: a 1920px RGBA frame is about 8 MB, so 200 frames is about 1.6 GB. Use smaller mobile frames, a sliding window or decode on demand (CHRONOS, 6 BROTHERS).
- `ffmpeg concat` needs every input at the same resolution, fps and SAR. Normalise first (villa).
- Never transcode WebM → WebM for alpha. Encode every delivery format from ProRes 4444 (ECHOES).
- Running several AI agents on one single-file codebase means the last writer wins (BRACE).
- Audit for template leftovers after cloning: strings, videos, labels (APEX, SMASH, 6 BROTHERS).
- Vercel Deployment Protection can put an SSO wall on aliases. Vercel **Hobby is non-commercial**, so client sites need Pro or other hosting (Cloudflare Pages, Netlify, GitHub Pages).

---

## 3. Cross-cutting cautions (apply before reusing anything)

- **Trademarks:** CHANEL, Coca-Cola, Nike (Swoosh, Jumpman), Lamborghini/Ferrari/Porsche/Bentley/Range Rover, CEREC, and protected food names (San Marzano DOP, Prosciutto di Parma, Parmigiano Reggiano, "Verace"/AVPN). Never put these in client work or a portfolio. Swap in the client's own product.
- **Fabricated content:** AI doctors and therapists, invented stats, AI before/after images and fake testimonials are deceptive advertising, especially for health businesses (Israeli Ministry of Health advertising rules, consumer protection law). AI food photos that don't match the real dish mislead customers. Use the client's real photos (AI-enhanced is fine) and real staff.
- **Accessibility:** Israeli law requires business websites to meet IS 5568 (WCAG 2.0 AA). None of the guides covers `prefers-reduced-motion`, keyboard use, focus traps, ARIA, alt text or canvas text alternatives.
- **Performance:** frame-sequence heroes weigh 13–90 MB, and 16–21 MB video heroes are common. That is far too heavy for Israeli mobile data. Budget about 1–3 MB per page for small-business sites.
- **RTL/Hebrew:** none of the guides handles it. Mirror arrows, swipe direction, carousel order, the slide-in side and the fly-to-price arc. Use `dir="rtl"`, logical CSS properties (`margin-inline-start`), and Hebrew fonts (Heebo, Assistant, Rubik, Frank Ruhl Libre).
- **Booking:** the calendars are front-end only and Web3Forms just sends an email. There is no availability check and nothing prevents double bookings. For Israel, use a WhatsApp deep link, or link to Ontopo/Tabit for restaurants or an existing booking app for barbers.
- **Privacy:** health enquiries sent through third-party forms need consent and a privacy notice (Israeli Privacy Protection Law, Amendment 13).
- **Music:** CapCut library tracks and unnamed SFX have unclear commercial licences.

---

## 4. Per-guide notes

Source URL pattern: `https://guides.docmo.agency/<file>.pdf`. Fit scores are out of 5 for an Israeli ₪500–1500 small-business site.

---

### 4.1 6 BROTHERS: smash burger that explodes and packs itself into the box
- **Source:** https://guides.docmo.agency/6-BROTHERS-Guide-EN.pdf
- **Built:** React 19 + Vite + Tailwind 4 + GSAP 3.15. 12 sections. Canvas ScrollStage hero from 6 chained Kling 3.0 clips (44s), 200 desktop frames (1928×1076, 19 MB) and 100 mobile frames (1080×602, 4.9 MB). Sequence: burger explodes → exploded column with callout labels → bag tips and pours the sides out → burger closes while the sides orbit → drops into the box → lid closes, shot top-down. Also: menu, configurator with fly-to-total, stats, The Sear, ingredients, materials, story, reserve calendar, footer. No sound; a START gate with a loading bar. Live: six-brothers-selfcut.vercel.app.
- **Tools:** Higgsfield Kling 3.0 and Nano Banana Pro (paid), React/Vite/Tailwind/GSAP, canvas + ImageBitmap, IntersectionObserver, Vercel.
- **Workflow:**
  1. Clone the best earlier build.
  2. Make 6 keyframes at 2K.
  3. Make 6 chained Kling clips.
  4. Trim dead frames by per-frame difference.
  5. Export 2 WebP sets.
  6. Build the pinned canvas: ScrollTrigger progress → frame index, frames pre-decoded to ImageBitmap.
  7. Drive the HUD, rail and callouts from one progress value.
  8. Build the configurator fly-to-total.
  9. Add one-shot reveals.
  10. Strip template leftovers (3.6 MB sound engine, slam flash).
  11. Set immutable cache headers on /frames.
  12. QA at 390/768/1440px.
- **Prompts:**
  - Kling clip 3 (the bag tips, the column stays frozen):
    (verbatim prompt: see the source PDF)
  - Kling clip 4 (the burger closes, fries orbit behind, the crust is protected):
    (verbatim prompt: see the source PDF)
  - Kling clip 5 (rules preamble; the guide truncates it):
    (verbatim prompt: see the source PDF)
  - Keyframe 02 (explosion), Nano Banana Pro 2K:
    (verbatim prompt: see the source PDF)
  - Keyframe 03 (exploded column):
    (verbatim prompt: see the source PDF)
  - Keyframe 06 (closed box, top-down, reference image scoped to materials only):
    (verbatim prompt: see the source PDF)
- **Key lessons:** the best AI-video rules in the series (see 2.4). Memory risk: pre-decoding 200 2K frames is about 1.6 GB. Counts in the guide contradict each other. The reserve section has no backend.
- **Fit:** 2/5. Mine it for video-prompt technique; sell the scrub hero only as an upsell.

### 4.2 APEX Physiotherapy: character-select clinic
- **Source:** https://guides.docmo.agency/APEX-Guide-EN.pdf
- **Built:** a single HTML file with GSAP 3.12. Hero: a green anatomy/spine hologram video at 2× speed with `mix-blend-mode: screen` behind the wordmark. Team intro video under a scrim. A 3-therapist character-select stage (click or ←/→, profile panel with skill bars and Book CTA, full profile view on mobile). 6 treatment cards with slide-in panels, 3 methods, a 4-step journey, count-up stats, a 6-item success gallery, a 3-review carousel, and booking (service dropdown, native-Date calendar, time slots, WhatsApp deep link). Web3Forms plus a Google Sheets log. Live: apex-physiotherapy.vercel.app.
- **Tools:** Nano Banana Pro (paid), an unnamed AI video model, GSAP, Web3Forms, Google Sheets, Vercel.
- **Workflow:**
  1. Generate portraits with one lighting rig (soft key upper-left + green rim, grey backdrop) and chained references.
  2. Generate action shots.
  3. Generate the hologram loop on pure black and screen-blend it.
  4. Build the stage (ground line, spotlight ring, hotspots, keyboard nav).
  5. Build one reusable slide-in panel.
  6. Add shared ScrollTrigger reveals.
  7. Build the Date-based calendar with a WhatsApp fallback.
  8. Wire Web3Forms + Sheets.
  9. Deploy and QA (leftover German strings, the 2×2 grid at tablet width).
- **Prompts:**
  - Portrait (Ryan Cole):
    (verbatim prompt: see the source PDF)
    (The "retoutouched" typo is in the original.)
  - Portrait (Sophie Chen):
    (verbatim prompt: see the source PDF)
  - Action shot:
    (verbatim prompt: see the source PDF)
  - Hologram video (unspecified model):
    (verbatim prompt: see the source PDF)
- **Fit:** 3/5. The screen-blend hero, a "choose your barber" stage and the WhatsApp booking all transfer. Use real staff photos.

### 4.3 ATLAS Orthopedics: character-select clinic with a ping-pong hologram hero
- **Source:** https://guides.docmo.agency/ATLAS-Guide-EN.pdf
- **Built:** vanilla HTML/CSS/JS, graphite and ice blue. Hero: a Seedance spine "build-up" clip plus its ffmpeg-reversed twin, alternating on `ended`, with an A/B/C version switcher. A 5-specialist fighting-game select with depth staging (keyboard, swipe, click, "02 / 05", right-side profile panel). A 10-member team grid, 8 specialties with panels, 6 tech systems, journey, stats, before/after gallery modal, testimonials, booking with WhatsApp, and Web3Forms. About 46 AI images. Live: atlas-orthopaedie.vercel.app.
- **Tools:** Nano Banana Pro, Seedance 2.0 (paid), ffmpeg, GSAP 3.12.5 + `ScrollTrigger.batch`, Web3Forms, Vercel.
- **Workflow:**
  1. Generate about 46 images at 2K with an ice-blue rim and convert them to WebP.
  2. Generate one Seedance spine-build clip.
  3. Reverse it with ffmpeg.
  4. Swap two identically positioned `<video>` elements on `ended`.
  5. Build the stage, panel and overlay from one JS array of people (name, specialty, years, cases, education, quote, skill scores 0–100).
  6. Add depth staging with GSAP.
  7. Drive keyboard, arrows and swipe from one shared index.
  8. Recalibrate the staging for bust crops.
  9. Apply `.reveal` + `ScrollTrigger.batch` site-wide.
  10. Reuse the slide-in panel.
  11. Build booking.
  12. Test on a real phone.
- **Prompts:**
  - Portrait (Dr. Lukas Brandt):
    (verbatim prompt: see the source PDF)
  - Portrait (Dr. Marie Hoffmann): same structure as Brandt ("…female orthopedic surgeon in her mid 40s, shoulder-length dark hair, calm reassuring expression, white medical coat over navy blouse, stethoscope draped naturally. Near-square bust crop… cool ice-blue rim light… dark graphite background. 85mm… Premium clinical editorial photography, sharp detail on face and coat texture. 2K, photorealistic, no text, no logos.").
  - Action shot (MRI consultation):
    (verbatim prompt: see the source PDF)
  - Seedance hologram (remove the trailing production note before pasting):
    (verbatim prompt: see the source PDF)
- **Note:** for a truly seamless loop, concat forward + reverse into one file and use `loop muted playsinline`. A typical reverse command is `ffmpeg -i in.mp4 -vf reverse -an out_rev.mp4`.
- **Fit:** 3/5. The ping-pong loop (an espresso or pastry "assembling" itself) and the data-driven roster are reusable.

### 4.4 BIRRIA: cinematic birria taco site
- **Source:** https://guides.docmo.agency/BIRRIA-Guide-EN.pdf
- **Built:** a white-label clone of UMAMI (Vite + React + TS + Tailwind + GSAP). A pinned 1000%-scroll canvas scrub of 488 desktop / 244 mobile frames: chef knife-duel cold open → taco lift with cheese pull → orbit → slow-motion burst into ingredients → reassembly by the consommé → a hand reaching for the bite. Camera HUD. Menu (ADD TO ORDER flies to the configurator), configurator (Duo/Trio/Cuarteto + 6 toppings), overview with a pot loop, stats, 3-stage process reel, layers grid, craft grid, Mexico×USA story, order/reserve form. Live: birria-chi.vercel.app.
- **Tools:** Higgsfield with Kling 3.0 pro (camera beats) + Seedance 2.0 (physics beats) + Nano Banana Pro, ffmpeg, React stack, Vercel.
- **Workflow:**
  1. Script 6 beats: Clash, Lift, Rise, Burst, Settle, Reach.
  2. Make stills with a style reference at 0.25–0.35.
  3. Make the duel in Seedance and the camera beats in Kling, with matching 2700K camera-left light.
  4. ffmpeg concat and re-extract one frame set.
  5. Build the pinned canvas with an `IS_MOBILE` swap.
  6. Build the menu → configurator fly.
  7. Build the other sections.
  8. Use 4 animation layers (canvas, fly clones, one-shot reveals, time-based HUD).
- **Prompts:**
  - Seedance, The Clash:
    (verbatim prompt: see the source PDF)
    (The "ending on a held frame" wording caused the freeze bug. Avoid it.)
  - Kling match-cut transition:
    (verbatim prompt: see the source PDF)
  - Menu 01 Classic Beef:
    (verbatim prompt: see the source PDF)
  - Menu 02 Quesabirria:
    (verbatim prompt: see the source PDF)
  - Menu 03 Consommé Especial:
    (verbatim prompt: see the source PDF)
  - Ingredient, beef:
    (verbatim prompt: see the source PDF)
  - Ingredient, chile guajillo:
    (verbatim prompt: see the source PDF)
- **Fit:** 2/5. Take the food-photo skeleton, the match-cut idea and the menu→configurator fly.

### 4.5 BRACE. Pizza: stone-fired cinematic pizza site
- **Source:** https://guides.docmo.agency/BRACE-Pizza-Guide-EN.pdf
- **Built:** a single HTML file, no build step. A 500vh sticky canvas scrub of 242 JPEG frames (8fps, 1280px, about 13 MB) from 3 Seedance clips (dolly to overhead Margherita, dolly-out with levitation, recession then burst toward the lens). HUD (24fps timecode, brackets, crosshair, scrubber, frame counter). 3 pizza cards with ORDER fly. Konfigurator: SVG concentric-circle size cards (26/30/34cm) with an ember pulse and a ghost pizza, 6 toppings that arc-fly into the price, a punching price, a CM badge. Editorial, stats (485°C/90s/72h/7), The Stone, a dual marquee, craft grid, story archive cards, and reserve (calendar, LUNCH/DINNER, party-size stepper, Web3Forms, confirmation). Live: brace-pizza.vercel.app.
- **Tools:** Higgsfield Seedance 2.0 (3–5 attempts per clip), Nano Banana Pro 2K, GSAP 3.12 (no MotionPathPlugin), ffmpeg, sips (macOS), Web3Forms, Vercel + `.vercelignore`, Cormorant Garamond, Claude agents.
- **Workflow:**
  1. Make stills (4:3 menu, 1:1 ingredients).
  2. Make 3 Seedance clips with the 6-step formula, chained via @-reference.
  3. ffmpeg concat, then extract frames at 8fps and 1280px as JPEG.
  4. Build the 500vh sticky canvas (`frame = round(p × 241)`).
  5. Add the CSS HUD.
  6. Build the ORDER fly (`power3.inOut` 0.7s, scroll after 200ms).
  7. Build the Konfigurator. Toppings are 72×72 fixed clones (`top:0; left:0`) on a two-tween arc: `power2.out` to a midpoint 100px up, then `power2.in`. On landing the price scales to 1.4× with a cream flash and 28px ember glow, then elastics back. CM badge uses `back.out(1.7)` and fades after 2.2s.
  8. Add reveals (y 40→0, stagger 0.1, killed after the first run).
  9. Write `.vercelignore` and deploy.
- **Prompts:**
  - Seedance clip 1:
    (verbatim prompt: see the source PDF)
  - Seedance clip 2 (chained):
    (verbatim prompt: see the source PDF)
  - Seedance clip 3 (contains a contradiction: "pulls back" vs "locked off"):
    (verbatim prompt: see the source PDF)
  - Menu 01 Margherita Verace:
    (verbatim prompt: see the source PDF)
  - Menu 02 Prosciutto e Burrata:
    (verbatim prompt: see the source PDF)
  - Menu 03 Tartufo Nero:
    (verbatim prompt: see the source PDF)
  - Ingredient, San Marzano:
    (verbatim prompt: see the source PDF)
  - Ingredient, fior di latte:
    (verbatim prompt: see the source PDF)
- **Fit:** 3/5. It has the most concrete GSAP numbers in the series, uses a single file and a modest 13 MB scrub. Drop the protected-designation names unless they are true.

### 4.6 BRÛLÉE / RÖSTWERK: one AI sequence used as a reel and a scroll hero
- **Source:** https://guides.docmo.agency/BRULEE-Guide-EN.pdf
- **Built:** a 6-beat coffee story (Glow → Charge → Detonation → Vortex → Pour → Torch, match-cut back to the bean to loop). 13 approved keyframe stills, 12 Kling pro clips, a 4K master. It ships as a 9:16 reel with the bass drop on the detonation, and as a canvas scroll hero (200 WebP frames at 1920px / 120 at 1080px, q95) with a wordmark shatter on entry and a product-line reveal on exit. No animation library. Hero and reel only, not a full site. Live: roestwerk-brulee.vercel.app.
- **Tools:** Nano Banana Pro (16:9, 4 variants each), Kling 3.0 pro, ffmpeg, canvas, vanilla rAF + sticky, Vercel, an unnamed editor.
- **Workflow:**
  1. Write a dead-centre shot list that works for both 16:9 and 9:16.
  2. Make one style-anchor reference image.
  3. Generate stills only, 4 variants each.
  4. Chain each approved still as the base for the next, with explicit reference roles.
  5. For late frames, lock everything and name the one change.
  6. Bridge each pair with a Kling clip (pro, sound off, start and end locked, one move).
  7. QA at 2fps before generating the next clip.
  8. Edit with a sound map and export a 4K master.
  9. Crop 9:16 for the reel and extract the two frame sets.
  10. Build sticky + canvas with DPR capped at 1.5 and a rAF lerp.
  11. Throttle loading with prioritised frames.
  12. Reserve the scroll ends.
- **Prompts:**
  - Keyframe 1A (dormant bean):
    (verbatim prompt: see the source PDF)
  - Keyframe 5E (flame sweep; lock-everything-change-one; partial in the original):
    (verbatim prompt: see the source PDF)
  - Reference-role snippet:
    (verbatim prompt: see the source PDF)
  - Kling settings:
    ```
    model: kling3_0
    mode: "pro"        ← default "standard" = 720p, silently
    sound: "off"
    aspect_ratio: "16:9"
    ```
  - Beat 02, The Charge (8s):
    (verbatim prompt: see the source PDF)
  - Beat 03, Detonation (5s):
    (verbatim prompt: see the source PDF)
  - Snippets: "Camera locked off. Subject moves. Explosive velocity on the burst." / "— already there" / "One single continuous photographic image, NOT a split screen … this is the SAME photograph, only a moment later" / "one unbroken pale stream from the top edge" (name the stream, not the liquid, so no jug gets invented).
- **Fit:** 3/5. The best methodology guide. Scale it down to 2–3 clips and sell "reel + hero from one shoot" as an upsell.

### 4.7 CHANEL N°5: cinematic AI ad (video, not a website)
- **Source:** https://guides.docmo.agency/CHANEL-No5-Guide-EN.pdf
- **Built:** a 12s 9:16 1080p spec ad. 5 Nano Banana keyframes (Liquid Genesis, Flacon Forms, Fills + Drop, Stopper Seals, Hero) and 4 Kling start/end morph clips of 3s each, joined by ffmpeg. ElevenLabs SFX + music bed, then CapCut. An HTML "CINEMATIC AI AD" reel overlay.
- **Tools:** Nano Banana Pro, Kling 3.0 pro, ElevenLabs, ffmpeg, CapCut, Chrome.
- **Workflow:**
  1. Make 5 keyframes at 2K 9:16 with the look locked from the first.
  2. Make 4 chained clips.
  3. Concat with ffmpeg.
  4. Place SFX frame-accurately over a low bed, behind a limiter.
  5. Composite into the HTML overlay inside Instagram safe zones.
- **Prompts** (trademark! Replace the brand and bottle with the client's product):
  - KF01:
    (verbatim prompt: see the source PDF)
  - KF02:
    (verbatim prompt: see the source PDF)
  - KF03–KF05: same structure. KF03 is the full flacon "filled with luminous golden perfume, a single perfume droplet falling toward the open neck", with the label text pinned ("Minimalist label reads "N°5" in clean black sans-serif… Sharp label typography, correct spelling"). KF04 is an extreme close-up of the faceted stopper descending, with condensation sparkles. KF05 is the hero beauty shot of the sealed flacon, centred, medium close-up 85mm, "flawless reflections".
  - Clip 1:
    (verbatim prompt: see the source PDF)
  - Clip 2:
    (verbatim prompt: see the source PDF)
    (Continuity bug: KF2 never shows a stopper.)
  - Clip 3:
    (verbatim prompt: see the source PDF)
  - Clip 4:
    (verbatim prompt: see the source PDF)
    (Verbatim. For reuse, replace "CHANEL N°5" with the client's own product; the brand name is a trademark.)
  - Suffix: "4K, Ultra HD, rich details, sharp clarity, cinematic texture, no blur, no ghosting, stable picture."
  - SFX cue sheet: 0s golden shimmer/whoosh; ~2s soft droplet with reverb tail; ~3s smooth liquid fill; ~6s fine plink into the neck; ~9s crystal glass cling + sparkle (the signature sound); 0–12s warm low ambient pad.
- **Fit:** 2/5. A clean keyframe→morph recipe for a product reel (perfume or oil shop, barber products). It is not about websites.

### 4.8 CHRONOS: skeleton watch scroll microsite
- **Source:** https://guides.docmo.agency/CHRONOS-Guide-EN.pdf
- **Built:** React + Vite, 3 sections. ScrollStage canvas (200 WebP frames, 100 on mobile): push from the watch face into the movement, then an exploded vertical tower of parts; scrolling back reassembles it. HeroOverview specs grid with a GSAP stagger, section status label and progress bar. CaliberBalance (parallax gallery, materials, journal, footer). Live: cinematic-product-template.vercel.app.
- **Tools:** Claude Code (~$20/mo), Nano Banana Pro, Seedance 2.0 via Higgsfield (~45 credits/clip; Plus ~$19/mo), ffmpeg, GSAP, React/Vite, Vercel.
- **Workflow:**
  1. Frame A (hero watch).
  2. Frame B (macro), with A as reference.
  3. Frame C (exploded tower), with B as reference.
  4. Clip A→B and clip B→C in Seedance ("Generate literally", decline presets, start from the exterior shot).
  5. ffmpeg to WebP sets.
  6. Have Claude Code build ScrollStage: `IS_MOBILE` via `matchMedia` once, `Promise.all` preload, canvas hidden until loaded, `pin` + `scrub`.
  7. Build the sections and deploy.
- **Code nuggets:**
  - `ffmpeg -i clip.mp4 -vf "fps=30,scale=1920:-1" -q:v 80 frames/f_%03d.webp` (this gives more frames than 200; adjust fps).
  - `frameIdx = Math.min(Math.floor(progress * FRAME_COUNT), FRAME_COUNT - 1)`
  - `IS_MOBILE = matchMedia('(max-width: 768px)').matches`
- **Prompts:**
  - Claude Code instruction:
    (verbatim prompt: see the source PDF)
  - Frame A:
    (verbatim prompt: see the source PDF)
  - Frame B (A as reference):
    (verbatim prompt: see the source PDF)
  - Frame C (B as reference):
    (verbatim prompt: see the source PDF)
  - Clip 1, push-in:
    (verbatim prompt: see the source PDF)
  - Clip 2, explosion:
    (verbatim prompt: see the source PDF)
- **Fit:** 3/5. The simplest complete scroll-scrub recipe (3 stills, 2 clips). For example, a croissant or layer cake "exploding" for a bakery. Cut it to 60–80 small frames with a poster fallback.

### 4.9 Coca-Cola: cinematic AI ad (video, not a website)
- **Source:** https://guides.docmo.agency/CocaCola-AI-Guide.pdf
- **Built:** a 27s 9:16 ad from 10 keyframes and 9 Kling clips of 3s each: ice fall → cluster → pour → fizz macro → bottle forms → fills → cap burst → label macro → hero → pack-shot. 14 ElevenLabs SFX + a bed, and an HTML reel overlay. The same system as CHANEL, scaled up.
- **Tools / workflow:** as CHANEL. Mixing spec: bed at −18 dB or lower with fades, master limiter at −1 dBTP.
- **Prompts** (trademarked brand: swap in a generic or client product):
  - KF01:
    (verbatim prompt: see the source PDF)
  - KF03 (pour):
    (verbatim prompt: see the source PDF)
  - KF04 (fizz):
    (verbatim prompt: see the source PDF)
  - KF02 and KF05–KF10: same structure (subject + macro 85mm + IOR + near-black void + caramel-amber backlight + warm upper-left key + cool right rim + hyperreal + 9:16). Their subjects are the ice cluster, bottle rising from the splash, bottle filling with the cap seated, cap burst with a spray crown, label macro with frost, hero with a splash crown, and a pack-shot on a dark reflective surface with a red splash ribbon.
  - Clips (start + end, 3s, pro, 1080p, sound off). The structure is one move + "then settles and holds":
    (verbatim prompt: see the source PDF)

    Clips 4, 5 and 7–9 follow the same pattern (bottle rises → fills → into the label → hero pull-back → pack-shot settle). Add the CHANEL quality suffix to each.
- **Reusable arc for a café:** ingredient → liquid → texture macro → product forms → climax → brand detail → hero → pack-shot.
- **Note:** the guide's own errors include ice at IOR 1.5 (real value about 1.31) and the cap-burst SFX timed at 14s while the clip runs 15–18s.
- **Fit:** 2/5.

### 4.10 Cosmos Engine: interactive constellation canvas
- **Source:** https://guides.docmo.agency/Cosmos-Guide-EN.pdf
- **Built:** a full-screen Canvas 2D starfield with no WebGL and no libraries. Hovering activates one of 9 constellations; clicking plays a full-screen reveal (name, myth, distance, brightest star…) in one of 5 styles: Starfire Write, Signal Scan, Shockwave Rings, Supernova, Northern Lights. Fonts: Cinzel + Cormorant Garamond. Live: cosmos-demo-docmo-s-projekt.vercel.app.
- **Tools:** Claude Code, Canvas 2D, rAF, Google Fonts, Vercel.
- **Workflow:**
  1. Describe the engine to Claude.
  2. Store constellations as data (fractional coordinates + edges array) with a separate INFO object.
  3. Run one rAF loop as a state machine (normal / reveal).
  4. Build hover activation and a click threshold.
  5. Build 5 progress-driven reveal styles, one picked at random per session.
  6. Fix the canvas pitfalls.
- **Prompts:** none given.
- **Lessons:** `draw(){ requestAnimationFrame(draw); try{…}catch(e){…} }` (log the error, don't swallow it); pair `save()`/`restore()`; paint an opaque base under overlays; add a dismiss cooldown. There is no touch support, because it depends on hover.
- **Fit:** 2/5. Inspiration only.

### 4.11 ECHOES: alpha-video character select
- **Source:** https://guides.docmo.agency/ECHOES-Guide-EN.pdf
- **Built:** a single HTML file (~800 lines). 5 fighters as looping transparent videos over a Torii-gate arena loop at 30% brightness. Info panel with stat bars, HUD list, GSAP crossfades. Inactive videos are frozen at frame 0. VP9 WebM + HEVC MOV, 10 files, about 21 MB. Live: echoes-character-select.vercel.app.
- **Tools:** Seedance 2.0, Kling 3.0 (NSFW-filter fallback), Higgsfield Remove Background, optional Premiere Ultra Key, ffmpeg/ffprobe (a Mac is needed for VideoToolbox HEVC alpha), GSAP, Claude Code, Vercel.
- **Workflow:**
  1. Generate each character on **pure white**: static camera, full body, feet planted, 3–5 iterations.
  2. Run Remove BG with a "Subject to keep" description that names every prop and glow as "SOLID PHYSICAL… PART OF THE CHARACTER". You get a colour video and a B/W mask.
  3. ffmpeg: `alphamerge`, erode 2px, `geq` white despill, then ProRes 4444 (`yuva444p12le`).
  4. Encode VP9 WebM (`libvpx-vp9`, `yuva420p`, about 900 KB) and HEVC (`hevc_videotoolbox -tag:v hvc1 -allow_sw 1 -alpha_quality`, about 1.8 MB).
  5. Embed with two `<source>` tags.
  6. Build the UI and test in Chrome and Safari.
- **Prompts:**
  - Seedance, ANUBIS (template for any cut-out subject):
    (verbatim prompt: see the source PDF)
  - KHAOS and MORRIGAN (Seedance): same structure (framing → white seamless → static camera → costume → slow gesture building to an energy peak → feet planted → full body visible → suffix).
  - RYUJIN (Kling, used when the Seedance filter blocked the sword raise):
    (verbatim prompt: see the source PDF)
  - Remove BG "Subject to keep" (ZEPHYR; the pattern is to list every element and mark props as solid):
    (verbatim prompt: see the source PDF)
  - MORRIGAN "Subject to keep": same pattern (the spear is a "SOLID PHYSICAL LONG SPEAR… ICE BLUE GLOWING CRYSTAL SPEARHEAD… SOLID POINTED BLADE… energy beam… PART OF THE WEAPON keep it entirely").
  - HTML:
    ```html
    <video loop muted playsinline>
      <source src="char-alpha-opt.webm" type="video/webm; codecs=vp9">
      <source src="char-alpha.mov" type="video/mp4; codecs=hvc1">
    </video>
    ```
- **Fit:** 2/5. The alpha pipeline (a floating cut-out barber or chef) is the useful part, but it is heavy and needs paid tools and a Mac.

### 4.12 IVORY DENTAL: "pick your dentist" clinic
- **Source:** https://guides.docmo.agency/IVORY-Guide-EN.pdf
- **Built:** a single HTML file, amber on graphite. Hero: the AUREA marble-and-gold molar in an 8s Kling 360° orbit. A 5-doctor height-staged character select (arrows, 01/05, info panel with stat bars, full-screen story overlay with a Book button). Team and tech grids, specialties, journey + stats, before/after gallery, testimonial carousel, booking calendar + Web3Forms. 7 themes in code (AMBER ships). Heavy variant 2.2 MB / Lighthouse 99 / LCP 1.6s; clean variant 0.75 MB. Live: ivory-tooth.vercel.app.
- **Tools:** Nano Banana Pro, Kling 3.0, GSAP 3.12.5, Web3Forms, Vercel, sips.
- **Workflow:**
  1. Concept.
  2. Make 5 portraits + 5 action shots with one rig (loupes on a neck strap).
  3. Explore 5 hero concepts and pick AUREA.
  4. Kling orbit, graded per theme.
  5. Build the single-file site.
  6. 4 animation layers (click stage, scroll reveals, overlay, first-visible counters and bars).
  7. Preload roster images.
  8. Web3Forms.
  9. Performance pass with 2 variants.
  10. Deploy and check the alias isn't behind Deployment Protection.
  11. Audit CSS/JS class mismatches, nav selector scope, and aspect-ratio vs width/height attributes.
- **Prompts:**
  - Portrait 01:
    (verbatim prompt: see the source PDF)
  - Portrait 02:
    (verbatim prompt: see the source PDF)
  - Action 03:
    (verbatim prompt: see the source PDF)
  - Hero object (the "trade object as jewellery" idea; works for scissors, a croissant or a coffee cup):
    (verbatim prompt: see the source PDF)
  - Kling orbit brief:
    (verbatim prompt: see the source PDF)
    (A 360° orbit from one image morphs at the back.)
- **Fit:** 4/5. The best clinic or barber template, and it has real performance numbers. Use real staff photos.

### 4.13 NEXUS Logistics: one-truck scroll film
- **Source:** https://guides.docmo.agency/NEXUS-Guide-EN.pdf
- **Built:** Vite + React 19 + TS + Tailwind 4 + GSAP. Sound gate. A pinned canvas hero (200 desktop / 100 mobile WebP) from a 66s film of 8 chained clips: depot at night → overhead rise → roofs turn into cyan contour lines → dive through the wall → belt → dock at dusk → van → doorstep. Stat strip labelled as fictional, 4 shipment cards, an "In transit" pinned gauge, an SVG tracking demo with a moving dot and ETA, a timeline built from hero frames, and a three.js truck (cut from 91 MB to 7 MB). Live: nexus-logistics-inky.vercel.app.
- **Tools:** the React stack, unnamed image/video/3D tools, three.js, Vercel.
- **Workflow:**
  1. Plan one physical camera path.
  2. Make 9 keyframes. Edit the previous image for small steps; for big jumps use it as a style-only reference with "do not copy framing"; drop the reference if the same flaw repeats twice.
  3. Diff the edits.
  4. Make 8 start/end clips with physical camera actions.
  5. Pixel-diff to detect fake motion.
  6. Extract the frame sets. Mobile stays landscape with a contain/cover blend.
  7. Size the canvas from its wrapper.
  8. ScrollTrigger `+=600%`, `scrub 0.4`, about 27px per frame, checked by a unit test.
  9. Fix device bugs (hysteresis, z-index curtain, `from()`, CSS vs GSAP).
- **Prompts:** none.
- **Fit:** 2/5. Use it for its engineering lessons (see 2.1 and 2.6).

### 4.14 NOIR BREW: "Coffee Drift" product slider
- **Source:** https://guides.docmo.agency/NOIR-BREW-Guide-EN.pdf
- **Built:** a full-screen slider for 4 coffee products (Espresso Nero, Cold Brew Reserve, Caramel Noir, Matcha Fusion). Colour-coded backgrounds, a giant word flipping on Y, the product spinning in, corner ingredient floats exiting via ghost clones. Arrows, keyboard and swipe with a 2.2s lock.
- **Tools:** Claude Code, GSAP, Nano Banana Pro, rembg (free), vanilla JS, Vercel.
- **Workflow:**
  1. Reuse the engine.
  2. Prompt Claude in plain language.
  3. Make 4 hero shots and 6 floats ("clean edges for cutout").
  4. Cut out with rembg.
  5. Build the 3 GSAP layers (values in 2.1).
  6. Float duration ×1.25.
  7. Add navigation and the lock.
  8. Deploy.
- **Prompts:**
  - Claude Code:
    (verbatim prompt: see the source PDF)
  - Formula: see 2.3 (product shot).
  - Espresso Nero:
    (verbatim prompt: see the source PDF)
  - Cold Brew Reserve:
    (verbatim prompt: see the source PDF)
  - Caramel Noir:
    (verbatim prompt: see the source PDF)
  - Matcha Fusion:
    (verbatim prompt: see the source PDF)
  - Floats (Nano Banana → rembg):
    (verbatim prompt: see the source PDF)
  - Cutout tip: translucent subjects (ice, glass, splash, steam) cut out badly with rembg. Use a contrasting plain background, or keep those as full images.
- **Fit:** 3/5. Directly reusable for a café or bakery "menu highlights" section (as one section, not the whole site).

### 4.15 Nike: sneaker slider (re-skin of NOIR BREW)
- **Source:** https://guides.docmo.agency/Nike-Guide-EN.pdf
- **Built:** the Coffee Drift engine with 4 real Nike sneakers (Jordan 1 Chicago #C41E3A, AM95 Neon #D4E842, Dunk Panda #1A5FA8, AM1 Anniversary Red #E8D5A3) and 3 "anatomical" floats per shoe, 16 assets in total. Dark UI on the light slides. Live: nike-slider.vercel.app.
- **Tools:** as NOIR BREW, plus Python/PIL BFS flood fill.
- **Workflow:**
  1. Make heroes at 2K 1:1 with the universal orientation rule.
  2. Make 12 floats: white background for solid objects, #1a1a1a for laces.
  3. Run rembg, then BFS flood fill from inside the enclosed holes to clear background trapped in lace loops.
  4. Colourway backgrounds.
  5. 3 GSAP layers (timings withheld).
- **Prompts** (all trademarked; reuse only the structure):
  - Universal rule:
    (verbatim prompt: see the source PDF)
  - Hero example (Jordan 1):
    (verbatim prompt: see the source PDF)
  - Other heroes (AM95 Neon, Dunk Low Panda, AM1 Anniversary Red): same structure ([model + colourway + materials] + universal rule + [material details]). Product errors in the guide: the Panda has no gum sole, and the AM95 Neon is mostly grey.
  - Float on white (template):
    (verbatim prompt: see the source PDF)
  - Lace float on dark (template, example):
    (verbatim prompt: see the source PDF)
  - Remaining floats (Wings badge, tongue label, Air unit, Swooshes in different finishes, ankle collar, heel counter) follow these two templates.
- **Fit:** 2/5. Use it for the prompt-engineering discipline only. The brand is a legal no-go.

### 4.16 Obsidian Riviera: supercar assembles mid-drive (hero video only)
- **Source:** https://guides.docmo.agency/Obsidian-Hero-Guide-EN.pdf
- **Built:** a 22s autoplay background hero video. 5 Kling clips from 7 keyframes: wheels with light trails form a wireframe chassis that skins into a car mid-drive → coast drive → cockpit POV → villa arrival + fleet pass → crane-up aerial. The site opens on the last frame. Live: obsidian-motors-riviera-scrollstage.vercel.app.
- **Tools:** Nano Banana Pro, Kling 3.0 (start/end frames), Higgsfield, CapCut (4K/60 HEVC master), ffmpeg (1080p/30 H.264, about 16 MB), Vercel.
- **Workflow:**
  1. Plan 6 story moments in 5 clips.
  2. Make a reference anchor (the car pinned into a forecourt plate).
  3. Make 7 keyframes.
  4. Make 3–5s Kling clips with start/end frames.
  5. Chain last frames.
  6. Keep direction left-to-right.
  7. CapCut: cut on matching motion, one grade, speed-ramp the end, about 22s loopable.
  8. Keep the master; export the web transcode.
- **Prompts** (prompt structure: subject + action, place + time + weather, camera + direction, motion blur, "4K, cinematic"; car brands are trademarks):
  - Clip 1, build:
    (verbatim prompt: see the source PDF)
  - Clip 2 ("[car]" stands in for "Lamborghini Huracán" in the original): "The assembled matte-black [car] tearing down a cliff coastal road, front three-quarter tracking shot, guardrail and turquoise sea behind, dusk sky, motion blur on the road. Camera matches car speed, stable frame. 4K, cinematic."
  - Clip 5, aerial:
    (verbatim prompt: see the source PDF)
  - KF01:
    (verbatim prompt: see the source PDF)
  - KF02 ("[car]" = "Lamborghini Huracán" in the original): "Matte-black [car] driving toward camera on a Mallorca cliff road, front three-quarter angle, guardrail and rocky coast behind, dusk sky, headlights on. Sharp on the car, motion blur on the background. 4K, cinematic."
  - KF05: "Wide shot of a full fleet of supercars lined up on a sunlit clifftop villa forecourt over the turquoise Mediterranean, palms and cliffs, golden hour, warm even light. All cars sharp and consistent, one plate. 4K, cinematic."
- **Web encode it should have given:** `ffmpeg -i master.mov -c:v libx264 -crf 23 -vf scale=-2:1080 -r 30 -an -movflags +faststart hero.mp4`. Also add a poster image, a smaller mobile encode, `muted playsinline`, and `prefers-reduced-motion` handling.
- **Fit:** 2/5. Reuse the editing rules (match motion, one grade, loop).

### 4.17 SMASH: burger explodes and lands assembled
- **Source:** https://guides.docmo.agency/Smash-Guide-EN.pdf
- **Built:** a 12-section React 19 + Vite + Tailwind 4 + GSAP 3.15 burger site. Canvas hero (200 desktop / 100 mobile WebP) from 8 Kling clips: gloved hand throws a beef ball → spatula smash → sear macro → cheese melt → pull-back to staged ingredients → toss → mid-air explosion → reassembled burger. Menu, configurator with fly-to-price, overview, stats (120g / 8s / 230°C), The Sear, ingredients, materials, story, reserve calendar, footer. The sound gate was replaced by "Scroll Now". `public/` pruned from 119 MB to 30 MB. Live: smash-burger-omega.vercel.app.
- **Tools:** Higgsfield Kling 2.x, Nano Banana Pro, React stack, Vercel.
- **Workflow:**
  1. One continuous beat.
  2. 9 keyframes, showing the hand only.
  3. 8 chained clips.
  4. A continuity pull-back keyframe.
  5. Anti-duplication explosion prompt.
  6. Two WebP sets.
  7. Clone the template.
  8. Canvas scrub.
  9. Configurator.
  10. One-shot reveals.
  11. Remove audio.
  12. Prune assets.
  13. Audit template leftovers.
- **Prompts:**
  - Clip 1:
    (verbatim prompt: see the source PDF)
  - Clip 2:
    (verbatim prompt: see the source PDF)
  - Clip 7:
    (verbatim prompt: see the source PDF)
  - KF 03A:
    (verbatim prompt: see the source PDF)
  - KF 06:
    (verbatim prompt: see the source PDF)
  - KF 07:
    (verbatim prompt: see the source PDF)
- **Fit:** 4/5 for the *idea* (food scroll hero) and the hand-anchor and anti-duplication tricks. The scope is too big for ₪500–1500, so cut it down.

### 4.18 Sultan Al-Oud: oud-oil ritual ad (video, not a website)
- **Source:** https://guides.docmo.agency/Sultan-Al-Oud-Guide-EN.pdf
- **Built:** an 18s 9:16 reel in 3 beats (oil drop on the palm → hands anoint the beard → confident reveal with the bottle). 3 keyframes, 3 Kling pro clips (5s / 8s / 5s), hard-cut on every beat of a CapCut violin track, HTML overlay. There is also an unprompted 8-keyframe product-only spot.
- **Tools:** Nano Banana Pro (character sheet + bottle lock), Kling 3.0 pro 1080p, CapCut, ffmpeg, Chrome/HTML.
- **Workflow:**
  1. Ground the concept in a real ritual and check it reads as dignified.
  2. Make a 4-panel character sheet.
  3. Lock the bottle with a hero shot.
  4. Make 3 keyframes referencing both.
  5. Write timeline motion prompts with one impact beat each.
  6. CapCut: put a marker on every beat, hard-cut on the markers, hold through the swell.
  7. Normalise to 1080×1920.
  8. Overlay and post.
- **Prompts:**
  - KF01:
    (verbatim prompt: see the source PDF)
  - KF02:
    (verbatim prompt: see the source PDF)
  - KF03:
    (verbatim prompt: see the source PDF)
  - Clip 1:
    (verbatim prompt: see the source PDF)
  - Clip 2:
    (verbatim prompt: see the source PDF)
  - Clip 3:
    (verbatim prompt: see the source PDF)
- **Fit:** 3/5. It maps almost directly onto a **barbershop beard-oil reel or hero**. Check music licensing; a royalty-free track is safer.

### 4.19 UMAMI: cinematic ramen site (the base template for the food series)
- **Source:** https://guides.docmo.agency/UMAMI-Guide-EN.pdf
- **Built:** Vite + React + TS + Tailwind + GSAP, 12 components. ScrollStage (289 desktop / 145 mobile WebP) in 3 acts (Serve, Stir, Burst) with a camera HUD. Menu (3 bowls, ADD TO ORDER fly), configurator (3 sizes, 6 toppings, itemised live price, arc fly + punch), HeroOverview (6 specs), stats, a pinned Simmer gauge 0→98°C with a background crossfade, a 9-ingredient bento with a kanji watermark, a horizontal pinned craft gallery, a 1910→today story with flags, reserve (calendar, slots, stepper, Web3Forms, confirmation). Live: umami-ramen-three.vercel.app.
- **Tools:** Higgsfield Kling 3.0 + Nano Banana Pro (reference at 0.25–0.35), ffmpeg, GSAP, React stack, Web3Forms, Vercel.
- **Workflow:**
  1. Make a frozen hero-bowl still.
  2. Make 7 chained Kling beats (Serve → Stir → Overhead → Explosion → Orbit → Impact → Catch).
  3. Concat and extract 2 WebP sets.
  4. Make menu and ingredient stills from one reference.
  5. 12 components.
  6. ScrollStage with a bounded preload pool + on-demand landed frame + `matchMedia`.
  7. HUD gated by IntersectionObserver.
  8. Configurator fly with `getBoundingClientRect` and the punch fired from `onComplete`.
  9. One-shot reveals.
  10. Gauge via the scroll proxy.
  11. Reserve + Web3Forms.
  12. Deploy and verify the alias.
- **Prompts:**
  - Act 01, THE SERVE (Kling):
    (verbatim prompt: see the source PDF)
  - Act 02, THE STIR:
    (verbatim prompt: see the source PDF)
  - Act 03, THE BURST:
    (verbatim prompt: see the source PDF)
  - 01 Hero bowl:
    (verbatim prompt: see the source PDF)
  - 02 Tonkotsu:
    (verbatim prompt: see the source PDF)
  - 03 Spicy miso:
    (verbatim prompt: see the source PDF)
  - 04 Shoyu:
    (verbatim prompt: see the source PDF)
  - Chashu ingredient:
    (verbatim prompt: see the source PDF)
- **Kling notes:** the word "turntable" makes Kling spin the subject instead of orbiting the camera. A pure black background gives no parallax. Kling barely moves objects between two very different keyframes.
- **Fit:** 2/5 as a whole. The configurator and fly patterns are good.

### 4.20 EXPLOSION / PRIME – THE STACK: burger site, desktop + mobile, one codebase
- **Source:** https://guides.docmo.agency/explosion-guide.pdf
- **Built:** 7 sections: LINE-UP (3 burgers on rotating 3D cards), BUILD (canvas scrub of about 200 frames + scroll-synced sound cues), THE STACK (layer anatomy reveal), SEAR (SVG thermometer to 260°C via `stroke-dashoffset`), CRAFT (click-to-build configurator), CUT (horizontal ingredient strip, native swipe on mobile), STORY + contact CTA. Plus 2 short Kling/Seedance clips.
- **Tools:** Claude Code (~$20/mo), Higgsfield Nano Banana Pro (~70 credits for 10 photos, High Fidelity on), Kling/Seedance (~12 credits per clip), GSAP, React, canvas, SVG, .mp3 cues, Vercel, domain (~$12/yr).
- **Workflow:**
  1. Make all photos with the universal formula.
  2. Make 2 start/end clips.
  3. Make about 200 frames (the method is not explained; use the villa ffmpeg method).
  4. Have Claude Code build ScrollStage: preload, `window` scroll listener, `frameIndex = round(progress × (total−1))`, cover-fit canvas at 100vw×100vh.
  5. Add sound cues at frame thresholds (they need a user-gesture gate).
  6. Build the GSAP sections.
  7. Build responsive from the start.
  8. Deploy.
- **Prompts:**
  - Universal formula:
    (verbatim prompt: see the source PDF)
  - THE SMOKE:
    (verbatim prompt: see the source PDF)
  - THE CLASSIC:
    (verbatim prompt: see the source PDF)
  - THE TRUFFLE:
    (verbatim prompt: see the source PDF)
  - THE PRIME:
    (verbatim prompt: see the source PDF)
  - Cheese pull (edited: the original has "food porn aesthetic," between "extreme macro," and "4K,"; it is removed below because it can trip filters):
    (verbatim prompt: see the source PDF)
  - Explosion frame:
    (verbatim prompt: see the source PDF)
  - Overhead flat lay:
    (verbatim prompt: see the source PDF)
  - Sear:
    (verbatim prompt: see the source PDF)
  - Gold pedestal:
    (verbatim prompt: see the source PDF)
  - Ingredient cards (universal formula, same surface and light):
    (verbatim prompt: see the source PDF)
- **Kosher note for Israel:** cheese + beef and bacon are not kosher. Rewrite these for kosher clients.
- **Fit:** 3/5. The most copy-paste photo prompts in the series, plus cheap wow-sections (SVG gauge, layer reveal, configurator).

### 4.21 VILLA SCROLL: luxury real-estate scroll tour
- **Source:** https://guides.docmo.agency/villa-guide.pdf
- **Built:** a pinned 300-frame canvas tour through 6 spaces with progress windows (exterior 0–9%, corridor 9–19%, lounge 19–28%, kitchen 28–47%, staircase 47–61%, pool 61–100%) and labels per window. A "Sound ON" gate with an ambient drone. Stats counter, film gallery, material tabs (marble, oak, travertine), contact CTA. Live: villa-scroll-demo.vercel.app.
- **Tools:** Claude Code (~€18/mo), Nano Banana Pro (~60 credits for 9 photos), Seedance 2.0 (exteriors) / Kling 3.0 (interiors) (~8–15 credits per clip), ffmpeg, GSAP, canvas, React, Vercel.
- **Workflow:**
  1. Reject presets. Use 16:9 for frames and 4:3 for gallery cards. Make 3–5 variants, pick the sharpest, and chain it as a reference.
  2. Keyframe pairs: same angle and light, only the camera position changes.
  3. Clips: one camera move each, no cuts, no shake.
  4. ffmpeg concat (normalise resolution, fps and SAR first) + frame extraction.
  5. `ls | wc -l` = 300.
  6. ScrollStage.
  7. Labels per window.
  8. Sound gate.
  9. Sections.
  10. Deploy.
- **ffmpeg (the most complete commands in the series):**
  ```bash
  ffmpeg -i clip1.mp4 -i clip2.mp4 -i clip3.mp4 \
    -filter_complex "[0][1][2]concat=n=3:v=1:a=0" combined.mp4
  ffmpeg -i combined.mp4 -vf "fps=10,scale=1920:-1" \
    -q:v 3 public/frames/villa/f_%03d.jpg
  ls public/frames/villa/ | wc -l
  ```
  Before the concat, normalise each input, e.g. `-vf "scale=1920:1080,fps=24,setsar=1"`. Frames = total seconds × fps. Also make a 1080px mobile set (`scale=1080:-1`) and consider WebP.
- **Prompts:**
  - 01 Exterior:
    (verbatim prompt: see the source PDF)
  - 02 Lounge:
    (verbatim prompt: see the source PDF)
  - 03 Kitchen:
    (verbatim prompt: see the source PDF)
  - 04 Pool:
    (verbatim prompt: see the source PDF)
  - 05 Terrace:
    (verbatim prompt: see the source PDF)
  - 06 Drone:
    (verbatim prompt: see the source PDF)
  - 07 Marble macro:
    (verbatim prompt: see the source PDF)
  - 08 Oak and 09 Travertine: same macro structure ("Close-up macro studio shot of [material], [tone/finish/grain]. Soft diffused light from [dir]. 100mm macro lens, Phase One. Pure material sample photography, ultra-detailed, photorealistic. [Material] fills the entire frame edge to edge.").
  - Clip (Kling, interior):
    (verbatim prompt: see the source PDF)
- **Fit:** 3/5. The most actionable pipeline. A push-in through a real barbershop or bakery interior works best when it is built from the client's own photos.

---

## 5. Quick-pick for a ₪500–1500 Israeli small-business site

- **Barber:** IVORY/APEX character-select ("choose your barber") with real photos + Sultan Al-Oud beard-oil reel technique for one hero clip + WhatsApp booking. RTL.
- **Café/bakery:** NOIR BREW slider as one "menu highlights" section, the explosion universal photo formula for consistent product shots (AI-enhanced real photos), optional ATLAS ping-pong loop or a light CHRONOS-style scrub (60–80 frames, poster fallback).
- **Eatery:** BRACE single-file patterns (menu cards, simple configurator) at a reduced scope. Skip the 200-frame hero unless sold as an upsell. Add a Wolt link, opening hours, a map and WhatsApp.
- **Always:** Hebrew RTL, IS 5568 basics (`prefers-reduced-motion`, alt text, keyboard focus, an accessibility statement page), page weight under ~3 MB, hosting that allows commercial use on a free tier (Cloudflare Pages / Netlify), no trademarks, no fake staff, reviews or stats.
