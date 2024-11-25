document.addEventListener("DOMContentLoaded", async function () {
    const form = document.getElementById("feedbackForm");

    // Get CSRF token
    let csrfToken;
    try {
        const csrfResponse = await fetch(`${window.location.origin}/api/get-csrf-token`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            },
        });

        if (!csrfResponse.ok) {
            throw new Error('Failed to fetch CSRF token');
        }

        const csrfData = await csrfResponse.json();
        csrfToken = csrfData.token;
    } catch (error) {
        console.error("Error fetching CSRF token:", error);
        return;
    }

    form.addEventListener("submit", async (event) => {
        event.preventDefault();

        const feedbackData = {
            name: document.getElementById("name").value,
            email: document.getElementById("email").value,
            phone: document.getElementById("phone").value,
            message: document.getElementById("message").value
        };

        // Get CAPTCHA token
        const captchaToken = grecaptcha.getResponse();
        if (!captchaToken) {
            alert("Please complete the CAPTCHA.");
            return;
        }

        try {
            let button = document.getElementById("submit");

            let buttonText = replaceButtonText(button, "Wait<span class='dot'></span>");

            const response = await fetch(`${window.location.origin}/api/feedback`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "X-CSRF-TOKEN": csrfToken,
                    "X-Captcha-Token": captchaToken
                },
                body: JSON.stringify(feedbackData)
            });            

            if (response.ok) {                
                alert("Feedback sent successfully!");
                replaceButtonText(button, "Success");                
                grecaptcha.reset(); // Reset the CAPTCHA after successful submission
                button.className = "rounded-button disabled";
                button.disabled = true;
                button.title = "Please press F5 if you need to send another message";
            } else {
                alert("Error sending feedback." + response.statusText);
                replaceButtonText(button, buttonText);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Error sending feedback." + error);
        }
    });
});

function replaceButtonText(button, text) {
    if (button && text) {
        let oldText = null;
        if (button.innerHTML) {
            oldText = button.innerHTML;
            button.innerHTML = text;
        }
        else if (button.childNodes[0]) {
            oldText = button.childNodes[0].nodeValue;
            button.childNodes[0].nodeValue = text;
        }
        else if (button.value) {
            oldText = button.value;
            button.value = text;
        }

        return oldText;
    }

    return null;
}