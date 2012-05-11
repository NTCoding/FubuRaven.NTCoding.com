
$(function () {

	hookUpAutoSubmits();

	$('form').submit(function (event) {
		setSequentialIndicesForHiddenFields();
	});

	$('a.addItem').click(function (event) {
		event.preventDefault();

		var input = $(this).siblings('input');
		var text = input.val();

		$(this).siblings('ul').append('<li>' + text + ' <a href="#" class="listDelete">Delete</a>' + '</li>');

		var name = input.attr('name');
		$(this).parent().append('<input type="hidden" name="' + name + '" value="' + text + '" />');
	});

	$(document).delegate('form li a.listDelete', "click", (function () {
		$(this).parent().remove();
	}));

});

function hookUpAutoSubmits() {
	$('.autosubmit').change(function () {
		$(this).closest('form').submit();
	});
}

function setSequentialIndicesForHiddenFields() {
	var hiddenGroups = $('div.hasHiddenGroup');

	hiddenGroups.each(function () {
		var hiddenFields = $(this).children('input[type=hidden]');

		hiddenFields.each(function (index) {
			var name = $(this).attr('name');
			name = name + '[' + index + ']Text'; // .Text because we are binding to StringWrappers for now
			$(this).attr('name', name);
		});
	});

	$('*[name=Authors]').attr('name', ' ');
}