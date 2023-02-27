


//mission landingplateform page
$(document).ready(function () {
    $(window).resize(function () {0
        $(window).height(); 
        $(window).width(); 
        if ($(window).width() < 767) {
            gridview();
        }
    });
});
let items = document.getElementsByClassName("item")
let cards = document.getElementsByClassName("thumbnail")
let list = document.getElementById("list")
let grid = document.getElementById("grid")
let allchoices = [];

function listview() {
    for (var i = 0; i < items.length; i++) {
        items[i].classList.remove("col-lg-4")
        items[i].classList.add("col-lg-12")
        items[i].classList.remove("col-md-6")
        items[i].classList.add("col-md-12")
        items[i].classList.remove("col-sm-6")
        items[i].classList.add("col-sm-12")
        items[i].classList.add("mb-5")
        cards[i].classList.remove("card")
        cards[i].classList.add("d-flex")
    }
    grid.classList.remove("view")
    list.classList.add("view")
    list.style.marginLeft = 20 + "px";
}
function gridview() {
    for (var i = 0; i < items.length; i++) {
        items[i].classList.add("col-lg-4")
        items[i].classList.remove("col-lg-12")
        items[i].classList.add("col-md-6")
        items[i].classList.remove("col-md-12")
        items[i].classList.add("col-sm-6")
        items[i].classList.remove("col-sm-12")
        items[i].classList.remove("mb-5")
        cards[i].classList.remove("d-flex")
        cards[i].classList.add("card")
    }
    grid.classList.add("view")
    list.classList.remove("view")
    list.style.marginLeft = 0 + "px";
}

