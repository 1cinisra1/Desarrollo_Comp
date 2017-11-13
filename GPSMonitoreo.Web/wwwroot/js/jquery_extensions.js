(function($){

    $.fn.extend({
        outerHTML: function( value ){

            // If there is no element in the jQuery object
            if(!this.length)
                return null;

            // Returns the value
            else if(value === undefined) {

                var element = (this.length) ? this[0] : this,
                    result;

                // Return browser outerHTML (Most newer browsers support it)
                if(element.outerHTML)
                    result = element.outerHTML;

                // Return it using the jQuery solution
                else
                    result = $(document.createElement("div")).append($(element).clone()).html();

                // Trim the result
                if(typeof result === "string")
                    result = $.trim(result);

                return result;
            } else if( $.isFunction(value) ) {
                // Deal with functions

                this.each(function(i){
                    var $this = $( this );
                    $this.outerHTML( value.call(this, i, $this.outerHTML()) );
                });

            } else {
                // Replaces the content

                var $this = $(this),
                    replacingElements = [],
                    $value = $(value),
                    $cloneValue;

                for(var x = 0; x < $this.length; x++) {

                    // Clone the value for each element being replaced
                    $cloneValue = $value.clone(true);

                    // Use jQuery to replace the content
                    $this.eq(x).replaceWith($cloneValue);

                    // Add the replacing content to the collection
                    for(var i = 0; i < $cloneValue.length; i++)
                        replacingElements.push($cloneValue[i]);

                }

                // Return the replacing content if any
                return (replacingElements.length) ? $(replacingElements) : null;
            }
        }
    });




  var methods = {
    init: function (options) {
      var opts = $.extend({}, $.fn.loadingOverlay.defaults, options);
      var target = $(this).addClass(opts.loadingClass);
      var overlay = '<div class="' + opts.overlayClass + '">' +
        //'<p class="' + opts.spinnerClass + '">' +
        '<span class="' + opts.iconClass + '"></span>' +
        //'<span class="' + opts.textClass + '">' + opts.loadingText + '</span>' +
        //'</p></div>';
		'</div>';
      // Don't add duplicate loading-overlay
      if (!target.data('loading-overlay')) {
        target.prepend($(overlay)).data('loading-overlay', true);
      }
      return target;
    },

    remove: function (options) {
      var opts = $.extend({}, $.fn.loadingOverlay.defaults, options);
      var target = $(this).data('loading-overlay', false);
      target.find('.' + opts.overlayClass).detach();
      if (target.hasClass(opts.loadingClass)) {
        target.removeClass(opts.loadingClass);
      } else {
        target.find('.' + opts.loadingClass).removeClass(opts.loadingClass);
      }
      return target;
    },

    // Expose internal methods to allow stubbing in tests
    exposeMethods: function () {
      return methods;
    }
  };

  $.fn.loadingOverlay = function (method) {
    if (methods[method]) {
      return methods[method].apply(
        this,
        Array.prototype.slice.call(arguments, 1)
      );
    } else if (typeof method === 'object' || !method) {
      return methods.init.apply(this, arguments);
    } else {
      $.error('Method ' + method + ' does not exist on jQuery.loadingOverlay');
    }
  };

  /* Setup plugin defaults */
  $.fn.loadingOverlay.defaults = {
    loadingClass: 'loading',
    overlayClass: 'loading-overlay',
    iconClass: 'loading-icon'
  };

})(jQuery);