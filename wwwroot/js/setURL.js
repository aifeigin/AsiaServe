// Wait for the page to fully load
window.addEventListener('load', () => {
    // Select the span element
    const urlElement = document.getElementById('MyURL');

    // Set its text content to the current URL
    urlElement.textContent = window.location.href;

    if (urlElement.href) {
        urlElement.href = window.location.href;
    }
});