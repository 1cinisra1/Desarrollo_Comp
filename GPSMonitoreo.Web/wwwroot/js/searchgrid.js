function SearchGrid() {
	this.grid = null;
	this.adapter = null;
	this.form = null;
}

SearchGrid.prototype.search = function(){
	var postData = this.form.toJson();
	this.adapter._source.data = postData;

	var paginginfo = this.grid.getpaginginformation();

	if(paginginfo.pagenum == 0)
	{
		this.grid.host.jqxGrid('source', this.adapter);
	}
	else
		this.grid.gotopage(0);

}