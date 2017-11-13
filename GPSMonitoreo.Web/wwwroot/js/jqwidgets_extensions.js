function jqxWidgetHook($element, valMethod)
{
	this.host = $element;
	this._val = valMethod;
}

jqxWidgetHook.prototype.widgetName = 'jqxWidgetHook';

jqxWidgetHook.prototype.val = function(newVal){
	if(this._val)
	{
		if(newVal === undefined)
			return this._val();
		else
			this._val(newVal);
	}
	else
	{
		var notimplemented = 'jqxFormHook val not implemented';
		if(newVal === undefined)
			return notimplemented;
		else
			console.log(notimplemented);
	}
};


function jqxPicker($element, options, onclick)
{
	this.host = $element;
	
	this.options = options || {};

	this.onclick = onclick;

	this.value = null;


	$element.prop('class', 'jqx-widget-content jqx-input-metro jqx-widget jqx-widget-metro jqx-dropdownlist-state-normal jqx-dropdownlist-state-normal-metro jqx-rc-all jqx-rc-all-metro jqx-fill-state-normal-metro jqx-picker');
	$element.prop('style', 'position:relative');


	var $content = $('<div class="content"></div>');
	var $buttons = $('<div class="buttons"></div>');

	var $search = $('<div class="search"></div>');
	var $clear = $('<div class="clear"></div>');

	$buttons.append($search);
	$buttons.append($clear);

	var self = this;


	//onclick parameter should be removed.  via options is the right way
	var onClick = options.onClick || onclick;

	


	$clear.click(function() {
		//self.value = null;
		//self.setContent('');
		self.val(null);

		//instance onClear should be moved to options
		var onClear = options.onClear || self.onClear;

		if(onClear)
		{
			onClear.call(self);
		}
	});

	$search.click(function(){
		onClick.call(self);
	});


	this.$content = $content;


	$element.append($content);
	$element.append($buttons);
}

jqxPicker.prototype = {
	widgetName: 'jqxPicker',

	val : function(newVal) {

		if(newVal === undefined)
			return this.value;
		else
		{
			var raiseOnChange = false;
			if(newVal === null)
			{
				if(this.value !== null)
					raiseOnChange = true;

				this.value = null;
				this.setContent('');
			}
			else
			{
				if(newVal instanceof Object)
				{
					this.setContent(newVal.label);
					if(this.value != newVal.value)
						raiseOnChange = true;

					this.value = newVal.value;
				}
				else
				{
					if(this.value != newVal)
						raiseOnChange = true;

					this.value = newVal;
				}
			}

			if(raiseOnChange && this.onChange)
				this.onChange();
		}
	},

	setContent: function(content) {
		this.$content.html(content);
	},

	getContent: function() {
		return this.$content.html();
	},

	formToJson: function(formData) {
		formData[this._elementName] = this.value;
		formData[this.options.descriptionKey] = this.getContent();
	}
};


function jqxRadioButtons($element, options, values)
{
	this.host = $element;
	

	options = options || {};

	this.value = null;
	this.values = values;

	var groupName = (options && options.groupName) || $element[0].id;

	var x;
	var self = this;

	var $divs;

	if(options.elements)
	{
		$divs = options.elements;

		for(x = 0; x < $divs.length; x++)
		{
			$divs[x].innerHTML = values[x].label;
		}


	}
	else
	{
		var divs = '';

		for(x = 0; x < values.length; x++)
		{
			divs += '<div';

			if(values[x].checked)
			{
				divs += ' checked="checked"';
				this.value = values[x].value;
			}
				
			divs += '>' + values[x].label + '</div>';
		}
		$divs = $(divs);
		$element.append($divs);

		//$divs.on('checked', function(evt){
		//	var index = Array.prototype.slice.call(self.host[0].childNodes).indexOf(evt.currentTarget);
		//	self.value = self.values[index].value;
		//});
	}

	if(options.align == 'horizontal')
		$divs.css('display', 'inline-block');


	$divs.jqxRadioButton({groupName: groupName});

	$divs.on('checked', function(evt){
		var index = $divs.index(evt.currentTarget);
		self.value = self.values[index].value;
		if(options && options.onChecked)
		{
			options.onChecked(groupName, evt, self.value);
		}
	});

	

}

jqxRadioButtons.prototype = {
	widgetName : 'jqxRadioButtons',

	val : function() {
		return this.value;	
	}
};

//$.fn.jqxForm = function()
//{
//	this[0].$form = this;

//	//window.testform = this;

//	$.extend(this, $.fn._jqxForm.prototype);
//	$.fn._jqxForm.call(this);


//	return this;
//	//return new $.fn._jqxForm();
//};


$.fn.jqxForm = function()
{
	this[0].$form = this;

	//window.testform = this;

	this.instance = new jqxForm();
	this.instance.$form = this;
	this.instance.formElement = this[0];

	return this;
	//return new $.fn._jqxForm();
};

function jqxCreateTreeDropDown($element, options, source, onSelect) {
	var $dropDown = $element;
						
	var $tree = $('<div style="border: none;">');

	$dropDown.append($tree);

	$tree.jqxTree({width: '100%', height: '200px', source: source });

	var tree = $tree.data('jqxWidget');

	$dropDown.jqxDropDownButton({ width: '100%', dropDownWidth : 300});

	var dropDown = $dropDown.data('jqxWidget');

	dropDown._hostedWidget = tree;
	dropDown.tree = tree;

	dropDown.setDropDownContent = function(label) {
		var dropDownContent = '<div style="position: relative; margin-left: 3px;">' + label + '</div>';
		this.setContent(dropDownContent);
	};

	dropDown.reset = function() {
		this.setContent('');
		this.tree.selectItem(null);
	
	};

	$tree.on('select', function (event) {
		var args = event.args;
		var item = $tree.jqxTree('getItem', args.element);
		//console.log('setting setDropDownContent');
		dropDown.setDropDownContent(item.label);
		//console.log('setting setDropDownContent - finished');
		//var dropDownContent = '<div style="position: relative; margin-left: 3px;">' + item.label + '</div>';
		//dropDown.setContent(dropDownContent);
		dropDown.close();

		if(onSelect)
		{
			onSelect(item);
		}
	});

	$tree.on('itemClick', function (event) {
		dropDown.close();
	});

	//PENDIENTE SI EL TREE ES CON CHECKBOXES
	/*
	$categoryTree.on('checkChange', function (event) {
		var args = event.args;
		var items = categoryTree.getCheckedItems();

		var str = '';

		if(items.length == 0)
			str = '';
		else if(items.length == 1)
			str = items[0].label;
		else
			str = 'Items seleccionados: (' + items.length + ')';

		category.setContent('<div style="position: relative; margin-left: 3px;">' + str + '</div>');

	});
	*/

	return dropDown;
}


function jqxForm() {

	//this._widgets = [];
	//this._widgetsDict = Object.create(null);
	//this._formelements = [];

	this.widgets = Object.create(null);
	this.fromJsonHooks = [];
	this.toJsonHooks = [];
	this.resetHooks = [];
	this.$form = null;
	this.formElement = null;
};

jqxForm.prototype = {
	//addWidgetToDict: function(widget)
	//{
	//	//var element_name = this.getWidgetElementName(widget);
	//	if(widget._elementName)
	//		this._widgetsDict[widget._elementName] = widget;
	//},


	addWidget: function(widget)
	{
		widget._elementName = this.getWidgetElementName(widget);
		this.widgets[widget._elementName] = widget;
		//this._widgets.push(widget);
		//this.addWidgetToDict(widget);
	},

	add: function(selectorOrElement$, widgetName, options)
	{
		var self = this;
		var $elements = this._getElement$(selectorOrElement$);

		$elements.each(function () {
			self.addSingle($(this), widgetName, options);
			//var widget = $(this)[widgetName](options).data('jqxWidget');
			//self._widgets.push(widget);
		});	
	},

	addOuter: function(selector, widgetName, options)
	{
		var self = this;
		$(selector).each(function(){
			self.addSingle($(this), widgetName, options);
			//var widget = $(this)[widgetName](options).data('jqxWidget');
			//self._widgets.push(widget);
		});	
	},

	_getElement$: function(selectorOrElement$)
	{
		if (typeof selectorOrElement$ === 'string' || selectorOrElement$ instanceof String)
			return this.$form.find(selectorOrElement$);
		else if (selectorOrElement$ instanceof Array || (selectorOrElement$.nodeType === 1 || selectorOrElement$.nodeType === 11))
		{
			return $(selectorOrElement$);
		}
		else
		{
			return selectorOrElement$;
		}
	},


	addByName: function(name, widgetName, options)
	{
		
		var widget = $(this[0].elements[name])[widgetName](options).data('jqxWidget');
		//this._widgets.push(widget);
		this.addWidget(widget);
		return widget;
	},

	addSingle: function(selectorOrElement$, widgetName, options)
	{
		var $element = this._getElement$(selectorOrElement$);

		if(!options)
			options = {};

		var widget = $element[widgetName](options).data('jqxWidget');
		//this._widgets.push(widget);
		this.addWidget(widget);
		return widget;
	},

	addFormElement: function(selector)
	{
		var self = this;
		$(selector).each(function(){
			//console.log(this);
			//self._formelements.push(this);
			var widget = $(this);
			widget.widgetName = 'jQuery';
			widget._elementName = this.name;
			self.widgets[widget._elementName] = widget;
			//self._widgets.push(widget);
			//self.addWidgetToDict(widget);

			//var widget = $(this)[widgetName](options).data('jqxWidget');
			//self._widgets.push(widget);
		});	
	},


	addWidgetHookSingle: function(selectorOrElement$, valMethod)
	{
		var $element = this._getElement$(selectorOrElement$);


		var widget = new jqxWidgetHook($element, valMethod);
		this.addWidget(widget);
		return widget;
	},

	addWidgetHook: function(selector, valMethod)
	{
		var self = this;

		this.$form.find(selector).each(function(){
			//var widget = new jqxWidgetHook($(this), valMethod);
			//self.addWidget(widget);
			self.addWidgetHookSingle($(this), valMethod);
		});	
	},

	addById: function(id, widgetName, options)
	{
		var widget = this.$form.find('#' + id)[widgetName](options).data('jqxWidget');
		//this._widgets.push(widget);
		this.addWidget(widget);
		return widget;
	},

	addDropDownButton: function(idOrElement$, options, hostedWidget)
	{
		var widget;
		if(idOrElement$ && idOrElement$.substring)
			widget = this.addById(idOrElement$, 'jqxDropDownButton', options);
		else
			widget = this.addSingle(idOrElement$, 'jqxDropDownButton', options);

		widget._hostedWidget = hostedWidget;
		return widget;
	},

	addTreeDropDown: function(idOrElement$, options, source, onSelect)
	{

		var $dropDown = this._getElement$(idOrElement$);

		var dropDown = jqxCreateTreeDropDown($dropDown, options, source, onSelect);

		this.addWidget(dropDown);

		return dropDown;
						
		//var $tree = $('<div style="border: none;">');

		//$dropDown.append($tree);

		//$tree.jqxTree({width: '100%', height: '200px', source: source });

		//var tree = $tree.data('jqxWidget');

		//var dropDown = this.addDropDownButton($dropDown, { width: '100%', dropDownWidth : 300}, tree);
						
		//$tree.on('select', function (event) {
		//	var args = event.args;
		//	var item = $tree.jqxTree('getItem', args.element);
		//	var dropDownContent = '<div style="position: relative; margin-left: 3px;">' + item.label + '</div>';
		//	dropDown.setContent(dropDownContent);
		//	dropDown.close();

		//	if(onSelect)
		//	{
		//		onSelect(item);
		//	}
		//});

		//$tree.on('itemClick', function (event) {
		//	dropDown.close();
		//});

	},

	addPicker: function(idOrElement$, options, onclick)
	{
		var $element = this._getElement$(idOrElement$);


		var widget = new jqxPicker($element, options, onclick);
		this.addWidget(widget);
		return widget;
	},

	addRadioButtons: function(idOrElement$, options, values)
	{
		var $element = this._getElement$(idOrElement$);


		var widget = new jqxRadioButtons($element, options, values);
		this.addWidget(widget);
		return widget;
	},

	resetWidget: function(widget)
	{
		if(widget.customReset)
		{
			widget.customReset();
			return;
		}

		switch(widget.widgetName)
		{
			case 'jqxTextArea':
			case 'jqxInput':
			case 'jQuery':
				widget.val('');
				break;

			case 'jqxNumberInput':
				widget.val(0);
				break;

			case 'jqxDateTimeInput':
			case 'jqxPicker':
				widget.val(null);
				break;

			case 'jqxCheckBox':
				widget.val(false);
				break;	
				
			case 'jqxWidgetHook':
				widget.val(null);
				break;

			case 'jqxDropDownList':
			case 'jqxTree':
			case 'jqxListBox':
				if(widget.checkboxes)
					widget.uncheckAll();
				else
					widget.clearSelection();
				
				break;

			case 'jqxGrid':
				widget.clear();
				widget.clearselection();
				break;
		}
	},

	hasWidget: function(elementName) {
		return elementName in this.widgets;
	},

	reset: function()
	{
		var widget;
		var x;
		//var widgetData;
		var keys = Object.keys(this.widgets);

		//for(x = 0; x < this._widgets.length; x++)
		for(var x = 0; x < keys.length; x++)
		{
			widget = this.widgets[keys[x]];
			this.cleanError(widget);

			//this.resetWidget(widget);
			//console.log(widget);
			//widgetData = widget.data('jqxWidget');

			switch(widget.widgetName)
			{
				case 'jqxDropDownButton':
					this.resetWidget(widget._hostedWidget);
					widget.setContent('');
					break;

				default:
					this.resetWidget(widget);
					break;
			}
		}

		for(x = 0; x < this.resetHooks.length; x++)
			this.resetHooks[x]();

		if(this.$errorMessageContainer)
			this.$errorMessageContainer.html('');
	},

	getItemByValue: function(widget, value)
	{
		var items = widget.getItems();
		for(var x = 0; x < items.length; x++)
		{
			if(items[x].value == value)
				return items[x];
		}

		return null;
	},

	getWidgetCheckedValues: function(widget)
	{
		var ret = [];
		var checked = widget.getCheckedItems();
		for(var x = 0; x < checked.length; x++)
			ret.push(checked[x].value);
		return ret;
	},

	setWidgetCheckedValues: function(widget, values)
	{
		$.each(values, function(index, val){
			//self.checkItem()
			var item = widget.getItemByValue(val);
			if(item)
				widget.checkItem(item);
		
		
		});

		//var checked = widget.getCheckedItems();
		//for(var x = 0; x < checked.length; x++)
		//	ret.push(checked[x].value);
	},

	setWidgetValue: function(widget, value, data)
	{
		//console.log('widget:');
		//console.log(widget);

		if(widget.constructor === String)
			widget = this.widgets[widget];

		if(widget.customVal)
		{
			widget.customVal(value, data);
			return;
		}

		var item;

		switch(widget.widgetName)
		{

			case 'jQuery':
				widget.val(value);
				break;

			case 'jqxInput':
				if(value instanceof Object)
				{
					widget.val(value);
				}
				else if(widget.valueMember || widget.displayMember)
				{
					var val = $.grep(widget.source, function(e){ return e[widget.valueMember] == value;})[0];
					widget.val(val);
				}
				else
					widget.val(value);

				//console.log(widget.source);
				//console.log(widget);


				break;

			case 'jqxTextArea':
			case 'jqxNumberInput':
			case 'jqxWidgetHook':
				widget.val(value);
				break;

			case 'jqxPicker':
				
				var labelKey = widget.options.descriptionKey ? widget.options.descriptionKey : widget._elementName + '_label' /*temporary until all uses are updated*/;

				if(labelKey in data)
					widget.val({value: value, label: data[labelKey]});
				else
					widget.val(value);

				break;

			case 'jqxCheckBox':
				widget.val(value == 1 ? true : false);
				break;

			case 'jqxDropDownList':
			case 'jqxListBox':
				
				if(widget.checkboxes)
				{
					widget.uncheckAll();
					this.setWidgetCheckedValues(widget, value);
				}
				else
				{
					widget.clearSelection();
					item = widget.getItemByValue(value);
					if(item)
						widget.selectItem(item);
				}
				break;

			case 'jqxTree':
				if(widget.checkboxes)
				{
					for (var x = 0; x < value.length; x++)
						widget.checkItemByValue(value[x]);
				}
				else
				{
					item = this.getItemByValue(widget, value);
					if(item)
						widget.selectItem(item);
				}
				break;

			case 'jqxGrid':
				widget.refreshLocalData(value);
				//console.log(value);


				break;
		}
	},

	getWidgetValue: function(widget)
	{

		if(widget.constructor === String)
		{
			widget = this.widgets[widget];
		}

		if(widget.customVal)
			return widget.customVal();

		switch(widget.widgetName)
		{



			case 'jQuery':
				return widget.val();
				break;

			case 'jqxInput':
				/*
				if(widget.baseHost) //is a group input
				{
					if(widget.value instanceof Object)
						return widget.value[widget.valueMember];
					else
						return null;
				}
				*/

				if(widget.valueMember && widget.displayMember)
				{
					if(widget.value)
						return widget.value[widget.valueMember];
					else
						return null;
				}
				else
					return widget.val();

				break;

			case 'jqxDateTimeInput':
			case 'jqxTextArea':
			case 'jqxNumberInput':
			case 'jqxWidgetHook':
			case 'jqxPicker':
			case 'jqxRadioButtons':
				return widget.val();
				//console.log(widgetData);
				break;


			//case 'jqxMaskedInput':
			//	return widget.parseValue(widget.value);
			//	break;


			//case 'jqxComboBox':
			//	return widget.val();
			//	break;

			//case 'jqxDateTimeInput':
			//	return widget.val();
			//	break;

			case 'jqxTree':
			case 'jqxDropDownList':
			case 'jqxListBox':
				if(widget.checkboxes)
					return this.getWidgetCheckedValues(widget);
				else
				{
					var selected = widget.getSelectedItem();
					if(selected)
						return selected.value;
					else
						return null;
				}
				break;


			case 'jqxComboBox':
				return widget.val();
				break;

			//case 'jqxDropDownList':
			//	if(widget.checkboxes)
			//		return this.getWidgetCheckedValues(widget);
			//	else
			//		return widget.val();
			//	break;


			case 'jqxCheckBox':
				return widget.checked ? 1 : 0;
				break;

			case 'jqxGrid':
				return widget.getrowsforpost();
				break;
					

			default:
				return null;
				break;
		}
	},

	getWidgetElementName: function(widget)
	{
		if(widget.field && widget.field.name)
			return widget.field.name;
		else if(widget.element && widget.element.name)
			return widget.element.name;
		else if(widget.host && widget.host[0].attributes.name)
			return widget.host[0].attributes.name.value;
		else if(widget.host && widget.host[0].attributes.field_name)
			return widget.host[0].attributes.field_name.value;
		else if(widget.baseHost && widget.baseHost[0].attributes.field_name)
			return widget.baseHost[0].attributes.field_name.value;
		else if(widget.base && widget.base.element && widget.base.element.attributes.field_name)
			return widget.base.element.attributes.field_name.value;
		else
			return widget.element.id;

		return null;
	},


	toJson: function()
	{
		var ret = {};
		var element;
		var widget;
		//var widgetData;

		var val;
		var element_name = '';

		var x;

		//for(x = 0; x < this._formelements.length; x++)
		//{
		//	element = this._formelements[x];

		//	if(element.name)
		//		ret[element.name] = $(element).val();
		//}

		var keys = Object.keys(this.widgets);

		for(x = 0; x < keys.length; x++)
		{
			widget = this.widgets[keys[x]];
			//console.log(widget);
			//widgetData = widget.data('jqxWidget');

			//element_name = this.getWidgetElementName(widget);

			if(widget.options && widget.options.useFormToJson)
			{
				widget.formToJson(ret);
			}
			else
			{
				if(widget._elementName)
				{
					switch(widget.widgetName)
					{
						case 'jqxDropDownButton':
							ret[widget._elementName] = this.getWidgetValue(widget._hostedWidget);
							break;

						default:
							ret[widget._elementName] = this.getWidgetValue(widget);
							break;

					}
				}
			}
		}


		for(x = 0; x < this.toJsonHooks.length; x++)
		{
			this.toJsonHooks[x](ret);
		}

		return ret;
	},

	postJson: function(url, callback, extraVals)
	{
		var data = this.toJson();
		$.extend(data, extraVals);
		Ajax.postJson(url, data, callback, true);
	},

	fromJson: function(data)
	{
		var widget;
		var element;

		var x;

		//for(x = 0; x < this._formelements.length; x++)
		//{
		//	element = this._formelements[x];
		//	if(element.name in data)
		//	{
		//		element.value = data[element.name];
		//	}
		//}

		var keys = Object.keys(this.widgets);
		
		for(x = 0; x < keys.length; x++)
		{
			widget = this.widgets[keys[x]];
			
			//element_name = this.getWidgetElementName(widget);

			if(widget._elementName in data)
			{
				switch(widget.widgetName)
				{

					case 'jqxDropDownButton':
						this.setWidgetValue(widget._hostedWidget, data[widget._elementName], data);
						break;

					default:
						
						this.setWidgetValue(widget, data[widget._elementName], data);
						break;
				}			
			}
		}

		for(x = 0; x < this.fromJsonHooks.length; x++)
		{
			this.fromJsonHooks[x](data);
		}
	},

	updateWidgetsValue: function(data) {

		var keys = Object.keys(data);
		//console.log(data);
		var widget;
		for(x = 0; x < keys.length; x++) {
			key = keys[x];
			widget = this.widgets[key];
			if(widget)
			{
				switch(widget.widgetName)
				{

					case 'jqxDropDownButton':
						this.setWidgetValue(widget._hostedWidget, data[widget._elementName], data);
						break;

					default:
						
						this.setWidgetValue(widget, data[widget._elementName], data);
						break;
				}	
			}
		}
	},

	cleanError: function(widget)
	{
		if(widget._error_label)
		{
			//$widget = $(widget.element);
			widget._error_label.css('display', 'none');
			widget._error_label.html('');

			var host = widget.baseHost ? widget.baseHost : widget.host;
			host.removeClass('err');
			//widget.host.removeClass('err');
			//if(widget.wrapper)
			//	widget.wrapper.removeClass('err');
			//else
			//	widget._baseHost.removeClass('err');
				
		}
	},

	cleanErrors: function()
	{
		var widget;
		//var $widget;

		var keys = Object.keys(this.widgets);

		for(var x = 0; x < keys.length; x++)
		{
			widget = this.widgets[keys[x]];
			if(widget.cleanError)
				widget.cleanError();
			else
				this.cleanError(widget);
		}

		if(this.$errorMessageContainer)
			this.$errorMessageContainer.html('');
	},

	displayErrors: function(errors)
	{
		var widget;
		//var $widget;
		var error_label;
		var element_name;
		var error;
		var host;

		var keys = Object.keys(this.widgets);

		for(var x = 0; x < keys.length; x++)
		{
			widget = this.widgets[keys[x]];

			//element_name = this.getWidgetElementName(widget);

			if(widget._elementName)
			{
				error = errors[widget._elementName];

				if(error)
				{
					//$widget = $(widget.element);

					if(widget.displayError)
					{
						widget.displayError(error);
					}
					else
					{
						host = widget.baseHost ? widget.baseHost : widget.host;

						if(!widget._error_label)
						{
							widget._error_label = $('<div class="error_field_label"></div>');
							//$widget.after(widget._error_label);
							host.after(widget._error_label);
						}

						widget._error_label.html(error.error || error.Error);
						widget._error_label.css('display', 'block');

						host.addClass('err');

						//if(widget.wrapper)
						//	widget.wrapper.addClass('err');
						//else
						//	widget._baseHost.addClass('err');
						//$widget.addClass('err');
					}
				}
				else
				{
					if(widget.cleanError)
						widget.cleanError();
					else
						this.cleanError(widget);
				}
			}
		}
	}
};

$(function(){
	if($.jqx._jqxDateTimeInput)
	{
		$.jqx._jqxDateTimeInput.prototype.customVal = function(newVal)
		{
			if(newVal === undefined)
			{
				var d = this.getDate();
				if(d)
				{
					var val = d.getFullYear() + '/' + ('0'+(d.getMonth()+1)).slice(-2) + '/' + ('0' + d.getDate()).slice(-2)
					+ ' ' + ('0' + d.getHours()).slice(-2) + ':' + ('0' + d.getMinutes()).slice(-2);
					return val;
				}
				else
					return null;
			}
		};
	}




	if($.jqx._jqxInput)
	{
		$.jqx._jqxInput.prototype.makeAutocompleteStrict = function()
		{
			var self = this;
			self.host.on('change', function(event){
				if(event.args.value === '')
				{
					if(self.clearing)
						return;

					self.clearing = true;
					//console.log('clear');
					//factura_proveedor.val({value:0, label:''});
					self.clear();
					self.selectedItem = null;
					self.clearing = false;
					//console.log(factura_proveedor);
				}
				else
				{
					//console.log('resetting');
					//var currentVal = factura_proveedor.val();
					//console.log(currentVal);
					//console.log('--------');
					if(self.selectedItem)
						self.val(self.selectedItem);
					else
					{
						self.clearing = true;
						self.clear();
						self.selectedItem = null;
						self.clearing = false;
					}

					//console.log('end resettings');
				}
			});
		
		
		};
		

		$.jqx._jqxInput.prototype.setFocusOnSelect = function(nextField)
		{
			var self = this;
			self.host.on('select', function(event){
				if(event.args)
				{
					//console.log(event);
					//console.log(factura_proveedor.val());
					nextField.focus();
					//factura_proveedor.host.trigger({type:'keydown', which: 65});
				}
		
		
				////console.log(factura_proveedor);
				//console.log('--------------');
				//if(event.args.value === undefined)
				//{
				//	//factura_proveedor.clear();
		
				//}
			});			
		
		};


	}


	if($.jqx._jqxTree)
	{
		$.jqx._jqxTree.prototype.getItemByValue = function (value) {
			var items = this.getItems();
			for (var x = 0; x < items.length; x++) {
				if (items[x].value == value)
					return items[x];
			}

			return null;
		};

		$.jqx._jqxTree.prototype.checkItemByValue = function (value) {
			var item = this.getItemByValue(value);
			if (item)
				this.checkItem(item, true);
		};

		$.jqx._jqxTree.prototype.selectItemByValue = function (value) {
			var item = this.getItemByValue(value);
			if (item)
				this.selectItem(item, true);

		};
	}

	if($.jqx._jqxTabs)
	{
		$.jqx._jqxTabs.prototype.hideAt = function(index) {
			//this._titleList[index].css('display', 'none');
			this._titleList[index].style.display = 'none';
		};


		$.jqx._jqxTabs.prototype.showAt = function(index) {
			var li = this._titleList[index];
			li.style.display = 'block';
			li.firstChild.style.marginTop = '0px';
			//this._titleList[index].css('display', 'block');
			//this._titleList[index].find('.jqx-tabs-titleContentWrapper').css('margin-top', '0px');
		};
	}

	if($.jqx._jqxDropDownList) {
		$.jqx._jqxDropDownList.prototype.disableAll = function() {
			var length = this.source.length;

			for(var x = 0; x < length; x++)
			{
				this.disableAt(x);
			}
		};

		$.jqx._jqxDropDownList.prototype.enableAll = function() {
			var length = this.source.length;

			for(var x = 0; x < length; x++)
			{
				this.enableAt(x);
			}
		};

		$.jqx._jqxDropDownList.prototype.enableByValue = function(value, enabled) {

			if(enabled)
			{
				this.enableItem(value);
			}
			else
			{
				this.disableItem(value);
			}
		};
	}

});