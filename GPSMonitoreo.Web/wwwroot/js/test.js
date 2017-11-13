function inherits(derived, base, addBase, extendPrototype)
{
	derived.prototype = Object.create(base.prototype);
	derived.prototype.constructor = derived;
	

	if(extendPrototype)
	{
		extend(derived.prototype, extendPrototype);
	}
	
	if(addBase)
	{
		var self = null;
		var baseName;
		
		if(base._name)
			baseName = base._name;
		else
		{
			baseName = (base.name && base.name) || base.toString().match(/function ([^\(]+)/);
			base._name = baseName;
		}
		

		var clonedBase = function(){
			//console.log(this);
			self = this;
			base.apply(this, arguments);
		};

		//derived.prototype.base = clonedBase;
		
		derived.prototype[baseName] = clonedBase;
		
		/*
		if(base.prototype.base)
			derived.prototype.base.base = base.prototype.base;*/

/*
		derived.prototype.base = Object.create(null);
		derived.prototype.base.constructor = base;
		*/
		//derived.prototype.base = base.prototype;
		
		
		var keys = Object.keys(base.prototype);
		var key;
		var f;
		for(var x = 0; x < keys.length; x++)
		{
			key = keys[x];
			
			if(key != 'constructor' && key != 'base')
			{
				console.log('key: ' + key);
				clonedBase[key] = function(f) {
					//base.prototype[key].apply(self, arguments);
					//f.apply(self, arguments);
					return function(){
						f.apply(self, arguments);
					}
				}(base.prototype[key]);				
			}
		}

	}
}


function MyBase(par1, par2) {
	console.log('MyBase constructing');
	this.var1 = 1;
	this.var2 = 2;
	this.passed1 = par1;
	this.passed2 = par2;
	console.log('MyBase constructed');
}

MyBase.prototype.test = function(passedArg1) {
	console.log('var 1 is: ' + this.var1 + ', passed: ' + passedArg1);
}

MyBase.prototype.test2 = function() {
	console.log('var 2 is: ' + this.var2);
}


function MyDerived(par1, par2) {
	console.log('MyDerived constructing');
	this.MyBase(par1, par2);
	//this.base(par1, par2);
	//this.base.call(this, par1, par2);
	//this.base.constructor.call(this);
	
	//this.base.test('fabian');
	//this.base.test();
	
	this.var3 = 3;
	this.var4 = 4;
	
	console.log('MyDerived constructed');
	
}

inherits(MyDerived, MyBase, true);



function MyDerived2(par1, par2) {
	console.log('MyDerived2 constructing');
	this.MyDerived(par1, par2);
	
	//this.base.call(this, par1, par2);
	//this.base.constructor.call(this);
	
	console.log('MyDerived2 constructed');
	
}


inherits(MyDerived2, MyDerived, true);


var xx = new MyDerived2('passed1', 'passed2');