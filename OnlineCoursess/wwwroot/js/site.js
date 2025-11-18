// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Enable carousel auto sliding if not already initialized by data attributes
(() => {
  document.addEventListener('DOMContentLoaded', () => {
    const el = document.getElementById('heroImageSlider');
    if (!el) return;
    // Ensure Bootstrap carousel is initialized (for cases when data attributes aren't picked)
    const carousel = bootstrap.Carousel.getInstance(el) || new bootstrap.Carousel(el, {
      interval: 5000,
      ride: 'carousel',
      pause: false,
      touch: true,
      keyboard: true
    });
  });
})();
