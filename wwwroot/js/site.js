// JavaScript for animations, form handling, and chart interactions
$(document).ready(function () {
    // Button hover animations
    $('.btn-animated').hover(
        function () {
            $(this).addClass('animate__animated animate__pulse');
        },
        function () {
            $(this).removeClass('animate__animated animate__pulse');
        }
    );

    // Fade in main content
    $('main').addClass('animate__animated animate__fadeIn');

    // Chart interaction enhancements
    initializeChartInteractions();
});

function initializeChartInteractions() {
    // Add click effects to chart containers
    $('.chart-container').on('click', function () {
        $(this).toggleClass('chart-active');
    });

    // Smooth scroll to charts section
    $('a[href="#charts"]').on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({
            scrollTop: $('#charts').offset().top - 20
        }, 1000);
    });
}

// Loading states for async operations
function showLoadingState(element) {
    $(element).html('<div class="loading-spinner"></div> Loading...');
    $(element).prop('disabled', true);
}

function hideLoadingState(element, originalText) {
    $(element).html(originalText);
    $(element).prop('disabled', false);
}

// Toast notifications
function showToast(message, type = 'info') {
    const toast = $(`
        <div class="toast align-items-center text-white bg-${type} border-0" role="alert">
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `);

    $('#toastContainer').append(toast);
    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();

    // Remove toast after hide
    toast.on('hidden.bs.toast', function () {
        $(this).remove();
    });
}

// Form validation enhancements
function enhanceFormValidation() {
    $('form').on('submit', function (e) {
        const form = $(this);
        if (!form[0].checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
        }
        form.addClass('was-validated');
    });
}

// Initialize when document is ready
$(document).ready(function () {
    enhanceFormValidation();
});