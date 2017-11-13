function AppLayout() {
	this.url = null;
	this.ownerTabs = null;
	this.ownerTabLI = null;
	this.container = null;
	this._initializers = [];
	this.options = null;

	this.title = null;
	this.subtitle = null;
	this.events = {};

	this.onActivate = null;
	this.onLoaded = null;
	this.onLayoutLoaded = null;

	//this._initializers.push(function(self){
	//	var header = self.container.find('> div .layout_header');

	//	self.title = header.find('.layout_title');
	//	self.subtitle = header.find('.layout_subtitle');
	//});
	this._initializers.push(function(){
		var header = this.container.find('> div .layout_header');

		this.title = header.find('.layout_title');
		this.subtitle = header.find('.layout_subtitle');
	});
}






AppLayout.prototype = {

	addInitializer : function(initializer) {
		this._initializers.push(initializer);
	},


	setTitle: function(content) {
		this.title.html(content);
	},

	setSubTitle: function(content) {
		if(content)
			this.subtitle.html(' - ' + content);
		else
			this.subtitle.html('');
	},

	setTabHoverTitle: function(title) {
		if(this.ownerTabLI)
			this.ownerTabLI.title = this.ownerTabLI.firstChild.innerHTML + ': ' + title;
	},

	getOwnerTabIndex: function() {
		var tabIndex = Array.prototype.slice.call(this.ownerTabs._titles).indexOf(this.ownerTabLI);
		return tabIndex;
	},

	activate: function() {
		this.ownerTabs.select(this.getOwnerTabIndex());
	},

	init: function() {
		for(var x = 0; x < this._initializers.length; x++)
		{
			//this._initializers[x](this);
			this._initializers[x].call(this);
		}
	},


	load: function(url) {
		var self = this;
		App.getHtml(url, function(response){
			var tabIndex = self.getOwnerTabIndex();
			self.ownerTabs.select(tabIndex);
			self.ownerTabs.setContentAt(tabIndex, response);

			if(self.onLayoutLoaded)
				self.onLayoutLoaded();


			if(self.onLoaded)
				self.onLoaded();


			//if(onLoaded)
				//onLoaded(self);
			//if(onActivate)
			//	onActivate(loaded.$container);
		});
	}
}


function AppLayoutTabbed() {
	//AppLayoutForm.call(this);
	this.AppLayout(this);
	this.tabs = null;
	this.tabInitializers = {};

	this._initializers.push(function(self){
		//self.form = self.container.find('#' + self.options.formId).jqxForm();

		var self = this;

		this.tabs = this.container.find('> div > div.body > div').jqxTabs({
			width: '100%', height: '100%',
			initTabContent: function (tab) {
				//alert('initTabContent: ' + tab);
				if (self.tabInitializers[tab])
					self.tabInitializers[tab](this);

			}
		}).data('jqxWidget');
	});
}


inherits(AppLayoutTabbed, AppLayout, true, {
	disableAllTabs: function(){
		this.tabs.select(0);
		var length = this.tabs.length();
		for(var x = 1; x < length; x++)
			this.tabs.disableAt(x);
	},

	enableAllTabs: function(){
		this.tabs.select(0);
		var length = this.tabs.length();
		for(var x = 1; x < length; x++)
			this.tabs.enableAt(x);
	}
});




function AppLayoutSearchGrid() {
	this.AppLayout(this);
	this.form = null;
	this.grid = null;


	this._initializers.push(function(self){
		//var $form = self.container.find('#' + self.options.formId).jqxForm();
		
		//self.form = $form.instance;




		//self.form = self.container.find('#' + self.options.formId).jqxForm();


		//var $button = self.container.find('>div >div button');
		//self.form.$errorMessageContainer = $button.parent().find('.input_error');
		//$button.form = self.form;

		//$button.click(function(){
		//	App.postFormJsonFromButton($button, function(response){
		//		if(self.onSavedSetEditingId)
		//		{
		//			if(self.form.widgets.Id)
		//			{
		//				self.form.widgets.Id.val(response.id)
		//			}
		//			else
		//			{
		//				self.form.widgets.id.val(response.id)
		//			}
		//			self.editingId = response.id;
		//		}

		//		if(self.onSaved)
		//			self.onSaved();

		//		if(self.options.disableTabsOnEditNew)
		//			self.enableAllTabs();

		//		self.setSubTitle(response.description);
		//		self.setTabHoverTitle(response.description);

		//	}, null, {popupErrorMessage: true, resetFormErrors : true});
		//});

		//$button.jqxButton({template: 'info'});
	});

}


inherits(AppLayoutSearchGrid, AppLayout, true, {
	addGridInitializer: function(gridInitializer) {
		this._initializers.push(function(self){
			//var options = AppGrid2.getDefaultOptions();
			var options = gridInitializer(options);
			var $body = this.container.find('> div > div.body');
			options.$element = $body.find(':first-child');
			this.grid = new AppGrid2(options);
		});
	}
});




function AppLayoutForm() {
	//AppLayout.call(this);
	this.AppLayout(this);
	this.editDataUrl = null;
	this.form = null;
	this.grids = Object.create(null);
	this.editingId = 0;
	this.onEditStarted = null;
	this.onEditNewStarted = null;
	this.onSaved = null;
	this.onReset = null;
	this.onSavedSetEditingId = true;

	
	this._initializers.push(function(self){
		//var $form = self.container.find('#' + self.options.formId).jqxForm();
		var $form = this.container.find('#' + this.options.formId).jqxForm();
		
		this.form = $form.instance;


		//self.form = self.container.find('#' + self.options.formId).jqxForm();


		var $button = this.container.find('>div >div button');
		this.form.$errorMessageContainer = $button.parent().find('.input_error');
		$button.form = this.form;

		var self = this;

		$button.click(function(){
			App.postFormJsonFromButton($button, function(response){
				if(self.onSavedSetEditingId)
				{
					if(self.form.widgets.Id)
					{
						self.form.widgets.Id.val(response.id)
					}
					else
					{
						self.form.widgets.id.val(response.id)
					}
					self.editingId = response.id;
				}

				if(self.onSaved)
					self.onSaved();

				if(self.options.disableTabsOnEditNew)
					self.enableAllTabs();

				self.setSubTitle(response.description);
				self.setTabHoverTitle(response.description);

			}, null, {popupErrorMessage: true, resetFormErrors : true});
		});

		$button.jqxButton({template: 'info'});
	});
}


inherits(AppLayoutForm, AppLayout, true, {
	reset : function() {
		this.form.reset();
		var keys = Object.keys(this.grids);
		var key;
		for(var x = 0; x < keys.length; x++)
		{
			key = keys[x];
			if(!(key in this.form.widgets))
			{
				this.grids[key].clear();
				this.grids[key].clearselection();
			}
		}

		if(this.onReset)
			this.onReset();
	},


	edit : function(id, options) {
		options = options || {};
		if(!options.skipReset)
			this.reset();

		this.editingId = id;
		this.activate();
		var arr = this.url.split('/');
		arr.pop();

		//var url = arr.join('/') + '/EditData/' + id;
		var self = this;

		//onsole.log(this.events);

		App.get(this.editDataUrl + '/' + id, function(response){
			if(options.onDataLoaded)
				options.onDataLoaded();


			if(response.description)
			{
				self.setSubTitle(response.description);
				self.setTabHoverTitle(response.description);
			}
			self.form.fromJson(response.record);
			
			if(options.initialValues)
				self.form.updateWidgetsValue(options.initialValues);

			if(self.onEditStarted)
				self.onEditStarted(response);
		});
	},


	editNew : function(options) {
		this.editingId = 0;
		this.activate();
		if(!options.skipReset)
			this.reset();


		this.setSubTitle('NUEVO REGISTRO');
		this.setTabHoverTitle('NUEVO REGISTO');

		if(options.initialValues)
			this.form.updateWidgetsValue(options.initialValues);

		if(this.onEditNewStarted)
			this.onEditNewStarted();
	},

	addFormInitializer : function(initializer) {
		var form = this.form;
		this._initializers.push(function(self){
			//initializer(self.form);
			initializer.call(this, this.form);
		});
	},


	addGridInitializer : function(initializer) {
		//var form = this.form;
		var options = initializer();
		this._initializers.push(function(self){
			//options.addToForm = self.form;
			var $element = this.container.find(options.elementId);
			options.$element = $element;

			var appGrid = new AppGrid(options);

			if($element[0].attributes.field_name)
				this.grids[$element[0].attributes.field_name.value] = appGrid;

			if(options.addToLayoutForm)
				this.form.addWidget(appGrid.grid);

			//initializer(self.form);
		});
	},
	addGridInitializer2 : function(initializer) {
		//var form = this.form;
		var options = initializer();
		this._initializers.push(function(self){
			//options.addToForm = self.form;
			var $element = options.$element || this.container.find(options.elementId);
			//options.$element = $element;

			var appGrid = new AppGrid2(options);

			if($element[0].attributes.field_name)
				this.grids[$element[0].attributes.field_name.value] = appGrid;

			if(options.addToLayoutForm)
				this.form.addWidget(appGrid.grid);

			//initializer(self.form);
		});
	}
});




function AppLayoutTabbedForm() {
	//AppLayoutForm.call(this);
	this.AppLayoutForm(this);
	this.tabs = null;
	this.tabInitializers = {};

	this._initializers.push(function(self){
		//self.form = self.container.find('#' + self.options.formId).jqxForm();

		var self = this;

		this.tabs = this.container.find('> div > div.body > div').jqxTabs({
			width: '100%', height: '100%',
			initTabContent: function (tab) {
				//alert('initTabContent: ' + tab);
				if (self.tabInitializers[tab])
					self.tabInitializers[tab](this);

			}
		}).data('jqxWidget');

		if(this.options.disableTabsOnEditNew)
			this.disableAllTabs();
	});
}


inherits(AppLayoutTabbedForm, AppLayoutForm, true, {
	editNew: function(options) {
		//AppLayoutForm.prototype.editNew.call(this);
		this.tabs.select(0);
		this.AppLayoutForm.editNew(this, options);

		if(this.options.disableTabsOnEditNew)
			this.disableAllTabs();
	},

	edit: function(id, options) {
		this.tabs.select(0);
		var onDataLoaded = null

		if(this.options.disableTabsOnEditNew)
		{
			var self = this;
			options = options || {};
			options.onDataLoaded = function() {
				self.enableAllTabs();
			}
		}


		//AppLayoutForm.prototype.edit.call(this, id, onDataLoaded);
		this.AppLayoutForm.edit(this, id, options);
	},

	disableAllTabs: function(){
		this.tabs.select(0);
		var length = this.tabs.length();
		for(var x = 1; x < length; x++)
			this.tabs.disableAt(x);
	},

	enableAllTabs: function(){
		this.tabs.select(0);
		var length = this.tabs.length();
		for(var x = 1; x < length; x++)
			this.tabs.enableAt(x);
	}

});