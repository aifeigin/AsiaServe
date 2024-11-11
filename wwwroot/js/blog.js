document.addEventListener('DOMContentLoaded', () => {
    const toggleLinks = document.querySelectorAll('.toggle-link');
    console.log('click init');
    toggleLinks.forEach(link => {
        link.addEventListener('click', (event) => {
            event.preventDefault();
            const article = link.closest('.article');
            const contentWrapper = article.querySelector('.content-wrapper');

            if (article.classList.contains('expanded')) {
                // Collapse: Set height back to fixed preview height
                contentWrapper.style.height = '86px';
                link.textContent = 'Read More';
            } else {
                // Expand: Set height to full content height
                const fullHeight = contentWrapper.scrollHeight + 'px';
                contentWrapper.style.height = fullHeight;
                link.textContent = 'Read Less';
            }

            article.classList.toggle('expanded');
        });
    });
});