LunchApp.Order = function () {
	this.init();
};

LunchApp.Order.prototype = {
	init: function () {
	    this.initMenuOrder();
	    this.initFavoriteFoodInMenu();
	},

	initMenuOrder: function () {
		$('.box a.button').on('click', function (e) {
			e.preventDefault();
			var inner = $(this);
			inner.addClass('alt');

			//TOOD Call an endpoint instead outputting to the console
			console.log(inner.attr('data-food'));
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