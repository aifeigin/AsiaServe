// Function to change the language when a new option is selected
function changeLanguage() {
    const languageSelector = document.getElementById('language-switcher');
    const selectedLanguage = languageSelector.value;
    
    if (selectedLanguage) {
        // Redirect the browser to the selected language's page
        window.location.href = selectedLanguage;
    } else {
        console.error("No language URL is selected.");
    }
}

window.onload = function () {
    const currentPath = window.location.pathname;
    const selector = document.getElementById('language-switcher');
    if (selector) { 
        for (const option of selector.options) {
            if (currentPath.includes(option.value)) {
                option.selected = true;
                break;
            }
        }
    }
};