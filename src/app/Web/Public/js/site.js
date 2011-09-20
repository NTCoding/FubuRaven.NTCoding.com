
$(function () {

	$('form').submit(function (event) {
		setSequentialIndicesForHiddenFields();
	});

	$('a.addItem').click(function (event) {
		event.preventDefault();

		var text = $(this).siblings('input').val();

		$(this).siblings('ul').append('<li>' + text + '</li>');

		var name = $(this).parent().attr('name');
		$(this).parent().append('<input type="hidden" name="' + name + '" value="' + text + '" />');
	});
});

function setSequentialIndicesForHiddenFields() {
	var hiddenGroups = $('div.hasHiddenGroup');

	hiddenGroups.each(function () {
		var hiddenFields = $(this).children('input[type=hidden]');

		hiddenFields.each(function (index) {
			var name = $(this).attr('name');
			name = name + '[' + index + ']';
			alert(name);
			$(this).attr('name', name);
		});
	});
}