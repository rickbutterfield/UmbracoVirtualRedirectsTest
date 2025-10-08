# Umbraco Redirect Methods Testing

This project includes a test setup to verify the behavior of different redirect methods in Umbraco when used with Content Finders and virtual pages, specifically addressing [this GitHub issue discussion](https://github.com/umbraco/Umbraco.Forms.Issues/issues/1456).

## What's Been Implemented

### 1. Custom Content Finder (`VirtualPageContentFinder.cs`)
- Creates a virtual page at `/virtual-test`
- The URL is `/virtual-test` but it serves content from the home page
- Uses the custom `VirtualTest.cshtml` template
- Located in: `ContentFinders/VirtualPageContentFinder.cs`

### 2. Surface Controller (`RedirectTestSurfaceController.cs`)
A test controller with three POST methods to test different redirect behaviors:
- `TestRedirectToCurrentUmbracoPage()` - Tests `RedirectToCurrentUmbracoPage()`
- `TestRedirectToCurrentUmbracoUrl()` - Tests `RedirectToCurrentUmbracoUrl()`
- `TestCurrentUmbracoPage()` - Tests `CurrentUmbracoPage()`

Located in: `Controllers/RedirectTestSurfaceController.cs`

### 3. Test View (`VirtualTest.cshtml`)
- Interactive test page with three forms
- Shows current URL, content information, and whether it's a virtual page
- Displays results after form submission
- Located in: `Views/VirtualTest.cshtml`

### 4. Composer (`VirtualPageComposer.cs`)
- Registers the custom content finder with Umbraco
- Located in: `Composers/VirtualPageComposer.cs`

## How to Test

### Step 1: Create the Template in Umbraco
Before testing, you need to create the template in the Umbraco backoffice:

1. Run the application: `dotnet run`
2. Log into the Umbraco backoffice
3. Go to **Settings** → **Templates**
4. Create a new template called **VirtualTest**
5. The template should already exist as a file (`Views/VirtualTest.cshtml`), so Umbraco should recognize it
6. Make sure the template is saved and available

### Step 2: Access the Virtual Page
1. Navigate to `/virtual-test` in your browser
2. You should see the test page with current page information
3. Notice that:
   - The URL is `/virtual-test`
   - The content is from the home page
   - The page shows "Is Virtual Page: Yes"

### Step 3: Test Each Redirect Method

#### Test 1: RedirectToCurrentUmbracoPage()
**Expected Behavior:**
- Should redirect to the **published content item's URL** (the home page URL, likely `/`)
- Does **NOT** go through content finders again
- You should end up at the actual home page, not the virtual page
- URL should change to `/` (or the home page URL)

**How to Test:**
1. Click "Submit" on the first form
2. Observe the URL after redirect
3. Check the result message

#### Test 2: RedirectToCurrentUmbracoUrl()
**Expected Behavior:**
- Should redirect to the **current path** (`/virtual-test`)
- **DOES** go through content finders again
- You should stay at the virtual page
- URL should remain `/virtual-test`

**How to Test:**
1. Click "Submit" on the second form
2. Observe the URL after redirect
3. Check the result message

#### Test 3: CurrentUmbracoPage()
**Expected Behavior:**
- Does **NOT** redirect - returns `View()` instead
- Keeps the current URL
- You should stay at `/virtual-test` without a redirect
- The form POST URL should remain in the address bar momentarily

**How to Test:**
1. Click "Submit" on the third form
2. Observe that there's no redirect (no page reload)
3. Check the result message (displayed via ViewData, not TempData)

## Expected Results Summary

| Method | Redirects? | Goes through Content Finders? | Final URL | Final Page |
|--------|-----------|-------------------------------|-----------|------------|
| `RedirectToCurrentUmbracoPage()` | Yes | No | `/` (home) | Actual home page |
| `RedirectToCurrentUmbracoUrl()` | Yes | Yes | `/virtual-test` | Virtual page |
| `CurrentUmbracoPage()` | No | No | `/virtual-test` | Virtual page (no reload) |

## Key Observations

Based on the discussion:

1. **RedirectToCurrentUmbracoPage()**: Redirects to the current **published content item**, which was resolved on the way in. Avoids content finders because you've already "found" the content.

2. **RedirectToCurrentUmbracoUrl()**: Redirects to the current **path**, going through content finders again, as you are redirecting to a URL rather than a specific content item.

3. **CurrentUmbracoPage()**: Does a `return View()` (like standard MVC), keeping the current URL but not going through content finders either.

## Troubleshooting

### Template Not Found Error
If you get a template error:
1. Make sure the `VirtualTest` template is created in the Umbraco backoffice
2. Check that `Views/VirtualTest.cshtml` exists
3. Restart the application after creating the template

### Virtual Page Returns 404
If `/virtual-test` returns 404:
1. Check that the composer is being loaded (`Composers/VirtualPageComposer.cs`)
2. Verify the content finder is registered by checking logs
3. Make sure you have at least one published page (home page) in Umbraco

### Forms Don't Submit
If forms don't work:
1. Check that the Surface Controller is in the correct namespace
2. Verify the controller name matches the form declarations
3. Check browser console for any JavaScript errors

## Files Created/Modified

```
ContentFinders/
  └── VirtualPageContentFinder.cs       (NEW)
Controllers/
  └── RedirectTestSurfaceController.cs  (NEW)
Views/
  └── VirtualTest.cshtml                (NEW)
Composers/
  └── VirtualPageComposer.cs            (NEW)
```

## Related Issue

This implementation addresses the questions raised in:
https://github.com/umbraco/Umbraco.Forms.Issues/issues/1456

The test setup allows you to verify whether `RedirectToCurrentUmbracoPage()` should respect `IContentFinder` rules and virtual pages like `CurrentUmbracoPage()` does.
