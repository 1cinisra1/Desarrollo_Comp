//prueba original22344

function RemoteContent($target, url, onload, onactivate)
{
	this.url = url;
	this.onload = onload;
	this.$container = null;
	this.$target = $target;
	this.onactivate = onactivate;
}

RemoteContent._loaded = {};







RemoteContent.load = function($target, url, onload, onactivate)
{
	var instance;

	if($target._currentRemote && $target._currentRemote.url == url)
	{
		instance = $target._currentRemote;
		instance.onactivate = onactivate;
		if(instance.onactivate)
			instance.onactivate(instance);
	}
	else
	{
		if($target._currentRemote)
			$target._currentRemote.detach();


		var id = $target[0].id + '-' + url;
		
		instance = RemoteContent._loaded[id];

		if(!instance)
		{
			instance = new RemoteContent($target, url, onload, onactivate);
			RemoteContent._loaded[id] = instance;
			var $container = $('<div style="height:100%"></div>');
			instance.$container = $container;
			$target.append($container);
			$container.load(url, function(){
				//if(instance.onactivate)
				//	instance.onactivate(instance);
			
			});
		}
		else
		{
			instance.attach();
			instance.onactivate = onactivate;
			if(instance.onactivate)
				instance.onactivate(instance);
			//if onactivate
		}
	}
};

RemoteContent.hook = function($target, url, onload)
{
	//alert('hooking');
	var id = $target[0].id + '-' + url;
	instance = RemoteContent._loaded[id];
	instance.onload = onload;
	instance.$target._currentRemote = instance;

	if(onload)
		instance.onload(instance.$container);


	//var scripts = this.target.find('> script');

	//if(scripts.length)
	//	scripts.remove();


	//this.$container = instance.$target.find('> div');

	if(instance.onactivate)
		instance.onactivate(instance);
	//instance.attach(true);
};

RemoteContent.prototype.detach = function()
{
	this.$container.detach();
};

RemoteContent.prototype.attach = function()
{
	this.$container.appendTo(this.$target);
	this.$target._currentRemote = this;

	if(this.onactivate)
		this.onactivate();
};


function getErrorLabel(field, options, fieldAppendMode)
{
	var error_label = field[0].error_label;

	var appendMode = fieldAppendMode ? fieldAppendMode : (options ? options.appendMode : null);

	//console.log(field);
	//console.log(appendMode);

	if(!error_label)
	{
		error_label = document.createElement('div');
		error_label.className = 'error_field_label';

		switch(appendMode)
		{
			case 'nextSibling':
				field.after(error_label);
				break;

			default:
				field.closest('td').append(error_label);
				break;
		}

		field[0].error_label = error_label;
	}
	else
	{
		error_label.style.display = '';
	}

	return error_label;


}


var Ajax =
{
	get: function(url, callback, options)
	{
		if(!options) options = {};

		var async = !options.async ? false : true;
		var dataType = options.dataType ? options.dataType : 'json';

		var final_options = {
			type	: 'GET',
			url		: url,
			success	: function(response){
				if(options.loadingElement)
					options.loadingElement.loadingOverlay('remove');

				callback(response);
			},
			dataType: dataType,
			async	: async
		};

		if(options.loadingElement)
		{
			options.loadingElement.loadingOverlay();
			final_options.error = function() {options.loadingElement.loadingOverlay('remove');};
		}

		jQuery.ajax(final_options);	
	},

	post: function(url, data, callback, options)
	{
		// async, contentType, loadingElement
		async = !options.async ? false : true;

		var final_options = {
			type	: 'POST',
			url		: url,
			data	: data,
			success	: function(response){
				if(options.loadingElement)
					options.loadingElement.loadingOverlay('remove');
				callback(response);
			},
			dataType: 'json',
			async	: async,
			contentType: options.contentType ? options.contentType : 'application/x-www-form-urlencoded; charset=UTF-8'
		};

		if(options.loadingElement)
		{
			options.loadingElement.loadingOverlay();
			final_options.error = function() {options.loadingElement.loadingOverlay('remove');};
		}


		jQuery.ajax(final_options);	
	},

	postForm: function(url, form, callback, async)
	{
		this.post(url, jQuery(form).serializeArray(), callback, async);
	},

	postFormJson: function(url, form, callback, options)
	{
		options.contentType = 'application/json';
		this.post(url, JSON.stringify($(form).serializeObject()), callback, options);
	},

	postJson: function(url, data, callback, options)
	{
		options.contentType = 'application/json';
		this.post(url, JSON.stringify(data), callback, options);
	},

	postMultipart: function(url, data, callback, async)
	{
		jQuery.ajax({
			type	: 'POST',
			url		: url,
			data	: data,
			contentType: false,
			processData: false,
			success	: function(response){
				callback(response);
			},
			dataType: 'json',
			async	: async
		});	
	}
};

var Response =
{
	show: function(type, target, message, afterMessage)
	{
		var html = '';

		switch(type)
		{
			case 'error':
				html = '<div class="response_box error"><i class="fa fa-ban"></i>Error: ' + message + '</div>';
				break;

			case 'success':
				html = '<div class="response_box success"><i class="fa fa-check-circle"></i>Info: ' + message + '</div>';
				break;
		
		}

		if(afterMessage)
			html += afterMessage;

		$(target).html(html);
	},



	handleForm: function(response, form, options)
	{
		var dom_form = form[0];
		var field;
		var $field;
		var firstErrorField = null;
		var errors = response.errors;
		var error_label;
		var error;
		var field_name;

		var elements = Form.getElements(dom_form);
		//console.log(elements);

		var isNodeList;

		dom_form.lastErrors = errors;
		


		for(var x = 0; x < elements.length; x++)
		{
			field = elements[x];

			if(field.element.form)
			{
				isNodeList = false;
				$field = jQuery(field.element);
			}
			else
			{
				isNodeList = true;
				$field = jQuery(field.element[field.element.length-1]);
			}

			error_label = $field[0].error_label;


			if(field.name in errors)
			{
				error = errors[field.name];

				if(!firstErrorField)
					firstErrorField = isNodeList ? field.element[0] : field.element;

			


				if(isNodeList)
				{
			
			
				}
				else
				{
					if((field.element.type == 'select' || field.element.type == 'select-one') && $field.parent().hasClass('field_select2'))
					{
						$field.parent().addClass('err');
					}
					else
						$field.addClass('err');
				}


				if(error.error)
				{
					if(error.error_label_after)
						error_label = getErrorLabel(jQuery(dom_form[error.error_label_after]), options, error.appendMode);
					else
						error_label = getErrorLabel($field, options, error.appendMode);

					error_label.innerHTML = error.error;

					if(error.filtered_input)
						$field.val(error.filtered_input);


				}

				//error_label = field[0].error_label;



			}
			else
			{
				Form.cleanFieldError(field.element);
				//if(isNodeList)
				//{
			
				//}
				//else
				//{
				//	if((field.element.type == 'select' || field.element.type == 'select-one') && $field.parent().hasClass('field_select2'))
				//	{
				//		$field.parent().removeClass('err');
				//	}
				//	else
				//		$field.removeClass('err');
				//}

				//if(error_label)
				//{
				//	error_label.style.display = 'none';
				//	error_label.innerHTML = '';
				//}
			}

		}

		//if(options && options.scrollUp)
		//	jQuery('html, body').animate({
		//		'scrollTop' : jQuery(firstErrorField).offset().top-100
		//	});
	},

	handle: function(response, options)
	{
		if(options)
		{
			if(options.closeModal)
			{
				options.modal.modal('hide');
			}
		}

		switch(response.status)
		{
			case 'OK':
				if(options)
				{
					if(options.form) 
					{
						if(options.resetForm)
						{
							if(options.form.widgets)
								options.form.reset();
							else
							{
								Form.cleanFieldErrors(options.form[0]);
								options.form[0].reset();
							}
						}
						else if(options.resetFormErrors)
						{
							options.form.cleanErrors();
						}
						else if(options.resetFormMode)
						{
							switch(options.resetFormMode)
							{
								case 'form':
									options.form.reset();
									break;

								case 'errors':
									options.form.cleanErrors();
									break;
							}
						}
					}

					if(options.modal && options.closeModalOnOK)
					{
						options.modal.modal('hide');
						if(options.response_container)
							options.response_container.html('');

						if(options.ok)
							options.ok(response);
					
					}
					else
					{
						if(options.response_container && response.message)
							this.show('success', options.response_container, response.message, response.detailedmessage);

						if(options.ok)
							options.ok(response);
					}
				}
				break;

			case 'ERROR':
				if(options && options.form && !options.form.widgets) 
					Form.cleanFieldErrors(options.form[0]);
				
				if(options && options.response_container)
				{
					this.show('error', options.response_container, response.message, response.detailedmessage);
				}
				else if(response.message)
					App.showError(response.message);
				break;

			case 'ERRORFORM':
				if(options.form && options.form.widgets)
				{
					options.form.displayErrors(response.errors);
					if(options.form.$errorMessageContainer)
						options.form.$errorMessageContainer.html(response.errorMessage);

					if(options.popupErrorMessage)
						App.showError(response.errorMessage);

				}
				else
					this.handleForm(response, options.form, options);
				break;

			case 'OBJECTMETHOD':
				if(response.arguments)
					window[response.object][response.method].apply(null, response.arguments);
				else
					window[response.object][response.method]();
				break;

			case 'REDIRECT':
				App.redirect(response.url);
				break;

			case 'LOGINREQUIRED':
				App.redirect('/login');
				break;
		}

		if(response.gritter)
			Gritter.show(response.gritter);

		if(response.notification)
			App.showNotification(response.notification);

		if(response.responseActions)
		{
			App.handleResponseActions(response);
		}

			//jQuery.gritter.add(response.gritter);
	}

};




