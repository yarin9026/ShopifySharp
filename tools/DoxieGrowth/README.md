<div dir="rtl">

# DoxieGrowth: a free growth tool for Doxie Den

## Why this tool exists (store diagnosis, 10/10/2026)

| Metric | Value |
|---|---|
| Visits in the last 90 days | **56** |
| Add to cart | 1 |
| Orders | 0 |
| Active products | 6 (good SEO, 20 images each) |
| Draft products | 6 (invisible to customers) |

**The problem isn't the products. It's traffic.** 56 visits in 3 months is basically zero. At a normal conversion rate (1–2%), you need about 100 visits to get 1–2 sales. So the tool focuses on the cheapest traffic source with the best fit for your niche: **Pinterest**. It's free, people go there specifically to look for gift ideas, the "dachshund mom/dad" niche is strong there, and right now (October to December) is peak Christmas gift search season. You already have two Christmas sweatshirts.

## What the tool does

| Command | What it does |
|---|---|
| `audit` | Scans all products and reports what's hurting traffic: drafts, missing SEO, images without alt text, short descriptions |
| `fix-seo` | Writes SEO titles and descriptions and image alt text (Google Images) automatically. Preview by default; `--apply` saves |
| `pinterest` | Creates a CSV file for Pinterest bulk upload: one pin per image (up to 200), varied titles, hashtags, links with UTM tracking, and a schedule of 3 pins a day |

On your current store, `pinterest` creates **120 pins**, which is about 40 days of content on autopilot, uploaded in one click.

## Cost: $0

- The code runs on **GitHub Actions** (free) without installing anything on your computer.
- Pinterest Business account: free.
- Shopify custom app: free.

---

## Setup (one time, about 10 minutes)

### 1. Create an app in Shopify to get an API key
1. Shopify admin → **Settings → Apps and sales channels → Develop apps** (which redirects to the Shopify Dev Dashboard).
2. Create a new app named `DoxieGrowth`.
3. Under **Admin API scopes**, select: `read_products`, `write_products`, `read_files`, `write_files`.
4. Install the app on the Doxie Den store.
5. Copy the **Client ID** and **Client Secret**. The tool uses them to generate a temporary token on every run.

### 2. Add the keys to GitHub
In this repo: **Settings → Secrets and variables → Actions → New repository secret**. Add three:

| Name | Value |
|---|---|
| `SHOPIFY_SHOP` | `gsi1if-1h.myshopify.com` |
| `SHOPIFY_CLIENT_ID` | the Client ID from step 1 |
| `SHOPIFY_CLIENT_SECRET` | the Client Secret from step 1 |

> ⚠️ Never put these keys in the code or in a commit. Only in Secrets.

### 3. Pinterest
1. Open a **Pinterest Business** account (free) at business.pinterest.com.
2. Claim your site (Settings → Claimed accounts), so pins show your logo and you get analytics.
3. Create a board named **Dachshund Shirts & Gifts**. Optionally add another board, **Dachshund Christmas Gifts**.

---

## Running it

GitHub → **Actions** tab → **DoxieGrowth** → **Run workflow** → choose a command:

1. `audit`: see what's wrong.
2. `fix-seo (preview)` → if it looks good → `fix-seo --apply`.
3. `pinterest` → when it finishes, download the `pinterest-pins` artifact at the bottom of the page.
4. Pinterest → **Create → Create Pins in bulk → Upload .csv** → choose the file. Done.

Or run it locally (requires .NET 8):

```bash
cd tools/DoxieGrowth
export SHOPIFY_SHOP=gsi1if-1h.myshopify.com SHOPIFY_CLIENT_ID=... SHOPIFY_CLIENT_SECRET=...
dotnet run -- audit
dotnet run -- pinterest --per-day 4 --start 2026-10-12
```

---

## 30-day action plan (everything free)

**Week 1**
- [ ] Setup (above) + `fix-seo --apply` + upload the pins to Pinterest.
- [ ] **Google & YouTube** app in Shopify (free) → enable **Free listings** in Google Merchant Center. Your products will appear in the Google Shopping tab at no cost.
- [ ] The 6 drafts: their descriptions start with a size chart instead of copy that sells. Write 2–3 sentences for each (like the active products), then publish them. 6 more products means more pins and more chances.

**Weeks 2–4**
- [ ] TikTok / Instagram Reels: short videos of a dachshund + the shirt (Printify mockups + trending sound). One video a day. Organic, no ad budget.
- [ ] Facebook groups for dachshund owners (there are groups with hundreds of thousands of members). Share real value and cute photos, not just links. Check each group's rules on promotion.
- [ ] Re-run `pinterest` every month with a new `--start`, so there's always fresh content.

**Track results:** in Shopify → Analytics → Sessions by referrer, look for `pinterest`. Every pin link has `utm_content=pinN`, so you can see which pin brings sales.

> **An honest note:** no tool guarantees money. What this tool does is remove the main bottleneck (no traffic) at zero cost. Pinterest usually takes 4–8 weeks to "warm up". That's why it's important to start now, before the Christmas season (last order for Christmas: around December 12).

</div>
