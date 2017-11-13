$(function(){
	if($.jqx._jqxGrid)
	{

		$.jqx._jqxGrid.prototype.partialupdaterow = function(boundIndex, data){
			var row = this.getcleanrowdata(boundIndex);
			var keys = Object.keys(data);
			var key;

			for(var x = 0; x < keys.length; x++)
			{
				key = keys[x];
				row[key] = data[key];
			}

			this.updaterow(boundIndex, row);
		};

		$.jqx._jqxGrid.prototype.getcleanrowdata = function(boundIndex){
			var rowData = this.getboundrow(boundIndex);
			var newRowData = $.extend({}, rowData);
			delete newRowData.boundindex;
			delete newRowData.uid;
			delete newRowData.uniqueid;
			delete newRowData.visibleindex;
			delete newRowData.undefined;
			return newRowData;
		};


		//to be deleted, replaced by removeselectedrows
		$.jqx._jqxGrid.prototype.removecheckedrows = function()
		{
			var indexes = this.getselectedrowindexes();
			var ids = [];

			for (var i = 0; i < indexes.length; i++) {
				ids.push(this.getrowid(indexes[i]));
			}
			this.deleterow(ids);
			this.clearselection();
		};


		$.jqx._jqxGrid.prototype.removeselectedrows = function()
		{
			var indexes = this.getselectedrowindexes();
			var ids = [];

			for (var i = 0; i < indexes.length; i++) {
				ids.push(this.getrowid(indexes[i]));
			}
			this.deleterow(ids);
			this.clearselection();
		};

		$.jqx._jqxGrid.prototype.getselectedrows = function(){
			var ret = [];
			var indexes = this.getselectedrowindexes();
			var rows = this.getboundrows();

			for (var x = 0; x < indexes.length; x++) {
				ret.push(rows[indexes[x]]);
			}
			return ret;
		};



		$.jqx._jqxGrid.prototype.getselecteddatavalues = function(datafieldName)
		{
			var ret = [];
			var indexes = this.getselectedrowindexes();
			var rows = this.getboundrows();
			for (var x = 0; x < indexes.length; x++) {
				ret.push(rows[indexes[x]][datafieldName]);
			}

			return ret;
		};

		$.jqx._jqxGrid.prototype.selectbyvalues = function(datafieldName, values)
		{
			var indexes = [];
			var rows = this.getboundrows();
			var itemVal;

			var x;

			for(x = 0; x < rows.length; x++)
			{
				itemVal = rows[x][datafieldName];
				if(values.indexOf(itemVal) > -1)
				{
					indexes.push(x);
				}
			}

			for(x = 0; x < indexes.length; x++)
			{
				this.selectrow(indexes[x]);
			}
		};

		$.jqx._jqxGrid.prototype.refreshLocalData = function (data) {
			this.source._source.localdata = data;
			this.host.jqxGrid('source', this.source);
		};

		$.jqx._jqxGrid.prototype.reset = function () {
			this.clearselection();
			this.clear();
		};

		$.jqx._jqxGrid.prototype.getboundrow = function (boundIndex) {
			return this.source.records[boundIndex];
			//var start_index = this._startvisibleindex;
			//console.log('startindex: ' + start_index);
			//console.log('recordstartindex: ' + this.source._source.recordstartindex);
			//console.log(this.source.records);
			//console.log(this.source.records[this.source._source.recordstartindex + start_index + boundIndex]);
			//return this.source.records[this.source._source.recordstartindex + start_index + boundIndex];
		};

		$.jqx._jqxGrid.prototype.getboundrowfrombutton = function (button) {
			return this.getboundrow(button.parentNode.parentNode.widgetrow);
		};

		$.jqx._jqxGrid.prototype.getbuttonrowindex = function (button) {
			return button.parentNode.parentNode.widgetrow;
		};

		$.jqx._jqxGrid.prototype.getrowindex = function (boundIndex) {
			var start_index = this._startvisibleindex;
			return this.source._source.recordstartindex + start_index + boundIndex;
		};


		$.jqx._jqxGrid.prototype.setloader = function(loaderCallback){
			this._loaderCallback = loaderCallback;
		};

		$.jqx._jqxGrid.prototype.reload = function(){
			if(this._loaderCallback)
				this._loaderCallback();
		};


		$.jqx._jqxGrid.prototype.getFieldValues = function(fieldName){
			var rows = this.getboundrows();
			var ret = [];
			for(var x = 0; x < rows.length; x++)
			{
				ret.push(rows[x][fieldName]);
			}
			return ret;

		};

		$.jqx._jqxGrid.prototype.getrowsforpost = function(fields){
			var rows = this.getrows();
			var ret = [];

			var datafields = this.source._source.datafields;
			var row;
			var cleanedRow;
			var datafield;

			if(fields)
			{
				var key;
				for(var x = 0; x < rows.length; x++)
				{
					row = rows[x];
					cleanedRow = {};
					
					for(var y = 0; y < fields.length; y++)
					{
						key = fields[y];
						cleanedRow[key] = row[key];
					}
					ret.push(cleanedRow);
				}
			
			}
			else
			{
				for(var x = 0; x < rows.length; x++)
				{
					row = rows[x];
					cleanedRow = {};
					for(var y = 0; y < datafields.length; y++)
					{
						datafield = datafields[y];

						if(datafield.type)
						{
							cleanedRow[datafield.name] = row[datafield.name];
						}
					}
					ret.push(cleanedRow);
				}
			}
			return ret;
		};

		$.jqx._jqxGrid.prototype.displayError = function(error){
			
			var popOver = this._errorPopOver;
			if(!popOver)
			{
				var $parent = this.host.parent();
				var $before = $('<div>');
				var $div = $('<div><div></div></div>');

				//this.content.children().first().before($before);
				this.content.before($before);
				//this.host.before($before);

				//var $divContent = $('<div>');
				$parent.append($div);
				//$div.append($divContent);
				$div.jqxPopover({title: 'Errores', showCloseButton: true, autoClose: false, selector: $before, width: 500});
				popOver = $div.data('jqxWidget');
				//popOver._$divContent = $divContent;
				////$("#popover").jqxPopover({offset: {left: -50, top:0}, arrowOffsetValue: 50, title: "Employees", showCloseButton: true, selector: $("#button") });
				popOver._content.parent().addClass('err');
				popOver._content.parent().css('background-color', 'white');
				popOver.host.detach();
				popOver.host.addClass('jqx-popover-relative-centered');
				$before.append(popOver.host);

				this._errorPopOver = popOver;
			}

			var error_html = '';

			if(error.push) //is array
			{
				var err;
				
				error_html = '<table class="grid_table_error">';
				var columns = this.columns.records;
				var column;
				var y;
				var inner_table;
				var errors;
				var inner_error;

				for(var x = 0; x < error.length; x++)
				{
					err = error[x];
					errors = err.errors;

					error_html += '<tr><td style="width: 50px;vertical-align:top">Fila #' + (err.rowIndex + 1) + '</td><td>' ;

					inner_table = '<table>';

					for(y = 0; y < columns.length; y++)
					{
						column = columns[y];

						

						if(column.datafield)
						{
							inner_error = errors[column.datafield];

							if(inner_error)
							{
								inner_table += '<tr><td style="width:100px">' + column.text + '</td>';
								inner_table += '<td>' + inner_error.error + '</td></tr>';
							}
						}
					}

					inner_table += '</table>';
				
					error_html += inner_table + '</td></tr>';
				}
				error_html += '</table>';
			}
			else if(error.ItemsErrors)
			{
				var itemError;
				var itemErrors;
				error_html = error.Error + '<br/>';
				error_html += '<table class="grid_table_error">';
				var columns = this.columns.records;
				var column;
				var y;
				var inner_table;
				
				var inner_error;

				var itemsErrors = error.ItemsErrors;

				for(var x = 0; x < itemsErrors.length; x++)
				{
					itemError = itemsErrors[x];
					//errors = err.errors;
					itemErrors = itemError.Errors;

					error_html += '<tr><td style="width: 50px;vertical-align:top">Fila #' + (itemError.Index + 1) + '</td><td>' ;

					if(itemErrors)
					{
						inner_table = '<table style="width:100%">';

						for(y = 0; y < columns.length; y++)
						{
							column = columns[y];

							if(column.datafield)
							{
								inner_error = itemErrors[column.datafield];

								if(inner_error)
								{
									inner_table += '<tr><td style="width:100px">' + column.text + '</td>';
									inner_table += '<td>' + inner_error.Error + '</td></tr>';
								}
							}
						}

						inner_table += '</table>';
				
						error_html += inner_table
					}
					else
					{
						error_html += itemError.Error;
					}

					error_html += '</td></tr>';
				}
				error_html += '</table>';

				
			}
			else
			{
				error_html = error.error || error.Error;
			}

			popOver._content.html(error_html);
			popOver.open();

		};

		$.jqx._jqxGrid.prototype.cleanError = function(error){
			if(this._errorPopOver)
			{
				this._errorPopOver._content.html('');
				this._errorPopOver.close();
			}
		};
	}
});
