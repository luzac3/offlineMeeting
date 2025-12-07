export class BlurImage {
    static loaded = () => {
        document.addEventListener('DOMContentLoaded', () => {
            document.querySelectorAll('img.blur-up').forEach(_img => {
                const img = _img as HTMLImageElement;

                if (!img.dataset.src || img.classList.contains('loaded')) {
                    return;
                }

                const loadHDImage = () => {
                    img.onload = () => {
                        img.classList.add('loaded');
                        img.onload = null;
                    };

                    if (img.dataset.src) {
                        img.src = img.dataset.src;
                    }
                };

                if (img.loading === 'lazy' && 'IntersectionObserver' in window) {
                    const observer = new IntersectionObserver((entries, observer) => {
                        entries.forEach(entry => {
                            if (entry.isIntersecting) {
                                loadHDImage();
                                observer.unobserve(img);
                            }
                        });
                    });
                    observer.observe(img);
                } else {
                    loadHDImage();
                }
            });
        });
    }
}