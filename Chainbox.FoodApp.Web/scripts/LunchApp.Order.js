LunchApp.Order = function () {
	this.init();
};

LunchApp.Order.prototype = {
	init: function () {
	    this.initMenuOrder();
	    this.initFavoriteFoodInMenu();
        /* TOOD Update view according to current order + favorites */
	},

	initMenuOrder: function () {
		$('.box a.button').on('click', function (e) {
			e.preventDefault();
			var inner = $(this);
			inner.addClass('alt');
			var foodItem = inner.attr('data-food');
			var foodItemId = inner.attr('data-food-item');

		    var url = '/umbraco/api/FoodOrder/OrderFoodItem';
		    $.post(url, { FoodItem: foodItem, FoodItemId: foodItemId })
		        .done(function (data) { /*LunchOrderDto is returned*/ });
		});
	},

    initFavoriteFoodInMenu: function() {
        $('.box span.favorite-pointer').on('click', function (e) {
            e.preventDefault();
            var inner = $(this);
            inner.removeClass('favorite-pointer');
            inner.addClass('favorite-pointer-alt');
            var foodItemId = inner.attr('data-food-item');
            
            var url = '/umbraco/api/FoodRelation/FavoriteFood';
            $.post(url, { FoodItemId: foodItemId });
        });
    }
};

$(function () {
	new LunchApp.Order();
});