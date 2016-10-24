/*
 * 分页控件
 * 依赖：
 * bootstrap css v3
 * jquery-1.10.2.js
 * 用法：
 * <div id="pager1" class="row text-center" data-pagesize="10" data-total="250" data-arg="page"></div>
 * or
 * <div id="pager1" class="row text-center" data-pagesize="10" data-total="250" date-pageindex="1" data-pagefunc="query"></div>
 * $("#pager1").pager();
 */

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

    function getUrl($this, idx, txt, enable) {
        var link = $('<a></a>').html(txt);
        if (enable) {
            if (idx == $this.index) {
                link.prop('href', 'javascript:void(0)');
            } else {
                if ($this.pagefunc) {
                    link.prop('href', 'javascript:void(0)').attr('onclick', $this.pagefunc + '(' + idx + ')');
                } else {
                    link.prop('href', updateUrl($this.arg, idx));
                }
            }
        } else {
            link.prop('disabled', 'disabled').prop('href', 'javascript:void(0)');
        }
        return link;
    }

    function showFirstLink($this, el) {
        var obj = $('<li></li>');
        if ($this.index == 1) {
            obj.prop('class', 'disabled');
        }
        var link = getUrl($this, 1, "首页", $this.index > 1);
        obj.append(link).appendTo(el);
    }

    function showLastLink($this, el) {
        var obj = $('<li></li>');
        if ($this.index == $this.count) {
            obj.prop('class', 'disabled');
        }
        var link = getUrl($this, $this.count, "尾页", $this.index < $this.count);
        obj.append(link).appendTo(el);
    }

    function showPrevLink($this, el) {
        var idx = $this.index - 1;
        var obj = $('<li></li>');
        if (idx < 1) {
            obj.prop('class', 'disabled');
        }
        var link = getUrl($this, idx, '<span aria-hidden="true">&laquo;</span>', $this.index > 1);
        obj.append(link).appendTo(el);
    }

    function showNextLink($this, el) {
        var idx = $this.index + 1;
        var obj = $('<li></li>');
        if (idx > $this.count) {
            obj.prop('class', 'disabled');
        }
        var link = getUrl($this, idx, '<span aria-hidden="true">&raquo;</span>', $this.index < $this.count);
        obj.append(link).appendTo(el);
    }

    function showMoreBefore($this, el) {
        if ($this.start > 1) {
            var index = $this.start - 1;
            if (index < 1) index = 1;
            var obj = $('<li></li>');
            var link = getUrl($this, index, '...', true);
            obj.append(link).appendTo(el);
        }
    }

    function showMoreAfter($this, el) {
        if ($this.end < $this.count) {
            var index = $this.start + $this.options.itemcount;
            if (index > $this.count) { index = $this.count; }
            var obj = $('<li></li>');
            var link = getUrl($this, index, '...', true);
            obj.append(link).appendTo(el);
        }
    }

    function showNumberLinks($this, el) {
        for (var i = $this.start; i <= $this.end; i++) {
            var obj = $('<li></li>');
            if (i == $this.index) {
                obj.prop('class', 'active');
            }
            var link = getUrl($this, i, i, true);
            obj.append(link).appendTo(el);
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
        
        if (this.index > this.count || this.index < 1 || this.count <= 1) {
            this.$element.hide();
            return;
        }
        if (this.options.pagefunc) {
            this.pagefunc = this.options.pagefunc;
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