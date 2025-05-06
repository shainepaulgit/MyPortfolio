
let sidebar = $("#sidebar");
let menu = $("#menu");
let menuItem1 = $("#menu li:nth-child(1)");
let menuItem2 = $("#menu li:nth-child(2)");
let menuItem3 = $("#menu li:nth-child(3)");
let menuItem4 = $("#menu li:nth-child(4)");
let menuItem5 = $("#menu li:nth-child(5)");
let menuItem6 = $("#menu li:nth-child(6)");
let menuItem7 = $("#menu li:nth-child(7)");

let menuToggle = $("#menu-toggle");
let toggleSidebarHeader = $("#toggle-sidebar-header");
let sidebarHeaderPopover = $("#sidebar-header-popover");


let submitButton = $("#submit-button");
let loadingSpinner = $(".loading-spinner");
let loadingText = $(".loading-text");
let buttonText = $(".button-text");
let submitButtonIcon = submitButton.find("i");
let alertMessage = $("#alert");


//alert
setTimeout(() => alertMessage.addClass("active-alert"), 200);
setTimeout(() => alertMessage.removeClass("active-alert"), 10000);
alertMessage.find("#alert-close").on("click", () => alertMessage.removeClass("active-alert"));

//submit button
submitButton.on('click', function (e) {
    $(window).on('beforeunload', function () {
        $(this).prop('disabled', true);
        loadingSpinner.removeClass('d-none');
        loadingText.removeClass('d-none');
        buttonText.addClass('d-none d-lg-none d-md-none d-sm-none');
        submitButtonIcon.addClass('d-none');
    })
});




menuToggle.on("click", function () {
    sidebar.toggleClass("clicked");
})

toggleSidebarHeader.on("click", function (event) {
    event.stopPropagation(); // Prevents the document click event from immediately hiding it
    sidebarHeaderPopover.toggleClass("d-none");
});

$(document).on("click", function () {
    if (!sidebarHeaderPopover.hasClass("d-none")) {
        sidebarHeaderPopover.addClass("d-none");
    }
});

$(document).ready(function () {
    function fadeInOnScroll() {
        $(".fade-in-hidden").each(function () {
            let elementTop = $(this).offset().top;
            let windowBottom = $(window).scrollTop() + $(window).height();

            if (elementTop < windowBottom - 50) {
                $(this).addClass("fade-in-visible");
            }
        });
    }

    // Run on page load
    fadeInOnScroll();

    // Run on scroll
    $(window).on("scroll", function () {
        fadeInOnScroll();
    });
});

