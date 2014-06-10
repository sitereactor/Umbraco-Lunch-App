LunchApp.Profile = function () {
    this.init();
};

LunchApp.Profile.prototype = {
    init: function () {
        this.initShowFavoriteFoodItems();
        this.initShowCurrentFoodOrder();
        this.initResetOrder();
        this.initOrderFavorites();
    },

    initShowFavoriteFoodItems: function () {
        var url = '/umbraco/api/FoodRelation/GetFavoriteFoodItems';
        var menuId = $('#TodaysMenu').attr('data-menu');
        
        $.get(url, function (data) {
            $.each(data, function (index, value) {
                if (menuId == value.ParentId) {
                    $(".divided").append('<li data-fooditem-id="' + value.Id + '" data-supplier-id="' + value.ParentId + '"><strong>' + value.Name + '</strong></li>');
                } else {
                    $(".divided").append('<li data-fooditem-id="' + value.Id + '" data-supplier-id="' + value.ParentId + '">' + value.Name + '</li>');
                }
                
            });
        });
    },

    initShowCurrentFoodOrder: function() {
        var url = '/umbraco/api/FoodOrder/GetOrderFoodItems';
        $.get(url, function (data) {
            $.each(data, function(index, value) {
                $('#TodaysOrder').append('<li data-fooditem-id="' + value.FoodItemId + '"><strong>' + value.FoodItem + '</strong></li>');
            });
        });
    },

    initResetOrder: function () { },

    initOrderFavorites: function () { }
};

$(function () {
    new LunchApp.Profile();
});