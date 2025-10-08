# Quick Start Guide

## What This Test Does

This implementation tests the behavior of three Umbraco redirect methods when working with virtual pages created by custom Content Finders:

1. `RedirectToCurrentUmbracoPage()` - Redirects to the actual content item
2. `RedirectToCurrentUmbracoUrl()` - Redirects to the current URL path  
3. `CurrentUmbracoPage()` - Returns view without redirecting

## Setup Steps

### 1. Build and Run
```powershell
dotnet build
dotnet run
```

### 2. Create the Template
1. Log into Umbraco backoffice (usually https://localhost:5001/umbraco)
2. Go to **Settings** → **Templates**
3. Create a new template called **VirtualTest**
4. Save it

### 3. Test It
1. Navigate to `/virtual-test` in your browser
2. You'll see a test page with three forms
3. Submit each form and observe:
   - Where you end up (URL)
   - Whether the page reloads
   - What the result message says

## Expected Results

| Method | Behavior | Final URL | Content Finders Used? |
|--------|----------|-----------|---------------------|
| `RedirectToCurrentUmbracoPage()` | Redirects to content's actual URL | `/` (home page) | ❌ No |
| `RedirectToCurrentUmbracoUrl()` | Redirects to current path | `/virtual-test` | ✅ Yes |
| `CurrentUmbracoPage()` | Returns view, no redirect | `/virtual-test` | ❌ No |

## Why This Matters

This addresses the question from [Umbraco Forms Issue #1456](https://github.com/umbraco/Umbraco.Forms.Issues/issues/1456) about whether `RedirectToCurrentUmbracoPage()` should respect Content Finder rules and virtual pages.

**Key Finding:** `RedirectToCurrentUmbracoPage()` redirects to the **resolved content item**, NOT the current URL. If you need to stay on a virtual page after a form post, use `RedirectToCurrentUmbracoUrl()` instead.

## Files to Review

- **ContentFinders/VirtualPageContentFinder.cs** - Creates the virtual page
- **Controllers/RedirectTestSurfaceController.cs** - Test methods
- **Views/VirtualTest.cshtml** - Test interface
- **REDIRECT_TESTING_README.md** - Full documentation
