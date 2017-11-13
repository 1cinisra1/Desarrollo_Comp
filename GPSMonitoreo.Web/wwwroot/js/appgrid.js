function AppGrid(options) {

	//this._options = Object.create(this._defaultOptions);
	this._options = AppGrid.getDefaultOptions();
	this.grid = null;

	$.extend(this._options, options);



	this._addRow = null;
	this._removeSelectedRows = null;
	this._editRow = null;
	this._reload = null;

	this.init();

	return this;
};



AppGrid.prototype = {

	_defaultOptions : {
		$element		: null,
		elementId		: null,
		width			: '100%',
		height			: '100%',
		addToForm		: null,
		addToLayoutForm	: false,
		editable		: false,
		showtoolbar		: false,
		selectionmode	: 'none',
		addMethod		: null,
		removeMethod	: null,
		editRowMethod: null,
		rowQuickViewMethod : null,
		showAddButton	: false,
		showRemoveButton: false,
		showReloadButton: false,
		showRowEditButton: false,
		showRowQuickViewButton : false,
		reloadMethod	: null,
		columns			: null,
		columnsresize	: true,
		datafields		: null,
		customVal		: null,
		postableFields	: null,
		swapable		: false,
		events			: {}
	},

	reload: function() {
		//this.grid.reload();
		if(this._reload)
			this._reload();
	},

	clear: function() {
		this.grid.clear();
	},

	clearSelection : function() {
		if(this.grid.getselectedrowindexes().length > 0)
			this.grid.clearselection();
	}
};

AppGrid.prototype.getGridOptions = function(){
	var commonOptions = [
		'width',
		'height',
		'showtoolbar',
		'editable',
		'selectionmode',
		'columns',
		'columnsresize'
	];

	var gridOptions = {};
	

	var key;

	for(var x = 0; x < commonOptions.length; x++)
	{
		key = commonOptions[x];
		if(this._options[key] !== undefined && this._options[key] !== null)
		{
			gridOptions[key] = this._options[key];
		}
	}
		
	return gridOptions;
};

AppGrid.prototype.setLocalData = function(data) {
	this.grid.refreshLocalData(data);
};

AppGrid.prototype.addRow = function(data) {
	data = data || {};
	this.grid.addrow(null, data);
};

AppGrid.prototype.removeSelectedRows = function() {
	this.grid.removeselectedrows();

};

AppGrid.prototype.init = function(){
	var options = this._options;

	var columns = options.columns;


	var $grid = options.$element;
	

	var gridOptions = this.getGridOptions();

	var self = this;

	if (options.showRowEditButton || options.showRowQuickViewButton)
	{
		this._editRow = options.editRowMethod;

		var editWidgets = function (row, column, value, htmlElement) {
			var $element = $(htmlElement);
			$element.css('padding', '3px');

			if(options.showRowEditButton && options.editRowMethod)
			{
				$editButton = $('<webicon class="file-4 clickable" style="margin-right:5px"></webicon>');
				$element.append($editButton);

				$editButton.on('click', function(){
					var boundRow = self.grid.getboundrowfrombutton(this);
					self._editRow(boundRow.id, boundRow);
				});
			}

			if (options.showRowQuickViewButton && options.rowQuickViewMethod)
			{
				$viewButton = $('<webicon class="binoculars clickable" style="margin-right:5px"></webicon>');
				$element.append($viewButton);
				$viewButton.on('click', function () {
					var boundRow = self.grid.getboundrowfrombutton(this);
					//self._editRow(boundRow.id, boundRow);
					options.rowQuickViewMethod.call(self, boundRow.id, boundRow);
					//quickview(searchgrid.grid.getboundrow(rowIndex).id);
					//alert(row.bounddata.id);
				});
			}




		};
		columns.push({ text: '', width: 100, createwidget: editWidgets, initwidget: function(){}});
	}


	if(options.swapable)
	{
		var swapWidget = function (row, column, value, htmlElement) {
			var grid = self.grid;
			$up = $('<webicon class="up-arrow-3 clickable"></webicon>');
			$down = $('<webicon class="down-arrow-3 clickable" style="margin-right:5px"></webicon>');

			var $element = $(htmlElement);
			$element.css('padding', '3px');
			$element.append($down);
			$element.append($up);

			$up.on('click', function(){
				var rowIndex = grid.getbuttonrowindex(this);

				if(rowIndex > 0)
				{
					var newRowData = grid.getcleanrowdata(rowIndex - 1);
					var newPrevRowData = grid.getcleanrowdata(rowIndex);
					grid.updaterow(rowIndex, newRowData);
					grid.updaterow(rowIndex - 1, newPrevRowData);
				}
			});

			$down.on('click', function(){
				var rowIndex = grid.getbuttonrowindex(this);
				if(rowIndex < grid.getdatainformation().rowscount-1)
				{
					var newRowData = grid.getcleanrowdata(rowIndex + 1);
					var newNextRowData = grid.getcleanrowdata(rowIndex);
					grid.updaterow(rowIndex, newRowData);
					grid.updaterow(rowIndex + 1, newNextRowData);
				}
			});
		};

		columns.push({ text: '', width: 100, createwidget: swapWidget, initwidget: function(){}});
	}







	

	if(options.showtoolbar)
	{

		gridOptions.rendertoolbar = function ($toolbar) {

			
			
			var $toolbar_container = $('<div style="height:100%;padding:3px"></div>');

			if(options.showAddButton)
			{
				var $add = $('<webicon class="placeholder-29 clickable"></webicon>');
				$toolbar_container.append($add);

				if(options.addMethod)
				{
					self._addRow = options.addMethod;

					$add.click(function(){
						self._addRow();
					});
				}
				else
				{
					$add.click(function(){
						self.addRow();
					});
				}
			}



			if(options.showRemoveButton)
			{
				var $remove = $('<webicon class="garbage-1 clickable"></webicon>');
				$toolbar_container.append($remove);


				if(options.removeMethod)
				{
					self._removeSelectedRows = options.removeMethod;
					$remove.click(function(){
						self._removeSelectedRows();
					});
				}
				else
				{
					$remove.click(function(){
						self.removeSelectedRows();
					});
				}
			}

			if(options.showReloadButton)
			{
				var $reload = $('<webicon class="arrows_repeat clickable"></webicon>');
				$toolbar_container.append($reload);
				$reload.click(function(){
					self.reload();
				});
			}





			$toolbar.append($toolbar_container);
		}
	}


	var source_grid = {
		localdata: [],
        datatype: "array",
		datafields: options.datafields,
        deleterow: function (rowid, commit) {
			commit(true);
		},
        addrow: function (rowid, rowdata, position, commit) {
			commit(true);
		},
        updaterow: function (rowid, newdata, commit) {
			commit(true);
		}
	};

	var dataAdapter = new $.jqx.dataAdapter(source_grid);

	gridOptions.source = dataAdapter;



	$grid.jqxGrid(gridOptions);

	var events = this._options.events;

	var eventKeys = Object.keys(events);

	for(var x = 0; x < eventKeys.length; x++)
	{
		$grid.on(eventKeys[x], events[eventKeys[x]]);
	}

	var grid = $grid.data('jqxWidget');
	this.grid = grid;


	if(options.showtoolbar && options.reloadMethod)
		this._reload = options.reloadMethod;

		//grid.setloader(options.reloadMethod);


	
	if(options.addToForm)
		options.addToForm.addWidget(grid);

	if(options.customVal)
		grid.customVal = options.customVal;
	else if(options.postableFields)
	{
		grid.customVal = function() {
			return this.getrowsforpost(options.postableFields);
		};
	}



};


AppGrid.getDefaultOptions = function(){

	var ret = {};
	var keys = Object.keys(AppGrid.prototype._defaultOptions);
	var key;
	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		ret[key] = AppGrid.prototype._defaultOptions[key];
	}


	return ret;
	
	//return $.extend({}, AppGrid.prototype._defaultOptions);
};

