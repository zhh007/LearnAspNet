
+function ($) {
    "use strict";
    var Pager = function (element, options) {
        this.options = options;
        this.$element = $(element);
        this.pagecount = 0;
    }

    Pager.DEFAULTS = {
        pagesize: 10,
        pageindex: 1,
        total: 0,
        arg: 'page',
        pagefunc: '',
        itemcount: 8//数字链接数
    }

    function getPageIndex(para) {
        var sUrl = location.href;
        if (sUrl.indexOf("?") > -1) {
            var keyidx = sUrl.indexOf(para);
            if (keyidx > -1) {
                var sReg = "(?:\\?|&){1}" + para + "=([^&]*)";
                var re = new RegExp(sReg, "gi");
                re.exec(sUrl);
                if ($.trim(RegExp.$1) != '' && RegExp.$1 != "link" && RegExp.$1 != "type")
                    return parseInt(RegExp.$1, 10);
            }
        }
        return 1;
    }

    function updateUrl(param, newval) {
        var r = location.href.split('?');
        var regex = new RegExp("([?;&])" + param + "[^&;]*[;&]?");
        var query = location.search.replace(regex, "$1").replace(/&$/, '');
        return r[0] + (query.length > 2 ? query + "&" : "?") + (newval ? param + "=" + newval : '');
    }

    function getUrl(para, idx) {
        return updateUrl(para, idx);
    }

    function showFirstLink($this, el) {
        var h = '<li';
        if ($this.index == 1) {
            h += ' class="disabled"';
        }
        h += '><a';
        if ($this.index == 1) {
            h += ' disabled="disabled" href="javascript:void(0)"';
        } else {
            h += ' href="' + getUrl($this.arg, 1) + '"';
        }
        h += '>首页</a></li>';
        $(h).appendTo(el);
    }

    function showLastLink($this, el) {
        var h = '<li';
        if ($this.index == $this.count) {
            h += ' class="disabled"';
        }
        h += '><a';
        if ($this.index == $this.count) {
            h += ' disabled="disabled" href="javascript:void(0)"';
        } else {
            h += ' href="' + getUrl($this.arg, $this.count) + '"';
        }
        h += '>尾页</a></li>';
        $(h).appendTo(el);
    }

    function showPrevLink($this, el) {
        var prev = $this.index - 1;
        var h = '<li';
        if (prev < 1) {
            h += ' class="disabled"';
        }
        h += '><a';
        if (prev < 1) {
            h += ' disabled="disabled" href="javascript:void(0)"';
        } else {
            h += ' href="' + getUrl($this.arg, prev) + '"';
        }
        h += '><span aria-hidden="true">&laquo;</span></a></li>';
        $(h).appendTo(el);
    }

    function showNextLink($this, el) {
        var next = $this.index + 1;
        var h = '<li';
        if (next > $this.count) {
            h += ' class="disabled"';
        }
        h += '><a';
        if (next > $this.count) {
            h += ' disabled="disabled" href="javascript:void(0)"';
        } else {
            h += ' href="' + getUrl($this.arg, next) + '"';
        }
        h += '><span aria-hidden="true">&raquo;</span></a></li>';
        $(h).appendTo(el);
    }

    function showMoreBefore($this, el) {
        if ($this.start > 1) {
            var index = $this.start - 1;
            if (index < 1) index = 1;
            var h = '<li><a href="' + getUrl($this.arg, index) + '">...</a></li>';
            $(h).appendTo(el);
        }
    }

    function showMoreAfter($this, el) {
        if ($this.end < $this.count) {
            var index = $this.start + $this.options.itemcount;
            if (index > $this.count) { index = $this.count; }
            var h = '<li><a href="' + getUrl($this.arg, index) + '">...</a></li>';
            $(h).appendTo(el);
        }
    }

    function showNumberLinks($this, el) {
        for (var i = $this.start; i <= $this.end; i++) {
            var h = '<li';
            if (i == $this.index) {
                h += ' class="active"';
            }
            h += '><a';
            if (i == $this.index) {
                h += ' href="javascript:void(0)"';
            } else {
                h += ' href="' + getUrl($this.arg, i) + '"';
            }
            h += '>' + i + '</a></li>';
            $(h).appendTo(el);
        }
    }

    Pager.prototype.show = function () {
        var that = this;
        this.arg = this.options.arg;
        this.index = getPageIndex(this.arg);
        if (this.options.pageindex > 1) {
            this.index = this.options.pageindex;
        }
        this.size = this.options.pagesize;
        this.total = this.options.total;
        this.count = parseInt((this.total + this.size - 1) / this.size, 10);
        console.log(this.index);
        if (this.index > this.count || this.index < 1) {
            this.$element.hide();
            return;
        }

        // start page index
        this.start = Math.ceil(this.index - (this.options.itemcount / 2));
        if (this.start + this.options.itemcount > this.count)
            this.start = this.count + 1 - this.options.itemcount;
        if (this.start < 1)
            this.start = 1;

        // end page index
        this.end = this.start + this.options.itemcount - 1;
        if (this.end > this.count)
            this.end = this.count;

        var linksBox = $('<ul class="pagination"></ul>');
        showFirstLink(this, linksBox);
        showPrevLink(this, linksBox);
        showMoreBefore(this, linksBox);
        showNumberLinks(this, linksBox);
        showMoreAfter(this, linksBox);
        showNextLink(this, linksBox);
        showLastLink(this, linksBox);

        this.$element.append(linksBox);

        var h = '<div style="display:inline-block;vertical-align:top;">';
        h += '<div style="height:34px; line-height:34px;margin:20px 0">';
        h += '&nbsp;&nbsp;&nbsp;&nbsp;第' + this.index + '/' + this.count + '页，共' + this.total + '条，每页' + this.size + '条';
        h += '</div>';
        h += '</div>';
        this.$element.append(h);
    }

    Pager.prototype.refresh = function () {
        this.show();
        console.log('refresh');
    }

    var old = $.fn.pager;

    $.fn.pager = function (option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('jquery.pager')
            var options = $.extend({}, Pager.DEFAULTS, $this.data(), typeof option == 'object' && option)

            if (!data) $this.data('jquery.pager', (data = new Pager(this, options)))
            if (typeof option == 'string') data[option]();
            else data.show();
        })
    }

    $.fn.pager.Constructor = Pager;

    $.fn.pager.noConflict = function () {
        $.fn.pager = old
        return this
    }
}(jQuery);