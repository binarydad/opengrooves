(function () {

    var currTimeout; // temp variable

    var OpenGrooves = {

        BootStrap: function () {
            OpenGrooves.UI.Init();
            OpenGrooves.ToolTip.Init();
        },

        AjaxForms: {

            FormNotify: function (text, obj) {
                OpenGrooves.AjaxForms.ClearNotify();
                var saved = $(document.createElement('span')).addClass('form-saved').html(text);
                obj.find('input:submit').before(saved);
            },

            ClearNotify: function () {
                $('span.form-saved').remove();
            },

            OnBegin: function () {
                OpenGrooves.AjaxForms.FormNotify('Saving...', $(this));
            },

            OnComplete: function (content) {
                var r = content.get_response().get_object();

                var errorsList = '';
                for (e in r.errors) {
                    errorsList += '* ' + r.errors[e];
                }

                if (r.errors) {
                    alert(errorsList);
                }

                var t = $(this);

                if (r.redirect) {
                    document.location.href = r.redirect;
                }
                else {
                    if (r.success) {
                        OpenGrooves.AjaxForms.FormNotify('Saved!', t);
                    }
                    else {
                        OpenGrooves.AjaxForms.FormNotify('<span class="red">Errors were found.</span>', t);
                    }
                }
            }
        },

        AjaxAction: {

            Render: function (action, controller, routeValues, containerSelector, displayLoading) {

                var container = $(containerSelector);

                if (displayLoading == null || displayLoading) {
                    container.addClass('loading');
                }

                $.ajax({
                    url: '/' + controller + '/' + action,
                    data: JSON.stringify(routeValues),
                    dataType: 'html',
                    contentType: 'application/json',
                    type: 'post', // TODO: handle GET option
                    success: function (d) {
                        container.removeClass('loading').html(d).css('opacity', 0).animate({ opacity: 1 }, 200);
                        container.find('form').submit(function () {
                            container.animate({ opacity: 0 }, 200);
                            var values = $(this).serializeObject();
                            $.extend(true, routeValues, values);
                            OpenGrooves.AjaxAction.Render(action, controller, routeValues, containerSelector, false);
                            return false;
                        });
                        OpenGrooves.AjaxAction.PanelsLoaded(container);
                    }
                });
            },

            PanelsLoaded: function (c) {
                OpenGrooves.UI.LoadTableRowHover(c);
                OpenGrooves.UI.LoadWatermarks(c);
                OpenGrooves.UI.LoadFancyBox(c);
            }
        },

        ToolTip: {

            Init: function () {
                $('a[data-popup-type]').live({
                    mouseenter: function () { OpenGrooves.ToolTip.DisplayToolTip($(this)); },
                    mouseleave: function () { OpenGrooves.ToolTip.HideToolTip(); }
                }).live('click', function () {
                    OpenGrooves.ToolTip.KillToolTip();
                });
            },

            DisplayToolTip: function (container) {
                var pop = $(document.createElement('div')).attr({ 'id': 'popup', 'class': 'popup loading' });
                var type = container.attr('data-popup-type');
                var data = container.attr('data-popup-data');

                $('div#popup').remove();
                pop.hide();
                container.before(pop);
                currTimeout = setTimeout(function () {

                    pop.delay(600).fadeIn().css({ display: 'inline' });

                    var url = type == 'user' ? '/popup/userinfo' :
                    type == 'band' ? '/popup/bandinfo' :
                    type == 'event' ? '/popup/eventinfo' : '';

                    $.ajax({
                        url: url,
                        data: 'data=' + data,
                        type: 'post',
                        success: function (d) {
                            pop.html(d).removeClass('loading');
                        }
                    });
                }, 500);
            },

            HideToolTip: function () {
                clearTimeout(currTimeout);
                $('div#popup').delay(1000).fadeOut(300, function () { $(this).remove(); });
            },

            KillToolTip: function () {
                clearTimeout(currTimeout);
                $('div#popup').remove();
            }
        },

        UI: {

            Init: function () {

                $('input.confirm, a.confirm').click(function () {
                    return confirm('Are you sure?');
                });

                // make anchor tags into submit buttons
                $('form a.submit-button').click(function () {
                    $(this).parents('form').submit();
                    return false;
                });

                // cufon
                Cufon.replace('h1, h2');

                OpenGrooves.UI.LoadTableRowHover();
                OpenGrooves.UI.LoadWatermarks();
                OpenGrooves.UI.LoadFancyBox();

            },

            Messages: {
                DeleteMessage: function () {
                    if (confirm('Are you sure?')) {
                        var row = $(this);
                        var id = row.attr('data-id');

                        $.ajax({
                            url: '/messages/delete',
                            data: 'messageId=' + id,
                            type: 'post',
                            success: function (d) {
                                //
                            }
                        });

                        row.parents('li').fadeOut('fast', function () { $(this).remove(); });

                        return false;
                    }
                }
            },

            LoadWatermarks: function (c) {

                // side search watermark
                var watermarks = $('input.watermark:not([data-watermark-loaded="true"])', c || document.body);
                watermarks.each(function () {

                    var box = $(this);

                    //console.log(box.attr('data-watermark'));

                    if (box.val().length == 0) {
                        box.val(box.attr('data-watermark'));
                        box
                            .click(function () {
                                if (box.hasClass('watermark')) {
                                    box.removeClass('watermark');
                                    box.val('');
                                }
                            })
                            .blur(function () {
                                if (box.val().length == 0) {
                                    box.addClass('watermark');
                                    box.val(box.attr('data-watermark'));
                                }
                            });

                        box.attr('data-watermark-loaded', true);
                    }
                    else {
                        box.removeClass('watermark');
                    }

                });
            },

            LoadFancyBox: function (c) {

                var images = $("div.gallery a:has(img), ul.images a:has(img), a.fancy", c || document.body);

                if (images.length > 0) {

                    images.fancybox({
                        href: $(this).attr('href'),
                        rel: $(this).attr('rel'),
                        transitionIn: 'fade',
                        transitionOut: 'fade',
                        speedIn: 600,
                        speedOut: 200,
                        overlayShow: true,
                        enableEscapeButton: true
                    });

                    //console.log('fancybox loaded in ' + (c ? (c.attr('id')) : 'window'));
                }
            },

            LoadTableRowHover: function (c) {

                // table row hover
                $('#shell #side-navigation ul > li, #shell ul.list > li', c || document.body).hover(
                    function () { $(this).addClass('hover'); },
                    function () { $(this).removeClass('hover'); }
                );
            },

            Confirm: function (msg) {
                return confirm(msg);
            }
        },

        AutoCompleteActions: {

            DataRedirect: function (value, data) {
                document.location.href = '/' + data;
            }
        }
    };

    window.OpenGrooves = OpenGrooves;

    // kick it off
    $(function () {
        OpenGrooves.BootStrap();
    });

})();


$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
