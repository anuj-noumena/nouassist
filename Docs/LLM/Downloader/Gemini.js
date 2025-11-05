/**
 * Process all elements under the infinite scroller and collect
 * the clipboard contents after clicking the "More" and "Copy" buttons.
 */
async function processMenuButtons() {
    // Start from the first div in the infinite scroller.
    let elm = document.querySelector("infinite-scroller > div");
    // This will hold all the clipboard texts we copy.
    let clipboardContent = [];

    while (elm) {
        // If multiple ".more-menu-button[mattooltip=More]" can exist in this div,
        // we’ll process each of them. If you only expect one, you could
        // replace querySelectorAll with querySelector.
        const moreButtons = elm?.querySelectorAll?.(".more-menu-button[mattooltip='More']");
        if(!moreButtons){
            break;
        }
        // If no "More" button is found, move to the next element.
        if (!moreButtons.length) {
            elm = elm.nextSibling;
            continue;
        }

        // Iterate over all "More" buttons found in this element.
        for (const button of moreButtons) {
            // Scroll the button into view to ensure it’s visible/clickable.
            button.scrollIntoView();
            // Small delay to allow scrolling to finish/render.
            await delay(200);

            // Click the "More" button.
            button.click();

            // Wait for the "Copy" button to appear in the menu, up to a few tries.
            const copyButton = await waitForElement(".mat-mdc-menu-item[aria-label='Copy']", 20, 1000);
            if (!copyButton) {
                console.warn("Copy button did not appear; skipping this element.");
                continue; // Move to the next "More" button
            }

            // Click the "Copy" button.
            await new Promise((resolve) => {
                function onCopy(e) {
                    // Once we catch the event, remove the listener and resolve.
                    document.removeEventListener('copy', onCopy);
                    clearTimeout(timer);
                    resolve(e);
                }

                // Listen for the "copy" event on the document.
                document.addEventListener('copy', onCopy);

                // Click the "Copy" button
                copyButton.click();

                // **New**: Wait for the actual `copy` event before proceeding.
                try {
                    // In case the copy event never fires, reject after `timeout` ms.
                    const timer = setTimeout(() => {
                        document.removeEventListener('copy', onCopy);
                        resolve("")
                    }, 5000);
                } catch (err) {
                    console.warn("Timed out waiting for the clipboard copy event", err);
                    // You can decide to continue or break here.
                    
                }
            })

            // (Optional) Wait for the "Copy" button (menu) to disappear
            // before reading from the clipboard, just as a visual check
            // that the menu has closed.
            const menuStillOpen = await waitForElementGone(".mat-mdc-menu-item[aria-label='Copy']", 20, 100);
            if (!menuStillOpen) {
                console.warn("Menu did not close; clipboard read may still work, but skipping checks.");
            }

            // Now that the copy event has fired, attempt to read from the clipboard.
            // Note: This requires a secure context (https or localhost).
            try {
                const clipboardText = await navigator.clipboard.readText();
                clipboardContent.push(clipboardText);
            } catch (error) {
                console.error("Failed to read clipboard:", error);
            }

            // Optional: If only one "More" button matters per div, you can break here.
        }

        // Move on to the next sibling in the infinite scroller.
        elm = elm.nextSibling;
    }

    console.log("All buttons processed.");
    // Return joined text, or keep it as an array if you prefer.
    return clipboardContent.join("\n");
}

/**
 * Helper function to wait for a `copy` event to fire.
 * By default waits up to 5s before rejecting.
 */
function waitForClipboardCopyEvent(timeout = 50000) {
    return new Promise((resolve, reject) => {
        function onCopy(e) {
            // Once we catch the event, remove the listener and resolve.
            document.removeEventListener('copy', onCopy);
            clearTimeout(timer);
            resolve(e);
        }

        // In case the copy event never fires, reject after `timeout` ms.
        const timer = setTimeout(() => {
            document.removeEventListener('copy', onCopy);
            reject(new Error("Clipboard copy event timed out"));
        }, timeout);

        // Listen for the "copy" event on the document.
        document.addEventListener('copy', onCopy);
    });
}

/**
 * Helper function to add a delay (in ms).
 */
function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

/**
 * Wait for an element matching `selector` to appear in the DOM.
 * Retries up to `retries` times, waiting `delayMs` between checks.
 * Returns the element if found, otherwise `null`.
 */
async function waitForElement(selector, retries = 20, delayMs = 100) {
    for (let i = 0; i < retries; i++) {
        const el = document.querySelector(selector);
        if (el) {
            return el;
        }
        await delay(delayMs);
    }
    return null;
}

/**
 * Wait for an element matching `selector` to disappear from the DOM.
 * Retries up to `retries` times, waiting `delayMs` between checks.
 * Returns `true` if the element is gone, `false` if it still remains.
 */
async function waitForElementGone(selector, retries = 20, delayMs = 100) {
    for (let i = 0; i < retries; i++) {
        const el = document.querySelector(selector);
        if (!el) {
            return true;
        }
        await delay(delayMs);
    }
    return false;
}

// Run it!
processMenuButtons()
    .then(result => {
        console.log("Final clipboard content:\n", result);
    })
    .catch(err => console.error("Error in processMenuButtons:", err));
